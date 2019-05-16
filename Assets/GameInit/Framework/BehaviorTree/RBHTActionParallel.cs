#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2017 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： RBHTActionParallel 
     * 创建人	： roy
     * 创建时间	： 2017/7/18 10:40:57 
     * 描述  	：   	
*/
#endregion

using System.Collections.Generic;

//引入parallel节点，主要是解决边放技能，边移动逻辑，进入此节点，依赖了Precondition。
public class RBHTActionParallel : RBHTAction
{
    protected class RBHTActionParallelContext : RBHTActionContext
    {
        internal List<bool> checkStatus;

        public RBHTActionParallelContext()
        {
            checkStatus = new List<bool>();
        }
    }

    public RBHTActionParallel()
        : base(-1)
    {

    }

    //如果直接return true，行为树主逻辑逻辑会有问题
    protected override bool DoCheck(RBHTData data)
    {
        RBHTActionParallelContext context = GetContext<RBHTActionParallelContext>(data);
        InitListValue<bool>(context.checkStatus, false);
        int count = GetChildCount();
        RBHTAction actionNode;
        bool retCheck;
        for (int i = 0; i < count; i++)
        {
            actionNode = GetChild<RBHTAction>(i);
            retCheck = actionNode.Check(data);
            context.checkStatus[i] = retCheck;
        }
        return true;
    }

    protected override int DoUpdate(RBHTData data)
    {
        RBHTActionParallelContext context = GetContext<RBHTActionParallelContext>(data);
        int childCount = GetChildCount();
        RBHTAction actionNode;
        for (int i = 0; i < childCount; i++)
        {
            actionNode = GetChild<RBHTAction>(i);
            if (context.checkStatus[i])
                actionNode.Update(data);
        }
        return RBHTStatus.EXECUTING;
    }

    protected override void DoTransition(RBHTData data)
    {
        RBHTActionParallelContext context = GetContext<RBHTActionParallelContext>(data);
        int childCount = GetChildCount();
        InitListValue<bool>(context.checkStatus, false);
        RBHTAction actionNode;
        for (int i = 0; i < childCount; i++)
        {
            actionNode = GetChild<RBHTAction>(i);
            actionNode.Transition(data);
        }
    }

    private void InitListValue<T>(List<T> lst, T value)
    {
        int count = GetChildCount();
        if (lst.Count != count)
        {
            lst.Clear();
            for (int i = 0; i < count; i++)
                lst.Add(value);
        }
        else
        {
            for (int i = 0; i < count; i++)
                lst[i] = value;
        }
    }
}