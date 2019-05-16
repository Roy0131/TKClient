using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class MailItemView : UIBaseView
{
    private Text _mailText;
    private Text _timeText;
    private Button _mailBtn;

    private GameObject _notObj;
    private GameObject _hasObj;
    private GameObject _notAnnexObj;
    private GameObject _hasAnnexObj;
    private GameObject _seleObj;
    private GameObject _seleAnnexObj;

    private bool _blSelected;


    public MailDataVO mMailDataVO { get; private set; }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _mailText = Find<Text>("MailText");
        _timeText = Find<Text>("TimeText");
        _mailBtn = Find<Button>("MailBtn");
        _notObj = Find("NotObj");
        _hasObj = Find("HasObj");
        _notAnnexObj = Find("NotAnnexObj");
        _hasAnnexObj = Find("HasAnnexObj");
        _seleObj = Find("SeleObj");
        _seleAnnexObj = Find("SeleAnnexObj");

        _mailBtn.onClick.Add(OnBreak);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        mMailDataVO = args[0] as MailDataVO;
        OnMail();
    }

    private void OnMail()
    {
        if (mMailDataVO.mMailBasicData.Type== MailTypeConst.SYSTEM)
            _mailText.text = LanguageMgr.GetLanguage(5002802);
        else
            _mailText.text = mMailDataVO.mMailBasicData.SenderName;
        _timeText.text = TimeHelper.GetTime(mMailDataVO.mMailBasicData.SendTime, "yyyy-MM-dd");
        _notObj.SetActive(mMailDataVO.mMailBasicData.IsRead == false && mMailDataVO.mMailBasicData.HasAttached == false && _blSelected == false);
        _hasObj.SetActive(mMailDataVO.mMailBasicData.IsRead == true && mMailDataVO.mMailBasicData.HasAttached == false && _blSelected == false);
        _notAnnexObj.SetActive(mMailDataVO.mMailBasicData.IsRead == false && mMailDataVO.mMailBasicData.HasAttached == true && _blSelected == false);
        _hasAnnexObj.SetActive(mMailDataVO.mMailBasicData.IsRead == true && mMailDataVO.mMailBasicData.HasAttached == true && _blSelected == false);
    }

    private void OnBreak()
    {
        OnMail();
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(MailEvent.ShowMailDetail, this);
    }

    public override void Hide()
    {
        base.Hide();
        _blSelected = false;
        mMailDataVO = null;
        _seleObj.SetActive(false);
        _seleAnnexObj.SetActive(false);
    }

    public bool BlSelected
    {
        get { return _blSelected; }
        set
        {
            if (_blSelected == value)
                return;
            _blSelected = value;
            _seleObj.SetActive(mMailDataVO.mMailBasicData.HasAttached == false && _blSelected == true);
            _seleAnnexObj.SetActive(mMailDataVO.mMailBasicData.HasAttached == true && _blSelected == true);
        }
    }
}