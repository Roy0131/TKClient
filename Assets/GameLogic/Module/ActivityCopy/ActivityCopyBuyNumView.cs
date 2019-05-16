using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class ActivityCopyBuyNumView : UIBaseView
{
    private Button _btnBuy;
    private Text _diamondNum;
    private Transform _root;
    private Button _btnBack;

    private Button _btnAdd;
    private Button _btnSub;
    private Text _textNum;

    private int _num = 1;
    private int _remainBuyNum;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _btnBuy = Find<Button>("Root/ButtonBuy");
        _diamondNum = Find<Text>("Root/ImageBack02/TextNum");
        _root = Find<Transform>("Root");
        _btnBack = Find<Button>("BlackBack");

        _btnAdd = Find<Button>("Root/BuyTicket/ButtonAdd");
        _btnSub = Find<Button>("Root/BuyTicket/ButtonSub");
        _textNum = Find<Text>("Root/BuyTicket/InputField/TextNum");

        _btnBuy.onClick.Add(OnBuy);
        _btnBack.onClick.Add(Hide);

        _btnAdd.onClick.Add(AddNum);
        _btnSub.onClick.Add(SubNum);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        ActivityCopyDataModel.Instance.AddEvent(ActivityCopyEvent.RemainBuyRefresh, Hide);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        ActivityCopyDataModel.Instance.RemoveEvent(ActivityCopyEvent.RemainBuyRefresh, Hide);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _remainBuyNum = (int)args[0];
        DiamondNum();
    }

    private void DiamondNum()
    {
        _textNum.text = _num.ToString();
        _diamondNum.text = HeroDataModel.Instance.mHeroInfoData.mDiamond + "/" + ActivityCopyDataModel.Instance.mChallengeNumPrice * _num;
    }

    private void AddNum()
    {
        if (_num < _remainBuyNum)
        {
            _num++;
            DiamondNum();
        }
    }
    private void SubNum()
    {
        if (_num > 1)
        {
            _num--;
            DiamondNum();
        }
    }
    private void OnBuy()
    {
        GameNetMgr.Instance.mGameServer.ReqActiveStageBuyChallengeNumData(ActivityCopyDataModel.Instance._curType, _num);
        TDPostDataMgr.Instance.DoCostDiamond(TDCostDiamondType.BuyActivityStage, _num, ActivityCopyDataModel.Instance.mChallengeNumPrice);
    }

    public override void Hide()
    {
        _num = 1;
        base.Hide();
    }
}
