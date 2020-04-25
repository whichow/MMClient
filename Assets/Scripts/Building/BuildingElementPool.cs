using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class BuildingElementPool<T> where T : new()
    {
        //Dictionary<int,T>
        /// <summary>
        ///  已使用元素
        /// </summary>
        private List<T> _elementPool;
        private int _curPos;
        private int _count;
        private Func<T> _newElementFun;
        private Action<T> _onDelElement;

        public BuildingElementPool()
        {
            _elementPool = new List<T>();
            _curPos = -1;
            //_elenentUnusedPool
        }
        /// <summary>
        /// 设置实例化实体的方法,如果不设置  直接new一个实例
        /// </summary>
        /// <param name="newElementFun"></param>
        public void NewElementFunSet(Func<T> newElementFun)
        {
            _newElementFun = newElementFun;
        }
        /// <summary>
        /// 设置实例化实体的方法,如果不设置  直接new一个实例
        /// </summary>
        /// <param name="newElementFun"></param>
        public void onDelElementSet(Action<T> onDelElement)
        {
            _onDelElement = onDelElement;
        }
        /// <summary>
        /// 获取可以是使用的实体
        /// </summary>
        /// <param name="elementCtrlCall"></param>
        /// <returns></returns>
        public T GetElementCanUsed(System.Action<T> elementCtrlCall = null)
        {

            if (_curPos < _count - 1)
            {
                _curPos++;
                if (elementCtrlCall != null)
                    elementCtrlCall(_elementPool[_curPos]);
                return _elementPool[_curPos];
            }
            else
            {
                if (_newElementFun != null)
                    _elementPool.Add(_newElementFun());
                else
                    _elementPool.Add(new T());
                _count++;
                _curPos = _count - 1;
                if (elementCtrlCall != null)
                    elementCtrlCall(_elementPool[_count - 1]);
                return _elementPool[_count - 1];
            }

        }
        /// <summary>
        /// 回收正在使用的物体
        /// </summary>
        /// <param name="element"></param>
        /// <param name="elementCtrlCall"></param>
        /// <param name="isDel"></param>
        /// <returns></returns>
        public bool DelElement(T element,System.Action<T> elementCtrlCall=null, bool isDel = false)
        {
            if (_onDelElement!=null)
                _onDelElement(element);
            for (int i = 0; i < _count; i++)
            {
                if (_elementPool[i].GetHashCode() == element.GetHashCode())
                {
                    if (elementCtrlCall != null)
                    {
                        elementCtrlCall(_elementPool[i]);
                    }

                    if (isDel)
                    {
                        _elementPool.Remove(element);
                        _count--;
                        if (i <= _curPos)
                        {
                            _curPos--;
                        }
                    }              
                    else
                    {
                        if (i <= _curPos)
                        {
                            T _element = _elementPool[i];
                            _elementPool.Remove(element);
                            _elementPool.Add(_element);
                            _curPos--;
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取正在使用的实体
        /// </summary>
        /// <returns></returns>
        public List<T> GetElementCanUseAll()
        {
            return _elementPool.GetRange(0,_curPos+1);
        }
        /// <summary>
        /// 在池中获取未使用的实体
        /// </summary>
        /// <returns></returns>
        public List<T> GetElementUnusedAll()
        {
            return _elementPool.GetRange(_curPos+1, _curPos);
        }
    }
}
