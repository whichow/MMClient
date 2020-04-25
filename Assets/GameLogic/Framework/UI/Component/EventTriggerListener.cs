/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.18063
 * 
 * Author:          Coamy
 * Created:	        2014/11/5 15:02:23
 * Description:     鼠标事件扩展  委托此类实现点击等事件
 * 
 * Update History:  2015.8.27 增加长按功能
 * 
 *******************************************************************************/
using UnityEngine;
using UnityEngine.EventSystems;

/*
Supported Events
The Eventsystem supports a number of events, and they can be customised further in user custom user written InputModules.
The events that are supported by the StandaloneInputModule and TouchInputModule are provided by interface and can be implemented on a MonoBehaviour by implementing the interface. If you have a valid EventSystem configured the events will be called at the correct time.

IPointerEnterHandler    – OnPointerEnter – Called when a pointer enters the object
IPointerExitHandler     – OnPointerExit – Called when a pointer exits the object
IPointerDownHandler     – OnPointerDown – Called when a pointer is pressed on the object
IPointerUpHandler       – OnPointerUp – Called when a pointer is released (called on the original the pressed object)
IPointerClickHandler    – OnPointerClick – Called when a pointer is pressed and released on the same object
IBeginDragHandler       – OnBeginDrag – Called on the drag object when dragging is about to begin
IDragHandler            – OnDrag – Called on the drag object when a drag is happening
IEndDragHandler         – OnEndDrag – Called on the drag object when a drag finishes
IDropHandler            – OnDrop – Called on the object where a drag finishes
IScrollHandler          – OnScroll – Called when a mouse wheel scrolls
IUpdateSelectedHandler  – OnUpdateSelected – Called on the selected object each tick
ISelectHandler          – OnSelect – Called when the object becomes the selected object
IDeselectHandler        – OnDeselect – Called on the selected object becomes deselected
IMoveHandler            – OnMove – Called when a move event occurs (left, right, up, down, ect)
ISubmitHandler          – OnSubmit – Called when the submit button is pressed
ICancelHandler          – OnCancel – Called when the cancel button is pressed
*/

public class EventTriggerListener : MonoBehaviour, IEventTriggerListener, 
    IPointerClickHandler, 
    IPointerDownHandler, 
    IPointerEnterHandler, 
    IPointerExitHandler, 
    IPointerUpHandler
    //ISelectHandler, 
    //IUpdateSelectedHandler,
    //IBeginDragHandler,
    //IDragHandler,
    //IEndDragHandler
{
    /// <summary>
    /// 获取 EventTriggerListener ，无则自动附加
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public static EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null) {
            listener = go.AddComponent<EventTriggerListener>();
        }
        return listener;
    }

    public void             OnPointerClick()
    {
        OnPointerClick(null);
    }

    public void             OnPointerClick(PointerEventData eventData)
    {
        if (IsTriggerLongPress) {
            IsTriggerLongPress = false;
        }
        else {
            if (OnClick != null) {
                OnClick(this, eventData);
            }
        }
    }

    public void             OnPointerDown(PointerEventData eventData)
    {
        if (OnDown != null) {
            OnDown(this, eventData);
        }

        IsTriggerLongPress = false;
        //TimeOut = SNTimer.SetTimeOut(delegate(object args, uint count) {
        //    IsLongPress = true;
        //    IsTriggerLongPress = true;
        //    TimeDestroy();
        //    if (OnLongPress != null) {
        //        OnLongPress(this, eventData);
        //    }
        //}, null, LongPressTime * 1000);
        longPressEventData = eventData;
        Invoke("OnLongPressHandler", LongPressTime );
    }

    private void            OnLongPressHandler()
    {
        IsLongPress = true;
        IsTriggerLongPress = true;
        CancelInvoke("OnLongPressHandler");
        if (OnLongPress != null) {
            OnLongPress(this, longPressEventData);
        }
    }

    private void        OnLongPressOverHandler(PointerEventData eventData)
    {
        CancelInvoke("OnLongPressHandler");
        longPressEventData = null;
        //TimeDestroy();
        if (IsLongPress) {
            IsLongPress = false;
            if (OnLongPressOver != null) {
                OnLongPressOver(this, eventData);
            }
        }
    }

    public void             OnPointerUp(PointerEventData eventData)
    {
        if (OnUp != null) {
            OnUp(this, eventData);
        }

        OnLongPressOverHandler(eventData);
    }

    public void             OnPointerEnter(PointerEventData eventData)
    {
        if (OnEnter != null) {
            OnEnter(this, eventData);
        }
    }

    public void             OnPointerExit(PointerEventData eventData)
    {
        if (OnExit != null) {
            OnExit(this, eventData);
        }

        OnLongPressOverHandler(eventData);
    }

    private void            OnDisable()
    {
        OnLongPressOverHandler(null);
    }

    //public void             OnSelect(BaseEventData eventData)
    //{
    //    if (onSelect != null) {
    //        onSelect(this);
    //    }
    //}

    //public void             OnUpdateSelected(BaseEventData eventData)
    //{
    //    if (onUpdateSelect != null) {
    //        onUpdateSelect(this);
    //    }
    //}

    //public void             OnBeginDrag(PointerEventData eventData)
    //{
    //    if (onBeginDrag != null) {
    //        onBeginDrag(this);
    //    }
    //}

    //public void             OnEndDrag(PointerEventData eventData)
    //{
    //    if (onEndDrag != null) {
    //        onEndDrag(this);
    //    }
    //}

    //public void             OnDrag(PointerEventData data)
    //{
    //    if (onDrag != null) {
    //        onDrag(this);
    //    }
    //}

    //private void            TimeDestroy()
    //{
    //    if (TimeOut != null) {
    //        TimeOut.Stop();
    //        TimeOut = null;
    //    }
    //}


    #region Variables 变量
    public delegate void VoidDelegate(IEventTriggerListener e, PointerEventData eventData);
    public VoidDelegate     OnClick;
    public VoidDelegate     OnDown;
    public VoidDelegate     OnUp;
    public VoidDelegate     OnEnter;
    public VoidDelegate     OnExit;

    public VoidDelegate     OnLongPress;
    public VoidDelegate     OnLongPressOver;


    //public VoidDelegate   onSelect;
    //public VoidDelegate   onUpdateSelect;
    //public VoidDelegate 	onBeginDrag;
    //public VoidDelegate 	onEndDrag;
    //public VoidDelegate 	onDrag;

    public float            LongPressTime = 0.3f;                            //长按触发时长 秒
    private bool            IsLongPress = false;
    private bool            IsTriggerLongPress = false;
    //private SgTimer         TimeOut = null;
    private PointerEventData longPressEventData;

    #endregion
}

public interface IEventTriggerListener
{

}