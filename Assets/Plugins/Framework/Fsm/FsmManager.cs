// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using System.Collections.Generic;
using UnityEngine;

namespace K.Fsm
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class FsmManager : MonoBehaviour, IFsmManager
    {
        #region Info

        private class FsmInfo
        {
            public float lastTime;
            public FsmBase fsm;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        private readonly Dictionary<string, FsmInfo> _allFsmInfos = new Dictionary<string, FsmInfo>();
        /// <summary>
        /// 
        /// </summary>
        private readonly List<FsmInfo> _tmpFsmInfos = new List<FsmInfo>();

        /// <summary>
        /// 获取状态机数量.
        /// </summary>
        public int count
        {
            get
            {
                return _allFsmInfos.Count;
            }
        }

        /// <summary>
        /// 检查是否存在状态机.
        /// </summary>
        /// <returns></returns>
        public bool HasFsm<T>() where T : class
        {
            return HasFsm<T>(string.Empty);
        }

        /// <summary>
        /// 检查是否存在状态机.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasFsm<T>(string name) where T : class
        {
            return _allFsmInfos.ContainsKey(Fsm<T>.GetFullName(name));
        }

        /// <summary>
        /// 获取状态机.
        /// </summary>
        /// <returns></returns>
        public IFsm<T> GetFsm<T>() where T : class
        {
            return GetFsm<T>(string.Empty);
        }

        /// <summary>
        /// 获取状态机.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IFsm<T> GetFsm<T>(string name) where T : class
        {
            FsmInfo fsmInfo = null;
            if (_allFsmInfos.TryGetValue(Fsm<T>.GetFullName(name), out fsmInfo))
            {
                return (IFsm<T>)fsmInfo.fsm;
            }

            return null;
        }

		//Lua不支持泛型
        public object GetFsm(string name)
        {
            FsmInfo fsmInfo = null;
            if (_allFsmInfos.TryGetValue(name, out fsmInfo))
            {
                return fsmInfo.fsm;
            }
            Debuger.LogError("not found fsm! " + name);
            return null;
        }

        /// <summary>
        /// 获取所有状态机.
        /// </summary>
        /// <returns></returns>
        public FsmBase[] GetAllFsms()
        {
            int index = 0;
            var fsms = new FsmBase[_allFsmInfos.Count];
            foreach (var fsmInfo in _allFsmInfos)
            {
                fsms[index++] = fsmInfo.Value.fsm;
            }

            return fsms;
        }

        /// <summary>
        /// 创建状态机.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        public IFsm<T> CreateFsm<T>(T owner, params FsmState<T>[] states) where T : class
        {
            return CreateFsm(string.Empty, owner, states);
        }

        /// <summary>
        /// 创建状态机.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="owner"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        public IFsm<T> CreateFsm<T>(string name, T owner, params FsmState<T>[] states) where T : class
        {
            if (HasFsm<T>(name))
            {
                return GetFsm<T>();
            }

            var fsm = new Fsm<T>(name, owner, states);
            _allFsmInfos.Add(Fsm<T>.GetFullName(name), new FsmInfo
            {
                lastTime = -1f,
                fsm = fsm,
            });
            return fsm;
        }

        /// <summary>
        /// 销毁状态机.
        /// </summary>
        /// <returns></returns>
        public bool DestroyFsm<T>() where T : class
        {
            return DestroyFsm<T>(string.Empty);
        }

        /// <summary>
        /// 销毁状态机.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DestroyFsm<T>(string name) where T : class
        {
            string fullName = Fsm<T>.GetFullName(name);
            FsmInfo fsmInfo = null;
            if (_allFsmInfos.TryGetValue(fullName, out fsmInfo))
            {
                fsmInfo.fsm.OnDestroy();
                return _allFsmInfos.Remove(fullName);
            }

            return false;
        }

        /// <summary>
        /// 销毁状态机.
        /// </summary>
        /// <param name="fsm"></param>
        /// <returns></returns>
        public bool DestroyFsm<T>(IFsm<T> fsm) where T : class
        {
            if (fsm == null)
            {
                LogUtil.LogError("Fsm is null.");
                return false;
            }

            return DestroyFsm<T>(fsm.name);
        }

        #region Unity

        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            _tmpFsmInfos.Clear();

            if (_allFsmInfos.Count <= 0)
            {
                return;
            }

            foreach (var item in _allFsmInfos)
            {
                _tmpFsmInfos.Add(item.Value);
            }

            foreach (var fsmInfo in _tmpFsmInfos)
            {
                if (fsmInfo.fsm.destroyed)
                {
                    continue;
                }
                fsmInfo.fsm.Update(Time.deltaTime);
                if (Time.time - fsmInfo.lastTime >= 1f)
                {
                    fsmInfo.lastTime = Time.time;
                    fsmInfo.fsm.UpdatePerSecond();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDestroy()
        {
            foreach (var fsmInfo in _allFsmInfos.Values)
            {
                fsmInfo.fsm.OnDestroy();
            }

            _allFsmInfos.Clear();
            _tmpFsmInfos.Clear();
        }

        #endregion
    }
}
