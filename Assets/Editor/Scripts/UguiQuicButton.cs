using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UguiQuicButton
{
	private static GameObject CheckSelection (MenuCommand menuCommand)
	{
		GameObject selectedObj = menuCommand.context as GameObject;
		
		if (selectedObj == null)
			selectedObj = Selection.activeGameObject;
		
		if (selectedObj == null || selectedObj != null && selectedObj.GetComponentInParent<Canvas> () == null)
			return null;
		return selectedObj;
	}
	//创建image
	[MenuItem ("GameObject/UGUI/Image #&z", false, 6)] 
	static void CreateImage (MenuCommand menuCommand)
	{
		GameObject selectedObj = CheckSelection (menuCommand);
		if (selectedObj == null)
			return;
		GameObject go = new GameObject ("Image");
		GameObjectUtility.SetParentAndAlign (go, selectedObj);
		Undo.RegisterCreatedObjectUndo (go, "Create " + go.name);
		Selection.activeObject = go;
		go.AddComponent<Image> ();
	}
	//创建Button
	[MenuItem ("GameObject/UGUI/Image #&b", false, 6)] 
	static void CreateButton (MenuCommand menuCommand)
	{
		GameObject selectedObj = CheckSelection (menuCommand);
		if (selectedObj == null)
			return;
		GameObject go = new GameObject ("Button");
		GameObjectUtility.SetParentAndAlign (go, selectedObj);
		Undo.RegisterCreatedObjectUndo (go, "Create " + go.name);
		Selection.activeObject = go;
		go.AddComponent<Image> ();
		go.AddComponent<Button> ();
	}

	//创建Text
	[MenuItem ("GameObject/UGUI/Text #&x", false, 6)]
	static void CreateText (MenuCommand menuCommand)
	{
		GameObject selectedObj = CheckSelection (menuCommand);
		if (selectedObj == null)
			return;
		GameObject go = new GameObject ("Text");
		GameObjectUtility.SetParentAndAlign (go, selectedObj);
		Undo.RegisterCreatedObjectUndo (go, "Create " + go.name);
		Selection.activeObject = go;

		Text t = go.AddComponent<Text> ();
		Font font = AssetDatabase.LoadAssetAtPath ("Assets/GameRes/Fonts/MYCat.ttf", typeof (Font)) as Font;
		t.font = font;
		t.fontSize = 24;
		t.alignment = TextAnchor.MiddleCenter;
		t.color = Color.white;
		t.text = "New Text";
		t.rectTransform.sizeDelta = new Vector2 (150f, 30f);
	}

	//添加描边
	[MenuItem ("GameObject/UGUI/Text #&f", false, 6)]
	static void CreateOutLine (MenuCommand menuCommand)
	{
		GameObject selectedObj = CheckSelection (menuCommand);
		if (selectedObj == null)
			return;
		Outline t = selectedObj.AddComponent<Outline> ();
		t.effectColor = new Color (0f,0f,0f,0.5f);
		t.effectDistance = new Vector2 (1f,-1f);
	}

	//添加阴影
	[MenuItem ("GameObject/UGUI/Text #&d", false, 6)]
	static void CreateShadow (MenuCommand menuCommand)
	{
		GameObject selectedObj = CheckSelection (menuCommand);
		if (selectedObj == null)
			return;
		Shadow t = selectedObj.AddComponent<Shadow> ();
		t.effectColor = new Color (0f,0f,0f,1f);
		t.effectDistance = new Vector2 (1f,-2f);
	}
}