// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "EditorHelper" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.Match3;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
public class EditorHelper : MonoBehaviour
{
    // 复制资源路径到剪贴板  
    [MenuItem("Assets/Copy Asset Path to ClipBoard")]
    static void CopyAssetPath2Clipboard()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
        var text2Editor = new TextEditor
        {
            text = path
        };
        text2Editor.OnFocus();
        text2Editor.Copy();
    }

    // 复制资源路径到剪贴板  
    [MenuItem("Assets/Copy Transfrom Path to ClipBoard &C")]
    static void CopyTransfromPath2Clipboard()
    {
        var transform = Selection.activeTransform;
        if (!transform)
        {
            return;
        }
        string path = transform.name;
        while (transform.parent)
        {
            transform = transform.parent;
            path = transform.name + "/" + path;
        }
        path = "\"" + path + "\"";
        var text2Editor = new TextEditor
        {
            text = path
        };
        text2Editor.OnFocus();
        text2Editor.Copy();
    }

    [MenuItem("Game/保存区域")]
    static void SerialzeObj()
    {
        var path = @"Assets\Res\Building\Area";
        var guids = AssetDatabase.FindAssets("t:GameObject", new string[] { path });
        System.Text.StringBuilder objText = new System.Text.StringBuilder();
        foreach (var guid in guids)
        {
            var objPath = AssetDatabase.GUIDToAssetPath(guid);
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(objPath);
            if (obj.activeSelf)
            {
                var areaData = obj.GetComponent<Game.Build.AreaData>();
                if (areaData)
                {
                    var text = JsonUtility.ToJson(areaData, true);
                    objText.Append(text);
                    objText.Append(',');
                }
            }
        }

        var jsonText = "{\"Area\":[" + objText.ToString(0, objText.Length - 1) + "]}";
        var savePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Area.txt");
        System.IO.File.WriteAllText(savePath, jsonText);
    }

    /// <summary>Replaces the script template.</summary>
    [MenuItem("Game/替换模板")]
    public static void ReplaceScriptTemplate()
    {
        var sourPath = Application.dataPath + @"\Editor\Templates\81-C# Script-NewBehaviourScript.cs.txt";
        var destPath = EditorApplication.applicationContentsPath + @"\Resources\ScriptTemplates\81-C# Script-NewBehaviourScript.cs.txt";

        System.IO.File.Copy(sourPath, destPath, true);
    }

    [MenuItem("Game/更换帐号")]
    static void ChangeAccount()
    {
        PlayerPrefs.SetString("account_postfix", DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    //[MenuItem("Assets/Game", false, 900)]
    static void GameMenu()
    {

    }

    /// <summary>Opens the document path.</summary>
    [MenuItem("Game/存档目录")]
    public static void OpenDocPath()
    {
        Object[] UnityAssets = AssetDatabase.LoadAllAssetsAtPath("Resources/unity_builtin_extra");
        foreach (var asset in UnityAssets)
        {
            try
            {
                var newa = Object.Instantiate(asset);
                AssetDatabase.CreateAsset(newa, "Assets/GameRes/buildin");
            }
            catch
            {

            }
            // create asset...
        }
        //Check();
        //System.Diagnostics.Process.Start(Application.persistentDataPath);
        //System.Diagnostics.Process.Start(Application.temporaryCachePath);
        ////System.Diagnostics.Process.Start(Application.temporaryCachePath);
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
    }

    public static void Check()
    {
        var files = System.IO.Directory.GetFiles("ProjectSettings", "*.*", System.IO.SearchOption.AllDirectories);
        foreach (var file in files)
        {
            using (var sr = new System.IO.StreamReader(file))
            {
                while (sr.Peek() >= 0)
                {
                    var line = sr.ReadLine();
                    if (line.Contains("<<<"))
                    {
                        Debug.Log(file);
                    }
                }
            }
        }
    }

    [MenuItem("Game/命名空间")]
    public static void AddNameSpace()
    {
        var sObj = Selection.activeObject;
        if (sObj)
        {
            var rootPath = AssetDatabase.GetAssetPath(sObj);
            var folders = new List<string> { rootPath };
            folders.AddRange(System.IO.Directory.GetDirectories(folders[0], "*", System.IO.SearchOption.AllDirectories));
            var guids = AssetDatabase.FindAssets("t:MonoScript", folders.ToArray());
            for (int i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                var script = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
                var sr = new System.IO.StringReader(script.text);
                var sw = new System.IO.StringWriter();

                bool writed = false;
                var line = sr.ReadLine();
                while (line != null)
                {
                    if (!writed)
                    {
                        if (line.StartsWith("using"))
                        {
                            sw.WriteLine("namespace SpineOld");
                            sw.WriteLine("{");
                            writed = true;
                        }
                    }
                    sw.WriteLine(line);
                    line = sr.ReadLine();
                }

                if (writed)
                {
                    sw.WriteLine("}");
                    System.IO.File.WriteAllText(assetPath, sw.ToString());
                }
            }
        }
    }

    [MenuItem("Game/修复Spine")]
    public static void RepairSpine()
    {
        var sObj = Selection.activeObject;
        if (sObj)
        {
            var rootPath = AssetDatabase.GetAssetPath(sObj);
            var jsonFiles = Directory.GetFiles(rootPath, "*.json", SearchOption.AllDirectories);
            foreach (string jsonFile in jsonFiles)
            {
                var content = File.ReadAllText(jsonFile);
                content = content.Replace("skinnedmesh", "mesh");
                File.WriteAllText(jsonFile, content);
            }
        }
    }

    //[MenuItem("Game/三消地图打包")]
    //public static void BuildMatchMap()
    //{
    //    string saveFile = "Assets/Res/Global/Map.prefab";
    //    Debug.Log(saveFile);
    //    TextAsset[] mapsTextAsset= Resources.LoadAll<TextAsset>("Map");
    //    GameObject go = new GameObject("Map");
    //    go.AddComponent<M3Map>().maps = mapsTextAsset;
    //    PrefabUtility.CreatePrefab(saveFile, go, ReplacePrefabOptions.ConnectToPrefab);
    //}

}

