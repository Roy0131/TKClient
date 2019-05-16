using UnityEngine.UI;
using Framework.UI;
using System;

public class SubAndAddGroup : UIBaseView
{
    private Text _textCount;
    private Button _addBtn;
    private Button _subBtn;

    public int mCurValue { get; private set; }
    private int _maxValue;
    private Action _onValueChange;

    public SubAndAddGroup(Action onValueChange = null)
    {
        _onValueChange = onValueChange;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _textCount = Find<Text>("CountText");
        _addBtn = Find<Button>("ButtonAdd");
        _subBtn = Find<Button>("ButtonSub");
        mCurValue = 0;
        _maxValue = 0;

        _addBtn.onClick.Add(OnAdd);
        _subBtn.onClick.Add(OnSub);
    }

    public void Reset(int maxValue = 0, int curValue = 0)
    {
        mCurValue = curValue > maxValue ? maxValue : curValue;
        _maxValue = maxValue;
        _textCount.text = mCurValue.ToString();
    }

    private void OnAdd()
    {
        if (mCurValue >= _maxValue)
            return;
        mCurValue += 1;
        ValueChange();
    }

    private void ValueChange()
    {
        _textCount.text = mCurValue.ToString();
        if (_onValueChange != null)
            _onValueChange.Invoke();
    }

    private void OnSub()
    {
        if (mCurValue <= 0)
            return;
        mCurValue -= 1;
        ValueChange();
    }

    public override void Dispose()
    {
        _onValueChange = null;
        base.Dispose();
    }
}