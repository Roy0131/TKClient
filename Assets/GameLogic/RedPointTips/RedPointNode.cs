using UnityEngine;
using System.Collections.Generic;

public class RedPointNode
{
    private RedPointEnum _redPointType;
    private List<RedPointNode> _lstChildrens;
    private List<GameObject> _lstRedObjects;

    public RedPointEnum mRedPointID { get; private set; }
    public RedPointNode mParentNode { get; private set; }
    public int mRedPointIntID { get; private set; }
    public RedPointNode(int pointID, bool blRedShow = false)
    {
        mRedPointIntID = pointID;
        Init(blRedShow);
    }

    private void Init(bool blRedShow = false)
    {
        _lstChildrens = new List<RedPointNode>();
        _lstRedObjects = new List<GameObject>();
        _blRedShow = blRedShow;
    }

    public RedPointNode(RedPointEnum redPointID, bool blRedShow = false)
    {
        mRedPointID = redPointID;
        Init(blRedShow);
    }

    public void AddChildren(RedPointNode children)
    {
        if (_lstChildrens.Contains(children))
        {
            LogHelper.LogWarning("[RedPointNode.AddChildren() => add children repeated!!!]");
            return;
        }
        _lstChildrens.Add(children);
        children.mParentNode = this;
        UpdateRedStates();
    }

    private void UpdateRedStates()
    {
        int i;
        if (_lstChildrens.Count > 0)
        {
            bool blValue = false;
            for (i = 0; i < _lstChildrens.Count; i++)
                blValue |= _lstChildrens[i].mBlRedShow;
            if (blValue != _blRedShow)
                _blRedShow = blValue;
        }
        for (i = 0; i < _lstRedObjects.Count; i++)
            _lstRedObjects[i].SetActive(mBlRedShow);
        if (mParentNode != null)
            mParentNode.UpdateRedStates();
    }

    private bool _blRedShow;
    public bool mBlRedShow
    {
        get { return _blRedShow; }
        set
        {
            if (_blRedShow == value)
                return;
            _blRedShow = value;
            UpdateRedStates();
        }
    }

    public void DynamicChildNodeUpdate(int childID, bool blShow)
    {
        RedPointNode childNode = GetChildrenByID(childID);
        if (childNode == null)
        {
            LogHelper.LogError("[RedPointNode.DynamicChildNodeUpdate() => child id:" + childID + " not found!!]");
            return;
        }
        childNode.mBlRedShow = blShow;
    }

    public void DynamicCreateChildNode(int childId, bool blShow = false)
    {
        RedPointNode childNode = GetChildrenByID(childId);
        if (childNode != null)
        {
            LogHelper.LogWarning("[RedPointNode.DynamicCreateChildNode() => childId: " + childId + " was created!!!]");
            return;
        }
        childNode = new RedPointNode(childId, blShow);
        AddChildren(childNode);
    }

    public void BindRedObject(GameObject gameObject)
    {
        if (_lstRedObjects.Contains(gameObject))
        {
            LogHelper.LogWarning("[RedPointNode.BindRedObject() => binding red point gameobject repeated!!!]");
            return;
        }
        _lstRedObjects.Add(gameObject);
        gameObject.SetActive(_blRedShow);
    }

    public void UnBindRedObject(GameObject gameObject)
    {
        if (_lstRedObjects.Contains(gameObject))
            _lstRedObjects.Remove(gameObject);
    }

    public void DynamicBindChildObject(int childrenID, GameObject redObject = null)
    {
        RedPointNode childNode = GetChildrenByID(childrenID);
        if (childNode == null)
        {
            LogHelper.LogError("[RedPointNode.DynamicBindChildObject() => child node found!!!]");
            return;
        }
        childNode.BindRedObject(redObject);
    }

    public void DynamicUnBindChildObject(int childrenID, GameObject redObject)
    {
        RedPointNode childNode = GetChildrenByID(childrenID);
        if (childNode == null)
        {
            LogHelper.LogError("[RedPointNode.DynamicUnBindChild() => child node not found!!!]");
            return;
        }
        childNode.UnBindRedObject(redObject);
    }

    private RedPointNode GetChildrenByID(int childrenID)
    {
        for (int i = 0; i < _lstChildrens.Count; i++)
        {
            if (_lstChildrens[i].mRedPointIntID == childrenID)
                return _lstChildrens[i];
        }
        return null;
    }
}