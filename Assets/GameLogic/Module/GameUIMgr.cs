using GameConsole;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Framework.UI;
using System;

public class GameUIMgr : Singleton<GameUIMgr>
{
    private RectTransform _uiModuleRoot;
    private RectTransform _uiWindowRoot;
    private RectTransform _uiPopupRoot;
    private RectTransform _uiTopRoot;
    private RectTransform _uiBattleRoot;

    public RectTransform mGuideRoot { get; private set; }

    private Dictionary<ModuleID, ModuleBase> _dictAllModule;
    private Canvas _uiCanvas;
    public bool blPadMode = false;

    public Camera mUICamera { get; private set; }
    private Dictionary<UILayer, ModuleBase> _dictShowModules;
    private Dictionary<UILayer, List<ModuleID>> _dictHideStackModules;
    
    public Canvas UICanvas
    {
        get { return _uiCanvas; }
    }

    public void Init()
    {
        Transform uiRoot = GameObject.Find("UIRoot").transform;
        if (uiRoot == null)
        {
            LogHelper.LogError("can't find uiroot, init uimanage failed");
            return;
        }
        _uiCanvas = uiRoot.Find("Canvas").GetComponent<Canvas>();
        mUICamera = uiRoot.Find("UICamera").GetComponent<Camera>();

        _dictAllModule = new Dictionary<ModuleID, ModuleBase>();
        _dictShowModules = new Dictionary<UILayer, ModuleBase>();
        _dictHideStackModules = new Dictionary<UILayer, List<ModuleID>>();

        _uiModuleRoot = uiRoot.Find("Canvas/UIModuleRoot").GetComponent<RectTransform>();
        _uiWindowRoot = uiRoot.Find("Canvas/UIWindowRoot").GetComponent<RectTransform>();
        _uiPopupRoot = uiRoot.Find("Canvas/UIPopupRoot").GetComponent<RectTransform>();
        _uiTopRoot = uiRoot.Find("Canvas/UITopRoot").GetComponent<RectTransform>();
        _uiBattleRoot = uiRoot.Find("Canvas/UIBattleRoot").GetComponent<RectTransform>();
        mGuideRoot = uiRoot.Find("Canvas/UIGuideRoot").GetComponent<RectTransform>();
        RegistModule();

        int ManualWidth = 1334;
        int ManualHeight = 750;
        if (Convert.ToSingle(Screen.height) / Screen.width > Convert.ToSingle(ManualHeight) / ManualWidth)
        {
            int manualHeight;
            manualHeight = Mathf.RoundToInt(Convert.ToSingle(ManualWidth) / Screen.width * Screen.height);
            blPadMode = true;
            float scale = Convert.ToSingle(manualHeight / 750f);
            mUICamera.fieldOfView *= scale;
        }
        else
        {
            blPadMode = false;
            ////Canvas Scaler
            var canvasScaler = _uiCanvas.GetComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            int sw = Screen.width;
            int sh = Screen.height;
            float sr = sw / (float)sh;
            int rw = (int)(sr * ManualHeight);
            int rh = ManualHeight;
            canvasScaler.referenceResolution = new Vector2(rw, rh);
        }

        RoleResPool.Instance.Init(uiRoot.Find("ResPoolRoot"));
    }

    private void RegistModule()
    {
        _dictAllModule.Add(ModuleID.Console, new ConsoleModule());
        _dictAllModule.Add(ModuleID.Home, new HomeModule());
        _dictAllModule.Add(ModuleID.Battle, new BattleModule());
        _dictAllModule.Add(ModuleID.Hangup, new HangupModule());
        _dictAllModule.Add(ModuleID.CampaignMap, new CampaignMapModule());
        _dictAllModule.Add(ModuleID.Lineup, new LineupModule());
        _dictAllModule.Add(ModuleID.Bag, new BagModule());
        _dictAllModule.Add(ModuleID.CTower, new CTowerModule());
        _dictAllModule.Add(ModuleID.BeforeBattle, new CTowerBeforeModule());
        _dictAllModule.Add(ModuleID.TowerVideo, new CTowerVideoModule());
        _dictAllModule.Add(ModuleID.TowerRank, new CTowerRankModule());
        _dictAllModule.Add(ModuleID.Mail, new MailModule());

        _dictAllModule.Add(ModuleID.ActivityCopy, new ActivityCopyModule());
        _dictAllModule.Add(ModuleID.ActivityFriend, new ActivityCopyFriendModule());
        #region
        _dictAllModule.Add(ModuleID.Explore, new ExploreModule());
        _dictAllModule.Add(ModuleID.ExploreGroup, new ExploreGroupModule());
        _dictAllModule.Add(ModuleID.ExploreBroadcast, new ExploreBroadcastModule());
        #endregion


        _dictAllModule.Add(ModuleID.Gold, new GoldModule());

        _dictAllModule.Add(ModuleID.MysteryShop, new MysteryShopModule());
        _dictAllModule.Add(ModuleID.HeroShop, new HeroShopModule());

        _dictAllModule.Add(ModuleID.Task, new TaskModule());

        #region Role Equipment
        _dictAllModule.Add(ModuleID.RoleBag, new RoleBagModule());
        _dictAllModule.Add(ModuleID.RoleInfo, new RoleInfoModule());
        _dictAllModule.Add(ModuleID.RoleRankup, new RoleRankupModule());
        _dictAllModule.Add(ModuleID.Recruit, new RecruitModule());
        _dictAllModule.Add(ModuleID.RoleSelect, new RoleSelectModule());
        _dictAllModule.Add(ModuleID.RoleFusion, new RoleFusionModule());
        _dictAllModule.Add(ModuleID.RoleDecompose, new RoleDecomposeModule());
        _dictAllModule.Add(ModuleID.EquipFunc, new EquipFuncModule());
        _dictAllModule.Add(ModuleID.Equipment, new EquipmentModule());

        #endregion

        #region Arena Module
        _dictAllModule.Add(ModuleID.Arena, new ArenaModule());
        #endregion

        _dictAllModule.Add(ModuleID.Friend, new FriendModule());
        _dictAllModule.Add(ModuleID.Rank, new RankModule());

        _dictAllModule.Add(ModuleID.Setting, new SettingModule());

        _dictAllModule.Add(ModuleID.Talent, new TalentModule());

        #region guild
        _dictAllModule.Add(ModuleID.Guild, new GuildModule());
        _dictAllModule.Add(ModuleID.HeroGuild, new HeroGuildModule());
        _dictAllModule.Add(ModuleID.MemberMgr, new MemberMgrModule());
        _dictAllModule.Add(ModuleID.GuildBoss, new GuildBossModule());
        #endregion

        #region chat
        _dictAllModule.Add(ModuleID.Chat, new ChatModule());
        #endregion

        _dictAllModule.Add(ModuleID.Player, new PlayerModule());

        _dictAllModule.Add(ModuleID.Welfare, new WelfareModule());

        _dictAllModule.Add(ModuleID.Recharge, new RechargeModule());

        _dictAllModule.Add(ModuleID.HeroCall, new HeroCallModule());

        _dictAllModule.Add(ModuleID.Attendance, new AttendanceModule());

        _dictAllModule.Add(ModuleID.Expedition, new ExpeditionModule());

        _dictAllModule.Add(ModuleID.Artifact, new ArtifactModule());

        _dictAllModule.Add(ModuleID.Carnival, new CarnivalModule());
    }

    public void OpenModule(ModuleID id, params object[] args)
    {
        if (!_dictAllModule.ContainsKey(id))
        {
            LogHelper.LogError("open module:" + id + ", but this module unregisted!!!");
            return;
        }
        if (id != ModuleID.Home)
        {
            //if (MainMapMgr.Instance != null)
            MainMapMgr.Instance.Enable = false;
        }
        ModuleBase module = _dictAllModule[id];
        if (_dictShowModules.ContainsKey(module.mLayer))
        {
            ModuleBase curModule = _dictShowModules[module.mLayer];
            if (curModule.mModuleID == id)
            {
                curModule.Show(args);
                return;
            }
            if (curModule.mBlStack)
            {
                List<ModuleID> lst;
                if (_dictHideStackModules.ContainsKey(curModule.mLayer))
                {
                    lst = _dictHideStackModules[curModule.mLayer];
                }
                else
                {
                    lst = new List<ModuleID>();
                    _dictHideStackModules.Add(curModule.mLayer, lst);
                }
                if (lst.IndexOf(curModule.mModuleID) == -1)
                    lst.Insert(0, curModule.mModuleID);
            }
            _dictShowModules[module.mLayer].Hide();
            _dictShowModules[module.mLayer] = module;
        }
        else
        {
            _dictShowModules.Add(module.mLayer, module);
        }

        float t = Time.realtimeSinceStartup;
        Action<GameObject> OnObjectLoaded = (uiObject) =>
        {
            //LogHelper.LogWarning("[load ab cost:" + (Time.realtimeSinceStartup - t) + "]");
            //t = Time.realtimeSinceStartup;
            module.InitModuleObject(uiObject);
            //LogHelper.LogWarning("[parse component cost:" + (Time.realtimeSinceStartup - t) + "]");
            //t = Time.realtimeSinceStartup;
            module.Show(args);
           // LogHelper.LogWarning("[do show logic cost:" + (Time.realtimeSinceStartup - t) + "]");
        };

        LogHelper.LogWarning("[GameUIMgr.OpenModule() => module id:" + id + "]");
        if (module.mRectTransform == null)
            GameResMgr.Instance.LoadUIObjectAsync(module.ModuleResName, OnObjectLoaded);
        else
            module.Show(args);
    }

    public void CloseModule(ModuleID id, params object[] args)
    {
        if (!_dictAllModule.ContainsKey(id))
        {
            LogHelper.LogError("open module:" + id + ", but this module unregisted!!!");
            return;
        }
        LogHelper.Log("close module:" + id);
        ModuleBase module = _dictAllModule[id];


        if (_dictShowModules.ContainsKey(module.mLayer))
            _dictShowModules.Remove(module.mLayer);
        module.Hide();
        if (module.mBlStack)
        {
            if (_dictHideStackModules.ContainsKey(module.mLayer))
            {
                List<ModuleID> stacks = _dictHideStackModules[module.mLayer];
                if (stacks.Count > 0)
                {
                    id = stacks[0];
                    stacks.RemoveAt(0);
                    OpenModule(id);
                }
            }
        }

        if (MainMapMgr.Instance != null)
        {
            if (_dictShowModules.ContainsKey(UILayer.Window) || _dictShowModules.ContainsKey(UILayer.Popup))
                return;
            MainMapMgr.Instance.Enable = true;
            //Debuger.LogWarning("reset");
            switch (module.mModuleID)
            {
                case ModuleID.Chat:
                case ModuleID.Player:
                case ModuleID.Gold:
                case ModuleID.Recharge:
                    break;
                default:
                    if (_dictShowModules.ContainsKey(UILayer.Module))
                        (_dictAllModule[ModuleID.Home] as HomeModule).PlayAnimation();
                    break;
            }
        }
    }

    public bool CanEnableMainMap()
    {
        return !_dictShowModules.ContainsKey(UILayer.Window) && !_dictShowModules.ContainsKey(UILayer.Popup);
    }

    public void LockMainMapDrag(bool blLock)
    {
        if (MainMapMgr.Instance == null)
            return;
        if (!blLock)
        {
            if (_dictShowModules.ContainsKey(UILayer.Window) || _dictShowModules.ContainsKey(UILayer.Popup))
                return;
        }
        MainMapMgr.Instance.Enable = true;
    }

    public void CloseAllModule()
    {
        if (_dictShowModules.Count > 0)
        {
            Dictionary<UILayer, ModuleBase>.ValueCollection value = _dictShowModules.Values;
            foreach (ModuleBase moduleBase in value)
                moduleBase.Hide();
            _dictShowModules.Clear();
        }
        _dictHideStackModules.Clear();
    }

    public void AddModuleToStage(ModuleBase module)
    {
        RectTransform parent = null;
        switch (module.mLayer)
        {
            case UILayer.Module:
                parent = _uiModuleRoot;
                break;
            case UILayer.Window:
                parent = _uiWindowRoot;
                break;
            case UILayer.Popup:
                parent = _uiPopupRoot;
                break;
        }
        ChildAddToParent(module.mRectTransform, parent);
    }

    public void ChildAddToParent(RectTransform child, RectTransform parent)
    {
        Vector3 achoredPos = child.anchoredPosition;
        Vector3 sizeDelta = child.sizeDelta;
        Vector3 scale = child.localScale;
        child.SetParent(parent, false);
        child.localScale = scale;
        child.anchoredPosition = achoredPos;
        child.sizeDelta = sizeDelta;
    }

    public void AddObjectToTopRoot(RectTransform child)
    {
        ChildAddToParent(child, _uiTopRoot);
    }

    public void AddBattleUIToStage(UIBaseView view)
    {
        ChildAddToParent(view.mRectTransform, _uiBattleRoot);
    }

    public Vector3 RTWorldToUIPoint(Vector3 position)
    {
        Vector2 world2ScreenPos = RoleRTMgr.Instance.mCamera.WorldToScreenPoint(position);//Camera.main.WorldToScreenPoint(worldPos);//世界坐标转屏幕坐标
        Vector2 uiPos = new Vector2();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_uiCanvas.transform as RectTransform, world2ScreenPos, _uiCanvas.worldCamera, out uiPos);
        return uiPos;
    }

    public Vector3 WorldToUIPoint(Vector3 position)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_uiCanvas.transform as RectTransform, Camera.main.WorldToScreenPoint(position), _uiCanvas.worldCamera, out pos);
        return pos;
    }

    public Vector2 ScreenPosToLocalPos(Vector2 screenPos)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_uiCanvas.transform as RectTransform, screenPos, mUICamera, out pos);
        return pos;
    }
}