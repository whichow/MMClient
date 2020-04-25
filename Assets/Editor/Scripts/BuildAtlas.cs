// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildAtlas : MonoBehaviour
{

    [MenuItem("Tools/Create Sprite Atlas")]
    static void CreateSpriteAtlas()
    {
        var obj = Selection.activeObject;
        CreateSpriteAtlasGO(AssetDatabase.GetAssetPath(obj));
    }

    [MenuItem("Tools/Icon/Create Icon Prefab (Form Directory)")]
    static void CreateIconPrefab()
    {
        if (Selection.objects.Length >= 1)
        {
            foreach (UnityEngine.Object o in Selection.objects)
            {
                string path = AssetDatabase.GetAssetPath(o);
                if ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory) // 如果是目录
                {
                    if (path.Contains("Icons"))
                    {
                        Debug.Log(path);
                        var go = CreateSpriteAtlasGO(path);
                        SavePrefab(go);
                    }
                }
            }
        }
    }

    [MenuItem("Tools/Icon/Create All Icon Prefab")]
    static void CreateAllIconPrefab()
    {
        string[] dirs = Directory.GetDirectories(Application.dataPath + "/ResRaw/Icons");
        foreach (var dir in dirs)
        {
            string path = dir.Substring(dir.IndexOf("Assets"));
            Debug.Log(path);
            var go = CreateSpriteAtlasGO(path);
            SavePrefab(go);
        }
    }

    private static GameObject CreateSpriteAtlasGO(string path)
    {
        path = path.Replace("\\", "/");
        Debug.Log(path);

        string name = path.Substring(path.LastIndexOf("/") + 1, path.Length - path.LastIndexOf("/") - 1);

        GameObject go = null;
        var paths = new string[] { path };
        var guids = AssetDatabase.FindAssets("t:Sprite", paths);
        if (guids != null && guids.Length > 0)
        {
            var sprites = new List<Sprite>();
            for (int i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (EditorUtility.DisplayCancelableProgressBar("CreateSpriteAtlasGO", assetPath, (float)i/ guids.Length))
                {
                    EditorUtility.ClearProgressBar();
                    return null;
                }

                var texImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (texImporter && string.IsNullOrEmpty(texImporter.spritePackingTag))
                {
                    texImporter.spritePackingTag = name;
                }
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                sprites.Add(sprite);
            }
            AssetDatabase.SaveAssets();
            go = new GameObject(name);
            go.AddComponent<KUIAtlas>().sprites = sprites.ToArray();
        }
        EditorUtility.ClearProgressBar();
        return go;
    }

    private static void SavePrefab(GameObject go)
    {
        if (go != null)
        {
            string savePath = "Assets/Res/Global/" + go.name + ".prefab";
            PrefabUtility.CreatePrefab(savePath, go);
            DestroyImmediate(go);
            Debug.Log("CreateIconPrefab complete: " + savePath);
        }
    }

}
