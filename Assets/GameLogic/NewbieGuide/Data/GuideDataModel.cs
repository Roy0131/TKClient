using System.Xml;
using Msg.ClientMessage;
using Google.Protobuf;
using System.Collections.Generic;

namespace NewBieGuide
{
    public class GuideDataModel : Singleton<GuideDataModel>
    {
        private List<GuideDataVO> _lstAllGuideDatas;
        public void Init()
        {
            _lstAllGuideDatas = new List<GuideDataVO>();

            XmlDocument xmlDoc = GameResMgr.Instance.LoadXml("NewBieGuide");
            if (xmlDoc == null)
            {
                LogHelper.LogError("[GuideDataModel.Init() => newbie guide xml not found!!!]");
                return;
            }

            XmlNodeList guideNodes = xmlDoc.SelectNodes("root/guide");
            if (guideNodes == null)
            {
                LogHelper.Log("[GuideDataModel.Init() => guide node was null!!!]");
                return;
            }
            GuideDataVO guideVO;
            foreach (XmlNode node in guideNodes)
            {
                if (node is XmlComment)
                    continue;
                guideVO = new GuideDataVO();
                guideVO.InitData(node);
                _lstAllGuideDatas.Add(guideVO);
            }
        }

        public GuideDataVO GetGuideDataVO(int index)
        {
            if (CheckIndexValid(index))
                return _lstAllGuideDatas[index];
            return null;
        }

        public bool CheckIndexValid(int idx)
        {
            return idx < _lstAllGuideDatas.Count;
        }

        //public void Dispose()
        //{
        //    if(_lstAllGuideDatas != null)
        //    {
        //        _lstAllGuideDatas.Clear();
        //        _lstAllGuideDatas = null;
        //    }
        //}

        public void SaveGuideData()
        {
            GameNetMgr.Instance.mGameServer.SaveGuideData(LocalDataMgr.NewBieGuideID, LocalDataMgr.NewBieGuildStepID);
        }

        public static void DoGuideSaveDataResult(S2CGuideDataSaveResponse value)
        {
            string tmp = value.Data.ToStringUtf8();
            LogHelper.Log("sava data back, data:" + tmp);
        }

        public static void DoGuideDataResponse(S2CGuideDataResponse value)
        {
            string tmp = value.Data.ToStringUtf8();
            if (string.IsNullOrEmpty(tmp))
            {
                LocalDataMgr.NewBieGuideID = 1;
                LocalDataMgr.NewBieGuildStepID = 1;
                return;
            } 
            string[] datas = tmp.Split('_');
            if (datas.Length != 2)
            {
                LocalDataMgr.NewBieGuideID = 1;
                LocalDataMgr.NewBieGuildStepID = 1;
                return;
            }   
            LocalDataMgr.NewBieGuideID = int.Parse(datas[0]);
            LocalDataMgr.NewBieGuildStepID = int.Parse(datas[1]);
            LogHelper.Log("guide data back, data:" + tmp);
        }
    }

    #region guide data
    public class GuideDataVO : DataBaseVO
    {
        public int mGuideId { get; private set; }
        public int mKeyStepId { get; private set; }
        public int mCameraType { get; private set; }

        private List<GuideStepDataVO> _lstStepDatas;
        public GuideDataVO()
        {
            _lstStepDatas = new List<GuideStepDataVO>();
        }

        protected override void OnInitData<T>(T value)
        {
            XmlElement ele = value as XmlElement;
            mGuideId = int.Parse(ele.GetAttribute("id"));
            mKeyStepId = int.Parse(ele.GetAttribute("keyStep"));
            mCameraType = int.Parse(ele.GetAttribute("cameraType"));

            if (ele.ChildNodes == null || ele.ChildNodes.Count == 0)
                return;
            XmlNodeList nodes = ele.ChildNodes;
            GuideStepDataVO stepVO;
            foreach (XmlNode node in nodes)
            {
                if (node is XmlComment)
                    continue;
                stepVO = new GuideStepDataVO();
                stepVO.InitData(node);
                _lstStepDatas.Add(stepVO);
            }
        }

        public GuideStepDataVO GetStepVO(int idx)
        {
            if (CheckIndexValid(idx))
                return _lstStepDatas[idx];
            return null;
        }

        public bool CheckIndexValid(int index)
        {
            return index < _lstStepDatas.Count;
        }
    }

    public class GuideStepDataVO : DataBaseVO
    {
        public int mStepID { get; private set; }
        public int mDialogID { get; private set; } = 0;
        public int mCondition { get; private set; } = 0;
        public int mMaskID { get; private set; } = 0;
        public int mModuleId { get; private set; } = 0;
        public int mEnterCondId { get; private set; } = 0;
        public int mJumpNextId { get; private set; } = 0;
        public int mSpecialLogicId { get; private set; } = 0;
        public int mBattleState { get; private set; } = 0;

        public int mWaitMaskStates { get; private set; } = 1;
        public int mEndConditionId { get; private set; } = 0;

        public int mSignOut { get; private set; } = 0;

        public GuideType mGuideType { get; private set; }
        public DialogStyle mDialogStyle { get; private set; }
        public GuideMaskType mGuideMaskType { get; private set; }

        public int mOpenMainMapDrag { get; private set; } = 0;

        protected override void OnInitData<T>(T value)
        {
            XmlElement ele = value as XmlElement;
            mStepID = int.Parse(ele.GetAttribute("id"));
            int tmp = 0;
            int.TryParse(ele.GetAttribute("type"), out tmp);
            mGuideType = (GuideType)tmp;
            tmp = 0;
            int.TryParse(ele.GetAttribute("dialogStyle"), out tmp);
            mDialogStyle = (DialogStyle)tmp;

            if (ele.HasAttribute("dialogId"))
                mDialogID = int.Parse(ele.GetAttribute("dialogId"));
            if (ele.HasAttribute("condition"))
                mCondition = int.Parse(ele.GetAttribute("condition"));
            if (ele.HasAttribute("maskid"))
                mMaskID = int.Parse(ele.GetAttribute("maskid"));

            tmp = 0;
            int.TryParse(ele.GetAttribute("maskType"), out tmp);
            mGuideMaskType = (GuideMaskType)tmp;

            if (ele.HasAttribute("openModule"))
                mModuleId = int.Parse(ele.GetAttribute("openModule"));
            if (ele.HasAttribute("enterCondition"))
                mEnterCondId = int.Parse(ele.GetAttribute("enterCondition"));
            if (ele.HasAttribute("jumpNextID"))
                mJumpNextId = int.Parse(ele.GetAttribute("jumpNextID"));
            if (ele.HasAttribute("specialLogic"))
                mSpecialLogicId = int.Parse(ele.GetAttribute("specialLogic"));
            if (ele.HasAttribute("battleState"))
                mBattleState = int.Parse(ele.GetAttribute("battleState"));
            if (ele.HasAttribute("waitMaskStatus"))
                mWaitMaskStates = int.Parse(ele.GetAttribute("waitMaskStatus"));
            if (ele.HasAttribute("endCondition"))
                mEndConditionId = int.Parse(ele.GetAttribute("endCondition"));
            if (ele.HasAttribute("openMainMapDrag"))
                mOpenMainMapDrag = int.Parse(ele.GetAttribute("openMainMapDrag"));
            if (ele.HasAttribute("SignOut"))
                mSignOut = int.Parse(ele.GetAttribute("SignOut"));
        }
    }
    #endregion
}
