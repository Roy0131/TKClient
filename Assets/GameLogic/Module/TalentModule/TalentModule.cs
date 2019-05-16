using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class TalentModule : ModuleBase
{
    private Button _disBtn;
    private Toggle[] _toggles;
    private TalentView _talentView;
    private GameObject _not;
    private int _talentType;
    private Transform _root;
    private ItemResGroup _resGroup;
    private Button _panel;
    private RectTransform _rectPanel;

    public TalentModule()
        : base(ModuleID.Talent, UILayer.Popup)
    {
        mBlStack = false;
        _modelResName = UIModuleResName.UI_Talent;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _disBtn = Find<Button>("CloseBtn");
        _panel = Find<Button>("Root/Panel");
        _rectPanel = Find<RectTransform>("Root/Panel");
        _rectPanel.offsetMin = new Vector2(595f, -30f);
        _rectPanel.offsetMax = new Vector2(580f, -20f);
        _talentView = new TalentView();
        _talentView.SetDisplayObject(Find("Root/TalentObj"));

        _resGroup = new ItemResGroup();
        _resGroup.SetDisplayObject(Find("uiResGroup"));
        _resGroup.Show(SpecialItemID.Talent, SpecialItemID.GuildCoin);

        _not = Find("Root/Not");
        _root = Find<Transform>("Root");
        _toggles = new Toggle[3];
        for (int i = 0; i < 3; i++)
            _toggles[i] = Find<Toggle>("Root/ToggleGroup/Tog" + (i + 1));
        foreach (Toggle tog in _toggles)
            tog.onValueChanged.Add((bool blSelect) => { if (blSelect) OnTalentTypeChange(tog); });

        _disBtn.onClick.Add(OnClose);
        _panel.onClick.Add(OnGuild);

        ColliderHelper.SetButtonCollider(_disBtn.transform);
    }

    private void OnTalentTypeChange(Toggle tog)
    {
        switch (tog.name)
        {
            case "Tog1":
                _not.SetActive(false);
                _talentType = TalentTypeConst.Basis;
                break;
            case "Tog2":
                _not.SetActive(false);
                _talentType = TalentTypeConst.Guild;
                break;
            case "Tog3":
                _not.SetActive(true);
                _talentType = TalentTypeConst.Not;
                break;
        }
        if (_talentType != TalentTypeConst.Not)
            _talentView.Show(_talentType);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _panel.gameObject.SetActive(HeroDataModel.Instance.mHeroInfoData.mGuildId == 0);
        _toggles[int.Parse(args[0].ToString()) - 1].isOn = true;
        OnTalentTypeChange(_toggles[int.Parse(args[0].ToString()) - 1]);
    }

    private void OnGuild()
    {
        PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001122));
    }

    public override void Dispose()
    {
        if (_talentView != null)
        {
            _talentView.Dispose();
            _talentView = null;
        }
        if (_resGroup != null)
        {
            _resGroup.Dispose();
            _resGroup = null;
        }
        base.Dispose();
    }

    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        ObjectHelper.PopAnimationLiner(_root);
    }
}
