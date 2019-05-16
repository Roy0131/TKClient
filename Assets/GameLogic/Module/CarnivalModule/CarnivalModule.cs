using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CarnivalConst
{
    public const int Comment = 401;//评论
    public const int Attention = 402;//关注
    public const int Share = 403;//分享
    public const int InviteFriend = 404;//邀请好友
    public const int Exchange = 405;//兑换
    public const int InviteAward = 406;//邀请奖励
}

public class CarnivalModule : ModuleBase
{
    private Text _tips;
    private Transform _root;
    private Button _disBtn;
    private RectTransform _rectMove;
    private List<Toggle> _listTog;

    private CarnivalOneView _carnivalOneView;
    private CarnivalTwoView _carnivalTwoView;
    private UIBaseView _uiShowView;
    private int _activeType;


    public CarnivalModule()
        : base(ModuleID.Carnival, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_Carnival;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _tips = Find<Text>("Tips");
        _root = Find<Transform>("Left");
        _disBtn = Find<Button>("Btn_Back");
        _rectMove = Find<RectTransform>("Left/Move/ToggleGroup");

        _carnivalOneView = new CarnivalOneView();
        _carnivalOneView.SetDisplayObject(Find("Right/OneObj"));

        _carnivalTwoView = new CarnivalTwoView();
        _carnivalTwoView.SetDisplayObject(Find("Right/TwoObj"));

        _disBtn.onClick.Add(OnClose);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        CarnivalDataModel.Instance.AddEvent<CarnivalEventVO>(CarnivalEvent.CarnivalData, OnCarnivalData);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        CarnivalDataModel.Instance.RemoveEvent<CarnivalEventVO>(CarnivalEvent.CarnivalData, OnCarnivalData);
    }

    private void OnCarnivalData(CarnivalEventVO vo)
    {
        if (vo.mCarnivalVO.Count == 0)
        {
            _tips.gameObject.SetActive(true);
            return;
        }
        _tips.gameObject.SetActive(false);
        _listTog = new List<Toggle>();
        foreach (var item in vo.mCarnivalVO)
        {
            GameObject obj = GameObject.Instantiate(Find("Left/Move/ToggleGroup/Tog"));
            obj.transform.SetParent(_rectMove, false);
            obj.SetActive(true);
            Toggle tog = obj.transform.GetComponent<Toggle>();
            tog.transform.Find("Text").gameObject.GetComponent<Text>().text = GameConst.CarnivalName(item.Key);
            tog.onValueChanged.Add((bool blSelect) => { if (blSelect) OnCarnivalType(item.Key, item.Value); });
            _listTog.Add(tog);
        }
        _listTog[0].isOn = true;
    }

    private void OnCarnivalType(int id, List<CarnivalDataVO> listVO)
    {
        if (_uiShowView != null)
            _uiShowView.Hide();
        switch (id)
        {
            case CarnivalConst.Comment:
                _uiShowView = _carnivalOneView;
                break;
            case CarnivalConst.Attention:
                _uiShowView = _carnivalOneView;
                break;
            case CarnivalConst.Share:
                _uiShowView = _carnivalOneView;
                break;
            case CarnivalConst.InviteFriend:
                _uiShowView = _carnivalTwoView;
                break;
            case CarnivalConst.Exchange:
                _uiShowView = _carnivalTwoView;
                break;
            case CarnivalConst.InviteAward:
                _uiShowView = _carnivalOneView;
                break;
        }
        _uiShowView.Show(id, listVO);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _tips.text = LanguageMgr.GetLanguage(5007524);
        CarnivalDataModel.Instance.ReqCarnivalData();
    }

    public override void Hide()
    {
        _root.transform.localPosition = new Vector3(371, 0, 0);
        _rectMove.anchoredPosition = new Vector2(0f, 0f);
        OnTogDestroy();
        if (_uiShowView != null)
            _uiShowView.Hide();
        base.Hide();
    }

    private void OnTogDestroy()
    {
        if (_listTog != null)
        {
            for (int i = 0; i < _listTog.Count; i++)
                GameObject.Destroy(_listTog[i].gameObject);
            _listTog.Clear();
        }
    }

    public override void Dispose()
    {
        if (_carnivalOneView != null)
        {
            _carnivalOneView.Dispose();
            _carnivalOneView = null;
        }
        if (_carnivalTwoView != null)
        {
            _carnivalTwoView.Dispose();
            _carnivalTwoView = null;
        }
        _uiShowView = null;
        base.Dispose();
    }

    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        DelayCall(0.25f, () => ObjectHelper.AnimationMove(_root, new Vector3(371, 0, 0), new Vector3(0, 0, 0), 0.3f));
    }
}
