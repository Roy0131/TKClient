using UnityEngine.UI;
using Framework.UI;

public class BagRoadView : UIBaseView
{
    private Button _jumpBtn;
    private Text _jumpText;
    private int _jumpId;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _jumpBtn = Find<Button>("JumpBtn");
        _jumpText = Find<Text>("JumpBtn/Text");

        _jumpBtn.onClick.Add(OnJump);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _jumpId = int.Parse(args[0].ToString());
        LinkListConfig cfg = GameConfigMgr.Instance.GetLinkListConfig(_jumpId);
        _jumpText.text = LanguageMgr.GetLanguage(cfg.Text);
    }

    private void OnJump()
    {
        JumpModule.JumpType((JumpType)_jumpId);
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(BagEvent.BagJump);
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
