using Framework.UI;
using UnityEngine.UI;
using System;

public class FriendBossSweepView : UIBaseView
{
    private SubAndAddGroup _subAndAddGroup;
    private Button _closeBtn;
    private Button _sweepBtn;

    private Action<int> _onSweepMethod;
    private Text _strengthText;
    public FriendBossSweepView(Action<int> sweepMethod)
    {
        _onSweepMethod = sweepMethod;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _subAndAddGroup = new SubAndAddGroup();
        _subAndAddGroup.SetDisplayObject(Find("SubAndAdd"));

        _strengthText = Find<Text>("Strength/TextNum");
        _closeBtn = Find<Button>("BtnClose");
        _sweepBtn = Find<Button>("SweepBtn");

        _sweepBtn.onClick.Add(OnSweep);
        _closeBtn.onClick.Add(Hide);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        int count = BagDataModel.Instance.GetItemCountById(SpecialItemID.FriendStrength);
        count = count > GameConst.FriendBossStrengthMax ? GameConst.FriendBossStrengthMax : count;
        _subAndAddGroup.Reset(count, 1);
        _strengthText.text = count + "/" + GameConst.FriendBossStrengthMax;
    }

    private void OnSweep()
    {
        if (_onSweepMethod == null)
            return;
        _onSweepMethod.Invoke(_subAndAddGroup.mCurValue);
    }

    public override void Dispose()
    {
        _onSweepMethod = null;
        base.Dispose();
    }
}
