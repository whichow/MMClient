///*******************************************************************************
// * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
// * 
// * Author:          Coamy
// * Created:	        2019/4/26 11:54:56
// * Description:     当是角色文件时, 导入fbx文件，设置相应的材质，删除冗余fbm文件夹，将所有材质都放在texture文件夹下
// * 
// * Update History:  
// * 
// *******************************************************************************/
//using System.IO;
//using UnityEditor;
//using UnityEngine;

//    public class FBXPostProcessor: AssetPostprocessor
//    {
//        void OnPostprocessModel(GameObject g)
//        {
//            // Only operate on FBX files
//            if (!assetPath.EndsWith("fbx", true, null))
//                return;

//            // 只有在filepath中有ResRaw\Characters下
//            if (!assetPath.Contains("ResRaw/Characters"))
//                return;

//            string fileName = Path.GetFileName(assetPath);
//            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(assetPath);
//            string fullDirName = Application.dataPath;
//            fullDirName = fullDirName.Remove(fullDirName.LastIndexOf("Asset"));
//            fullDirName += Path.GetDirectoryName(assetPath);
//            string fullFbmDirName = fullDirName + "/" + fileNameWithoutExt + ".fbm";
//            string fullMatDirName = fullDirName + "/Materials";
//            string fullTexDirName = fullDirName + "/Texture";
//            if (!Directory.Exists(fullTexDirName))
//                Directory.CreateDirectory(fullTexDirName);  // 如果没有，创建一个

//            if (Directory.Exists(fullFbmDirName))
//            {
//                // 将生成的材质贴图文件夹删掉，把贴图移到texture文件夹中去
//                string[] fileEntries = Directory.GetFiles(fullFbmDirName, "*.*", SearchOption.AllDirectories);
//                foreach (string picFile in fileEntries)
//                {
//                    //if(picFile.EndsWith("meta"))
//                    //    continue;

//                    string picFilepath = picFile;
//                    picFilepath = picFilepath.Replace('\\', '/');
//                    string picFileTexPath = fullTexDirName + "/" + Path.GetFileName(picFilepath);


//                    // 将贴图一道texture文件夹中，如果没有创建一个
//                    File.Delete(picFileTexPath);
//                    File.Move(picFilepath, picFileTexPath);
//                }
//            }
//            else
//            {
//                Debug.Log("[OnPostprocessModel] " + fullFbmDirName + " not found.");
//            }

//            try
//            {
//                // 删除fbm文件夹
//                if (Directory.Exists(fullFbmDirName))
//                    Directory.Delete(fullFbmDirName);

//                // 删除fbm meta
//                if (Directory.Exists(fullFbmDirName + ".meta"))
//                    Directory.Delete(fullFbmDirName);
//            }
//            catch
//            {
//                // 这里好像有时会滞后，但是行为是正确
//            }

//            /// 调整材质shader
//            string dirName = Application.dataPath;
//            string relMatPath;
//            Material mat;
//            string matPath = Path.GetDirectoryName(assetPath);
//            matPath += "/Materials";
//            string[] matFileEntries = Directory.GetFiles(fullMatDirName, "*.*", SearchOption.AllDirectories);
//            foreach (string matFile in matFileEntries)
//            {
//                if (!matFile.EndsWith(".mat"))
//                    continue;

//                relMatPath = matFile;
//                relMatPath = relMatPath.Replace('\\', '/');
//                relMatPath = relMatPath.Substring(relMatPath.IndexOf("Assets"), relMatPath.Length - relMatPath.IndexOf("Assets"));

//                mat = Resources.LoadAssetAtPath<Material>(relMatPath);
//                if (mat == null)
//                {
//                    Debug.LogError("[postProcess] Material is null! " + relMatPath);
//                }

//                if (mat.name.Contains("_al") && mat.name.Contains("_bs"))
//                {
//                    mat.shader = Shader.Find("X/Transparent/Bumped Diffuse Lighted Cutout");
//                }
//                else
//                {
//                    if (mat.name.Contains("_al"))
//                    {
//                        mat.shader = Shader.Find("X/Transparent/Diffuse ZWrite");
//                    }
//                    if (mat.name.Contains("_bs"))
//                    {
//                        mat.shader = Shader.Find("X/Double Face/Diffuse");
//                    }
//                }
//            }

//            //matPath = Path.GetDirectoryName(assetPath);
//            //Material mt = g.

//        }


//    }
