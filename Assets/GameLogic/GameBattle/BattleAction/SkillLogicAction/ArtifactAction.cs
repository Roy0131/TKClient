using UnityEngine;
using Framework.UI;
using System.Collections.Generic;

public class ArtifactAction : SkillActionBase
{
    private bool _blHeroArtifact;
    private ArtifactLevelConfig _artifactCfg;
    private GameObject _artifactEnterEffect;

    private FrameTicker _enterEffTick;
    private string _enterEffectName = "fx_artifact_enter";

    private GameObject _artifactObject;
    private FrameTicker _artifactTick;
    private bool _blArtifactEnd;

    

    protected override void ParseData()
    {
        _skillTotalFrame = mActionItemData.mSkillConfig.CastTime;
        _allDamageFighters = new List<Fighter>();
        _allBuffFighters = new List<Fighter>();
        _blArtifactEnd = false;
    }

    protected override void OnStart()
    {
        _blHeroArtifact = BattleDataModel.Instance.IsHeroFighter(mActionItemData.mSide);
        int artifactID = _blHeroArtifact ? BattleDataModel.Instance.mSelfArtifactId : BattleDataModel.Instance.mTargetArtifactId;
        _artifactCfg = GameConfigMgr.Instance.GetArtifactLevelConfig(artifactID);
        if (_artifactCfg == null)
        {
            LogHelper.LogError("[ArtifactAction.OnStart() => artifact id:" + artifactID + " config not found!!!]");
            return;
        }

        ResPoolMgr.Instance.GetBattleUnitObject(BattleUnitType.Effection, _enterEffectName, OnModelLoaded);
    }

    private void OnModelLoaded(GameObject gameObject)
    {
        _artifactEnterEffect = gameObject;

        MeshRenderer _shader1 = _artifactEnterEffect.transform.Find("fx_a_currency_03").GetComponent<MeshRenderer>();
        MeshRenderer _shader2 = _artifactEnterEffect.transform.Find("fx_a_currency_04").GetComponent<MeshRenderer>();
        MeshRenderer _shader4 = _artifactEnterEffect.transform.Find("fx_a_currency_05").GetComponent<MeshRenderer>();
        MeshRenderer _shader3 = _artifactEnterEffect.transform.Find("fx_a_currency_06").GetComponent<MeshRenderer>();
        ArtifactUnlockConfig _artifactUnlockConfig = GameConfigMgr.Instance.GetArtifactUnlockConfig(_artifactCfg.ArtifactID);
        string[] rgbs = _artifactUnlockConfig.BackGroundRGB.Split('|');
        if (rgbs.Length % 3 != 0)
            return;
        for (int i = 0; i < rgbs.Length; i++)
        {
            string[] rgb = rgbs[i].Split(',');
            if (rgb.Length % 4 != 0)
                return;
            for (int j = 0; j < rgb.Length; j += 4)
            {
                if (i == 0)
                {
                    _shader1.material.SetColor("_TintColor", new Color(float.Parse(rgb[j]) / 255,
                        float.Parse(rgb[j + 1]) / 255, float.Parse(rgb[j + 2]) / 255, float.Parse(rgb[j + 3]) / 255));
                    _shader4.material.SetColor("_TintColor", new Color(float.Parse(rgb[j]) / 255,
                        float.Parse(rgb[j + 1]) / 255, float.Parse(rgb[j + 2]) / 255, float.Parse(rgb[j + 3]) / 255));
                }
                else if (i == 1)
                {
                    _shader2.material.SetColor("_TintColor", new Color(float.Parse(rgb[j]) / 255,
                        float.Parse(rgb[j + 1]) / 255, float.Parse(rgb[j + 2]) / 255, float.Parse(rgb[j + 3]) / 255));
                }
                else if (i == 2)
                {
                    _shader3.material.SetColor("_TintColor", new Color(float.Parse(rgb[j]) / 255,
                        float.Parse(rgb[j + 1]) / 255, float.Parse(rgb[j + 2]) / 255, float.Parse(rgb[j + 3]) / 255));
                }
            }
        }

        MeshRenderer meshRenderer = _artifactEnterEffect.transform.Find("mainTexture").GetComponent<MeshRenderer>();
        meshRenderer.material.mainTexture = GameResMgr.Instance.LoadArtifactTexture(_artifactCfg.BattleGIFRes);
        float scale = _artifactCfg.BattleGIFScale / 100f;
        meshRenderer.transform.localScale = Vector3.one * scale;

        AddToStage(_artifactEnterEffect, BattleManager.Instance.mBattleScene.mBulletRoot);

        _enterEffTick = new FrameTicker(1.2f, OnHideEnterEffect);
    }

    private void OnHideEnterEffect()
    {
        if (_artifactEnterEffect != null)
            ResPoolMgr.Instance.ReturnBattleObject(BattleUnitType.Effection, _enterEffectName, _artifactEnterEffect);
        _artifactEnterEffect = null;

        string[] hitTimes = mActionItemData.mSkillConfig.HitShowTime.Split(',');
        int frame = int.Parse(hitTimes[0]) - _damageOffsetFrame;
        _damageTriggerTimer = new FrameTimer(frame, DoDamage);

        ResPoolMgr.Instance.GetBattleUnitObject(BattleUnitType.Effection, mActionItemData.mSkillConfig.BulletHitEffect, OnArtifactLoaded);
    }

    protected override void OnUpdate()
    {
        if (_enterEffTick != null && _enterEffTick.mBlEnable)
            _enterEffTick.Update();
        if (_artifactTick != null && _artifactTick.mBlEnable)
            _artifactTick.Update();
        base.OnUpdate();
    }

    private void OnArtifactLoaded(GameObject gameObject)
    {
        _artifactObject = gameObject;
        AddToStage(_artifactObject, BattleManager.Instance.mBattleScene.mBulletRoot);

        if (_blHeroArtifact)
            _artifactObject.transform.localScale = Vector3.one;
        else
            _artifactObject.transform.localScale = new Vector3(-1f, 1f, 1f);
        if (!string.IsNullOrEmpty(_artifactCfg.BattleEffectRes))
        {
            MeshRenderer meshRenderer = _artifactObject.transform.Find("mainTexture").GetComponent<MeshRenderer>();
            meshRenderer.material.mainTexture = GameResMgr.Instance.LoadArtifactTexture(_artifactCfg.BattleEffectRes);
        }
        EffectConfig cfg = GameConfigMgr.Instance.GetEffectConfig(mActionItemData.mSkillConfig.BulletHitEffect);
        _artifactTick = new FrameTicker((float)cfg.Duration / 1000f, OnArtifactEnd);
        _status = AttackNodeStatus.Attacking;

    }

    private void OnArtifactEnd()
    {
        _blArtifactEnd = true;
        if (_artifactObject != null)
        {
            ResPoolMgr.Instance.ReturnBattleObject(BattleUnitType.Effection, mActionItemData.mSkillConfig.BulletHitEffect, _artifactObject);
            _artifactObject = null;
        }
        if (_blHeroArtifact)
            GameEventMgr.Instance.mBattleDispatcher.DispathEvent(BattleEvent.SelfArtifactActionEnd);
        OnActionEnd();
    }

    protected override void OnActionEnd()
    {
        if (!AllActionEnd)
            _status = AttackNodeStatus.WaitingEndTime;
        else
            GameEventMgr.Instance.mBattleDispatcher.DispathEvent<SkillActionBase>(BattleEvent.BattleStepEnd, this);
    }

    protected override void OnAttackEnd()
    {
        OnActionEnd();
    }

    private void AddToStage(GameObject child, Transform parent)
    {
        Vector3 defScale = child.transform.localScale;
        child.transform.SetParent(parent, false);
        child.transform.localScale = defScale;
        child.transform.localPosition = new Vector3(0f, 2.61f, 0f);//Vector3.zero;
        child.SetActive(true);

        Renderer[] renders = child.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renders.Length; i++)
            renders[i].sortingOrder = 200;

        ObjectHelper.SetObjectLayer(child.transform, parent.gameObject.layer);
    }

    protected override void OnDispose()
    {
        if (_enterEffTick != null)
        {
            _enterEffTick.Dispose();
            _enterEffTick = null;
        }
        if (_artifactTick != null)
        {
            _artifactTick.Dispose();
            _artifactTick = null;
        }
        base.OnDispose();
    }

    protected override bool AllActionEnd
    {
        get
        {
            if (!_blArtifactEnd)
                return false;
            return base.AllActionEnd;
        }
    }
}
