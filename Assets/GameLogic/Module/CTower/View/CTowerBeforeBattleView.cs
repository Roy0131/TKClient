using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class CTowerBeforeBattleView : UIBaseView {

    //private Button _btnClose;
    private Button _onBattle;
    private GameObject _rewardGrid;
    private GameObject _rewardItem;
    private Text _textExplain;
    private GameObject _rewardTipsWindow;
    private Button _rewardTipsWindowBack;
    private Button _btnBlackback;

    private Text _textLayer;
    private Text _textAttack;
    private Button _btnVideo;
    private GameObject _videoWindow;
    #region
    private GameObject _roleTrans01;
    private GameObject _roleTrans02;
    private GameObject _roleTrans03;
    private GameObject _roleTrans04;
    private GameObject _roleTrans05;
    private GameObject _roleTrans06;
    private GameObject _roleTrans07;
    private GameObject _roleTrans08;
    private GameObject _roleTrans09;
#endregion
    private int _floorNum;

    public CTowerBeforeBattleView(int index)
    {
        _floorNum = index + 1;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        //_btnClose = Find<Button>("Btn_Back");
        _onBattle = Find<Button>("RightSide/ButtonBattle");
        _rewardGrid = Find("LeftSide/RewardGrid");
        _rewardItem = Find("LeftSide/RewardItem");
        _textExplain = Find<Text>("LeftSide/TextExplain");
        _rewardTipsWindow = Find("LeftSide/RewardTips");
        _rewardTipsWindowBack = Find<Button>("LeftSide/RewardTips/BlackBack");
        _btnBlackback = Find<Button>("BlackBack");

        _textLayer = Find<Text>("RightSide/TextLayer");
        _textAttack = Find<Text>("RightSide/TextAttack");
        _btnVideo = Find<Button>("RightSide/ButtonVideo");
        _videoWindow = Find("VideoWindow");
        #region
        _roleTrans01 = Find("RightSide/ImageBottom01");
        _roleTrans02 = Find("RightSide/ImageBottom02");
        _roleTrans03 = Find("RightSide/ImageBottom03");
        _roleTrans04 = Find("RightSide/ImageBottom04");
        _roleTrans05 = Find("RightSide/ImageBottom05");
        _roleTrans06 = Find("RightSide/ImageBottom06");
        _roleTrans07 = Find("RightSide/ImageBottom07");
        _roleTrans08 = Find("RightSide/ImageBottom08");
        _roleTrans09 = Find("RightSide/ImageBottom09");
        #endregion
        //_btnClose.onClick.Add(OnClose);
        //_btnBlackback.onClick.Add(OnClose);
        _onBattle.onClick.Add(GoBattle);
        _btnVideo.onClick.Add(VideoStart);
        _rewardTipsWindowBack.onClick.Add(CloseRewardTips);

        DisFloor();
        DisReward();
    }

    //关闭窗口
    //private void OnClose()
    //{
    //    Hide();
    //    ClearGrid(_rewardGrid);
    //}
    
    //奖励图片显示
    private void DisReward()
    {
        TowerConfig towerCfg = GameConfigMgr.Instance.GetTowerConfig(_floorNum);
        StageConfig stageCfg = GameConfigMgr.Instance.GetStageConfig(towerCfg.StageID);

        string[] lstReward = stageCfg.RewardList.Split(',');


        //图标生成个数
        for (int i = 0; i <(( lstReward.Length+1)/2); i++)
        {
            GameObject _item =  GameObject.Instantiate(_rewardItem, _rewardGrid.transform);
            _item.SetActive(true);
            _item.GetComponent<Button>().onClick.Add(DIsRewardTips);
        }
    }
    //奖励图片描述面板显示
    private void DIsRewardTips()
    {
        _rewardTipsWindow.SetActive(true);
    }
    //奖励图片描述面板关闭
    private void CloseRewardTips()
    {
        _rewardTipsWindow.SetActive(false);
    }
    //本关描述
    private void DIsDiscribe()
    {

    }
    //层数显示
    private void DisFloor()
    {
        _textLayer.text = "Layer " + _floorNum.ToString();
    }
    //战力数值显示
    private void TextAttack()
    {

    }
    //上阵敌方角色显示
    private void RoleCreate()
    {

    }
    //播放录像
    private void VideoStart()
    {
        GameUIMgr.Instance.OpenModule(ModuleID.TowerVideo);
        
    }
    //进入战斗
    private void GoBattle()
    {
        LineupSceneMgr.Instance.ShowLineupModule(TeamType.Ctower, _floorNum);
    }

    //清除grid下面的数据
    private void ClearGrid(GameObject parent)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject go = parent.transform.GetChild(i).gameObject;
            GameObject.Destroy(go);
        }
    }
}
