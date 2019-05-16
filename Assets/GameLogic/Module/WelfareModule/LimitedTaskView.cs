using Framework.UI;
using Msg.ClientMessage;
using UnityEngine;
using UnityEngine.UI;

//任务
public class LimitedTaskView : UIBaseView
{
    private Text _text;
    private Text _fillText;
    private Image _fillImg;
    private ItemView _view;
    private RectTransform _parent;

    private bool isGray;


    protected override void ParseComponent()
    {
        base.ParseComponent();
        _text = Find<Text>("Text");
        _fillText = Find<Text>("FillImg/Text");
        _fillImg = Find<Image>("FillImg/Fill");
        _parent = Find<RectTransform>("ItemObj");
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        LimitedItemDataVO limitedItemDataVO = args[0] as LimitedItemDataVO;
        int eventId = int.Parse(args[1].ToString());
        SubActiveConfig cfg = GameConfigMgr.Instance.GetSubActiveConfig(limitedItemDataVO.mSubActiveID);
        _text.text = LanguageMgr.GetLanguage(cfg.DescriptionId, cfg.Param1, cfg.Param2, cfg.Param3, cfg.Param4);
        int curNum;
        if (eventId == 304)
            curNum = cfg.Param2;
        else if (eventId == 306)
            curNum = cfg.Param2;
        else
            curNum = cfg.Param1;
        if (limitedItemDataVO.mCurValue >= curNum)
        {
            isGray = true;
            _fillText.text = curNum + "/" + curNum;
        }
        else
        {
            isGray = false;
            _fillText.text = limitedItemDataVO.mCurValue + "/" + curNum;
        }
        _fillImg.fillAmount = (float)limitedItemDataVO.mCurValue / (float)curNum;
        string[] rewards = cfg.Reward.Split(',');
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
            _view.mRectTransform.SetParent(_parent, false);
            if (isGray)
                _view.SetGray();
            else
                _view.SetNormal();
        }
    }

    public override void Dispose()
    {
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        _view = null;
        base.Dispose();
    }
}
