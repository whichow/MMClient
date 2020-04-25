// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KUISwitch" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

/// <summary>
/// Simple toggle -- something that has an 'on' and 'off' states: checkbox, toggle button, radio button, etc.
/// </summary>
[AddComponentMenu("UI/Custom/Switch", 0)]
[RequireComponent(typeof(RectTransform))]
public class KUISwitch : Selectable, IPointerClickHandler, ISubmitHandler, ICanvasElement
{

    public enum SwitchTransition
    {
        None,
        Fade
    }

    [Serializable]
    public class SwitchEvent : UnityEvent<bool>
    { }

    /// <summary>
    /// Transition type.
    /// </summary>
    public SwitchTransition toggleTransition = SwitchTransition.None;

    /// <summary>
    /// Graphic the toggle should be working with.
    /// </summary>
    public Graphic onGraphic;
    public Graphic offGraphic;

    // Whether the toggle is on
    [FormerlySerializedAs("m_IsActive")]
    [Tooltip("Is the toggle currently on or off?")]
    [SerializeField]
    private bool m_IsOn;

    /// <summary>
    /// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
    /// </summary>
    public SwitchEvent onValueChanged = new SwitchEvent();


    protected KUISwitch()
    { }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();

        var prefabType = UnityEditor.PrefabUtility.GetPrefabType(this);
        if (prefabType != UnityEditor.PrefabType.Prefab && !Application.isPlaying)
            CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
    }

#endif // if UNITY_EDITOR

    public virtual void Rebuild(CanvasUpdate executing)
    {
#if UNITY_EDITOR
        if (executing == CanvasUpdate.Prelayout)
            onValueChanged.Invoke(m_IsOn);
#endif
    }

    public virtual void LayoutComplete()
    { }

    public virtual void GraphicUpdateComplete()
    { }

    protected override void OnEnable()
    {
        base.OnEnable();
        PlayEffect(true);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void OnDidApplyAnimationProperties()
    {
        // Check if isOn has been changed by the animation.
        // Unfortunately there is no way to check if we don�t have a graphic.
        if (onGraphic != null)
        {
            bool oldValue = !Mathf.Approximately(onGraphic.canvasRenderer.GetColor().a, 0);
            if (m_IsOn != oldValue)
            {
                m_IsOn = oldValue;
                Set(!oldValue);
            }
        }

        base.OnDidApplyAnimationProperties();
    }

    /// <summary>
    /// Whether the toggle is currently active.
    /// </summary>
    public bool isOn
    {
        get { return m_IsOn; }
        set
        {
            Set(value);
        }
    }

    void Set(bool value)
    {
        Set(value, true);
    }

    void Set(bool value, bool sendCallback)
    {
        if (m_IsOn == value)
            return;

        // if we are in a group and set to true, do group logic
        m_IsOn = value;

        // Always send event when toggle is clicked, even if value didn't change
        // due to already active toggle in a toggle group being clicked.
        // Controls like Dropdown rely on this.
        // It's up to the user to ignore a selection being set to the same value it already was, if desired.
        PlayEffect(toggleTransition == SwitchTransition.None);
        if (sendCallback)
            onValueChanged.Invoke(m_IsOn);
    }

    /// <summary>
    /// Play the appropriate effect.
    /// </summary>
    private void PlayEffect(bool instant)
    {
        if (onGraphic == null)
            return;

        onGraphic.gameObject.SetActive(m_IsOn);
        offGraphic.gameObject.SetActive(!m_IsOn);

        //#if UNITY_EDITOR
        //        if (!Application.isPlaying)
        //        {
        //            onGraphic.canvasRenderer.SetAlpha(m_IsOn ? 1f : 0f);
        //        }
        //        else
        //#endif
        //        { 
        //            onGraphic.CrossFadeAlpha(m_IsOn ? 1f : 0f, instant ? 0f : 0.1f, true);
        //        }
    }

    /// <summary>
    /// Assume the correct visual state.
    /// </summary>
    protected override void Start()
    {
        PlayEffect(true);
    }

    private void InternalToggle()
    {
        if (!IsActive() || !IsInteractable())
            return;

        isOn = !isOn;
    }

    /// <summary>
    /// React to clicks.
    /// </summary>
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        InternalToggle();
    }

    public virtual void OnSubmit(BaseEventData eventData)
    {
        InternalToggle();
    }
}

