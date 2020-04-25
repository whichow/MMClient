using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(KUIAtlas))]
public class UIAtlasInspector : Editor
{
    #region Property

    /// <summary>
    /// 
    /// </summary>
    private KUIAtlas uiAtlas
    {
        get { return this.target as KUIAtlas; }
    }

    /// <summary>
    /// Gets the game object.
    /// </summary>
    private GameObject gameObject
    {
        get { return uiAtlas.gameObject; }
    }

    /// <summary>
    /// Gets the transform.
    /// </summary>
    private Transform transform
    {
        get { return uiAtlas.transform; }
    }

    #endregion

    #region Method

    private bool IsSame(string atlasName, string textureName)
    {
        return textureName.Contains("-" + atlasName + "-");
    }

    private void GetSprites()
    {
        var spriteList = new List<Sprite>();

        var objects = Selection.objects;
        foreach (Object obj in objects)
        {
            var path = AssetDatabase.GetAssetPath(obj);
            var guids = AssetDatabase.FindAssets("t:Sprite", new string[] { path });
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                if (sprite.packed)
                {
                    spriteList.Add(sprite);
                }
            }
        }

        this.uiAtlas.sprites = spriteList.ToArray();
    }

    #endregion

    #region Unity

    private void OnEnable()
    {
    }

    private void OnSceneGUI()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();

        this.serializedObject.Update();

        EditorUtils.BeginArea("");

        if (GUILayout.Button("获取图集"))
        {
            GetSprites();
        }

        EditorUtils.EndArea();

        this.serializedObject.ApplyModifiedProperties();
        EditorGUI.EndChangeCheck();
    }

    #endregion

}
