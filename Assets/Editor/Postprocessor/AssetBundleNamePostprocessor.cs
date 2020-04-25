/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/21 17:55:34
 * Description:     自动设置AB名
 * 
 * Update History:  
 * 
 *******************************************************************************/
using UnityEditor;
using UnityEngine;

namespace Game
{
    //public class AssetBundleNamePostprocessor : AssetPostprocessor
    //{
    //    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    //    {
    //        foreach (var str in importedAssets)
    //        {
    //            if (!str.Contains("Assets/Res/"))
    //            {
    //                continue;
    //            }

    //            Debug.Log("Reimported Asset: " + str);
    //            if (!str.EndsWith(".cs"))
    //            {
    //                AssetImporter importer = AssetImporter.GetAtPath(str);
    //                importer.assetBundleName = str;
    //            }
    //        }

    //        foreach (var str in deletedAssets)
    //        {
    //            Debug.Log("Deleted Asset: " + str);
    //            if (!str.EndsWith(".cs"))
    //            {
    //            }
    //        }

    //        for (var i = 0; i < movedAssets.Length; i++)
    //        {
    //            Debug.Log("Moved Asset: " + movedAssets[i] + " from: " + movedFromAssetPaths[i]);
    //            AssetImporter importer = AssetImporter.GetAtPath(movedAssets[i]);
    //            Debug.Log(importer.assetBundleName);
    //        }
    //    }

    //}
}
