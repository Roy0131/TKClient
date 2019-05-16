#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2016 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： EventDele 
     * 创建人	： roy
     * 创建时间	： 2016/12/19 15:40:17 
     * 描述  	： 事件回调delegate
*/
#endregion

namespace Plugin.Core
{
    public delegate void REventDelegate();
    public delegate void REventDelegate<T>(T a);
    public delegate void REventDelegate<T1, T2>(T1 a1, T2 a2);
    public delegate void REventDelegate<T1, T2, T3>(T1 a1, T2 a2, T3 a3);
    public delegate void REventDelegate<T1, T2, T3, T4>(T1 a1, T2 a2, T3 a3, T4 a4);
}