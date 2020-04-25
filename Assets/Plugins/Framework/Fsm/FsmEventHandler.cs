// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
namespace K.Fsm
{
    /// <summary>
    /// 状态机事件响应函数.
    /// </summary>
    /// <param name="fsm">状态机.</param>
    /// <param name="sender">事件源.</param>
    /// <param name="userData">自定义数据.</param>
    public delegate void FsmEventHandler<T>(IFsm<T> fsm, object sender, object userData) where T : class;
}
