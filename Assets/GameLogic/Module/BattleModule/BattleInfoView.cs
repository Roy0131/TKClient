using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Framework.UI;

public class BattleInfoView : UIBaseView
{
    private Toggle _damageToggle;
    private Toggle _healToggle;

    private RectTransform _leftItemRoot;
    private RectTransform _rightItemRoot;

    private GameObject _itemObject;
    private Button _closeBtn;
    private int _curToggleIndex = -1;

    private List<BattleInfoItemView> _lstHeroItems;
    private List<BattleInfoItemView> _lstTargetItems;
    protected override void ParseComponent()
    {
        base.ParseComponent();

        _leftItemRoot = Find<RectTransform>("ScrollViewLeft/Content");
        _rightItemRoot = Find<RectTransform>("ScrollViewRight/Content");
        _itemObject = Find("ImageItem");

        _damageToggle = Find<Toggle>("ToggleGroup/ToggleDamage");
        _healToggle = Find<Toggle>("ToggleGroup/ToggleHeal");
        _closeBtn = Find<Button>("ButtonClose");

        _damageToggle.onValueChanged.Add((bool value) => { OnToggleChange(0); });
        _healToggle.onValueChanged.Add((bool value) => { OnToggleChange(1); });

        _closeBtn.onClick.Add(delegate { GameEventMgr.Instance.mBattleDispatcher.DispathEvent(BattleEvent.HideBattleDetailView); });

        ColliderHelper.SetButtonCollider(_closeBtn.transform);
    }

    private void GenBattleItemView()
    {
        ClearStaticItemView();

        List<FighterStatisticVO> lstHeroItems = BattleDataModel.Instance.mlstHeroStatistDatas;

        _lstHeroItems = new List<BattleInfoItemView>();
        int i = 0;
        BattleInfoItemView view;
        for (i = 0; i < lstHeroItems.Count; i++)
        {
            view = new BattleInfoItemView();
            view.SetDisplayObject(GameObject.Instantiate(_itemObject));
            view.Show(lstHeroItems[i]);
            view.mRectTransform.SetParent(_leftItemRoot, false);
            _lstHeroItems.Add(view);
        }

        _lstTargetItems = new List<BattleInfoItemView>();
        List<FighterStatisticVO> targetItems = BattleDataModel.Instance.mlstTargetStatistDatas;
        for (i = 0; i < targetItems.Count; i++)
        {
            view = new BattleInfoItemView();
            view.SetDisplayObject(GameObject.Instantiate(_itemObject));
            view.Show(targetItems[i]);
            view.mRectTransform.SetParent(_rightItemRoot, false);
            _lstTargetItems.Add(view);
        }
        _damageToggle.isOn = true;
        OnToggleChange(0);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        GenBattleItemView();
    }

    private void OnToggleChange(int idx)
    {
        if (_curToggleIndex == idx)
            return;
        int i = 0;
        for (i = 0; i < _lstHeroItems.Count; i++)
            _lstHeroItems[i].ShowStaticData(idx == 0);
        for (i = 0; i < _lstTargetItems.Count; i++)
            _lstTargetItems[i].ShowStaticData(idx == 0);
    }

    private void ClearStaticItemView()
    {
        if (_lstHeroItems != null)
        {
            for (int i = _lstHeroItems.Count - 1; i >= 0; i--)
                _lstHeroItems[i].Dispose();
            _lstHeroItems.Clear();
            _lstHeroItems = null;
        }
        if (_lstTargetItems != null)
        {
            for (int i = _lstTargetItems.Count - 1; i >= 0; i--)
                _lstTargetItems[i].Dispose();
            _lstTargetItems.Clear();
            _lstTargetItems = null;
        }
    }

	public override void Dispose()
	{
        ClearStaticItemView();
        base.Dispose();
	}
}