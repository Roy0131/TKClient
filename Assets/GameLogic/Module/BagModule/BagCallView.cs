using Framework.UI;
using NewBieGuide;
using UnityEngine;
using UnityEngine.UI;


public class BagCallView : UIBaseView
{
    private Text _name;
    private Text _callPanalText;
    private Image _callPanImg;
    private InputField _inputField;
    private Button _cutBtn;
    private Button _addBtn;
    private Button _callBtn;
    private Button _wholeBtn;
    private ItemView _itemView;
    private RectTransform _parent;
    private ItemView _view;
    private ItemConfig _cfg;

    int Callnum = 1;//召唤个数
    int Existing = 0;//现有的碎片
    int Number = 0;//需要的碎片
    int CallID;//物品ID


    protected override void ParseComponent()
    {
        base.ParseComponent();
        _name = Find<Text>("CallPanel/Name");
        _callPanalText = Find<Text>("CallPanel/Callnum/Text");
        _callPanImg = Find<Image>("CallPanel/Callnum/Img");
        _inputField = Find<InputField>("CallPanel/InputField");
        _cutBtn = Find<Button>("CallPanel/CutBtn");
        _addBtn = Find<Button>("CallPanel/AddBtn");
        _callBtn = Find<Button>("CallPanel/CallBtn");
        _wholeBtn = Find<Button>("WholeBtn");
        _parent = Find<RectTransform>("CallPanel/BagItem");

        _cutBtn.onClick.Add(OnCutnum);
        _addBtn.onClick.Add(OnAddnum);
        _callBtn.onClick.Add(OnCallSend);
        _wholeBtn.onClick.Add(OnHideBtn);

        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.FragmentCallOk, _callBtn.transform);

        _inputField.onValueChanged.Add(delegate { OnDele(); });
    }

    public void OnCall()
    {
        CallID = _cfg.ID;
        Existing = _itemView.mItemDataVO.mCount;
        Number = _cfg.ComposeNum;
        _callPanalText.text = (Existing + "/" + Number);
        _name.text = LanguageMgr.GetLanguage(_cfg.NameID);
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        _view = ItemFactory.Instance.CreateItemView(_itemView.mItemDataVO.mItemConfig, ItemViewType.ShopItem, OnClick);
        _view.mRectTransform.SetParent(_parent, false);
    }

    private void OnClick(ItemView view)
    {

    }

    private void OnCallNum()
    {
        if (Existing>= Number)
            _callPanalText.text = (Existing + "/" + (Number * Callnum));
        else
            _callPanalText.text= (Existing + "/" + Number);
    }

    private void OnCallPanImg()
    {
        if (Existing>= Number)
            _callPanImg.fillAmount = 1;
        else
            _callPanImg.fillAmount = (float)Existing / (float)Number;
    }

    private void OnDele()
    {
        if (_inputField.text != "")
        {
            if ((Existing / Number) >= int.Parse(_inputField.text))
            {
                if ((Existing / Number) < (GameConst.CardBagNum - HeroDataModel.Instance.mAllCards.Count))
                {
                    if (int.Parse(_inputField.text) >= 1)
                    {
                        Callnum = int.Parse(_inputField.text);
                    }
                    else
                    {
                        Callnum = 1;
                        OnPlader(Callnum);
                    }
                }
                else
                {
                    if ((GameConst.CardBagNum - HeroDataModel.Instance.mAllCards.Count) < int.Parse(_inputField.text))
                    {
                        Callnum = GameConst.CardBagNum - HeroDataModel.Instance.mAllCards.Count;
                        OnPlader(Callnum);
                    }
                    else
                    {
                        Callnum = int.Parse(_inputField.text);
                        OnPlader(Callnum);
                    }
                }
            }
            else
            {
                _inputField.text = (Existing / Number).ToString();
            }
        }
        OnCallNum();
    }

    private void OnPlader(int num)
    {
        _inputField.text = num.ToString();
    }

    private void OnCutnum()
    {
        if (Callnum <= 1)
            return;
        Callnum -= 1;
        OnCallNum();
        OnPlader(Callnum);
    }

    private void OnAddnum()
    {
        if ((Callnum + 1) * Number > Existing || (GameConst.CardBagNum - HeroDataModel.Instance.mAllCards.Count) <= Callnum)
            return;
        Callnum += 1;
        OnCallNum();
        OnPlader(Callnum);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _itemView = args[0] as ItemView;
        _cfg = _itemView.mItemDataVO.mItemConfig;
        OnCall();
        OnPlader(Existing / Number);
        OnCallPanImg();
    }
    
    private void OnCallSend()
    {
        GameNetMgr.Instance.mGameServer.ReqItemFusion(CallID, Callnum);
        Hide();
    }

    private void OnHideBtn()
    {
        Hide();
    }

    public override void Dispose()
    {
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        _view = null;
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.FragmentCallOk);
        base.Dispose();
    }
}
