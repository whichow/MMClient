// ***********************************************************************
// Company          : 
// Author           : KimCh
// Copyright(c)     : KimCh
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
using DG.Tweening;
using Game;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
/// <summary>
/// 
/// </summary>
public abstract class KUIWindow
{
    #region Enum

    /// <summary>
    /// 
    /// </summary>
    public enum UILayer : byte
    {
        kBackground = 0,
        kNormal = 1,
        kPop = 2,
        kTutorial = 4
    }

    /// <summary>
    /// 
    /// </summary>
    public enum UIMode : byte
    {
        /// <summary>
        /// 不加入对列,独立管理 
        /// </summary>
        kNone = 0,
        /// <summary>
        /// 加入对列 不处理其它界面 
        /// </summary>
        kSequence = 1,
        /// <summary>
        /// 加入对列 隐藏所有界面(Normal层 不会处理背景层)
        /// </summary>
        kSequenceHide = 2,
        /// <summary>
        /// 加入对列 移除所有界面(Normal层 不会处理背景层)
        /// </summary>
        kSequenceRemove = 4,
    }

    /// <summary>
    /// 动画
    /// </summary>
    public enum UIAnim : byte
    {
        kNone = 0,
        kAnim1 = 1,
        kAnim2 = 2,
        kAnim3 = 3,
        kAnim_ChatEnable = 4,
        kAnimator = 240,
        kCustom = 250
    }

    #endregion

    #region Field

    /// <summary>
    /// 
    /// </summary>
    private GameObject _gameObject;
    /// <summary>
    /// 
    /// </summary>
    private Transform _transform;
    /// <summary>
    /// 
    /// </summary>
    private UIBehaviour _behaviour;
    /// <summary>
    /// 
    /// </summary>
    private GameObject _maskObject;

    #endregion

    #region Property

    /// <summary>
    /// 
    /// </summary>
    public GameObject gameObject
    {
        get { return _gameObject; }
        internal set
        {
            _gameObject = value;
            if (_gameObject)
            {
                _transform = _gameObject.transform;
                _behaviour = _gameObject.AddComponent<UIBehaviour>();
                _behaviour.OnDestroyHandler = delegate ()
                {
                    RemoveEvents();
                    OnDestroy();
                    _gameObject = null;
                    _transform = null;
                    _behaviour = null;
                    _maskObject = null;
                };
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Transform transform
    {
        get { return _transform; }
    }

    /// <summary>
    /// 打开的界面
    /// </summary>
    public bool active
    {
        get { return _gameObject && _gameObject.activeSelf; }
    }

    /// <summary>
    /// this window's type
    /// </summary>
    public UILayer uiLayer
    {
        get;
        private set;
    }

    /// <summary>
    /// how to show this window.
    /// </summary>
    public UIMode uiMode
    {
        get;
        private set;
    }

    /// <summary>
    /// 动画
    /// </summary>
    public UIAnim uiAnim
    {
        get;
        protected set;
    }

    /// <summary>
    /// path to load ui
    /// </summary>
    public string uiPath
    {
        get;
        protected set;
    }

    public bool hasMask
    {
        get;
        protected set;
    }

    /// <summary>
    /// 
    /// </summary>
    public string name
    {
        get;
        protected set;
    }

    /// <summary>
    /// refresh page 's data.
    /// </summary>
    public object data
    {
        get;
        internal set;
    }

    /// <summary>
    /// record this ui load mode.async or sync.
    /// </summary>
    public bool asyncUI
    {
        get;
        internal set;
    }

    #endregion

    #region Constructor

    /// <summary>
    /// 
    /// </summary>
    private KUIWindow()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="mode"></param>
    public KUIWindow(UILayer layer, UIMode mode)
    {
        this.uiLayer = layer;
        this.uiMode = mode;
        this.name = this.GetType().ToString();
    }

    #endregion

    #region Method

    /// <summary>
    /// 
    /// </summary>
    public virtual void Awake()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void Start()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void OnEnable()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void OnDisable()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void OnDestroy()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void Update()
    {

    }

    /// <summary>
    /// 每秒更新
    /// </summary>
    public virtual void UpdatePerSecond()
    {

    }

    /// <summary>
    /// 响应背景事件
    /// </summary>
    public virtual void OnBlackMaskClick()
    {
        CloseWindow(this);
    }

    protected virtual void OnAnimationStart()
    {
        return; //TODO 新手引导位置容易错乱，需要重构部分代码
        if (uiAnim != UIAnim.kNone)
        {
            switch (uiAnim)
            {
                case UIAnim.kAnim1:
                    _transform.localScale = Vector3.one * 0.1f;
                    _transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic, 0f, 2f).OnComplete(OnAnimationFinish);
                    break;

                case UIAnim.kAnim_ChatEnable:
                    _transform.localPosition = new Vector3(825, 0, 0);
                    //第二个参数使用KChatManager.PanelMoveTime.
                    (_transform as RectTransform).DOAnchorPos(new Vector3(10, 0), 0.4f);
                    break;
            }
        }
    }

    protected virtual void OnAnimationFinish()
    {
        if (uiAnim != UIAnim.kNone)
        {
            switch (uiAnim)
            {
                case UIAnim.kAnim1:
                    _transform.localScale = Vector3.one;
                    break;
            }
        }
    }

    #region Coroutine

    /// <summary>
    /// 
    /// </summary>
    /// <param name="routine"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        if (_behaviour)
        {
            return _behaviour.StartCoroutine(routine);
        }
        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="routine"></param>
    public void StopCoroutine(IEnumerator routine)
    {
        if (_behaviour)
        {
            _behaviour.StopCoroutine(routine);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="routine"></param>
    public void StopCoroutine(Coroutine routine)
    {
        if (_behaviour)
        {
            _behaviour.StopCoroutine(routine);
        }
    }

    public void StopAllCoroutine()
    {
        if (_behaviour)
        {
            _behaviour.StopAllCoroutines();
        }
    }

    #endregion

    #region Event

    public virtual void AddEvents()
    {

    }

    public virtual void RemoveEvents()
    {

    }

    protected void DispatchEvent(string eventType, IEventData args = null)
    {
        EventManager.Instance.UIEvtDispatcher.DispatchEvent(eventType, args);
    }

        #endregion

        #endregion

        #region Helper

        public T Find<T>(string path) where T : Component
        {
            if (_transform)
            {
                var child = _transform.Find(path);
                if (child)
                {
                    return child.GetComponent<T>();
                }
                else
                {
                    Debuger.LogError("[KUIWindow.Find T] not found! " + name + " - " + path);
                }
            }
            return null;
        }

        // ForLua
        public Component Find(string type, string path)
        {
            if (_transform)
            {
                Transform child;
                if (string.IsNullOrEmpty(path))
                {
                    child = _transform;
                }
                else
                {
                    child = _transform.Find(path);
                }
                if (child)
                {
                    return child.GetComponent(type);
                }
                else
                {
                    Debuger.LogError("[KUIWindow.Find Component] not found! " + name + " - " + path);
                }
            }
            return null;
        }

        protected T FindOnSelf<T>() where T : UnityEngine.Object
        {
            return gameObject.GetComponent<T>();
        }

        public GameObject Find(string path)
        {
            if (_transform)
            {
                var child = _transform.Find(path);
                if (child)
                {
                    return child.gameObject;
                }
                else
                {
                    Debuger.LogError("[KUIWindow.Find GameObject] not found! " + name + " - " + path);
                }
            }
            return null;
        }

    /// <summary>
    /// 获取界面
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static T GetWindow<T>() where T : KUIWindow
    {
        T retW = null;
        if (KUIManager.Instance)
        {
            retW = KUIManager.Instance.GetWindow(typeof(T)) as T;
        }
        return retW;
    }

    // ForLua
    public static KUIWindow GetWindow(Type type)
    {
        KUIWindow retW = null;
        if (KUIManager.Instance)
        {
            retW = KUIManager.Instance.GetWindow(type);
        }
        return retW;
    }

    /// <summary>
    /// 获取所有界面
    /// </summary>
    /// <returns></returns>
    public static KUIWindow[] GetAllWindows()
    {
        KUIWindow[] retW = null;
        if (KUIManager.Instance)
        {
            retW = KUIManager.Instance.GetAllWindows();
        }
        return retW;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static KUIWindow[] GetActiveWindows()
    {
        KUIWindow[] retW = null;
        if (KUIManager.Instance)
        {
            retW = KUIManager.Instance.GetActiveWindows();
        }
        return retW;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static int GetActiveWindowsCount()
    {
        int retC = 0;
        if (KUIManager.Instance)
        {
            retC = KUIManager.Instance.GetActiveWindowsCount();
        }
        return retC;
    }

    /// <summary>
    /// 打开窗口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="windowData"></param>
    /// <param name="callback"></param>
    public static void OpenWindow<T>(object windowData = null, System.Action callback = null) where T : KUIWindow
    {
        OpenWindow(typeof(T), windowData, callback);
    }

    /// <summary>
    /// 打开窗口 Lua不支持泛型
    /// </summary>
    /// <param name="type"></param>
    /// <param name="windowData"></param>
    /// <param name="callback"></param>
    public static void OpenWindow(Type type, object windowData = null, System.Action callback = null)
    {
        if (KUIManager.Instance)
        {
            KUIManager.Instance.OpenWindow(type, windowData, callback);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void CloseWindow<T>() where T : KUIWindow
    {
        CloseWindow(typeof(T));
    }

    public static void CloseWindow(Type type)
    {
        if (KUIManager.Instance)
        {
            KUIManager.Instance.CloseWindow(type);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="window"></param>
    public static void CloseWindow(KUIWindow window)
    {
        if (KUIManager.Instance)
        {
            KUIManager.Instance.CloseWindow(window);
        }
    }

    public static void DestroyWindow(KUIWindow window)
    {
        if (KUIManager.Instance)
        {
            KUIManager.Instance.DestroyWindow(window);
        }
    }

    #endregion

    #region Internal Method    

    internal void Show(bool self = true)
    {
        OnAnimationStart();
        if (hasMask)
        {
            if (!_maskObject)
            {
                //_maskObject = KUIBind.CreateUI("BlackMask");
                //_maskObject.name = this.name + ".Mask";

                _maskObject = new GameObject(this.name + ".Mask");
                var rt = _maskObject.AddComponent<RectTransform>();
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                _maskObject.AddComponent<Image>().color = new Color(0, 0, 0, 0.5f);
                _maskObject.AddComponent<Button>().onClick.AddListener(OnBlackMaskClick);

                _maskObject.transform.SetParent(_transform.parent, false);
                _maskObject.transform.SetSiblingIndex(_transform.GetSiblingIndex());
            }
            else
            {
                _maskObject.SetActive(true);
            }
        }
    }

    internal void Hide(bool self = true)
    {
        if (hasMask)
        {
            if (_maskObject)
            {
                _maskObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return "Name:" + name + ",Layer:" + uiLayer.ToString() + ",Mode:" + uiMode.ToString();
    }

    #endregion

    #region Behaviour

    private class UIBehaviour : MonoBehaviour
    {
        public Action OnDestroyHandler;

        public void OnDestroy()
        {
            OnDestroyHandler?.Invoke();
        }
    }

    #endregion

}
}
