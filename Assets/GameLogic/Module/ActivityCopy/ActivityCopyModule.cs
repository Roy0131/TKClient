using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class ActivityCopyModule : ModuleBase
{
    private Text _textTitle;
    private Button _btnClose;
    private Transform _root;
    private HomeModule _home;
    private ActivityCopyView _activityCopyView;
    private bool isData = false;
    private int _curType;
    private int _VIPLevel = 0;

    public ActivityCopyModule():base(ModuleID.ActivityCopy,UILayer.Window)
	{
		_modelResName = UIModuleResName.UI_Activity;
	}

    protected override void ParseComponent()
	{
        _home = new HomeModule();
        _home.RefreshARedPoint(false);

        _activityCopyView = new ActivityCopyView();
        _activityCopyView.SetDisplayObject(Find("ActivityCopy"));

        _textTitle = Find<Text>("Root/TextTitle");
        _btnClose = Find<Button>("Root/ButtonClose");
        ColliderHelper.SetButtonCollider(_btnClose.transform);
        _btnClose.onClick.Add(OnClose);
        _root = Find<Transform>("Root");

        for (int i = 0; i < 3; i++)
        {
           Button btnBattle = Find<Button>("Root/ActivityGrid/ActivityItem0" + (i + 1) + "/ButtonBattle");
            int type = i + 1;
            btnBattle.onClick.Add(delegate { OpenRank(type); });

            Button btnHelp = Find<Button>("Root/ActivityGrid/ActivityItem0" + (i + 1) + "/ButtonHelp");
            int id = i;
            btnHelp.onClick.Add(delegate { HelpTipsMgr.Instance.ShowTIps(HelpType.GoldCopyHelp + id); });

            int num = i;
            Text title = Find<Text>("Root/ActivityGrid/ActivityItem0" + (num + 1) + "/TextTitle");
            title.text = LanguageMgr.GetLanguage(5001602 + num);
        }
        _textTitle.text = LanguageMgr.GetLanguage(5001601);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        ActivityCopyDataModel.Instance.AddEvent<ActivityCopyVO>(ActivityCopyEvent.ActivityCopyData, OnActivityCopyData);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        ActivityCopyDataModel.Instance.RemoveEvent<ActivityCopyVO>(ActivityCopyEvent.ActivityCopyData, OnActivityCopyData);
    }

    private void OnActivityCopyData(ActivityCopyVO vo)
    {
        if (!isData)
        {
            ActivityCopyDataModel.Instance.OnType(vo.mStageType);
            _activityCopyView.Show(vo);
        }
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        if (_VIPLevel < HeroDataModel.Instance.mHeroInfoData.mVipLevel && ActivityCopyDataModel.Instance.mDictActivityVO.Count > 0)
        {
            isData = true;
            GameNetMgr.Instance.mGameServer.ReqActiveStageData(ActivityCopyConst.OneCopy);
        }
        _VIPLevel = HeroDataModel.Instance.mHeroInfoData.mVipLevel;
        if (bool.Parse(args[0].ToString()))
        {
            if (BattleDataModel.Instance.mBlWin)
                ActivityCopyDataModel.Instance.OnActivityCopyVOType(ActivityCopyDataModel.Instance._curType).OnRemainChallengeNum();
            _activityCopyView.Show(ActivityCopyDataModel.Instance.OnActivityCopyVOType(ActivityCopyDataModel.Instance._curType));
        }
    }

    private void OpenRank(int type)
    {
        isData = false;
        if (ActivityCopyDataModel.Instance.OnActivityCopyVOType(type) != null)
            OnActivityCopyData(ActivityCopyDataModel.Instance.OnActivityCopyVOType(type));
        else
            ActivityCopyDataModel.Instance.ReqActivityCopyData(type);
    }

    public override void Hide()
    {
        if (_activityCopyView != null)
            _activityCopyView.Hide();
        base.Hide();
    }

    public override void Dispose()
    {
        if (_activityCopyView != null)
        {
            _activityCopyView.Dispose();
            _activityCopyView = null;
        }
        base.Dispose();
    }

    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        ObjectHelper.PopAnimationLiner(_root);
    }

}
