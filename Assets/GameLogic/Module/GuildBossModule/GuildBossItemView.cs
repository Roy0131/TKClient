using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class GuildBossItemView : UIBaseView
{
    private Text _copyNum;
    private Image _byImg;
    private Image _doImg;
    private Button _byBtn;
    private Button _doBtn;
    private Button _waitBtn;
    private GameObject _byObj;
    private GameObject _doObj;
    private GameObject _waitObj;

    private ImageGray _gray;
    private ImageGray _grays;

    private int _bossId;
    private int _curBossId;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _copyNum = Find<Text>("CopyNum");
        _byImg = Find<Image>("ByObj/Img");
        _doImg = Find<Image>("DoObj/Img");
        _byBtn = Find<Button>("ByObj/Img");
        _doBtn = Find<Button>("DoObj/Img");
        _waitBtn = Find<Button>("WaitObj/Img");
        _byObj = Find("ByObj");
        _doObj = Find("DoObj");
        _waitObj = Find("WaitObj");

        _gray = Find<ImageGray>("ByObj/Img");
        _grays = Find<ImageGray>("ByObj/BJ");

        _byBtn.onClick.Add(OnBy);
        _doBtn.onClick.Add(OnDo);
        _waitBtn.onClick.Add(OnWait);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _bossId = (int.Parse(args[0].ToString()) + 1);
        _curBossId = int.Parse(args[1].ToString());
        _copyNum.text = _bossId.ToString();
        if (_curBossId < 0)
        {
            _byObj.SetActive(true);
            _doObj.SetActive(false);
            _waitObj.SetActive(false);
        }
        else
        {
            _byObj.SetActive(_bossId < _curBossId);
            _doObj.SetActive(_bossId == _curBossId);
            _waitObj.SetActive(_bossId > _curBossId);
            GuildBossConfig cfg2 = GameConfigMgr.Instance.GetGuildBossConfig(_curBossId);
            _doImg.sprite = GameResMgr.Instance.LoadItemIcon(cfg2.Image);
        }
        GuildBossConfig cfg1 = GameConfigMgr.Instance.GetGuildBossConfig(_bossId);
        _byImg.sprite = GameResMgr.Instance.LoadItemIcon(cfg1.Image);
        ObjectHelper.SetSprite(_byImg,_byImg.sprite);
        if (_bossId < _curBossId)
        {
            _gray.SetGray();
            _grays.SetGray();
        }
        else
        {
            _gray.SetNormal();
            _grays.SetNormal();
        }
    }

    private void OnBy()
    {
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(GuildEvent.ShowHurt, _bossId);
    }

    private void OnDo()
    {
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(GuildEvent.ReqCurBoss);
    }

    private void OnWait()
    {
        PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001147));
    }
}
