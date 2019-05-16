using Framework.Core;
using UnityEngine;

namespace IHLogic
{
    public class LogicMain
    {
        #region instance logic
        private static LogicMain _instance;
        private static LogicMain Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LogicMain();
                return _instance;
            }
        }
        #endregion

        #region init logic
        public static void RunGame()
        {
            Instance.Init();
        }

        private void Init()
        {
            LocationMgr.Instance.Init();
            LocalDataMgr.Init();
            ColliderHelper.InitColor();
            LanguageMgr.curLanguage = LocalDataMgr.IsChinese ? SystemLanguage.Chinese : LocalDataMgr.CurLanguage;

            if (GameDriver.Instance.UseAssetBundle)
            {
                try
                {
                    GameResMgr.Instance.InitGameABRes(InitGameResEnd);
                    return;
                }
                catch (System.Exception ex)
                {
                    LogHelper.Log("[GameDriver.InitDefaultRes() => init game res failed, ex:" + ex.Message + "]");
                }
                //InitGameResEnd();
            }
            else
            {
                InitGameResEnd();
            }
        }

        private void InitGameResEnd()
        {
            GameNetMgr.Instance.Init(GameDriver.Instance.m_serverName);
            GameDriver.Instance.mInitObject.SetActive(false);
            GameDriver.Instance.mLoginObject.SetActive(true);
            GameLoginMgr.Instance.Init(GameDriver.Instance.mLoginObject);
            GameConsole.ConsoleCmdMgr.Instance.Init();

            InitMainLogicMethod();
        }

        private void InitMainLogicMethod()
        {
            GameEntry.Instance.mHotLogic.mUpdateAction = () => { Update(); };
            GameEntry.Instance.mHotLogic.mLateUpteAction = () => { LateUpdate(); };
            GameEntry.Instance.mHotLogic.mKeyCodeAction = () => { DoKeyCodeFun(); };
            GameEntry.Instance.mHotLogic.mSoundAction = () => { PlayButtonSound(); };
            GameEntry.Instance.mHotLogic.mAppPauseAction = (bl) => { OnApplicationPause(bl); };
            GameEntry.Instance.mHotLogic.mAppQuitAction = () => { OnApplicationQuit(); };

            GameEntry.Instance.mHotLogic.mProvideContentAction = (value) => { NativeLogicInterface.ProvideContent(value); };
            GameEntry.Instance.mHotLogic.mAppStoreBuyFailedAction = () => { NativeLogicInterface.AppStoreBuyFailed(); };
            GameEntry.Instance.mHotLogic.mGooglePaySuccessAction = (value) => { NativeLogicInterface.GooglePaySuccess(value); };
            GameEntry.Instance.mHotLogic.mGooglePayFailedAction = () => { NativeLogicInterface.GooglePayFailed(); };
            GameEntry.Instance.mHotLogic.mFBLoginBackAction = (value) => { NativeLogicInterface.OnFacebookLoginBack(value); };
            GameEntry.Instance.mHotLogic.mFBLoginCancelAction = () => { NativeLogicInterface.OnFacebookLoginCancel(); };
            GameEntry.Instance.mHotLogic.mShopDetailAction = (value) => { NativeLogicInterface.OnShopDetailResult(value); };
            GameEntry.Instance.mHotLogic.mFBShareBackAction = (value) => {   NativeLogicInterface.OnFBShareBack(value); }
            ;
        }

        private static int _dropOutTime = 0;
        //public static bool mBlCarnivalState = false;
        public static int mCarnivalType = 0;
        private static void OnApplicationPause(bool pause)
        {
            if (string.IsNullOrEmpty(LoginHelper.mToken))
                return;
            if (pause)
            {
                _dropOutTime = (int)Time.realtimeSinceStartup;
            }
            else
            {
                if (mCarnivalType != 0)
                {
                    if (_dropOutTime > 0 && (int) Time.realtimeSinceStartup - _dropOutTime >= 20)
                    {
                        CarnivalDataModel.Instance.OnCarnivaleFinished(mCarnivalType);
                    }
                    mCarnivalType = 0;
                }
                else
                {
                    if (_dropOutTime > 0 && (int)Time.realtimeSinceStartup - _dropOutTime >= GameConst.PauseTime)
                        GameNetMgr.Instance.mGameServer.ReqReconnect();
                }
            }
        }

        public static void LateUpdate()
        {
            GameNetMgr.Instance.Update();
            GameStageMgr.Instance.Update();
        }

        public static void Update()
        {
            TimerHeap.Tick();
            GameComponent.Instance.Update();
        }

        public static void OnApplicationQuit()
        {
            ChatModel.Instance.SaveChatDataToCache();
            GameNetMgr.Instance.Dispose();
        }
        #endregion

        public static void PlayButtonSound()
        {
            SoundMgr.Instance.StopEffectSound("UI_btn_click");
            SoundMgr.Instance.PlayEffectSound("UI_btn_click");
        }

        public static void DoKeyCodeFun()
        {
            GameConsole.ConsoleCmdMgr.Instance.DoShortCutConsole();
        }
    }
}