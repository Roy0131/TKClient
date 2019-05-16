using Framework.UI;
using Msg.ClientMessage;
using UnityEngine;
using UnityEngine.UI;

public class ActivityCopyItemView : UIBaseView
{
    private ActiveStageConfig _curCfg;
    private int _id = 0;
    private int _num = 0;
    
    private Image _roleIcon;
    private Text _textBattle;
    private RectTransform _Parent;
    private Button _btnBattle;
    private Text _battleText;
    private ItemView _view;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _roleIcon = Find<Image>("ImageIcon");
        _textBattle = Find<Text>("TextBattle");
        _Parent = Find<RectTransform>("RewardGrid");
        _btnBattle = Find<Button>("ButtonBattle");
        _battleText = Find<Text>("ButtonBattle/Text");

        _btnBattle.onClick.Add(GoBattle);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _curCfg = args[0] as ActiveStageConfig;
        _id = int.Parse(args[1].ToString());
        _num = int.Parse(args[2].ToString());
        SetData();
    }
    
    private void SetData()
    {
        _roleIcon.sprite =GameResMgr.Instance.LoadItemIcon("levelicon/icon_huodongguai_" + _id);
        StageConfig _stageCfg = GameConfigMgr.Instance.GetStageConfig(_curCfg.StageID);
        string[] rewards = _stageCfg.RewardList.Split(',');
        if (rewards.Length % 2 != 0)
            return;
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        for (int i = 0; i < rewards.Length; i += 2)
        {
            ItemInfo itemInfo = new ItemInfo();
            itemInfo.Id = int.Parse(rewards[i]);
            itemInfo.Value = int.Parse(rewards[i + 1]);
            if (GameConfigMgr.Instance.GetItemConfig(itemInfo.Id).ItemType == 2)
                _view = ItemFactory.Instance.CreateItemView(itemInfo, ItemViewType.EquipHeroItem);
            else
                _view = ItemFactory.Instance.CreateItemView(itemInfo, ItemViewType.HeroItem);
            _view.mRectTransform.SetParent(_Parent, false);
        }

        _textBattle.text =string.Format(LanguageMgr.GetLanguage(5001612), _curCfg.PlayerLevelSuggestion.ToString()) ;

        if (HeroDataModel.Instance.mHeroInfoData.mLevel < _curCfg.PlayerLevelCond)
        {
            _btnBattle.interactable = false;
            _battleText.text= _curCfg.PlayerLevelCond.ToString();
        }
        else
        {
            _btnBattle.interactable = true;
            _battleText.text = LanguageMgr.GetLanguage(5001506);
        }
    }
    
    private void GoBattle()
    {
        if (_num == 0)
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000023));
        else
            GameUIMgr.Instance.OpenModule(ModuleID.ActivityFriend, _curCfg);

    }

    public override void Dispose()
    {
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        _view = null;
        base.Dispose();
    }
}