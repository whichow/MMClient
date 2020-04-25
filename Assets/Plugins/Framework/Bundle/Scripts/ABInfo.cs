// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
namespace K.AB
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Object = UnityEngine.Object;

    /// <summary>
    /// ab 信息
    /// </summary>
    public class ABInfo
    {
        /// <summary>
        /// 如果没有其它东西引用的情况下，此AB最小生存时间（单位秒）
        /// </summary>
        public const float MinLifeTime = 1f;

        private enum State
        {
            kNone,
            kReady,
            kDestory,
        }

        #region C&D
#if UNITY_EDITOR
        private static Transform m_abInfos;
        private GameObject m_go;
        public ABInfo()
        {
            if (m_abInfos == null)
            {
                m_abInfos = new GameObject("AbInfos").transform;
                GameObject.DontDestroyOnLoad(m_abInfos);
            }

            WaitNextFrame(() =>
            {
                m_go = new GameObject($"{abData.assetName}[{abData.fileSize}k]({refCount})");
                m_go.transform.SetParent(m_abInfos);
                m_abInfos.name = $"AbInfos({ m_abInfos.childCount})";
            });
        }

        ~ABInfo()
        {

        }

        private void WaitNextFrame(Action callback)
        {
            ABManager.Instance.StartCoroutine(_WaitNextFrame(callback));
        }

        private IEnumerator _WaitNextFrame(Action callback)
        {
            yield return null;
            callback?.Invoke();
        }
#endif
        #endregion

        #region Field

        public Action<ABInfo> onUnloaded;

        /// <summary>
        /// 源数据
        /// </summary>
        public ABData abData;

        /// <summary>
        /// 资源包
        /// </summary>
        private AssetBundle _assetBundle;

        /// <summary>
        /// 当前状态
        /// </summary>
        private State _state = State.kNone;

        /// <summary>
        /// 生命周期
        /// </summary>
        private float _lifeTime;
        /// <summary>
        /// 
        /// </summary>
        private Object _mainObject;

        /// <summary>
        /// 引用
        /// </summary>
        private List<WeakReference> _weakReferences = ListPool<WeakReference>.Get();
        /// <summary>
        /// 我依赖的
        /// </summary>
        private HashSet<ABInfo> _parentDependencies = HashSetPool<ABInfo>.Get();
        /// <summary>
        /// 依赖我的
        /// </summary>
        private List<ABInfo> _childrenDependencies = ListPool<ABInfo>.Get();

        #endregion

        #region Property

        /// <summary>
        /// 资源路径
        /// </summary>
        public string assetPath
        {
            get { return abData.assetPath; }
        }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string assetName
        {
            get { return abData.assetName; }
        }

        /// <summary>
        /// ab的文件名
        /// </summary>
        public string abName
        {
            get { return abData.fileName; }
        }

        /// <summary>
        /// 强制的引用计数
        /// </summary> 
        public int refCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 生命
        /// </summary>
        public float lifeTime
        {
            get { return Time.time - _lifeTime; }
        }

        /// <summary>
        /// 这个资源是否不用了
        /// </summary>
        /// <returns></returns>
        public bool isUnused
        {
            get { return refCount <= 0 && !UpdateReference() && lifeTime > MinLifeTime; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool isValid
        {
            get { return _assetBundle || _mainObject; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Object mainObject
        {
            get
            {
                if (!_mainObject && _state == State.kReady)
                {
                    var allNames = _assetBundle.GetAllAssetNames();
                    if (allNames != null && allNames.Length > 0)
                    {
                        _mainObject = _assetBundle.LoadAsset(allNames[0]);
                    }
                }
                return _mainObject;
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// 准备好
        /// </summary>
        public void Ready(AssetBundle assetBundle)
        {
            _assetBundle = assetBundle;
            _state = State.kReady;
            _lifeTime = Time.time;
        }

        /// <summary>
        /// 重置生命周期
        /// </summary>
        public void ResetLifeTime()
        {
            if (_state == State.kReady)
            {
                _lifeTime = Time.time;
            }
        }

        /// <summary>
        /// 引用计数增一
        /// </summary>
        public void Retain()
        {
            refCount++;
#if UNITY_EDITOR
            if (m_go != null)
                m_go.name = $"{abData.assetName}({refCount})";
#endif
        }

        /// <summary>
        /// 引用计数减一
        /// </summary>
        public void Release()
        {
            refCount--;
#if UNITY_EDITOR
            if(m_go!= null)
                m_go.name = $"{abData.assetName}({refCount})";
#endif
        }

        /// <summary>
        /// 增加引用
        /// </summary>
        /// <param name="owner">用来计算引用计数，如果所有的引用对象被销毁了，那么AB也将会被销毁</param>
        public void Retain(Object owner)
        {
            if (!owner)
            {
                return;
            }

            //只能遍历(切记切记)
            foreach (var wr in _weakReferences)
            {
                if (owner.Equals(wr.Target))
                {
                    return;
                }
            }
            _weakReferences.Add(new WeakReference(owner));
        }

        /// <summary>
        /// 释放引用
        /// </summary>
        /// <param name="owner"></param>
        public void Release(Object owner)
        {
            if (!owner)
            {
                return;
            }

            for (int i = _weakReferences.Count - 1; i >= 0; i--)
            {
                if (owner.Equals(_weakReferences[i].Target))
                {
                    _weakReferences.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 实例化对象
        /// </summary>
        public GameObject Instantiate()
        {
            var prefab = mainObject as GameObject;
            //只有GameObject才可以Instantiate
            if (prefab)
            {
                var gameObj = Object.Instantiate(prefab);
                gameObj.name = prefab.name;
                Retain(gameObj);
                return gameObj;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public GameObject Instantiate(Vector3 position, Quaternion rotation)
        {
            var prefab = mainObject as GameObject;
            //只有GameObject才可以Instantiate
            if (prefab)
            {
                var gameObj = Object.Instantiate(prefab, position, rotation);
                gameObj.name = prefab.name;
                Retain(gameObj);
                return gameObj;
            }
            return null;
        }

        /// <summary>
        /// 增加依赖项
        /// </summary>
        /// <param name="target"></param>
        public void AddDependencies(ABInfo target)
        {
            if (target != null && _parentDependencies.Add(target))
            {
                target.Retain();
                target._childrenDependencies.Add(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
            if (_state != State.kDestory)
            {
                _state = State.kDestory;

                foreach (var parent in _parentDependencies)
                {
                    if (parent._childrenDependencies != null)
                    {
                        parent._childrenDependencies.Remove(this);
                    }
                    parent.Release();
                }

                ListPool<WeakReference>.Release(_weakReferences);
                _weakReferences = null;
                HashSetPool<ABInfo>.Release(_parentDependencies);
                _parentDependencies = null;
                ListPool<ABInfo>.Release(_childrenDependencies);
                _childrenDependencies = null;

                this.UnloadBundle();

#if UNITY_EDITOR
                if (m_go != null)
                {
                    GameObject.DestroyImmediate(m_go);
                    m_go = null;
                    m_abInfos.name = $"AbInfos({ m_abInfos.childCount})";
                }
#endif
            }
        }

        /// <summary>
        /// 释放bundle
        /// </summary>
        private void UnloadBundle()
        {
            if (_assetBundle != null)
            {
                _assetBundle.Unload(false);
            }
            _assetBundle = null;

            if (onUnloaded != null)
            {
                onUnloaded(this);
            }
        }

        /// <summary>
        /// 更新引用
        /// </summary>
        /// <returns></returns>
        private bool UpdateReference()
        {
            for (int i = _weakReferences.Count - 1; i >= 0; i--)
            {
                var obj = _weakReferences[i].Target as Object;
                if (!obj)
                {
                    _weakReferences.RemoveAt(i);
                }
            }
            return _weakReferences.Count > 0;
        }

        #endregion

    }
}