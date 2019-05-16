using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class LanternView : UIBaseView
{
    private Text _pmdText;
    private RectTransform _rectGroup;
    private RectTransform _rectImg;

    private float _speed = 100f;    //滚动速度
    private float _txetWidth = 0;
    private float _duration = 0;

    private uint _time = 0;
    private int _endTime = 0;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _pmdText = Find<Text>("LanternImg/Mask/Group/Text");
        _rectGroup = Find<RectTransform>("LanternImg/Mask/Group");
        _rectImg = Find<RectTransform>("LanternImg");
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(HeroEvent.SwitchLanguage, OnSwitchLanguage);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(HeroEvent.SwitchLanguage, OnSwitchLanguage);
    }

    private void OnSwitchLanguage()
    {
        if (_endTime > 0)
            ChatModel.Instance.OnJsonDataLanguage(ChatModel.Instance._allMonsters);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _pmdText.text = args[0].ToString();

        _txetWidth = _rectImg.sizeDelta.x + _pmdText.preferredWidth;
        _duration = _txetWidth / _speed;
        DGHelper.DoLocalMoveX(_rectGroup, _rectImg.sizeDelta.x / 2, 0f);
        DGHelper.DoLocalMoveX(_rectGroup, -_txetWidth, _duration);

        _endTime = ChatModel.Instance.AnnouncementTime;
        if (_time != 0)
            TimerHeap.DelTimer(_time);
        int interval = 1000;
        _time = TimerHeap.AddTimer(0, interval, OnAddTime);
    }

    private void OnAddTime()
    {
        if (_rectGroup.anchoredPosition.x < -_txetWidth)
        {
            DGHelper.DoLocalMoveX(_rectGroup, _rectImg.sizeDelta.x / 2, 0f);
            DGHelper.DoLocalMoveX(_rectGroup, -_txetWidth, _duration);
        }
        _endTime--;
        if (_endTime < 0)
            LanternMgr.Instance.LanternHide();
    }

    public void OnHide()
    {
        _rectImg.gameObject.SetActive(false);
    }

    public void OnShow()
    {
        _rectImg.gameObject.SetActive(true);
    }
}
