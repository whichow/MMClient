// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KGameModule" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 游戏模块类。
    /// </summary>
    public class KGameModule : MonoBehaviour
    {
        /// <summary>
        /// 获取游戏模块优先级。
        /// </summary>
        public virtual int priority
        {
            get { return 0; }
        }

        public virtual void Init()
        {

        }
        /// <summary>
        /// 所有模块初始化结束
        /// </summary>
        public virtual void InitComplete()
        {

        }

        public virtual void Load()
        {

        }

        /// <summary>
        /// 所有模块加载结束
        /// </summary>
        public virtual void LoadComplete()
        {

        }
    }
}