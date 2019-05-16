using UnityEngine;
using System.Collections.Generic;

public class HangupScene : RTLogicBase
{
    private SpriteRenderer _mapRender;
    private List<Transform> _lstOwnerParents;

    private List<Vector3> _lstDefaultPos; 
	private Vector3 _targetDefPos;

    public Transform mBulletParent { get; private set; }
    public Transform mTargeterParent { get; private set; }

    public HangupScene()
        : base(RoleRTType.Hangup)
    {
    }

    protected override void OnInit()
    {
        base.OnInit();
        _lstOwnerParents = new List<Transform>();
        _lstDefaultPos = new List<Vector3>();
        _mapRender = _rtRootObject.transform.Find("Map").GetComponent<SpriteRenderer>();
        for (int i = 1; i <= 3; i++)
        {
            _lstOwnerParents.Add(_rtRootObject.transform.Find("LeftRoot/p" + i));
            _lstDefaultPos.Add(_lstOwnerParents[i - 1].localPosition);
        }

        mTargeterParent = _rtRootObject.transform.Find("RightRoot/p1");
        _targetDefPos = mTargeterParent.localPosition;
        mBulletParent = _rtRootObject.transform.Find("BulletRoot");
    }

    public void ResetLocalPosition(int stageID = 0)
    {
        mTargeterParent.localPosition = _targetDefPos;
        for (int i = 0; i < _lstOwnerParents.Count; i++)
            _lstOwnerParents[i].localPosition = _lstDefaultPos[i];
        string mapImage = "Map_Battle_jdc";
        if(stageID != 0)
        {
            StageConfig stageCfg = GameConfigMgr.Instance.GetStageConfig(stageID);
            mapImage = stageCfg.BackGroundMap;
        }
        GameResMgr.Instance.LoadMapImage(mapImage, (spr) => { _mapRender.sprite = spr; });
    }

    public Transform GetOwnerParentByIndex(int idx)
    {
        if (idx < 0 || idx >= _lstOwnerParents.Count)
            return null;
        return _lstOwnerParents[idx];
    }

    public override void Dispose()
    {
        if (_lstOwnerParents != null)
        {
            _lstOwnerParents.Clear();
            _lstOwnerParents = null;
        }
        if(_lstDefaultPos != null)
        {
            _lstDefaultPos.Clear();
            _lstDefaultPos = null;
        }
        mBulletParent = null;
        mTargeterParent = null;
        base.Dispose();
    }
}