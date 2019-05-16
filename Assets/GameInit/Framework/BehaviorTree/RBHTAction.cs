#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2017 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： RBHTAction 
     * 创建人	： roy
     * 创建时间	： 2017/7/17 15:14:27 
     * 描述  	：   	
*/
#endregion


public class RBHTActionContext
{

}

public class RBHTAction : RBHTreeNode
{
    private RBHTActionContext _context;

    protected RBHTPrecondition _precondition;

    public RBHTAction(int maxChildCount)
        : base(maxChildCount)
    {

    }

    public bool Check(RBHTData data)
    {
        if (_precondition != null)
        {
            return _precondition.IsTrue(data) && DoCheck(data);
        }
        return DoCheck(data);
    }

    protected virtual bool DoCheck(RBHTData data)
    {
        return true;
    }

    public int Update(RBHTData data)
    {
        return DoUpdate(data);
    }

    protected virtual int DoUpdate(RBHTData data)
    {
        return RBHTStatus.FINISHED;
    }

    public void Transition(RBHTData data)
    {
        DoTransition(data);
    }

    protected virtual void DoTransition(RBHTData data)
    {

    }

    public RBHTAction SetPrecondition(RBHTPrecondition prcondition)
    {
        _precondition = prcondition;
        return this;
    }

    protected T GetContext<T>(RBHTData data) where T : RBHTActionContext, new()
    {
        if (_context == null)
            _context = new T();
        return (T)_context;
    }

    public override void Dispose()
    {
        _precondition = null;
        _context = null;
        base.Dispose();
    }
}