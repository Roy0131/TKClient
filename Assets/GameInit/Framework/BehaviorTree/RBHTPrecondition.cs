#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2017 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： RBHTPrecondition 
     * 创建人	： roy
     * 创建时间	： 2017/7/17 15:17:23 
     * 描述  	：   	
*/
#endregion


public class RBHTData
{
    public T As<T>() where T : RBHTData
    {
        return (T)this;
    }
}

public abstract class RBHTPrecondition : RBHTreeNode
{
    public RBHTPrecondition(int maxChildCount)
        : base(maxChildCount)
    {}

    public abstract bool IsTrue(RBHTData data);
}

public abstract class RBHTPreconditionLeaf : RBHTPrecondition
{
    public RBHTPreconditionLeaf()
        : base(0)
    {

    }
}