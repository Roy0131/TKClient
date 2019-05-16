#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2017 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： RBHTActionSequence 
     * 创建人	： roy
     * 创建时间	： 2017/7/19 15:32:17 
     * 描述  	：   	
*/
#endregion

public class RBHTActionSequence : RBHTAction
{
    protected class RBHTActionSequenceContext : RBHTActionContext
    {
        internal int curSelctedIndex;
        public RBHTActionSequenceContext()
        {
            curSelctedIndex = -1;
        }
    }

    public RBHTActionSequence()
        : base(-1)
    {

    }

    protected override bool DoCheck(RBHTData data)
    {
        //RBHTActionSequenceContext context = GetContext<RBHTActionSequenceContext>(data);
        //int checkedSelectedIdx = -1;
        //if (IsIndexValid(context.curSelctedIndex))
        //    checkedSelectedIdx = context.curSelctedIndex;
        //else
        //    checkedSelectedIdx = 0;
        //if (IsIndexValid(checkedSelectedIdx))
        //{
        //    tb
        //}
        return base.DoCheck(data);
    }
}