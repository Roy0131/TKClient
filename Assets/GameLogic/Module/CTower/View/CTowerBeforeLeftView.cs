using Framework.UI;
using Msg.ClientMessage;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CTowerBeforeLeftView : UIBaseView {

    private GameObject _rewardGrid;
    private ItemView _rewardItem;
    private Text _textExplain;
    private GameObject _rewardTipsWindow;

    private List<GameObject> _obj = new List<GameObject>();

    private Button _rewardTipsWindowBack;
    private ItemConfig _itemCfg;
    private Text _name;
    private Image _imageIcon;
    private Text _textDes01;
    private Text _textDes02;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _rewardGrid = Find("RewardGrid");
        _textExplain = Find<Text>("TextDescribe");
        _rewardTipsWindow = Find("RewardTips");

        _rewardTipsWindowBack = Find<Button>("RewardTips/BlackBack");
        _name = Find<Text>("RewardTips/TextName");
        _imageIcon = Find<Image>("RewardTips/ImageIconBack/ImageIcon");
        _textDes01 = Find<Text>("RewardTips/TextDescribe01");
        _textDes02 = Find<Text>("RewardTips/TextDescribe02");

        _rewardTipsWindowBack.onClick.Add(CloseTips);
      
    }
    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        RefreshData();
    }
    //奖励图片显示
    private void RefreshData()
    {
        DiposeChildren();
        TowerConfig towerCfg = GameConfigMgr.Instance.GetTowerConfig(CTowerDataModel.Instance.currTowerID+1);
        StageConfig stageCfg = GameConfigMgr.Instance.GetStageConfig(towerCfg.StageID);

        string[] lstReward = stageCfg.RewardList.Split(',');
        
        //创建图片
        for (int i = 0; i < lstReward.Length; i++)
        {
            if (i % 2 == 0)
            {
                ItemInfo info = new ItemInfo();
                info.Id = Convert.ToInt32(lstReward[i]);
                info.Value = Convert.ToInt32(lstReward[i + 1]);
                _rewardItem = ItemFactory.Instance.CreateItemView(info, ItemViewType.RewardItem);
                _rewardItem.mRectTransform.SetParent(_rewardGrid.transform, false);
                AddChildren(_rewardItem);
            }
        }
        //关卡描述
        LogHelper.Log(stageCfg.DescrptionID + "---------------------刷新语言表id数据");
        _textExplain.text = LanguageMgr.GetLanguage(stageCfg.DescrptionID);
    }
    private void CloseTips()
    {
        _rewardTipsWindow.SetActive(false);
    }

    public override void Dispose()
    {
        base.Dispose();
        if (_obj != null)
        {
            _obj = null;
        }
    }
}
