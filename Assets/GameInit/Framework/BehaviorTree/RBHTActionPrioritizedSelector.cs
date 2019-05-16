#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2017 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： RBHTActionPrioritizedSelector 
     * 创建人	： roy
     * 创建时间	： 2017/7/18 10:23:15 
     * 描述  	：   	
*/
#endregion

public class RBHTActionPrioritizedSelector : RBHTAction
{
    protected class RBHTActionPrioritizedSelectorContext : RBHTActionContext
    {
        internal int curSelectorIndex;
        internal int lastSelectorIndex;

        public RBHTActionPrioritizedSelectorContext()
        {
            curSelectorIndex = -1;
            lastSelectorIndex = -1;
        }
    }

    public RBHTActionPrioritizedSelector()
        : base(-1)
    {

    }

    protected override bool DoCheck(RBHTData data)
    {
        RBHTActionPrioritizedSelectorContext context = GetContext<RBHTActionPrioritizedSelectorContext>(data);
        context.curSelectorIndex = -1;
        int childCount = GetChildCount();
        RBHTAction actionNode;
        for (int i = 0; i < childCount; i++)
        {
            actionNode = GetChild<RBHTAction>(i);
            if (actionNode.Check(data))
            {
                context.curSelectorIndex = i;
                return true;
            }
        }
        return false;
    }

    protected override int DoUpdate(RBHTData data)
    {
        RBHTActionPrioritizedSelectorContext context = GetContext<RBHTActionPrioritizedSelectorContext>(data);
        int runningStatus = RBHTStatus.FINISHED;
        RBHTAction actionNode;
        if (context.curSelectorIndex != context.lastSelectorIndex)
        {
            if (IsIndexValid(context.lastSelectorIndex))
            {
                actionNode = GetChild<RBHTAction>(context.lastSelectorIndex);
                actionNode.Transition(data);
            }
            context.lastSelectorIndex = context.curSelectorIndex;
        }

        if (IsIndexValid(context.lastSelectorIndex))
        {
            actionNode = GetChild<RBHTAction>(context.lastSelectorIndex);
            runningStatus = actionNode.Update(data);
            if (runningStatus == RBHTStatus.FINISHED)
                context.lastSelectorIndex = -1;
        }

        return runningStatus;
    }

    protected override void DoTransition(RBHTData data)
    {
        RBHTActionPrioritizedSelectorContext context = GetContext<RBHTActionPrioritizedSelectorContext>(data);
        RBHTAction actionNode = GetChild<RBHTAction>(context.lastSelectorIndex);
        if (actionNode != null)
            actionNode.Transition(data);
        context.lastSelectorIndex = -1;
    }
}