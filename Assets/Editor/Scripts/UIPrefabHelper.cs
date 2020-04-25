/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/7 10:25:33
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIPrefabHelper : MonoBehaviour
{
    [MenuItem("Tools/UI/UIPrefab检查替换问号图片")]
    public static void CheckUIImage()
    {
        var rSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/GameRes/Textures/UI/Icon/Icon_Cj_115.png");
        if (rSprite == null)
        {
            Debug.LogError("找不到问号图片");
            return;
        }

        var imageList = new List<Image>();
        var folders = new List<string> { "Assets/Res/UI" };
        folders.AddRange(System.IO.Directory.GetDirectories(folders[0], "*", System.IO.SearchOption.AllDirectories));
        var guids = AssetDatabase.FindAssets("t:GameObject", folders.ToArray());
        for (int i = 0; i < guids.Length; i++)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            EditorUtility.DisplayProgressBar("搜索预设图片组件", "搜索中..." + assetPath, i / (float)guids.Length);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            bool change = false;
            var gameObject = Instantiate(prefab);
            imageList.Clear();
            gameObject.GetComponentsInChildren(true, imageList);
            foreach (var image in imageList)
            {
                if (image.sprite)
                {
                    var ap = AssetDatabase.GetAssetPath(image.sprite);
                    if (!ap.StartsWith("Assets/GameRes/Textures/UI") && !ap.StartsWith("Resources"))
                    {
                        change = true;
                        image.sprite = rSprite;

                        var transform = image.transform;
                        string path = transform.name;
                        while (transform.parent)
                        {
                            transform = transform.parent;
                            path = transform.name + "/" + path;
                        }

                        Debug.Log("资源需要修改: " + path);
                    }
                }
            }
            try
            {
                if (change)
                {
                    // 自动修改Prefab
                    //PrefabUtility.ReplacePrefab(gameObject, prefab, ReplacePrefabOptions.Default);
                }
                DestroyImmediate(gameObject);
            }
            catch (Exception ex)
            {
                Debug.Log(ex + " : " + assetPath);
            }
        }
        EditorUtility.ClearProgressBar();
    }

    [MenuItem("Tools/UI/UIPrefab提取Text组件内文本")]
    public static void CheckUIText()
    {
        var strSet = new HashSet<string>();
        var textList = new List<Text>();
        var folders = new List<string> { "Assets/Res/UI" };
        folders.AddRange(System.IO.Directory.GetDirectories(folders[0], "*", System.IO.SearchOption.AllDirectories));
        var guids = AssetDatabase.FindAssets("t:GameObject", folders.ToArray());
        for (int i = 0; i < guids.Length; i++)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            EditorUtility.DisplayProgressBar("搜索预设文本组件", "搜索中..." + assetPath, i / (float)guids.Length);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            var tmpObject = Instantiate(prefab);
            textList.Clear();
            tmpObject.GetComponentsInChildren(true, textList);
            foreach (var text in textList)
            {
                strSet.Add(text.text);
            }
            DestroyImmediate(tmpObject);
        }

        var sb = new System.Text.StringBuilder();
        foreach (var item in strSet)
        {
            sb.AppendLine(item);
        }

        var savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/ui_text.txt";
        System.IO.File.WriteAllText(savePath, sb.ToString());
        Debug.Log(string.Format("已提取所有Text组件（{0}个）内文本 生成文件到: {1}", strSet.Count, savePath));

        EditorUtility.ClearProgressBar();
    }

}
