using Framework.UI;
using LitJson;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CTowerView :UIBaseView
{
    private GameObject _LevelItem;
    private GameObject _LevelItemParent;
    private GameObject _springScroll;
    private GameObject _bgItem;
    private GameObject _bgGrid;
    private ScrollRect _scroll;
    private RawImage _roleImage;

    private bool _refreshPos = false;
    protected override void ParseComponent()
    {
        base.ParseComponent();

        _LevelItem = Find("CTowerItem");
        _LevelItemParent = Find("ScrollView/Content");
        _springScroll = Find("ScrollView");
        _bgItem = Find("BgItem");
        _bgGrid = Find("ScrollViewBg/Content");
        _roleImage = Find<RawImage>("ScrollView/RoleImage");

        _scroll = Find<ScrollRect>("ScrollView");
        _scroll.onValueChanged.AddListener((v) => { RefreshSilder(); });
    }
    protected override void AddEvent()
    {
        base.AddEvent();
        CTowerDataModel.Instance.AddEvent(CTowerEvent.RefreshTowerData, RefreshData);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(CTowerEvent.SetTowerPos, SetPos);
    }
    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        CTowerDataModel.Instance.RemoveEvent(CTowerEvent.RefreshTowerData, RefreshData);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(CTowerEvent.SetTowerPos, SetPos);
    }

    private void SetPos()
    {
        _refreshPos = false;
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        RefreshData();
    }

    /// <summary>
    /// 刷新复制物体
    /// </summary>
    private void RefreshData()
    {
        DiposeChildren();

        CTowerLevelItemView itemView;
        int level;
        TowerConfig towerCfg;
        StageConfig stageCfg;
        JsonData allMonsters;
        CardConfig cardCfg;
        Dictionary<int, string> dict = new Dictionary<int, string>();
        int index = 0;
        for (int i = 0; i < 6; i++)
        {
            GameObject item = GameObject.Instantiate(_LevelItem);
            item.transform.SetParent(_LevelItemParent.transform, false);
            item.transform.SetAsFirstSibling();
            index = i * 2;


            if (CTowerDataModel.Instance.currTowerID < 5)
                level = 2 * i + 1;
            else if (CTowerDataModel.Instance.currTowerID > 293)
                level = 289 + 2 * i;
            else
                level = CTowerDataModel.Instance.currTowerID + 2 * i - 4;
            itemView = new CTowerLevelItemView(level);

            if (level > CTowerDataModel.Instance.currTowerID)
            {
                towerCfg = GameConfigMgr.Instance.GetTowerConfig(level);
                stageCfg = GameConfigMgr.Instance.GetStageConfig(towerCfg.StageID);
                allMonsters = JsonMapper.ToObject(stageCfg.MonsterList);

                cardCfg = GameConfigMgr.Instance.GetCardConfig(((int)allMonsters[2]["MonsterID"]) * 100 + (int)allMonsters[2]["Rank"]);
                dict.Add(index, cardCfg.Model);
            }
            if ((level + 1) > CTowerDataModel.Instance.currTowerID)
            {
                towerCfg = GameConfigMgr.Instance.GetTowerConfig(level + 1);
                stageCfg = GameConfigMgr.Instance.GetStageConfig(towerCfg.StageID);
                allMonsters = JsonMapper.ToObject(stageCfg.MonsterList);

                cardCfg = GameConfigMgr.Instance.GetCardConfig(((int)allMonsters[2]["MonsterID"]) * 100 + (int)allMonsters[2]["Rank"]);
                dict.Add(index + 1, cardCfg.Model);
            }
            itemView.SetDisplayObject(item);
            itemView.Show();
            AddChildren(itemView);
        }
        RoleRTMgr.Instance.ShowRoleRTLogic(RoleRTType.CTower, dict);
        _roleImage.texture = RoleRTMgr.Instance.GetRoleRTImage();
        if (!_roleImage.gameObject.activeSelf)
            _roleImage.gameObject.SetActive(true);
        //if (!_refreshPos)
            GetPoint();

        RefreshSilder();
    }

    private void RefreshSilder()
    {
        float per = Mathf.Abs(_scroll.content.anchoredPosition.y / 2454f);
        float y = (30.11f - 3.47f) * per;
        RoleRTMgr.Instance.mCamera.transform.localPosition = new Vector3(0.28f, y + 3.47f, 0f);
    }

    /// <summary>
    /// 打开时候定位置
    /// </summary>
    private void GetPoint()
    {
        float _levelItemHeight = _LevelItem.GetComponent<RectTransform>().sizeDelta.y;//item高度
        float _distance = _levelItemHeight / 2;//单层的高度
        if (CTowerDataModel.Instance.currTowerID < 5)
        {
            _scroll.content.anchoredPosition = new Vector2(0.0f, -_distance * CTowerDataModel.Instance.currTowerID);
        }
        else if (CTowerDataModel.Instance.currTowerID > 289)
        {
            _scroll.content.anchoredPosition = new Vector2(0.0f, -_distance *( CTowerDataModel.Instance.currTowerID-288));
        }
        else
        {
            _scroll.content.anchoredPosition = new Vector2(0.0f, -_distance * 5);
        }
        _refreshPos = true;
    }
}
