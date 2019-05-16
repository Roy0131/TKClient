using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSceneMgr : Singleton<TowerSceneMgr>
{
    private GameObject _sceneRoot;
    private List<Transform> _lstFighterRoots;
    private Transform _newFighterRoot;
    private Dictionary<int, LineupFighter> _dictAllFighters;
    public Camera mLineupCamera { get; private set; }
    public void Init()
    {
        _sceneRoot = GameStageMgr.Instance.GetStageByType<HomeStage>(StageType.Home).mTowerRolePreview;
        _lstFighterRoots = new List<Transform>();
        _newFighterRoot = _sceneRoot.transform.Find("pn");
        mLineupCamera = _sceneRoot.transform.Find("Camera").GetComponent<Camera>();
        for (int i = 0; i < 9; i++)
            _lstFighterRoots.Add(_sceneRoot.transform.Find("FighterRoot/p" + (i + 1)));
    }
    private LineupFighter CreateFighter(int posIndex, CardDataVO vo)
    {
        LineupFighter fighter = new LineupFighter();
        fighter.InitData(vo);
        Transform parent = _lstFighterRoots[posIndex];
        fighter.AddToStage(parent);
        return fighter;
    }
}
