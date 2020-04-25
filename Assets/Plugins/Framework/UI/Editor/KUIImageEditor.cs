// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KUIImageEditor" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(KUIImage), false)]
public class KUIImageEditor : ImageEditor
{
    #region Field

    SerializedProperty _sprites;
    SerializedProperty _usePolygon;

    #endregion

    #region Method  

    #endregion

    #region Unity 

    protected override void OnEnable()
    {
        base.OnEnable();
        _sprites = serializedObject.FindProperty("sprites");
        _usePolygon = serializedObject.FindProperty("usePolygon");

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.PropertyField(_sprites, true);
        EditorGUILayout.PropertyField(_usePolygon);

        serializedObject.ApplyModifiedProperties();
    }

    #endregion
}

