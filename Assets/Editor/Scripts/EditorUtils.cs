using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorUtils : MonoBehaviour
{
    #region EDITOR

    private static GUIStyle _BoxStyle = null;
    private static GUIStyle _LabelStyle = null;
    private static GUIStyle _TextAreaStyle = null;
    private static GUIStyle _SearchTextFieldStyle = null;
    private static GUIStyle _SearchCancelButtonStyle = null;
    private static GUIStyle _SearchCancelButtonEmptyStyle = null;

    internal static GUIStyle BoxStyle
    {
        get
        {
            if (_BoxStyle == null)
            {
                _BoxStyle = new GUIStyle(GUI.skin.box)
                {
                    fontSize = 12,
                    richText = true
                };
            }
            return _BoxStyle;
        }
    }

    internal static GUIStyle LabelStyle
    {
        get
        {
            if (_LabelStyle == null)
            {
                _LabelStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 14,
                    richText = true
                };
            }
            return _LabelStyle;
        }
    }

    /// <summary>Gets the text area style.</summary>
    internal static GUIStyle TextAreaStyle
    {
        get
        {
            if (_TextAreaStyle == null)
            {
                _TextAreaStyle = GUI.skin.textArea;
            }
            return _TextAreaStyle;
        }
    }

    internal static GUIStyle SearchTextField
    {
        get
        {
            if (_SearchTextFieldStyle == null)
            {
                _SearchTextFieldStyle = GUI.skin.FindStyle("SearchTextField");
            }
            return _SearchTextFieldStyle;
        }
    }

    internal static GUIStyle SearchCancelButton
    {
        get
        {
            if (_SearchCancelButtonStyle == null)
            {
                _SearchCancelButtonStyle = GUI.skin.FindStyle("SearchCancelButton");
            }
            return _SearchCancelButtonStyle;
        }
    }

    internal static GUIStyle SearchCancelButtonEmpty
    {
        get
        {
            if (_SearchCancelButtonEmptyStyle == null)
            {
                _SearchCancelButtonEmptyStyle = GUI.skin.FindStyle("SearchCancelButtonEmpty");
            }
            return _SearchCancelButtonEmptyStyle;
        }
    }

    internal static void DrawTips()
    {
        BeginArea("<color=white>友  情  提  示</color>");
        GUILayout.Space(16f);
        GUILayout.Label("<color=yellow> 1.请打开编辑器窗口 \n\n 2.请先保存为prefab再编辑</color>", LabelStyle);
        GUILayout.Space(16f);
        EndArea();
    }

    internal static void BeginArea(string title)
    {
        GUILayout.BeginVertical(title, BoxStyle);
        GUILayout.Space(16f);
        GUILayout.BeginVertical(TextAreaStyle);
    }

    internal static void EndArea()
    {
        GUILayout.EndVertical();
        GUILayout.Space(2f);
        GUILayout.EndVertical();
        GUILayout.Space(2f);
    }

    #endregion
}
