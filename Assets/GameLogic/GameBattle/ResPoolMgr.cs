using UnityEngine;
using System.Collections.Generic;
using System;

public class ResPoolMgr : Singleton<ResPoolMgr>
{
    class PreResData
    {
        public string mResName;
        public BattleUnitType mUnitType;
    }

    private Dictionary<BattleUnitType, Dictionary<string, Queue<GameObject>>> _dictPools;

    private Queue<PreResData> _preResQueue = new Queue<PreResData>();
    private List<string> _lstBulletValue = new List<string>();
    private List<string> _lstEffectValue = new List<string>();

    public void Init()
    {
        _dictPools = new Dictionary<BattleUnitType, Dictionary<string, Queue<GameObject>>>();
    }

    private void ParseRoleRes(List<FighterDataVO> value)
    {
        PreResData data;
        for (int i = 0; i < value.Count; i++)
        {
            data = new PreResData();
            data.mUnitType = BattleUnitType.Fighter;
            data.mResName = value[i].mCardConfig.Model;
            _preResQueue.Enqueue(data);
        }
    }

    private void ParseRoundRes(RoundNodeDataVO roundData)
    {
        if (roundData == null)
            return;
        SkillConfig config;
        PreResData data;
        for (int i = 0; i < roundData.mlstActionNodes.Count; i++)
        {
            for (int j = 0; j < roundData.mlstActionNodes[i].mActionItemDatas.Count; j++)
            {
                config = roundData.mlstActionNodes[i].mActionItemDatas[j].mSkillConfig;
                if (!string.IsNullOrEmpty(config.BulletAnim))
                {
                    if (_lstBulletValue.IndexOf(config.BulletAnim) == -1)
                    {
                        data = new PreResData();
                        data.mResName = config.BulletAnim;
                        data.mUnitType = BattleUnitType.Bullet;
                        _preResQueue.Enqueue(data);
                        _lstBulletValue.Add(config.BulletAnim);
                    }
                }
                if (!string.IsNullOrEmpty(config.BulletHitEffect))
                {
                    if (_lstEffectValue.IndexOf(config.BulletHitEffect) == -1)
                    {
                        data = new PreResData();
                        data.mResName = config.BulletHitEffect;
                        data.mUnitType = BattleUnitType.Effection;
                        _preResQueue.Enqueue(data);
                        _lstEffectValue.Add(config.BulletHitEffect);
                    }
                }
            }
        }
    }

    public void PrepareBattleRes()
    {
        ParseRoleRes(BattleDataModel.Instance.mlstHeroFighterDatas);
        ParseRoleRes(BattleDataModel.Instance.mlstTargetFighterDatas);
        ParseRoundRes(BattleDataModel.Instance.mEnterRound);
        for (int i = 0; i < BattleDataModel.Instance.mlstActionRounds.Count; i++)
            ParseRoundRes(BattleDataModel.Instance.mlstActionRounds[i]);
        _lstEffectValue.Clear();
        _lstBulletValue.Clear();
    }

    private bool _blLoading = false;
    public bool PreloadEffect()
    {
        if (_blLoading)
            return true;
        if (_preResQueue.Count == 0 && !_blLoading)
            return false;
        if (_preResQueue.Count > 0)
        {
            PreResData data = _preResQueue.Dequeue();
            Action<GameObject> OnLoad = (effObject) =>
            {
                ReturnBattleObject(data.mUnitType, data.mResName, effObject);
                _blLoading = false;
            };

            _blLoading = true;
            LoadObject(data.mUnitType, data.mResName, OnLoad);
        }
        return true;
    }

    private void LoadObject(BattleUnitType type, string name, Action<GameObject> OnLoaded)
    {
        switch (type)
        {
            case BattleUnitType.BattleUI:
                GameResMgr.Instance.LoadUIObjectAsync(name, OnLoaded);
                break;
            case BattleUnitType.Fighter:
                GameResMgr.Instance.LoadRole(name, OnLoaded);
                break;
            case BattleUnitType.Effection:
            case BattleUnitType.Bullet:
                GameResMgr.Instance.LoadEffect(name, OnLoaded);
                break;
        }
    }

    public void GetBattleUnitObject(BattleUnitType type, string name, Action<GameObject> OnObjectLoaded)
    {
        GameObject obj = GetQueueByType(type, name);
        if (obj != null)
        {
            Delay[] delays = obj.GetComponentsInChildren<Delay>(true);
            if (delays != null)
            {
                for (int i = 0; i < delays.Length; i++)
                    delays[i].gameObject.SetActive(true);
            }
            obj.SetActive(true);
            OnObjectLoaded(obj);
        }
        else
        {
            LoadObject(type, name, OnObjectLoaded);
        }
    }

    public void ReturnBattleObject(BattleUnitType type, string name, GameObject obj)
    {
        Dictionary<string, Queue<GameObject>> dict;
        if (_dictPools.ContainsKey(type))
        {
            dict = _dictPools[type];
        }
        else
        {
            dict = new Dictionary<string, Queue<GameObject>>();
            _dictPools.Add(type, dict);
        }
        Queue<GameObject> queue = null;
        if (dict.ContainsKey(name))
        {
            queue = dict[name];
        }
        else
        {
            queue = new Queue<GameObject>();
            dict.Add(name, queue);
        }
        queue.Enqueue(obj);
        Delay[] delays = obj.GetComponentsInChildren<Delay>(true);
        if (delays != null)
        {
            for (int i = 0; i < delays.Length; i++)
                delays[i].ReturnPool();//gameObject.SetActive(false);
        }
        obj.transform.SetParent(null, false);
        obj.SetActive(false);
    }

    private GameObject GetQueueByType(BattleUnitType type, string name)
    {
        Dictionary<string, Queue<GameObject>> dict = null;
        if (!_dictPools.ContainsKey(type))
            return null;
        dict = _dictPools[type];
        if (dict.ContainsKey(name))
        {
            Queue<GameObject> queue = dict[name];
            if (queue != null && queue.Count > 0)
                return queue.Dequeue();
        }
        return null;
    }

    public void Dispose()
    {
        if (_dictPools != null)
        {
            Dictionary<BattleUnitType, Dictionary<string, Queue<GameObject>>>.ValueCollection vallCol = _dictPools.Values;
            Dictionary<string, Queue<GameObject>>.ValueCollection v2;
            foreach (Dictionary<string, Queue<GameObject>> dict in vallCol)
            {
                v2 = dict.Values;
                foreach (Queue<GameObject> queue in v2)
                {
                    int len = queue.Count;
                    for (int i = 0; i < len; i++)
                        GameObject.Destroy(queue.Dequeue());
                }
                dict.Clear();
            }
            _dictPools.Clear();
            _dictPools = null;
        }
    }
}