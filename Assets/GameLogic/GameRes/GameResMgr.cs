using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System;

public class GameResMgr : Singleton<GameResMgr>
{

    #region 初始化时，加载必要assetbundle资源到内存里
    class CommonResName
    {
        public const string Config = "config";
        public const string Shader = "shader";
        public const string Mat = "mat";
        public const string CampionIcon = "campicon";
        public const string GuildBoos = "guildboss";
        public const string GuildIcon = "guildicon";
        public const string BuffIcon = "bufficon";
        public const string ItemIcon = "itemicon";
        public const string LevelIcon = "levelicon";
        public const string RoleIcon = "roleicon";
        public const string SkillIcon = "skillicon";
        public const string ArtifactIcon = "artifacticon";
        public const string Welfare = "welfare";
        public const string Sound = "sound";
        public const string SystemIcon = "systemicon";
        public const string Effect = "effect";
    }

    private List<string> _lstBundleName;
    private Dictionary<string, string> _dictABPathKey;
    private Action _method;

    private bool _blUseAssetBundle = false;

    private List<string> _lstSyncUIPrefabs = new List<string>()
    {
        SingletonResName.UICardItem,
        SingletonResName.UILoading,
        SingletonResName.UINewBieGuide,
        SingletonResName.UIItem,
        SingletonResName.UIConfirm,
        SingletonResName.UIGetItem,
        SingletonResName.UIEquipTips,
        SingletonResName.UIRewardTips,
        SingletonResName.UITips,
        SingletonResName.UIHpBar,
        SingletonResName.UIBlood,
        SingletonResName.UIBuffTips,
        SingletonResName.UIRecconetTips
    };

    public void InitGameABRes(Action method)
    {
        _dictABPathKey = new Dictionary<string, string>();
        _lstBundleName = new List<string>();
        _method = method;
        RegBundleNameByType(CommonResName.Config, "prefabs/configs/configs.bytes");
        RegBundleNameByType(CommonResName.Shader, "shader/shader.bytes");
        RegBundleNameByType(CommonResName.Mat, "mat/mat.bytes");
        RegBundleNameByType(CommonResName.Effect, "prefabs/effect/effect.bytes");
        RegBundleNameByType(CommonResName.GuildIcon, "prefabs/guildicon/guildicon.bytes");
        RegBundleNameByType(CommonResName.CampionIcon, "prefabs/campicon/campicon.bytes");
        RegBundleNameByType(CommonResName.GuildBoos, "prefabs/guildboss/guildboss.bytes");
        RegBundleNameByType(CommonResName.BuffIcon, "prefabs/bufficon/bufficon.bytes");
        RegBundleNameByType(CommonResName.ItemIcon, "prefabs/itemicon/itemicon.bytes");
        RegBundleNameByType(CommonResName.LevelIcon, "prefabs/levelicon/levelicon.bytes");
        RegBundleNameByType(CommonResName.RoleIcon, "prefabs/roleicon/roleicon.bytes");
        RegBundleNameByType(CommonResName.SkillIcon, "prefabs/skillicon/skillicon.bytes");
        RegBundleNameByType(CommonResName.ArtifactIcon, "prefabs/artifacticon/artifacticon.bytes");
        RegBundleNameByType(CommonResName.Welfare, "prefabs/welfare/welfare.bytes");
        RegBundleNameByType(CommonResName.Sound, "prefabs/sound/sound.bytes");
        RegBundleNameByType(CommonResName.SystemIcon, "prefabs/systemicon/systemicon.bytes");

        for(int i = 0; i < _lstSyncUIPrefabs.Count; i++)
            RegBundleNameByType(_lstSyncUIPrefabs[i], "uiprefabs/" + _lstSyncUIPrefabs[i].ToLower() + ".bytes");

        _curIdx = 0;
        _totalBundleCount = _lstBundleName.Count;
        string tips = LocalDataMgr.IsChinese ? "初始化资源" : "Initialize resources";
        GameInitLoading.Instance.ShowLoadingTips( tips + "(1/" + _totalBundleCount + ")", 0f);
        DoLoadBundle();
        _blUseAssetBundle = true;
    }

    private int _totalBundleCount;
    private int _curIdx;
    private void DoLoadBundle()
    {
        string tips = LocalDataMgr.IsChinese ? "初始化资源" : "Initialize resources";//LanguageMgr.GetLanguage(6001261);
        Action<RAssetBundle> OnLoaded = (RAssetBundle ab) =>
        {
            _curIdx++;
            GameInitLoading.Instance.ShowLoadingTips(tips + "(" + _curIdx + "/" + _totalBundleCount + ")", (float)_curIdx / (float)_totalBundleCount);
            if (_curIdx >= _totalBundleCount)
            {
                _method.Invoke();
                _method = null;
                return;
            }
            DoLoadBundle();
        };

        if (_lstBundleName.Count > 0)
        {
            string abName = _lstBundleName[0];
            _lstBundleName.RemoveAt(0);
            RAssetBundleMgr.Instance.LoadAssetBundleAsyn(abName, OnLoaded);
        }
    }

    private void RegBundleNameByType(string abName, string abPath)
    {
        _dictABPathKey.Add(abName, abPath);
        _lstBundleName.Add(abPath);
    }

    #endregion

    #region common assetbundle load interface

    private RAssetBundle GetAssetBundleByName(string name)
    {
        if (_dictABPathKey.ContainsKey(name))
        {
            string path = _dictABPathKey[name];
            return RAssetBundleMgr.Instance.LoadAssetBundle(path);
        }
        else
        {
            LogHelper.LogError("[GameResMgr.GetAssetBundleByName () => name:" + name + " not registed!!!!]");
            return null;
        }
    }

    public XmlDocument LoadXml(string fileName)
    {
        TextAsset ta = null;

        if (_blUseAssetBundle)
        {
            RAssetBundle rab = GetAssetBundleByName(CommonResName.Config);
            ta = rab.ABAssets.LoadAsset<TextAsset>(fileName);
        }
        else
        {
            ta = Resources.Load<TextAsset>(ResPathHelper.GetXmlPath(fileName));
        }
        if (ta == null)
            return null;
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(ta.text);
        return doc;
    }

    public Sprite LoadCardIcon(string name)
    {
        Sprite result = null;
        if (_blUseAssetBundle)
        {
            RAssetBundle rab = GetAssetBundleByName(CommonResName.RoleIcon);
            result = rab.ABAssets.LoadAsset<Sprite>(name);
        }
        else
        {
            result = Resources.Load<Sprite>(ResPathHelper.GetRoleIconPath(name));
        }
        return result;
    }

    public Sprite LoadItemIcon(string name)
    {
        Sprite result = null;
        if (_blUseAssetBundle)
        {
            string[] tmpName = name.Split('/');
            RAssetBundle rab = GetAssetBundleByName(tmpName[0]);
            result = rab.ABAssets.LoadAsset<Sprite>(tmpName[1]);
        }
        else
        {
            result = Resources.Load<Sprite>(name);
        }
        return result;
    }

    public Sprite LoadCampIcon(int camp)
    {
        return LoadCampIcon("camp" + camp);
    }

    private Sprite LoadCampIcon(string icon)
    {
        Sprite result = null;
        if (_blUseAssetBundle)
        {
            RAssetBundle rab = GetAssetBundleByName(CommonResName.CampionIcon);
            result = rab.ABAssets.LoadAsset<Sprite>(icon);
        }
        else
        {
            result = Resources.Load<Sprite>(ResPathHelper.GetCampPath(icon));
        }
        return result;
    }

    public Sprite LoadTypeIcon(int type)
    {
        return LoadCampIcon("cardtype" + type);
    }

    public Sprite LoadSkillIcon(string name)
    {
        Sprite result = null;
        if (_blUseAssetBundle)
        {
            RAssetBundle rab = GetAssetBundleByName(CommonResName.SkillIcon);
            result = rab.ABAssets.LoadAsset<Sprite>(name);
        }
        else
        {
            result = Resources.Load<Sprite>(ResPathHelper.GetSkillIconPath(name));
        }
        return result;
    }

    public Sprite LoadGuildIcon(string name)
    {
        Sprite result = null;
        if (_blUseAssetBundle)
        {
            RAssetBundle rab = GetAssetBundleByName(CommonResName.GuildIcon);
            result = rab.ABAssets.LoadAsset<Sprite>(name);
        }
        else
        {
            result = Resources.Load<Sprite>(ResPathHelper.GetGuildIconPath(name));
        }
        return result;
    }

    public Sprite LoadBuffIcon(string name)
    {
        Sprite result = null;
        if (_blUseAssetBundle)
        {
            RAssetBundle rab = GetAssetBundleByName(CommonResName.BuffIcon);
            result = rab.ABAssets.LoadAsset<Sprite>(name);
        }
        else
        {
            result = Resources.Load<Sprite>(ResPathHelper.GetBuffIconPath(name));
        }
        return result;
    }

    public AudioClip LoadSound(string name)
    {
        AudioClip clip = null;
        if (_blUseAssetBundle)
        {
            RAssetBundle rab = GetAssetBundleByName(CommonResName.Sound); ;
            clip = rab.ABAssets.LoadAsset<AudioClip>(name);
        }
        else
        {
            clip = Resources.Load<AudioClip>(ResPathHelper.GetSoundClipPath(name));
        }
        return clip;
    }

    public GameObject LoadUIObjectSync(string name, bool blInstance = true)
    {
        GameObject originalObj = null;
        if (_blUseAssetBundle)
        {
            RAssetBundle ab = GetAssetBundleByName(name);
            originalObj = ab.ABAssets.LoadAsset<GameObject>(name);
        }
        else
        {
            originalObj = Resources.Load<GameObject>(ResPathHelper.GetUIPath(name));
        }
        if (originalObj == null)
            return null;
        if (blInstance)
            return InstanceObject(originalObj);
        return originalObj;
    }

    public Texture LoadArtifactTexture(string name)
    {
        Texture result = null;
        if (_blUseAssetBundle)
        {
            RAssetBundle rab = GetAssetBundleByName(CommonResName.ArtifactIcon);
            result = rab.ABAssets.LoadAsset<Texture>(name);
        }
        else
        {
            result = Resources.Load<Texture>(ResPathHelper.GetArtifactTexturePath(name));
        }
        return result;
    }
    #endregion

    private GameObject InstanceObject(GameObject original)
    {
        return GameObject.Instantiate<GameObject>(original);
    }

    public void LoadUIObjectAsync(string name, Action<GameObject> onLoadEnd, bool blInstance = true)
    {
        GameObject originalObj = null;

        Action OnEnd = () =>
        {
            if (originalObj == null)
            {
                LogHelper.LogError("[GameResMgr.LoadUIObjectAsync() => prefab name:" + name + ", not found!!!!]");
                onLoadEnd(null);
                return;
            }
            if (blInstance)
                onLoadEnd.Invoke(InstanceObject(originalObj));
            else
                onLoadEnd.Invoke(originalObj);
        };

        if (_blUseAssetBundle)
        {
            Action<RAssetBundle> OnLoaded = (rab) =>
            {
                originalObj = rab.ABAssets.LoadAsset<GameObject>(name);
                OnEnd();
            };
            string abName = "uiprefabs/" + name.ToLower() + ".bytes";
            RAssetBundleMgr.Instance.LoadAssetBundleAsyn(abName, OnLoaded);
        }
        else
        {
            originalObj = Resources.Load<GameObject>(ResPathHelper.GetUIPath(name));
            OnEnd();
        }
    }

    public void LoadRole(string name, Action<GameObject> OnLoaded)
    {
        GameObject originalObj = null;
        Action OnInstance = () =>
        {
            if (originalObj == null)
                OnLoaded(null);
            else
                OnLoaded(InstanceObject(originalObj));
        };

        if (_blUseAssetBundle)
        {
            Action<RAssetBundle> OnABLoaded = (rab) =>
            {
                originalObj = rab.ABAssets.LoadAsset<GameObject>(name);
                OnInstance();
            };
            string abPath = "roles/" + name.ToLower() + ".bytes";
            RAssetBundleMgr.Instance.LoadAssetBundleAsyn(abPath, OnABLoaded);
        }
        else
        {
            originalObj = Resources.Load<GameObject>(ResPathHelper.GetRolePath(name));
            OnInstance();
        }
    }

    public void LoadEffect(string name, Action<GameObject> onLoaded)
    {
        GameObject originalObj = null;

        Action OnInstance = () =>
        {
            if (originalObj == null)
                onLoaded(null);
            else
                onLoaded(InstanceObject(originalObj));
        };
        if (_blUseAssetBundle)
        {
            string abPath = "prefabs/effect/effect.bytes";
            Action<RAssetBundle> OnABLoaded = (rab) =>
            {
                originalObj = rab.ABAssets.LoadAsset<GameObject>(name);
                OnInstance();
            };
            RAssetBundleMgr.Instance.LoadAssetBundleAsyn(abPath, OnABLoaded);
        }
        else
        {
            originalObj = Resources.Load<GameObject>(ResPathHelper.GetEffectPath(name));
            OnInstance();
        }
    }

    public void LoadMapImage(string name, Action<Sprite> onLoaded)
    {
        Sprite result = null;
        if (_blUseAssetBundle)
        {
            Action<RAssetBundle> OnABLoaded = (rab) =>
            {
                result = rab.ABAssets.LoadAsset<Sprite>(name);
                onLoaded(result);
            };
            string mapABPath = "maps/" + name.ToLower() + ".bytes";
            RAssetBundleMgr.Instance.LoadAssetBundleAsyn(mapABPath, OnABLoaded);
        }
        else
        {
            result = Resources.Load<Sprite>(ResPathHelper.GetMapPath(name));
            onLoaded(result);
        }
    }
}