using Framework.UI;
using LitJson;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CTowerBeforeRightView : UIBaseView
{

    private Text _textLayer;
    private Text _textAttack;
    private Button _btnVideo;
    private Button _onBattle;
    private RawImage _roleImage;

    private List<GameObject> _lstRoleFlags;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _onBattle = Find<Button>("ButtonBattle");
        _textLayer = Find<Text>("TextLayer");
        _textAttack = Find<Text>("TextAttack");
        _btnVideo = Find<Button>("ButtonVideo");
        ColliderHelper.SetButtonCollider(_btnVideo.transform);
        _roleImage = Find<RawImage>("RoleRoot/RawImage");

        _onBattle.onClick.Add(GoBattle);
        _btnVideo.onClick.Add(VideoStart);

        _lstRoleFlags = new List<GameObject>();
        for (int i = 1; i <= 9; i++)
            _lstRoleFlags.Add(Find("RoleRoot/t" + i + "/ImageHave"));
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        RefreshData();
    }
    private void RefreshData()
    {
        DisFloor();
        RoleCreate();
    }
    //进入战斗
    private void GoBattle()
    {
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(CTowerEvent.ClearTowerTimeHeap);
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(CTowerEvent.SetTowerPos);
        //GameUIMgr.Instance.CloseModule(ModuleID.BeforeBattle);
        LineupSceneMgr.Instance.ShowLineupModule(TeamType.Ctower, CTowerDataModel.Instance.currTowerID + 1);
    }

    //层数显示
    private void DisFloor()
    {
        _textLayer.text = LanguageMgr.GetLanguage(5001910)+" " + (CTowerDataModel.Instance.currTowerID + 1).ToString();
    }
    //战力数值显示
    private void TextAttack()
    {

    }
    //上阵敌方角色显示
    private void RoleCreate()
    {
        TowerConfig towerCfg = GameConfigMgr.Instance.GetTowerConfig(CTowerDataModel.Instance.currTowerID + 1);
        StageConfig stageCfg = GameConfigMgr.Instance.GetStageConfig(towerCfg.StageID);
        JsonData allMonsters = JsonMapper.ToObject(stageCfg.MonsterList);

        for (int i = 0; i < 9; i++)
            _lstRoleFlags[i].SetActive(false);
        LogHelper.Log(stageCfg.StageID);

        Dictionary<int, string> monsters = new Dictionary<int, string>();
        int slot;
        for (int i = 0; i < allMonsters.Count; i++)
        {
            JsonData jd = allMonsters[i];
            CardConfig cardCfg = GameConfigMgr.Instance.GetCardConfig(((int)jd["MonsterID"]) * 100 + (int)jd["Rank"]);
            slot = int.Parse(jd["Slot"].ToString());
            _lstRoleFlags[slot - 1].SetActive(true);
            monsters.Add(slot, cardCfg.Model);
        }

        RoleRTMgr.Instance.ShowRoleRTLogic(RoleRTType.BeforeBattle, monsters);
        if (!_roleImage.gameObject.activeSelf)
            _roleImage.gameObject.SetActive(true);
        _roleImage.texture = RoleRTMgr.Instance.GetRoleRTImage();
        //Debuger.Log(stageCfg.StageID + "创建角色成功");
    }
    //播放录像
    private void VideoStart()
    {
        LogHelper.Log("打开录像按钮");
        GameUIMgr.Instance.OpenModule(ModuleID.TowerVideo);
        CTowerDataModel.Instance.ReqTowerRecordInfoData();
    }

    public override void Dispose()
    {
        base.Dispose();
        if (_lstRoleFlags != null)
        {
            _lstRoleFlags.Clear();
            _lstRoleFlags = null;
        }
    }
}
