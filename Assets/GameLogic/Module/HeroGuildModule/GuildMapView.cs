using UnityEngine.UI;
using Framework.UI;

public class GuildMapView : UIBaseView
{
    private Button _bossBtn;
    private Button _talentBtn;
    private Button _shopBtn;
    private Button _guildBattleBtn;
    private Button _guildWarBtn;
    private Button _closeBtn;

    private Text _text01;
    private Text _text02;
    private Text _text03;
    private Text _text04;
    private Text _text05;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _bossBtn = Find<Button>("BossBtn/fuben");
        _talentBtn = Find<Button>("TalentBtn/keji");
        _shopBtn = Find<Button>("ShopBtn/shangdian");
        _guildBattleBtn = Find<Button>("GuildBattleBtn/skeleton");
        _guildWarBtn = Find<Button>("GuildwarBtn/shouwei");
        _text01 = Find<Text>("BossBtn/Image/TextTitle");
        _text02 = Find<Text>("TalentBtn/Image/TextTitle");
        _text03 = Find<Text>("ShopBtn/Image/TextTitle");
        _text04 = Find<Text>("GuildBattleBtn/Image/TextTitle");
        _text05 = Find<Text>("GuildwarBtn/Image/TextTitle");

        _text01.text = LanguageMgr.GetLanguage(5003137);
        _text02.text = LanguageMgr.GetLanguage(5003146);
        _text03.text = LanguageMgr.GetLanguage(5002105);
        _text04.text = LanguageMgr.GetLanguage(5003153);
        _text05.text = LanguageMgr.GetLanguage(5003154);

        _bossBtn.onClick.Add(OnShowBossMap);
        _talentBtn.onClick.Add(OnShowTalent);
        _shopBtn.onClick.Add(OnShowShop);
        _guildBattleBtn.onClick.Add(OnGuildWar);
        _guildWarBtn.onClick.Add(OnGuildWar);

        _closeBtn = Find<Button>("CloseBtn");
        _closeBtn.onClick.Add(Hide);

        ColliderHelper.SetButtonCollider(_closeBtn.transform, 120, 120);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mGlobalDispatcher.AddEvent(GuildEvent.ShowGuildBoss, OnShowGuildBoss);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mGlobalDispatcher.RemoveEvent(GuildEvent.ShowGuildBoss, OnShowGuildBoss);
    }

    private void OnShowGuildBoss()
    {
        GameUIMgr.Instance.OpenModule(ModuleID.GuildBoss,true);
    }

    private void OnShowBossMap()
    {
        GameUIMgr.Instance.OpenModule(ModuleID.GuildBoss,false);
    }

    private void OnShowTalent()
    {
        GameUIMgr.Instance.OpenModule(ModuleID.Talent, TalentTypeConst.Guild);
    }

    private void OnShowShop()
    {
        GameUIMgr.Instance.OpenModule(ModuleID.HeroShop, ShopIdConst.GUILDSHOP);
    }

    private void OnGuildWar()
    {
        PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000118));
    }
}