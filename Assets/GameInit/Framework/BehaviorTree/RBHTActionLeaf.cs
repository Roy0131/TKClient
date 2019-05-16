#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2017 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： RBHTActionLeaf 
     * 创建人	： roy
     * 创建时间	： 2017/7/18 11:22:31 
     * 描述  	：   	
*/
#endregion


public class RBHTActionLeaf : RBHTAction
{
    private const int ACTION_READY = 0;
    private const int ACTION_RUNNING = 1;
    private const int ACTION_FINISHED = 2;

    class RBHTActionLeafContext : RBHTActionContext
    {
        internal int status;
        internal bool needExit;

        public RBHTActionLeafContext()
        {
            status = ACTION_READY;
            needExit = false;
        }
    }

    public RBHTActionLeaf()
        : base(0)
    {

    }

    protected sealed override int DoUpdate(RBHTData data)
    {
        int runningStatus = RBHTStatus.FINISHED;
        RBHTActionLeafContext context = GetContext<RBHTActionLeafContext>(data);
        if (context.status == ACTION_READY)
        {
            OnEnter(data);
            context.needExit = true;
            context.status = ACTION_RUNNING;
        }
        if (context.status == ACTION_RUNNING)
        {
            runningStatus = OnExecute(data);
            if (runningStatus == RBHTStatus.FINISHED)
                context.status = ACTION_FINISHED;
        }
        if (context.status == ACTION_FINISHED)
        {
            if (context.needExit)
                OnExit(data, runningStatus);
            context.status = ACTION_READY;
            context.needExit = false;
        }
        return runningStatus;
    }

    protected sealed override void DoTransition(RBHTData data)
    {
        RBHTActionLeafContext context = GetContext<RBHTActionLeafContext>(data);
        if (context.needExit)
            OnExit(data, RBHTStatus.TRANSITION);
        context.status = ACTION_READY;
        context.needExit = false;
    }

    protected virtual void OnEnter(RBHTData data)
    {

    }

    protected virtual int OnExecute(RBHTData data)
    {
        return RBHTStatus.FINISHED;
    }


    protected virtual void OnExit(RBHTData data, int runningStatus)
    {

    }
}