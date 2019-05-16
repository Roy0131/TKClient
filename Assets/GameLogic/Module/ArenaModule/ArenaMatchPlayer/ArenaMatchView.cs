using UnityEngine.UI;
using Msg.ClientMessage;
using UnityEngine;
using Framework.UI;

public class ArenaMatchView : UIBaseView
{
    private Button _btnBattle;
    private Button _btnCancel;

    private Image _selfIcon;
    private Image _selfGradIcon;
    private Text _selfName;
    private Text _selfPower;
    private Text _selfGrade;

    private Image _targeterIcon;
    private Image _targeterGradIcon;
    private Text _targeterName;
    private Text _targeterPower;
    private Text _targeterGrade;

    private GameObject _speciallyObj;
    private UIEffectView _effect;
    private UIEffectView _effect02;
    private UIEffectView _effect03;

    private int _targetPlayerID;
    private S2CArenaMatchPlayerResponse vo;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _btnBattle = Find<Button>("Root/Buttons/BtnBattle");
        _btnCancel = Find<Button>("Root/Buttons/BtnCancel");

        _selfIcon = Find<Image>("Root/Role1/RoleIconback/RoleIcon");
        _selfGradIcon = Find<Image>("Root/Role1/RankNameIcon");
        _selfName = Find<Text>("Root/Role1/RoleName");
        _selfPower = Find<Text>("Root/Role1/PowerText");
        _selfGrade = Find<Text>("Root/Role1/RankNameIcon/RankName");

        _targeterIcon = Find<Image>("Root/Role2/RoleIconback/RoleIcon");
        _targeterGradIcon = Find<Image>("Root/Role2/RankNameIcon");
        _targeterName = Find<Text>("Root/Role2/RoleName");
        _targeterPower = Find<Text>("Root/Role2/PowerText");
        _targeterGrade = Find<Text>("Root/Role2/RankNameIcon/RankName");

        _speciallyObj = Find("Root/fx_ui_jingjichang");
        _effect = CreateUIEffect(_speciallyObj, UILayerSort.WindowSortBeginner + 3);

        _effect02 = CreateUIEffect(Find("Root/Role1"), UILayerSort.WindowSortBeginner + 4,false,SortObjType.Canvas);
        _effect03 = CreateUIEffect(Find("Root/Role2"), UILayerSort.WindowSortBeginner + 4,false,SortObjType.Canvas);


        _btnBattle.onClick.Add(OnBattle);
        _btnCancel.onClick.Add(OnCancel);

        NewBieGuide.NewBieGuideMgr.Instance.RegistMaskTransform(NewBieGuide.NewBieMaskID.ArenaMatchBattleBtn, _btnBattle.transform);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        vo = args[0] as S2CArenaMatchPlayerResponse;
        DelayCall(0.3f,DisRoot);
        SoundMgr.Instance.PlayEffectSound("UI_jingjichang_Battle");
    }

    private void OnBattle()
    {
        LineupSceneMgr.Instance.ShowLineupModule(TeamType.Arena, _targetPlayerID);
    }

    private void OnCancel()
    {
        if (LocalDataMgr.ArenaCancelBattleAlert)
        {
            ConfirmTipsMgr.Instance.ShowConfirmTips(LanguageMgr.GetLanguage(6001110), OnAlertBack, true);
        }
        else
        {
            Hide();
            _effect.StopEffect();
            _effect02.StopEffect();
            _effect03.StopEffect();
        }
    }

    private void OnAlertBack(bool value, bool blShowAgain)
    {
        if (value)
        {
            Hide();
            _effect.StopEffect();
            _effect02.StopEffect();
            _effect03.StopEffect();
        }

    }


    private void DisRoot()
    {
        _effect.PlayEffect();
        _effect02.PlayEffect();
        _effect03.PlayEffect();
        _selfName.text = HeroDataModel.Instance.mHeroInfoData.mHeroName;
        ArenaDivisionConfig config = GameConfigMgr.Instance.GetArenaDivisionConfig(ArenaDataModel.Instance.mArenaDataVO.mGrade);
        if (config != null)
            _selfGrade.text = LanguageMgr.GetLanguage(config.Name);
        _selfPower.text = HeroDataModel.Instance.GetBattlePowerByTeamType(TeamType.Arena).ToString();

        ArenaDivisionConfig config01 = GameConfigMgr.Instance.GetArenaDivisionConfig(ArenaDataModel.Instance.mlstRankItem[0].PlayerArenaGrade);
        ArenaDivisionConfig config02 = GameConfigMgr.Instance.GetArenaDivisionConfig(ArenaDataModel.Instance.mlstRankItem[0].PlayerArenaGrade);

        _selfGradIcon.sprite = GameResMgr.Instance.LoadItemIcon(config01.Icon);
        ObjectHelper.SetSprite(_selfGradIcon, _selfGradIcon.sprite);

        _targeterGradIcon.sprite = GameResMgr.Instance.LoadItemIcon(config02.Icon);
        ObjectHelper.SetSprite(_targeterGradIcon, _targeterGradIcon.sprite);

        _targetPlayerID = vo.PlayerId;
        _targeterName.text = vo.PlayerName;
        config = GameConfigMgr.Instance.GetArenaDivisionConfig(vo.PlayerGrade);
        if (config != null)
            _targeterGrade.text = LanguageMgr.GetLanguage(config.Name);
        _targeterPower.text = vo.PlayerPower.ToString();
        if (HeroDataModel.Instance.mHeroInfoData.mIcon > 0)
        {
            _selfIcon.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(HeroDataModel.Instance.mHeroInfoData.mIcon).Icon);
            ObjectHelper.SetSprite(_selfIcon,_selfIcon.sprite);
        }
        if (vo.PlayerHead > 0)
        {
            _targeterIcon.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(vo.PlayerHead).Icon);
            ObjectHelper.SetSprite(_targeterIcon,_targeterIcon.sprite);
        }
    }

    public override void Hide()
    {
        base.Hide();
        _effect.StopEffect();
        _effect02.StopEffect();
        _effect03.StopEffect();
    }
}