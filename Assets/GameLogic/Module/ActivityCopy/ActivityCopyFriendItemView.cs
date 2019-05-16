using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class ActivityCopyFriendItemView : UIBaseView
{
    private Text _level;
    private Text _name;
    private Text _time;
    private Image _headIcon;
    private Button _togBtn;
    private RectTransform _rectRole;
    private GameObject _seleObj;
    private CardView _card;
    public CardDataVO _vo { get; private set; }
    private bool _blSelected;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _level = Find<Text>("ImageLevelBack/Text");
        _name = Find<Text>("TextName");
        _time = Find<Text>("TextTime");
        _headIcon = Find<Image>("ImageIcon");
        _togBtn = Find<Button>("Toggle");
        _rectRole = Find<RectTransform>("ImageHero");
        _seleObj = Find("Toggle/Checkmark");

        _togBtn.onClick.Add(OnSeleBtn);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _vo = args[0] as CardDataVO;

        _card = CardViewFactory.Instance.CreateCardView(_vo, CardViewType.Common);
        _card.mRectTransform.SetParent(_rectRole, false);
        List<FriendDataVO> lstFriendVo = FriendDataModel.Instance.mlstAllFriends.ToList();
        for (int i = 0; i < lstFriendVo.Count; i++)
        {
            if (lstFriendVo[i].mPlayerId == _vo.mPlayerId)
            {
                _level.text = lstFriendVo[i].mPlayerLevel.ToString();
                _name.text = lstFriendVo[i].mPlayerName;
                _time.text = TimeHelper.FormatTimeBySecond(lstFriendVo[i].mOfflineTime);
                if (lstFriendVo[i].mPlayerIcon > 0)
                {
                    _headIcon.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(lstFriendVo[i].mPlayerIcon).Icon);
                    ObjectHelper.SetSprite(_headIcon, _headIcon.sprite);
                }
                else
                {
                    _headIcon.sprite = null;
                }
            }
        }
    }

    private void OnSeleBtn()
    {
        BlSelected = !BlSelected;
        if (!BlSelected)
        {
            ActivityCopyDataModel.Instance.CurAssistCardDataVO = null;
            ActivityCopyDataModel.Instance.AssistFriendID = 0;
        }
        if (BlSelected)
            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(ActivityCopyEvent.FriendSele, _vo.mPlayerId);
    }

    public override void Hide()
    {
        if (_card != null)
        {
            CardViewFactory.Instance.ReturnCardView(_card);
            _card = null;
        }
        BlSelected = false;
        base.Hide();
    }

    public override void Dispose()
    {
        if (_card != null)
        {
            CardViewFactory.Instance.ReturnCardView(_card);
            _card = null;
        }
        if (_vo != null)
            _vo = null;
        base.Dispose();
    }

    public bool BlSelected
    {
        get { return _blSelected; }
        set
        {
            if (_blSelected == value)
                return;
            _blSelected = value;
            _seleObj.SetActive(_blSelected);
            if (_blSelected)
            {
                ActivityCopyDataModel.Instance.CurAssistCardDataVO = _vo;
                ActivityCopyDataModel.Instance.AssistFriendID = _vo.mPlayerId;
            }
        }
    }
}