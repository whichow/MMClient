// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "CustomControls" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

static class CustomControls
{
    public struct Resources
    {
        public Sprite standard;
        public Sprite background;
        public Sprite inputField;
        public Sprite knob;
        public Sprite checkmark;
        public Sprite dropdown;
        public Sprite mask;
        public Material emojimaterial;
    }

    private const float kWidth = 160f;
    private const float kThickHeight = 30f;
    private const float kThinHeight = 20f;
    private static Vector2 s_ThickElementSize = new Vector2(kWidth, kThickHeight);
    private static Vector2 s_ThinElementSize = new Vector2(kWidth, kThinHeight);
    private static Vector2 s_ImageElementSize = new Vector2(100f, 100f);
    private static Color s_DefaultSelectableColor = new Color(1f, 1f, 1f, 1f);
    private static Color s_PanelColor = new Color(1f, 1f, 1f, 0.392f);
    private static Color s_TextColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);

    // Helper methods at top

    private static GameObject CreateUIElementRoot(string name, Vector2 size)
    {
        GameObject child = new GameObject(name);
        RectTransform rectTransform = child.AddComponent<RectTransform>();
        rectTransform.sizeDelta = size;
        return child;
    }

    static GameObject CreateUIObject(string name, GameObject parent)
    {
        GameObject go = new GameObject(name);
        go.AddComponent<RectTransform>();
        SetParentAndAlign(go, parent);
        return go;
    }

    private static void SetDefaultTextValues(Text lbl)
    {
        // Set text values we want across UI elements in default controls.
        // Don't set values which are the same as the default values for the Text component,
        // since there's no point in that, and it's good to keep them as consistent as possible.
        lbl.color = s_TextColor;

        lbl.font = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
        // Reset() is not called when playing. We still want the default font to be assigned
        //lbl.AssignDefaultFont();
    }

    private static void SetDefaultColorTransitionValues(Selectable slider)
    {
        ColorBlock colors = slider.colors;
        colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
        colors.pressedColor = new Color(0.698f, 0.698f, 0.698f);
        colors.disabledColor = new Color(0.521f, 0.521f, 0.521f);
    }

    private static void SetParentAndAlign(GameObject child, GameObject parent)
    {
        if (parent == null)
            return;

        child.transform.SetParent(parent.transform, false);
        SetLayerRecursively(child, parent.layer);
    }

    private static void SetLayerRecursively(GameObject go, int layer)
    {
        go.layer = layer;
        Transform t = go.transform;
        for (int i = 0; i < t.childCount; i++)
            SetLayerRecursively(t.GetChild(i).gameObject, layer);
    }

    // Actual controls  

    public static GameObject CreateImage(Resources resources)
    {
        GameObject go = CreateUIElementRoot("Image", s_ImageElementSize);
        go.AddComponent<KUIImage>();
        return go;
    }

    public static GameObject CreateSwitch(Resources resources)
    {
        // Set up hierarchy
        GameObject switchRoot = CreateUIElementRoot("Switch", s_ThinElementSize);

        GameObject background = CreateUIObject("Background", switchRoot);
        GameObject on = CreateUIObject("On", background);
        GameObject off = CreateUIObject("Off", background);
        GameObject childLabel = CreateUIObject("Label", switchRoot);

        // Set up components
        var @switch = switchRoot.AddComponent<KUISwitch>();
        @switch.transition = Selectable.Transition.None;
        @switch.isOn = true;

        Image bgImage = background.AddComponent<Image>();
        bgImage.sprite = resources.standard;
        bgImage.type = Image.Type.Sliced;
        bgImage.color = s_DefaultSelectableColor;

        Image onImage = on.AddComponent<Image>();
        //onImage.sprite = resources.checkmark;
        Image offImage = off.AddComponent<Image>();

        Text label = childLabel.AddComponent<Text>();
        label.text = "Switch";
        SetDefaultTextValues(label);

        @switch.onGraphic = onImage;
        @switch.offGraphic = offImage;
        @switch.targetGraphic = bgImage;
        SetDefaultColorTransitionValues(@switch);

        RectTransform bgRect = background.GetComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0f, 1f);
        bgRect.anchorMax = new Vector2(0f, 1f);
        bgRect.anchoredPosition = new Vector2(10f, -10f);
        bgRect.sizeDelta = new Vector2(kThinHeight, kThinHeight);

        RectTransform onRect = on.GetComponent<RectTransform>();
        onRect.anchorMin = new Vector2(0.5f, 0.5f);
        onRect.anchorMax = new Vector2(0.5f, 0.5f);
        onRect.anchoredPosition = Vector2.zero;
        onRect.sizeDelta = new Vector2(20f, 20f);

        RectTransform offRect = on.GetComponent<RectTransform>();
        offRect.anchorMin = new Vector2(0.5f, 0.5f);
        offRect.anchorMax = new Vector2(0.5f, 0.5f);
        offRect.anchoredPosition = Vector2.zero;
        offRect.sizeDelta = new Vector2(20f, 20f);

        RectTransform labelRect = childLabel.GetComponent<RectTransform>();
        labelRect.anchorMin = new Vector2(0f, 0f);
        labelRect.anchorMax = new Vector2(1f, 1f);
        labelRect.offsetMin = new Vector2(23f, 1f);
        labelRect.offsetMax = new Vector2(-5f, -2f);

        return switchRoot;
    }
    public static GameObject CreateScrollbar(Resources resources)
    {
        // Create GOs Hierarchy
        GameObject scrollbarRoot = CreateUIElementRoot("Scrollbar", s_ThinElementSize);

        GameObject sliderArea = CreateUIObject("Sliding Area", scrollbarRoot);
        GameObject handle = CreateUIObject("Handle", sliderArea);

        Image bgImage = scrollbarRoot.AddComponent<Image>();
        bgImage.sprite = resources.background;
        bgImage.type = Image.Type.Sliced;
        bgImage.color = s_DefaultSelectableColor;

        Image handleImage = handle.AddComponent<Image>();
        handleImage.sprite = resources.standard;
        handleImage.type = Image.Type.Sliced;
        handleImage.color = s_DefaultSelectableColor;

        RectTransform sliderAreaRect = sliderArea.GetComponent<RectTransform>();
        sliderAreaRect.sizeDelta = new Vector2(-20, -20);
        sliderAreaRect.anchorMin = Vector2.zero;
        sliderAreaRect.anchorMax = Vector2.one;

        RectTransform handleRect = handle.GetComponent<RectTransform>();
        handleRect.sizeDelta = new Vector2(20, 20);

        Scrollbar scrollbar = scrollbarRoot.AddComponent<Scrollbar>();
        scrollbar.handleRect = handleRect;
        scrollbar.targetGraphic = handleImage;
        SetDefaultColorTransitionValues(scrollbar);

        return scrollbarRoot;
    }

    public static GameObject CreateScrollView(Resources resources)
    {
        GameObject root = CreateUIElementRoot("Scroll View", new Vector2(200, 200));

        GameObject viewport = CreateUIObject("Viewport", root);
        GameObject content = CreateUIObject("Content", viewport);
        GameObject template = CreateUIObject("Template", content);

        // Sub controls.

        GameObject hScrollbar = CreateScrollbar(resources);
        hScrollbar.name = "Scrollbar Horizontal";
        SetParentAndAlign(hScrollbar, root);
        RectTransform hScrollbarRT = hScrollbar.GetComponent<RectTransform>();
        hScrollbarRT.anchorMin = Vector2.zero;
        hScrollbarRT.anchorMax = Vector2.right;
        hScrollbarRT.pivot = Vector2.zero;
        hScrollbarRT.sizeDelta = new Vector2(0, hScrollbarRT.sizeDelta.y);

        GameObject vScrollbar = CreateScrollbar(resources);
        vScrollbar.name = "Scrollbar Vertical";
        SetParentAndAlign(vScrollbar, root);
        vScrollbar.GetComponent<Scrollbar>().SetDirection(Scrollbar.Direction.BottomToTop, true);
        RectTransform vScrollbarRT = vScrollbar.GetComponent<RectTransform>();
        vScrollbarRT.anchorMin = Vector2.right;
        vScrollbarRT.anchorMax = Vector2.one;
        vScrollbarRT.pivot = Vector2.one;
        vScrollbarRT.sizeDelta = new Vector2(vScrollbarRT.sizeDelta.x, 0);

        // Setup RectTransforms.

        // Make viewport fill entire scroll view.
        RectTransform viewportRT = viewport.GetComponent<RectTransform>();
        viewportRT.anchorMin = Vector2.zero;
        viewportRT.anchorMax = Vector2.one;
        viewportRT.sizeDelta = Vector2.zero;
        viewportRT.pivot = Vector2.up;

        // Make context match viewpoprt width and be somewhat taller.
        // This will show the vertical scrollbar and not the horizontal one.
        RectTransform contentRT = content.GetComponent<RectTransform>();
        contentRT.anchorMin = Vector2.up;
        contentRT.anchorMax = Vector2.one;
        contentRT.sizeDelta = new Vector2(0, 300);
        contentRT.pivot = Vector2.up;

        // Setup UI components.

        var scrollRect = root.AddComponent<KUIGrid>();
        scrollRect.content = contentRT;
        scrollRect.viewport = viewportRT;
        scrollRect.horizontal = false;
        scrollRect.vertical = false;
        scrollRect.horizontalScrollbar = hScrollbar.GetComponent<Scrollbar>();
        scrollRect.verticalScrollbar = vScrollbar.GetComponent<Scrollbar>();
        scrollRect.horizontalScrollbarVisibility = KUIGrid.ScrollbarVisibility.AutoHideAndExpandViewport;
        scrollRect.verticalScrollbarVisibility = KUIGrid.ScrollbarVisibility.AutoHideAndExpandViewport;
        scrollRect.horizontalScrollbarSpacing = -3;
        scrollRect.verticalScrollbarSpacing = -3;

        //Image rootImage = root.AddComponent<Image>();
        //rootImage.sprite = resources.background;
        //rootImage.type = Image.Type.Sliced;
        //rootImage.color = s_PanelColor;

        //viewport.AddComponent<RectMask2D>();
        Mask viewportMask = viewport.AddComponent<Mask>();
        viewportMask.showMaskGraphic = false;

        Image viewportImage = viewport.AddComponent<Image>();
        viewportImage.sprite = resources.mask;
        viewportImage.type = Image.Type.Sliced;

        scrollRect.uiPool = content.AddComponent<KUIPool>();
        content.AddComponent<GridLayoutGroup>();
        content.AddComponent<ContentSizeFitter>();

        return root;
    }

    public static GameObject CreateEmojiText(Resources resources)
    {
        GameObject go = CreateUIElementRoot("EmojiText", s_ThickElementSize);

        EmojiText lbl = go.AddComponent<EmojiText>();
        lbl.text = "New EmojiText";
        SetDefaultTextValues(lbl);

        return go;
    }
}

