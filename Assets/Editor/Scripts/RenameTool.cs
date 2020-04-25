/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/28 18:44:58
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class RenameTool
    {
        [MenuItem("Tools/宠物FBX改名/RenamePetFBX (Form Directory)")]
        private static void SelectRenamePet()
        {
            UnityEngine.Object[] objects = Selection.objects;
            for (int i = 0; i < objects.Length; i++)
            {
                string path = AssetDatabase.GetAssetPath(objects[i]);
                if ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory) // 如果是目录
                {
                    string[] fileEntries = Directory.GetFiles(path, "*.fbx", SearchOption.AllDirectories);
                    if (fileEntries.Length > 0)
                    {
                        foreach (var item in fileEntries)
                        {
                            RenamePet(item);
                        }
                    }
                }
                else if (".fbx" == Path.GetExtension(path))
                {
                    RenamePet(path);
                }
            }
            AssetDatabase.Refresh();
        }

        private static void RenamePet(string path)
        {
            FileInfo fi = new FileInfo(path);
            path = path.Replace("2leg", "Pet");
            path = path.Replace("4leg", "Pet");
            fi.MoveTo(path);
            Debug.Log("Rename: " + path);
        }

    }
}
