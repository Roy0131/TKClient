using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class TalentItemView : UIBaseView
{
    public TalentVO _talentVO { get; private set; }
    private Text _talentLevel;
    private Image _skillImg;
    private Button _skillBtn;
    private GameObject _sele;
    private bool _blSelected;
    private ImageGray _gray;
    private ImageGray _grays;
    public bool mIsGray { get; private set; }
    private int _index;

    public TalentItemView(int index)
    {
        
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _talentLevel = Find<Text>("Text");
        _skillImg = Find<Image>("Skill");
        _skillBtn = Find<Button>("Btn");
        _sele = Find("Sele");
        _gray = Find<ImageGray>("Skill");
        _grays = Find<ImageGray>("Slot");

        _skillBtn.onClick.Add(OnSkillBtn);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _talentVO = args[0] as TalentVO;
        _index = int.Parse(args[1].ToString());
        _skillImg.sprite = GameResMgr.Instance.LoadItemIcon(_talentVO.mTalentIcon);
        if (_talentVO.mBlLearned)
            _talentLevel.text = _talentVO.mTalentLevel + "/" + _talentVO.mTalentMaxLevel;
        else
            _talentLevel.text = "0/" + _talentVO.mTalentMaxLevel;
        if (_talentVO.mTalengPreSkill > 0)
        {
            if (TalentDataModel.Instance.GetTalentVO(_talentVO.mTalentType, _talentVO.mTalengPreSkill).mTalentLevel >= _talentVO.mTalentPreSkillLevel)
            {
                mIsGray = false;
                _gray.SetNormal();
                _grays.SetNormal();
            }
            else
            {
                mIsGray = true;
                _gray.SetGray();
                _grays.SetGray();
            }
        }
        else
        {
            mIsGray = false;
            _gray.SetNormal();
            _grays.SetNormal();
        }
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(TalentEvent.TalentUnlocked, _index, mIsGray);
    }

    private void OnSkillBtn()
    {
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(TalentEvent.Click, this);
    }

    public bool BlSelected
    {
        get { return _blSelected; }
        set
        {
            if (_blSelected == value)
                return;
            _blSelected = value;
            _sele.SetActive(_blSelected == true);
        }
    }
}
