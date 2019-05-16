using System.Collections.Generic;
using UnityEngine;

public class LineupScene : RTLogicBase
{
    public LineupScene()
        : base(RoleRTType.Lineup)
    {
    }
    #region lineup status logic
    private List<Transform> _lstFighterRoots;
    private Transform _newFighterRoot;
    private GameObject _lineupObject;

    public Transform GetFighterParentByIndex(int index)
    {
        if (index < 0 || index >= _lstFighterRoots.Count)
            return null;
        return _lstFighterRoots[index];
    }

    public Transform NewFighterRoot
    {
        get { return _newFighterRoot; }
    }
    #endregion

    #region hangup module show battle status
    private List<Transform> _leftFighterRoot;
    private List<Transform> _rightFighterRoot;
    private GameObject _hangupBattleObject;

    public void SetHangUpStatus(bool blBattleStatus)
    {
        _lineupObject.SetActive(!blBattleStatus);
        _hangupBattleObject.SetActive(blBattleStatus);
    }

    public Transform GetBattlePosByIdxAndSide(int pos, bool blLeft = false)
    {
        List<Transform> tmp = blLeft ? _leftFighterRoot : _rightFighterRoot;
        if (pos < 0 || pos >= tmp.Count)
            return null;
        return tmp[pos];
    }

    #endregion
    protected override void OnInit()
    {
        base.OnInit();
        _lstFighterRoots = new List<Transform>();
        _lineupObject = _rtRootObject.transform.Find("LineupStatus").gameObject;
        _newFighterRoot = _rtRootObject.transform.Find("LineupStatus/pn");

        _leftFighterRoot = new List<Transform>();
        _rightFighterRoot = new List<Transform>();
        _hangupBattleObject = _rtRootObject.transform.Find("BattleStatus").gameObject;

        int idx;
        for (int i = 0; i < 9; i++)
        {
            idx = i + 1;
            _lstFighterRoots.Add(_rtRootObject.transform.Find("LineupStatus/FighterRoot/p" + idx));

            _leftFighterRoot.Add(_rtRootObject.transform.Find("BattleStatus/LeftRoot/p" + idx));
            _rightFighterRoot.Add(_rtRootObject.transform.Find("BattleStatus/RightRoot/p" + idx));
        }
    }
    

    public override void Dispose()
    {
        base.Dispose();
        if (_lstFighterRoots != null)
        {
            _lstFighterRoots.Clear();
            _lstFighterRoots = null;
        }
        if (_leftFighterRoot != null)
        {
            _leftFighterRoot.Clear();
            _leftFighterRoot = null;
        }
        if (_rightFighterRoot != null)
        {
            _rightFighterRoot.Clear();
            _rightFighterRoot = null;
        }
        _newFighterRoot = null;
    }
}

