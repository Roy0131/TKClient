using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class PlayerModule : ModuleBase
{
    private Button _disBtn;
    private Transform _root;

    public PlayerModule()
        : base(ModuleID.Player, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_Player;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _disBtn = Find<Button>("Root/Btn_Back");

        PlayerView _playerView = new PlayerView();
        _playerView.SetDisplayObject(Find("Root/PlayerInfo"));
        AddChildren(_playerView);

        _root = Find<Transform>("Root");

        _disBtn.onClick.Add(OnClose);

        ColliderHelper.SetButtonCollider(_disBtn.transform);
    }

    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        ObjectHelper.PopAnimationLiner(_root);
    }
}