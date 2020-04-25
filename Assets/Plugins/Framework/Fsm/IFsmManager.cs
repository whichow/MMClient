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
    /// 
    /// </summary>
    public interface IFsmManager
    {
        /// <summary>
        /// 
        /// </summary>
        int count
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool HasFsm<T>() where T : class;

        /// <summary>
        /// 检查是否存在状态机.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool HasFsm<T>(string name) where T : class;

        /// <summary>
        /// 获取状态机.
        /// </summary>
        /// <returns></returns>
        IFsm<T> GetFsm<T>() where T : class;

        /// <summary>
        /// 获取状态机.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IFsm<T> GetFsm<T>(string name) where T : class;

        /// <summary>
        /// 获取所有状态机.
        /// </summary>
        /// <returns></returns>
        FsmBase[] GetAllFsms();

        /// <summary>
        /// 创建状态机.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        IFsm<T> CreateFsm<T>(T owner, params FsmState<T>[] states) where T : class;

        /// <summary>
        /// 创建状态机.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="owner"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        IFsm<T> CreateFsm<T>(string name, T owner, params FsmState<T>[] states) where T : class;

        /// <summary>
        /// 销毁状态机.
        /// </summary>
        /// <returns></returns>
        bool DestroyFsm<T>() where T : class;

        /// <summary>
        /// 销毁状态机.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool DestroyFsm<T>(string name) where T : class;

        /// <summary>
        /// 销毁状态机.
        /// </summary>
        /// <param name="fsm"></param>
        /// <returns></returns>
        bool DestroyFsm<T>(IFsm<T> fsm) where T : class;
    }
}
