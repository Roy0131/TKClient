using Framework.UI;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RarityItem : UIBaseView
{
    public int mRarity { get; private set; }
    public bool mBlSelected { get; private set; }
    
    private Action<RarityItem> _onRarityMethod;
    private Button _btn;
    private GameObject _flagObj;

    public RarityItem(Action<RarityItem> OnMethod)
    {
        _onRarityMethod = OnMethod;
        mBlSelected = false;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _btn = FindOnSelf<Button>();
        _flagObj = Find("FlagObject");
        string value = mDisplayObject.name.Substring(mDisplayObject.name.Length - 1, 1);
        mRarity = int.Parse(value);

        _btn.onClick.Add(OnClick);
    }

    private void OnClick()
    {
        mBlSelected = !mBlSelected;
        _flagObj.SetActive(mBlSelected);
        if (_onRarityMethod != null)
            _onRarityMethod.Invoke(this);
    }

    public void SetSelectedStatus(bool blSelected)
    {
        if (mBlSelected == blSelected)
            return;
        mBlSelected = blSelected;
        _flagObj.SetActive(mBlSelected);
    }

    public void Reset()
    {
        mBlSelected = false;
        _flagObj.SetActive(mBlSelected);
    }

    public override void Dispose()
    {
        _onRarityMethod = null;
        base.Dispose();
    }
}