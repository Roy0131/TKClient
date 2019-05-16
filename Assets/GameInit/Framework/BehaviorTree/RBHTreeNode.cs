#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2017 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： RBHTreeNode 
     * 创建人	： roy
     * 创建时间	： 2017/7/17 15:01:16 
     * 描述  	：   	
*/
#endregion

using System;
using System.Collections.Generic;

public class RBHTreeNode : IDisposable
{
    private List<RBHTreeNode> _lstChildren;
    private int _maxChildCount;
    public RBHTreeNode(int maxChildCount)
    {
        _lstChildren = new List<RBHTreeNode>();
        _maxChildCount = maxChildCount;
        if (_maxChildCount > 0)
            _lstChildren.Capacity = _maxChildCount;
    }

    public RBHTreeNode AddChild(RBHTreeNode childNode)
    {
        if (_lstChildren.Contains(childNode))
        {
            Debuger.LogError("[RBHTreeNode.AddChild() => 重复添加同一个节点！]");
            return this;
        }
        if (_maxChildCount > 0 && _lstChildren.Count >= _maxChildCount)
        {
            Debuger.LogError("[RBHTreeNode.AddChild() => 添加节点已达上限!]");
            return this;
        }
        _lstChildren.Add(childNode);
        return this;
    }

    public int GetChildCount()
    {
        return _lstChildren.Count;
    }

    public bool IsIndexValid(int index)
    {
        return index >= 0 && index < _lstChildren.Count;
    }

    public T GetChild<T>(int idx) where T : RBHTreeNode
    {
        if (!IsIndexValid(idx))
            return null;
        return (T)_lstChildren[idx];
    }

    public virtual void Dispose()
    {
        if (_lstChildren != null)
        {
            for (int i = _lstChildren.Count - 1; i >= 0; i--)
            {
                _lstChildren[i].Dispose();
                _lstChildren.RemoveAt(i);
            }
            _lstChildren = null;
        }
        _maxChildCount = 0;
    }
}