using UnityEngine.UI;
using Framework.UI;

public class RewardLoseView : UIBaseView
{
    private Button _btnEquipUp;
    private Button _btnHeroUp;
    private Button _btnGetHero;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _btnEquipUp = Find<Button>("ImageEquip");
        _btnHeroUp = Find<Button>("ImageHero");
        _btnGetHero = Find<Button>("ImageGet");

        _btnEquipUp.onClick.Add(() => GameUIMgr.Instance.OpenModule(ModuleID.Equipment));
        _btnHeroUp.onClick.Add(HeroUp);
        _btnGetHero.onClick.Add(() => RecruitDataModel.Instance.ReqDrawCardList());
    }

    private void HeroUp()
    {

        GameStageMgr.Instance.ChangeStage(StageType.Home);
        LineupSceneMgr.Instance.mBattleType = BattleType.None;
        //DelayCall(0.3f, () => GameUIMgr.Instance.OpenModule(ModuleID.RoleBag));
        GameUIMgr.Instance.OpenModule(ModuleID.RoleBag);

    }
}