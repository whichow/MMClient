// ***********************************************************************
// Company          : Kunpo
// Author           : KimCh
// Created          : 2016-10-12
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AliasAttribute))]
public class AliasDrawer : PropertyDrawer
{ 
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var aliasAttribute = (AliasAttribute)base.attribute;
        EditorGUI.PropertyField(position, property, new GUIContent(aliasAttribute.alias), true);
    }
}
