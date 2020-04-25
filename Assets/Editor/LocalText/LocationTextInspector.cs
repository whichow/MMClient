
using Game;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(LocationText))]
public class LocationTextInspector : TextEditor
{
    private int _newIDLanguage;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LocationText text = (LocationText)target;
        _newIDLanguage = text.IDLanguage;
        text.IDLanguage = EditorGUILayout.IntField("IDLanguage", _newIDLanguage);
    }
}