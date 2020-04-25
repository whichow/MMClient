/** 
 *FileName:     M3EditorFileOperation.cs 
 *Author:        
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-12-27 
 *Description:    
 *History: 
*/
#if UNITY_EDITOR

using System.IO;
using UnityEngine;

namespace M3Editor
{
    public static class M3EditorFileOperation 
    {

        public static string OpenFile(/*string spath,*/string fileName,byte[] bytes, string extension)
        {
            Debug.Log(Application.dataPath);
            string path = UnityEditor.EditorUtility.SaveFilePanel("Load png Textures of Directory", Application.dataPath+ "/Res/Chessboard", fileName, extension);
            if (path.Length != 0)
            {
                File.WriteAllBytes(path, bytes);
                UnityEditor.AssetDatabase.Refresh();
            }
            return path;
        }

    }


}
#endif