/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.18063
 * 
 * Author:          Coamy
 * Created:	        2014/12/3 10:12:34
 * Description:     UIMenuOptions UI菜单设置
 * 
 * Update History:  
 * 
 *******************************************************************************/
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuOptions
{
    #region MenuAddUIList

    [MenuItem("GameObject/UI/UIList")]
    private static void AddUIList(MenuCommand menuCommand)
    {
        GameObject root = CreateUIElementRoot("List", menuCommand);

        RectTransform _listRt = root.GetComponent<RectTransform>();
        _listRt.sizeDelta = new Vector2(300, 300);
        _listRt.anchorMin = Vector2.up;
        _listRt.anchorMax = Vector2.up;
        _listRt.pivot = Vector2.up;

        Image _image = root.AddComponent<Image>();
        _image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
        _image.type = Image.Type.Sliced;
        _image.color = new Color(1f, 1f, 1f, 0.392f);

        GameObject _item = new GameObject("Item");
        GameObjectUtility.SetParentAndAlign(_item, root);
        RectTransform   _itemRt = _item.AddComponent<RectTransform>();
        _itemRt.anchorMin = Vector2.up;
        _itemRt.anchorMax = Vector2.up;
        _itemRt.pivot = Vector2.up;

        UIList _uiList = root.AddComponent<UIList>();
        _uiList.item = _item;
    }

    #endregion

    #region MenuAddPageBar

    [MenuItem("GameObject/UI/PageBar")]
    private static void AddPageBar(MenuCommand menuCommand)
    {
        GameObject root = CreateUIElementRoot("PageBar", menuCommand);

        GameObject      _stack = new GameObject("ViewStack");
        GameObject      _item0 = new GameObject("Item0");
        GameObject      _item1 = new GameObject("Item1");

        GameObjectUtility.SetParentAndAlign(_stack, root);
        GameObjectUtility.SetParentAndAlign(_item0, _stack);
        GameObjectUtility.SetParentAndAlign(_item1, _stack);

        RectTransform _boxRt = root.GetComponent<RectTransform>();
        if (_boxRt == null)
            root.AddComponent<RectTransform>();
        HorizontalLayoutGroup _boxHLG = root.AddComponent<HorizontalLayoutGroup>();
        root.AddComponent<CanvasRenderer>();
        root.AddComponent<PageBar>();

        _boxRt.sizeDelta = new Vector2(0, 20f);
        _boxRt.anchorMin = Vector2.zero;
        _boxRt.anchorMax = Vector2.right;
        _boxRt.pivot = new Vector2(0.5f, 0);

        _boxHLG.spacing = 2;
        _boxHLG.childAlignment = TextAnchor.MiddleCenter;
        _boxHLG.childForceExpandHeight = false;
        _boxHLG.childForceExpandWidth = false;

        RectTransform _item0Rt = _item0.AddComponent<RectTransform>();
        RectTransform _item1Rt = _item1.AddComponent<RectTransform>();
        _item0.AddComponent<Image>();
        _item1.AddComponent<Image>();
        _item0Rt.anchorMin = Vector2.zero;
        _item0Rt.anchorMax = Vector2.one;
        _item0Rt.sizeDelta = Vector2.zero;
        _item1Rt.anchorMin = Vector2.zero;
        _item1Rt.anchorMax = Vector2.one;
        _item1Rt.sizeDelta = Vector2.zero;

        _stack.AddComponent<RectTransform>();
        _stack.AddComponent<CanvasRenderer>();
        _stack.AddComponent<LayoutElement>();
        _stack.AddComponent<ViewStack>().m_isToggle = true;
        _stack.GetComponent<LayoutElement>().preferredWidth = 10f;
        _stack.GetComponent<LayoutElement>().preferredHeight = 10f;

        _item1.GetComponent<Image>().color = Color.red;
    }

    #endregion

    private static GameObject CreateUIElementRoot(string name, MenuCommand menuCommand)
    {
        GameObject parent = menuCommand.context as GameObject;
        GameObject child = new GameObject(name);

        Undo.RegisterCreatedObjectUndo(child, "Create " + name);
        Undo.SetTransformParent(child.transform, parent.transform, "Parent " + child.name);
        GameObjectUtility.SetParentAndAlign(child, parent);

        RectTransform rectTransform = child.AddComponent<RectTransform>();

        Selection.activeGameObject = child;
        return child;
    }
     
}