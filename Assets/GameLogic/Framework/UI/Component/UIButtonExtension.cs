using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIButtonExtension : UnityEngine.UI.Selectable,
    IPointerClickHandler,
    ISubmitHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    /// <summary>  
    /// 长按等待时间  
    /// </summary>  
    private float onLongWaitTime = 0.2f;
    /// <summary>  
    /// 不断的产生长按事件  
    /// </summary>  
    public bool onLongContinue = false;

    [System.Serializable]
    public class ButtonClickedEvent : UnityEvent { }

    [FormerlySerializedAs("onClick")]
    [SerializeField]
    private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();
    [FormerlySerializedAs("onLongClick")]
    [SerializeField]
    private ButtonClickedEvent m_OnLongClick = new ButtonClickedEvent();
    [FormerlySerializedAs("onDown")]
    [SerializeField]
    private ButtonClickedEvent m_OnDown = new ButtonClickedEvent();
    [FormerlySerializedAs("onUp")]
    [SerializeField]
    private ButtonClickedEvent m_OnUp = new ButtonClickedEvent();
    [FormerlySerializedAs("onEnter")]
    [SerializeField]
    private ButtonClickedEvent m_OnEnter = new ButtonClickedEvent();
    [FormerlySerializedAs("onExit")]
    [SerializeField]
    private ButtonClickedEvent m_OnExit = new ButtonClickedEvent();

    //private System.Reflection.PropertyInfo pro_isDown;  
    //private System.Reflection.PropertyInfo pro_isEnter;  

    protected UIButtonExtension() : base() { }

    /// <summary>  
    /// 文本值  
    /// </summary>  
    public string text
    {
        get
        {
            Text v = getText();
            if (v != null)
                return v.text;
            return null;
        }
        set
        {
            Text v = getText();
            if (v != null)
                v.text = value;
        }
    }

    private bool isPointerDown = false;
    private bool isPointerInside = false;

    /// <summary>  
    /// 是否被按下  
    /// </summary>  
    public bool isDown
    {
        get
        {
            return isPointerDown;
            /**  
            if (pro_isDown == null) {  
                pro_isDown = typeof(Selectable).GetProperty ("isPointerDown", getRBFlag());  
            }  
            if (pro_isDown == null)  
                return false;  
            bool b = (bool) pro_isDown.GetValue (this, null);  
            return b;  
            */
        }
    }

    /// <summary>  
    /// 是否进入  
    /// </summary>  
    public bool isEnter
    {
        get
        {
            return isPointerInside;
            /**  
            if (pro_isEnter == null) {  
                pro_isEnter = typeof(Selectable).GetProperty ("isPointerInside", getRBFlag());  
            }  
            if (pro_isEnter == null)  
                return false;  
            bool b = (bool) pro_isEnter.GetValue (this, null);  
            return b;  
            */
        }
    }

    /// <summary>  
    /// 点击事件  
    /// </summary>  
    public ButtonClickedEvent onClick
    {
        get { return m_OnClick; }
        set { m_OnClick = value; }
    }

    /// <summary>  
    /// 长按事件  
    /// </summary>  
    public ButtonClickedEvent onLongClick
    {
        get { return m_OnLongClick; }
        set { m_OnLongClick = value; }
    }

    /// <summary>  
    /// 按下事件  
    /// </summary>  
    public ButtonClickedEvent onDown
    {
        get { return m_OnDown; }
        set { m_OnDown = value; }
    }

    /// <summary>  
    /// 松开事件  
    /// </summary>  
    public ButtonClickedEvent onUp
    {
        get { return m_OnUp; }
        set { m_OnUp = value; }
    }

    /// <summary>  
    /// 进入事件  
    /// </summary>  
    public ButtonClickedEvent onEnter
    {
        get { return m_OnEnter; }
        set { m_OnEnter = value; }
    }

    /// <summary>  
    /// 离开事件  
    /// </summary>  
    public ButtonClickedEvent onExit
    {
        get { return m_OnExit; }
        set { m_OnExit = value; }
    }

    private System.Reflection.BindingFlags getRBFlag()
    {
        return System.Reflection.BindingFlags.Static |
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.GetProperty |
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.GetField |
            System.Reflection.BindingFlags.ExactBinding;
    }

    private UnityEngine.UI.Text getText()
    {
        Transform f = transform.Find("Text");
        Text v = null;
        if (f != null)
        {
            v = f.gameObject.GetComponent<Text>();
        }
        if (v == null && transform.childCount > 0)
        {
            GameObject obj = transform.GetChild(0).gameObject;
            v = obj.GetComponent<Text>();
        }
        return v;
    }

    private void Press()
    {
        if (!IsActive() || !IsInteractable())
            return;
        m_OnClick.Invoke();
    }

    private void Down()
    {
        if (!IsActive() || !IsInteractable())
            return;
        m_OnDown.Invoke();
        StartCoroutine(grow());
    }

    private void Up()
    {
        if (!IsActive() || !IsInteractable() || !isDown)
            return;
        m_OnUp.Invoke();
    }

    private void Enter()
    {
        if (!IsActive())
            return;
        m_OnEnter.Invoke();
    }

    private void Exit()
    {
        if (!IsActive() || !isEnter)
            return;
        m_OnExit.Invoke();
    }

    private void LongClick()
    {
        if (!IsActive() || !isDown)
            return;
        m_OnLongClick.Invoke();
    }

    private float downTime = 0f;
    private IEnumerator grow()
    {
        downTime = Time.time;
        while (isDown)
        {
            if (Time.time - downTime > onLongWaitTime)
            {
                LongClick();
                if (onLongContinue)
                    downTime = Time.time;
                else
                    break;
            }
            else
                yield return null;
        }
    }

    protected override void OnDisable()
    {
        isPointerDown = false;
        isPointerInside = false;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        Press();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        isPointerDown = true;
        Down();
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        Up();
        isPointerDown = false;
        base.OnPointerUp(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        isPointerInside = true;
        Enter();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        Exit();
        isPointerInside = false;
        base.OnPointerExit(eventData);
    }

    public virtual void OnSubmit(BaseEventData eventData)
    {
        Press();

        // if we get set disabled during the press  
        // don't run the coroutine.  
        if (!IsActive() || !IsInteractable())
            return;

        DoStateTransition(SelectionState.Pressed, false);
        StartCoroutine(OnFinishSubmit());
    }

    private IEnumerator OnFinishSubmit()
    {
        var fadeTime = colors.fadeDuration;
        var elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        DoStateTransition(currentSelectionState, false);
    }

    public static GameObject CreateUIElementRoot(string name, Vector2 size)
    {
        GameObject go = new GameObject(name);
        RectTransform rect = go.AddComponent<RectTransform>();
        rect.sizeDelta = size;
        return go;
    }
    public static GameObject CreateUIElementRoot(string name, float w, float h)
    {
        return CreateUIElementRoot(name, new Vector2(w, h));
    }

    public static GameObject CreateUIText(string name, string text, GameObject parent)
    {
        GameObject childText = CreateUIObject(name, parent);
        Text v = childText.AddComponent<Text>();
        v.text = text;
        v.alignment = TextAnchor.MiddleCenter;
        SetDefaultTextValues(v);

        RectTransform r = childText.GetComponent<RectTransform>();
        r.anchorMin = Vector2.zero;
        r.anchorMax = Vector2.one;
        r.sizeDelta = Vector2.zero;

        return childText;
    }

    public static GameObject CreateUIObject(string name, GameObject parent)
    {
        GameObject go = new GameObject(name);
        go.AddComponent<RectTransform>();
        SetParentAndAlign(go, parent);
        return go;
    }

    public static void SetDefaultTextValues(Text lbl)
    {
        lbl.color = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);
    }

    public static void SetParentAndAlign(GameObject child, GameObject parent)
    {
        if (parent == null)
            return;

        child.transform.SetParent(parent.transform, false);
        SetLayerRecursively(child, parent.layer);
    }
    public static void SetLayerRecursively(GameObject go, int layer)
    {
        go.layer = layer;
        Transform t = go.transform;
        for (int i = 0; i < t.childCount; i++)
            SetLayerRecursively(t.GetChild(i).gameObject, layer);
    }

    public static T findRes<T>(string name) where T : Object
    {
        T[] objs = Resources.FindObjectsOfTypeAll<T>();
        if (objs != null && objs.Length > 0)
        {
            foreach (Object obj in objs)
            {
                if (obj.name == name)
                    return obj as T;
            }
        }
        objs = AssetBundle.FindObjectsOfType<T>();
        if (objs != null && objs.Length > 0)
        {
            foreach (Object obj in objs)
            {
                if (obj.name == name)
                    return obj as T;
            }
        }
        return default(T);
    }   

}
