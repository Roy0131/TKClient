using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Framework.UI;

public class GuildDonateItem : UIBaseView
{
    private Text _nameText;
    private RectTransform _itemIcon;
    private Text _heroOwnText;
    private Image _donateSilder;
    private Text _donateText;

    private Button _donateBtn;
    private DragHelper _infoBtn;

    private GuildDonateVO _vo;

    private GameObject _donateTips;

    private ItemView _view;

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _nameText = Find<Text>("NameText");
        _itemIcon = Find<RectTransform>("ItemIcon");
        _heroOwnText = Find<Text>("StaticRoot/Num");
        _donateSilder = Find<Image>("Silder");
        _donateText = Find<Text>("Silder/Count");
        _donateBtn = Find<Button>("DonateBtn");

        GameObject infoBtn = Find("InfoBtn");
        _infoBtn = infoBtn.AddComponent<DragHelper>();

        _donateTips = Find("DonateTips");
        _tipDonateText = Find<Text>("DonateTips/Image/Count");
        _tipDonateTimeText = Find<Text>("DonateTips/Image/CDTime");
        _donateBtn.onClick.Add(OnDonate);
        _infoBtn.mDragMethod = OnDrag;
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        BagDataModel.Instance.AddEvent<List<int>>(BagEvent.BagItemRefresh, OnGuildRefresh);
        GuildDataModel.Instance.AddEvent<bool>(GuildEvent.GuildDonateRefresh, OnDonateRefresh);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        BagDataModel.Instance.RemoveEvent<List<int>>(BagEvent.BagItemRefresh, OnGuildRefresh);
        GuildDataModel.Instance.RemoveEvent<bool>(GuildEvent.GuildDonateRefresh, OnDonateRefresh);
    }

    private void OnDonateRefresh(bool over)
    {
        if (!over)
            OnDonateInit();
    }

    private void OnGuildRefresh(List<int> lsitId)
    {
        if (lsitId.Contains(_vo.mDonateItemID))
            _heroOwnText.text = BagDataModel.Instance.GetItemCountById(_vo.mDonateItemID).ToString();
    }

    private Text _tipDonateText;
    private Text _tipDonateTimeText;
    private int _lastTime;
    private uint _cdTimeKey;
    private void ClearCDTime()
    {
        if (_cdTimeKey != 0)
            TimerHeap.DelTimer(_cdTimeKey);
        _cdTimeKey = 0;
    }
    private void OnShowTip()
    {
        _donateTips.SetActive(true);
        _tipDonateText.text = LanguageMgr.GetLanguage(5003130) + "：" + GuildDataModel.Instance.mGuildDataVO.mDonateNum + "/" + GameConst.GuildDonateIntegralUp;
        if (_vo.RemainCDTime > 0)
        {
            _lastTime = _vo.RemainCDTime;
            ClearCDTime();
            _cdTimeKey = TimerHeap.AddTimer(0, 1000, OnCDTime);
        }
    }

    private void OnCDTime()
    {
        if (_lastTime < 0)
        {
            ClearCDTime();
            return;
        }
        _tipDonateTimeText.text = LanguageMgr.GetLanguage(5003131) + TimeHelper.GetCountTime(_lastTime);
        _lastTime--;
    }

    private void OnDrag(DragEventType type, PointerEventData eventData, DragHelper dragHelper)
    {
        if (type != DragEventType.MouseDown && type != DragEventType.MouseUp)
            return;
        if (type == DragEventType.MouseDown)
        {
            OnShowTip();
        }
        else
        {
            _donateTips.SetActive(false);
            ClearCDTime();
        }
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _vo = args[0] as GuildDonateVO;
        OnDonateInit();
    }

    private void OnDonateInit()
    {
        _nameText.text = _vo.mPlayerName + " " + LanguageMgr.GetLanguage(5003113);
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        _view = ItemFactory.Instance.CreateItemView(GameConfigMgr.Instance.GetItemConfig(_vo.mDonateItemID), ItemViewType.ShopItem, OnClick);
        _view.mRectTransform.SetParent(_itemIcon, false);
        int count = BagDataModel.Instance.GetItemCountById(_vo.mDonateItemID);
        _heroOwnText.text = count.ToString();
        _donateSilder.fillAmount = (float)_vo.mDonateItemNum / (float)_vo.mDonateItemMax;
        _donateText.text = _vo.mDonateItemNum + "/" + _vo.mDonateItemMax;
        if (_vo.mDonateItemNum >= _vo.mDonateItemMax || count - 1 < 0 || _vo.mPlayerID == HeroDataModel.Instance.mHeroPlayerId || _vo.mIsDonate)
            ObjectHelper.SetEnableStatus(_donateBtn, false);
        else
            ObjectHelper.SetEnableStatus(_donateBtn, true);
    }

    private void OnClick(ItemView view)
    {

    }

    public override void Hide()
    {
        OnCDTime();
        base.Hide();
    }

    private void OnDonate()
    {
        GameNetMgr.Instance.mGameServer.ReqGuildDonate(_vo.mPlayerID, _vo.mDonateItemID, _vo.mDonateItemNum);
    }

    public override void Dispose()
    {
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        _view = null;
        _vo = null;
        OnCDTime();
        base.Dispose();
    }
}
