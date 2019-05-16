using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class RankModule : ModuleBase
{
    private Toggle[] _toggles;
    private Button _disBtn;
    private GameObject _rankPanl;
    private RankView _rankView;
    private int _curRankType;
    private Transform _root;

    public RankModule()
        : base(ModuleID.Rank, UILayer.Popup)
    {
        _modelResName = UIModuleResName.UI_Rank;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _toggles = new Toggle[5];
        for (int i = 0; i < 5; i++)
            _toggles[i] = Find<Toggle>("Root/ToggleGroup/Tog" + (i + 1));
        foreach (Toggle tog in _toggles)
            tog.onValueChanged.Add((bool blSelect) => { if (blSelect) OnRankTypeChange(tog); });

        _disBtn = Find<Button>("Btn_Back");
        _rankPanl = Find("Root/RankPanl");

        _rankView = new RankView();
        _rankView.SetDisplayObject(Find("Root/RankObj"));
        AddChildren(_rankView);
        _root = Find<Transform>("Root");

        _disBtn.onClick.Add(OnClose);

        ColliderHelper.SetButtonCollider(_disBtn.transform, 120, 120);
    }

    private void OnRankTypeChange(Toggle tog)
    {
        switch (tog.name)
        {
            case "Tog1":
                _rankPanl.SetActive(false);
                _curRankType = RankTypeConst.Arena;
                RankDataModel.Instance.ReqRankData(_curRankType);
                break;
            case "Tog2":
                _rankPanl.SetActive(false);
                _curRankType = RankTypeConst.Points;
                RankDataModel.Instance.ReqRankData(_curRankType);
                break;
            case "Tog3":
                _rankPanl.SetActive(false);
                _curRankType = RankTypeConst.ComBat;
                RankDataModel.Instance.ReqRankData(_curRankType);
                break;
            case "Tog4":
                _rankPanl.SetActive(true);
                //_curRankType = RankTypeConst.Guild;
                //RankDataModel.Instance.ReqRankData(_curRankType);
                break;
            case "Tog5":
                _rankPanl.SetActive(true);
                //_curRankType = RankTypeConst.Artifact;
                //RankDataModel.Instance.ReqRankData(_curRankType);
                break;
        }
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        OnRankTypeChange(_toggles[1]);
    }

    public override void Hide()
    {
        _toggles[1].isOn = true;
        base.Hide();
    }

    public override void Dispose()
    {
        if (_rankView != null)
        {
            _rankView.Dispose();
            _rankView = null;
        }
        base.Dispose();
    }
    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        ObjectHelper.PopAnimationLiner(_root);
    }
}
