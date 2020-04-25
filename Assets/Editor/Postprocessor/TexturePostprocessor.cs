// ***********************************************************************
// Company          : Kunpo
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
namespace Game
{
    using UnityEngine;
    using System.Collections;
    using UnityEditor;
    using System.IO;
    using System.Collections.Generic;

    /// <summary>
    /// UI图片导入解析类
    /// 在指定文件下，创建文件夹，会自动生成与新创建文件夹同名图集
    /// 在指定文件夹下导入图片资源时，对其进行解析。符合条件，自动添加进与文件夹同名的图集；不符合条件，不允许导入
    /// </summary>
    public class TexturePostprocessor : AssetPostprocessor
    {
        #region Method

        /// <summary>
        /// 图片导入
        /// </summary>
        public void OnPreprocessTexture()
        {
            if (assetPath.Contains("GameRes/Spines"))
            {
                var ti = assetImporter as TextureImporter;
                ti.textureType = TextureImporterType.Default;
                //ti.npotScale = TextureImporterNPOTScale.None;
                ti.mipmapEnabled = false;
                ti.isReadable = false;

                ti.textureCompression = TextureImporterCompression.Compressed;
            }

            if (assetPath.Contains("Textures/Match3"))
            {
                TextureImporter tir = assetImporter as TextureImporter;
                tir.textureType = TextureImporterType.Sprite;
                tir.spritePixelsPerUnit = 116f;
                tir.npotScale = TextureImporterNPOTScale.None;
                tir.mipmapEnabled = false;
                tir.isReadable = false;
                //tir.maxTextureSize = 2048;
                tir.textureCompression = TextureImporterCompression.Compressed;
            }
            else
            {
                //TextureImporter tir = assetImporter as TextureImporter;
                //tir.textureType = TextureImporterType.Sprite;
                //tir.spritePixelsPerUnit = 100f;
                //tir.npotScale = TextureImporterNPOTScale.None;
                //tir.mipmapEnabled = false;
                //tir.isReadable = true;
                //tir.maxTextureSize = 2048;
                //tir.textureCompression = TextureImporterCompression.CompressedHQ;

            }
        }

        public void OnPostprocessSprites(Texture2D texture, Sprite[] sprites)
        {
            //if (!assetPath.Contains("GameRes"))
            //{
            //    return;
            //}

            //var textureImporter = this.assetImporter as TextureImporter;
            //if (textureImporter.textureType == TextureImporterType.Sprite)
            //{
            //    //var tagName = Directory.GetParent(this.assetPath).Name;
            //    textureImporter.spritePackingTag = "m3_item_purple";
            //}

            //if (assetPath.Contains("GameRes"))
            //{
            //    var tagName = Directory.GetParent(assetPath).Name;

            //}
        }

        #endregion
        /*
        #region Model
        /// <summary>
        /// 图片信息数据结构
        /// </summary>
        public class TextureInfo
        {
            public string path;
            public Texture2D texture2D;
        }
        #endregion

        #region Method
        /// <summary>
        /// 图片导入
        /// </summary>
        public void OnPreprocessTexture()
        {
            EditorWindow w = EditorWindow.GetWindow(typeof(EditorWindow), false, "Server");
            if (w!=null && w.titleContent.text == "Server")
            {
                return;
            }
            if (assetPath.Contains(_AtlasTexturesPath))
            {
                TextureImporter tir = assetImporter as TextureImporter;
                tir.textureType = TextureImporterType.Advanced;
                tir.npotScale = TextureImporterNPOTScale.None;
                tir.mipmapEnabled = false;
                tir.isReadable = true;
                tir.maxTextureSize = 2048;
                tir.textureFormat = TextureImporterFormat.AutomaticTruecolor;
            }
            else if (assetPath.Contains(_AtlasPath))
            {
                TextureImporter tir1 = assetImporter as TextureImporter;
                tir1.textureType = TextureImporterType.Advanced;
                tir1.npotScale = TextureImporterNPOTScale.None;
                if (assetPath.Contains("_RGB"))
                {
                    tir1.textureFormat = TextureImporterFormat.AutomaticCompressed;
                    tir1.isReadable = false;
                }
                else if (assetPath.Contains("_Alpha"))
                {
                    tir1.textureFormat = TextureImporterFormat.Alpha8;
                    tir1.isReadable = false;
                }
                else
                {
                    tir1.alphaIsTransparency = true;
                    tir1.textureFormat = TextureImporterFormat.AutomaticTruecolor;
                     tir1.isReadable = true;
                }
                tir1.wrapMode = TextureWrapMode.Clamp;
                tir1.mipmapEnabled = false;
                tir1.maxTextureSize = 4096;
                
            }
            else if (assetPath.Contains(_AtlasRootTexturePath))
            {
                TextureImporter tir3 = assetImporter as TextureImporter;
                tir3.textureType = TextureImporterType.Advanced;
                tir3.npotScale = TextureImporterNPOTScale.None;
                tir3.mipmapEnabled = false;
                tir3.isReadable = true;
                tir3.maxTextureSize = 2048;
                tir3.textureFormat = TextureImporterFormat.AutomaticTruecolor;
            }
            else if(assetPath.Contains(_UITexturesPath))
            {
                TextureImporter tir2 = assetImporter as TextureImporter;
                tir2.textureType = TextureImporterType.Advanced;
                tir2.mipmapEnabled = false;
                tir2.maxTextureSize = 2048;
                if (assetPath.EndsWith(".jpg"))
                {
                   // tir2.textureFormat = TextureImporterFormat.AutomaticCompressed;
                   // tir2.npotScale = TextureImporterNPOTScale.ToSmaller;
                }
                else
                {
                   // tir2.npotScale = TextureImporterNPOTScale.None;
                    tir2.textureFormat = TextureImporterFormat.AutomaticTruecolor;
                }
            }
            else if (assetPath.Contains(_LightingTexturePath))
            {
                TextureImporter tir4 = assetImporter as TextureImporter;
                tir4.textureType = TextureImporterType.Advanced;
                tir4.npotScale = TextureImporterNPOTScale.None;
                tir4.mipmapEnabled = false;
                tir4.maxTextureSize = 512;
                tir4.isReadable = false;
                tir4.lightmap = true;
                tir4.wrapMode = TextureWrapMode.Clamp;
                tir4.filterMode = FilterMode.Bilinear;
                tir4.anisoLevel = 1;
                tir4.textureFormat = TextureImporterFormat.AutomaticCompressed;
            }
        }

        /// <summary>
        /// 图片导入
        /// </summary>
        public void OnPostprocessTexture(Texture2D texture)
        {
            EditorWindow w = EditorWindow.GetWindow(typeof(EditorWindow), false, "Server");
            if (w != null && w.titleContent.text == "Server")
            {
                return;
            }
            if (assetPath.Contains(_AtlasTexturesPath) && !assetPath.Contains(_IgnorPath) && !assetPath.Contains(_AtlasRootTexturePath))
            {
                TextureInfo info = new TextureInfo()
                {
                    texture2D = texture,
                    path = assetPath,
                };
                if (!_TextureInfo.Contains(info))
                {
                    _TextureInfo.Add(info);
                }
            }
        }
        #endregion

        #region Static
        /// <summary>
        /// 导入图片列表
        /// </summary>
        private static List<TextureInfo> _TextureInfo = new List<TextureInfo>();
        /// <summary>
        /// UI图片路径
        /// </summary>
        private static string _AtlasTexturesPath = "Assets/Textures/UI/";
        /// <summary>
        /// UI图片路径
        /// </summary>
        private static string _UITexturesPath = "Assets/Resources/UI/Texture/";
        /// <summary>
        /// UI忽略图片路径
        /// </summary>
        private static string _IgnorPath = "Assets/Textures/UI/Font/";
        /// <summary>
        /// UI图集路径
        /// </summary>
        private static string _AtlasPath = "Assets/Resources/UI/Atlas/";
        /// <summary>
        /// 图集分离带有透明通道源图片
        /// </summary>
        private static string _AtlasRootTexturePath = "Assets/Textures/AtlasRoot/";

        private static string _LightingTexturePath = "Assets/GameRes/Lightmaps/";
        /// <summary>
        /// 图集名称
        /// </summary>
        private static string _AtlasName;
        /// <summary>
        /// 图集最大尺寸
        /// </summary>
        private static int _AtlasMax = 2048;
        /// <summary>
        /// 图片最大尺寸
        /// </summary>
        private static int _TextureMax = 256;
        /// <summary>
        /// 分离图片大小参数
        /// </summary>
        private static float _SizeScale = 1f;
        /// <summary>
        /// 分离带有透明通道图片的开关
        /// </summary>
        private static bool _SeperateAlphaFlag = true;
        public static bool InTexture = false;
        /// <summary>
        /// 图片信息列表
        /// </summary>
        private static List<Hashtable> _SpriteList = new List<Hashtable>();
        /// <summary>
        /// 当前图集
        /// </summary>
        private static UIAtlas _CurrentAtlas;
        /// <summary>
        /// 是否正在创建图集
        /// </summary>
        private static bool _IsCreate;

        /// <summary>
        /// 资源更新
        /// </summary>
        /// <param name="importedAssets">外部导入资源路径</param>
        /// <param name="deletedAssets">删除资源路径</param>
        /// <param name="movedAssets">内部资源移动目的地路径</param>
        /// <param name="movedFromAssetPaths">内部资源移动来源路径</param>
        public static void OnPostprocessAllAssets(string[] importedAssets,
                        string[] deletedAssets,
                        string[] movedAssets,
                        string[] movedFromAssetPaths)
        {
            EditorWindow w = EditorWindow.GetWindow(typeof(EditorWindow), false, "Server");
            if (w != null && w.titleContent.text == "Server")
            {
                return;
            }
           // if (InTexture)
           // {
                foreach (var importedAsset in importedAssets)
                {
                    if (importedAsset.Contains("UIData.xls"))
                    {
                        _SpriteList.Clear();
                    }
                    if (importedAsset.Contains(_AtlasTexturesPath) && !importedAsset.Contains(_IgnorPath)
                        && !GetFileNameByIm(importedAsset).Contains(".") && !importedAsset.Contains(_AtlasRootTexturePath))
                    {
                        CreateAtlas(GetFileNameByIm(importedAsset));
                    }
                }
                CheckMove(movedAssets, movedFromAssetPaths);
                CheckTextures();
                CheckDelete(deletedAssets);
                ReadToAtlas();
               // InTexture = false;
            //}
        }

        public static void Seperate()
        {
            Object[] objs = Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
            if (objs != null && objs.Length > 0)
            {
                for (int k = 0; k < objs.Length; k++)
                {
                    string path = AssetDatabase.GetAssetPath(objs[k]);
                    Texture2D sourcetex = objs[k] as Texture2D;
                    if (HasAlphaChannel(sourcetex))
                    {
                        Texture2D rgbTex = new Texture2D(sourcetex.width, sourcetex.height, TextureFormat.RGB24, true);
                        Texture2D alphaTex = new Texture2D((int)(sourcetex.width * _SizeScale), (int)(sourcetex.height * _SizeScale)
                                      , TextureFormat.Alpha8, true);
                        for (int i = 0; i < sourcetex.width; ++i)
                            for (int j = 0; j < sourcetex.height; ++j)
                            {
                                Color color = sourcetex.GetPixel(i, j);
                                Color rgbColor = new Color(color.r, color.g, color.b);
                                Color alphaColor = new Color(0f, 0f, 0f, color.a);
                                rgbTex.SetPixel(i, j, rgbColor);
                                alphaTex.SetPixel((int)(i * _SizeScale), (int)(j * _SizeScale), alphaColor);
                            }

                        rgbTex.Apply();
                        alphaTex.Apply();

                        byte[] bytes = null;
                        bytes = rgbTex.EncodeToPNG();
                        File.WriteAllBytes(GetRGBTexPath(path), bytes);
                        bytes = alphaTex.EncodeToPNG();
                        File.WriteAllBytes(GetAlphaTexPath(path), bytes);
                        bytes = null;
                        AssetDatabase.MoveAsset(path, _AtlasRootTexturePath + GetFileNameByIm(path));
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }
            }
        }

        /// <summary>
        /// 分离带有透明通道的图片
        /// </summary>
        /// <param name="path"></param>
        /// <param name="atlas"></param>
        public static void Seperate(string path, UIAtlas atlas = null)
        {
            Texture2D sourcetex = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
            if (!sourcetex)
            {
                EditorUtility.DisplayDialog("错误的操作", "找不到图片 path["+path+"]", "OK");
                return;
            }
            if (!HasAlphaChannel(sourcetex))
            {
                EditorUtility.DisplayDialog("错误的操作", sourcetex + "图片没有Alpha通道", "OK");
                return;
            }

             string atlasName = GetNameWithoutEx(path);
            string newPath = _AtlasPath + atlasName + ".png";
            bool needWriteRGB = false;
            bool needWriteAlpha = false;
            Texture2D rgbTex = null;// AssetDatabase.LoadAssetAtPath<Texture2D>(GetRGBTexPath(newPath));
            if (rgbTex == null)
            {
                needWriteRGB = true;
                rgbTex = new Texture2D(sourcetex.width, sourcetex.height, TextureFormat.RGB24, true);
            }
            else
            {
                rgbTex.Resize(sourcetex.width, sourcetex.height);
            }
            Texture2D alphaTex = null;// AssetDatabase.LoadAssetAtPath<Texture2D>(GetAlphaTexPath(newPath));
            if (alphaTex == null)
            {
                needWriteAlpha = true;
                alphaTex = new Texture2D((int)(sourcetex.width * _SizeScale), (int)(sourcetex.height * _SizeScale)
                    , TextureFormat.Alpha8, true);
            }
            else
            {
                alphaTex.Resize(sourcetex.width, sourcetex.height);
            }
            for (int i = 0; i < sourcetex.width; ++i)
                for (int j = 0; j < sourcetex.height; ++j)
                {
                    Color color = sourcetex.GetPixel(i, j);
                    Color rgbColor = new Color(color.r, color.g, color.b);
                    Color alphaColor = new Color(0f, 0f, 0f,color.a);
                   // alphaColor.r = color.a;
                    //alphaColor.g = color.a;
                   // alphaColor.b = color.a;
                    rgbTex.SetPixel(i, j, rgbColor);
                    alphaTex.SetPixel((int)(i * _SizeScale), (int)(j * _SizeScale), alphaColor);
                }

            rgbTex.Apply();
            alphaTex.Apply();

            byte[] bytes = null;
            bytes = rgbTex.EncodeToPNG();
            File.WriteAllBytes(GetRGBTexPath(newPath), bytes);
            bytes = alphaTex.EncodeToPNG();
            File.WriteAllBytes(GetAlphaTexPath(newPath), bytes);
            bytes = null;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            if (atlas == null) atlas = Resources.Load<UIAtlas>("UI/Atlas/" + atlasName);
            if (atlas != null)
            {
                SetAtlasShader(atlas);
            }
        }

        /// <summary>
        /// 创建图集
        /// </summary>
        /// <param name="atlasName">图集名称</param>
        private static void CreateAtlas(string atlasName)
        {
            if (!_IsCreate)
            {
                _IsCreate = true;
                CreateAtlasByName(atlasName);
            }
        }

        /// <summary>
        /// 将符合条件的图片添加到图集
        /// </summary>
        private static void ReadToAtlas()
        {
            UIAtlas a = null;
            _CurrentAtlas = null;
            if (_TextureInfo != null && _TextureInfo.Count > 0)
            {
                string atlasName = GetLocalFileName(_TextureInfo[0].path);
                if (atlasName != null)
                {
                    a = Resources.Load<UIAtlas>("UI/Atlas/" + atlasName);
                }
                if (a != null)
                {
                    NGUISettings.atlas = a;
                    string newPath = _SeperateAlphaFlag ? _AtlasRootTexturePath + a.name + ".png" : _AtlasPath + a.name + ".png";
                    a.spriteMaterial.mainTexture = AssetDatabase.LoadAssetAtPath<Texture>(newPath);
                    List<Texture> list = new List<Texture>();
                    for (int i = 0; i < _TextureInfo.Count; i++)
                    {
                        list.Add(_TextureInfo[i].texture2D as Texture);
                    }
                    List<UIAtlasMaker.SpriteEntry> listS = UIAtlasMaker.CreateSprites(list);
                    list.Clear();
                    List<string> listStr = new List<string>();
                    for (int i = 0; i < listS.Count; i++)
                    {
                        UIAtlasMaker.SpriteEntry se = listS[i];
                        if (!listStr.Contains(se.name))
                        {
                            listStr.Add(se.name);
                        }
                       // SetBorder(ref se);
                    }
                    _TextureInfo.Clear();
                    if (a != null)
                    {
                        _CurrentAtlas = a;
                        _AtlasName = a.name;
                        if (!_IsCreate)
                        {
                            string pathTex = _SeperateAlphaFlag ? _AtlasRootTexturePath + a.name + ".png" : _AtlasPath + a.name + ".png";
                            if (a.spriteMaterial.mainTexture == null)
                            {
                                UpdateAtlasAM(listS, false);
                                RefreshBorder(listStr);
                            }
                            else
                            {
                                UpdateAtlasAM(listS, true);
                                _CurrentAtlas = Resources.Load<UIAtlas>("UI/Atlas/" + _AtlasName);
                                NGUISettings.atlas = _CurrentAtlas;
                                RefreshBorder(listStr);
                                _CurrentAtlas = Resources.Load<UIAtlas>("UI/Atlas/" + _AtlasName);
                                NGUISettings.atlas = _CurrentAtlas;
                                CheckAtlas(listStr, _AtlasTexturesPath + _CurrentAtlas.name);
                                NGUIEditorTools.RepaintSprites();
                                AssetDatabase.SaveAssets();
                                AssetDatabase.Refresh();
                            }
                            if (_SeperateAlphaFlag)
                            {
                                Seperate(pathTex, a);
                            }
                        }
                    }
                }
                else
                {
                    CreateAtlas(atlasName);
                }
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

       // private static void 
        /// <summary>
        /// 检查文件以及资源删除
        /// </summary>
        private static void CheckDelete(string[] deleteAssets)
        {
            if (deleteAssets != null && deleteAssets.Length > 0)
            {
                List<string> path = new List<string>();
                string atlasName = "";
                bool deleteTextrue = true;
                bool ignore = false;
                int state = System.Array.FindIndex(deleteAssets, (d) => !d.Contains("."));
                if (state != -1)
                {
                    deleteTextrue = false;
                    if (atlasName == "")
                    {
                        atlasName = GetFileNameByIm(deleteAssets[state]);
                        if (atlasName == "AtlasRoot")
                        {
                            ignore = true;
                        }
                    }
                }
                else
                {
                    foreach (var deletedAsset in deleteAssets)
                    {
                        if (deletedAsset.Contains(_AtlasTexturesPath) && !deletedAsset.Contains(_IgnorPath) && !deletedAsset.Contains(_AtlasRootTexturePath))
                        {
                            if (atlasName == "")
                            {
                                atlasName = GetLocalFileName(deletedAsset);
                            }
                            path.Add(GetNameWithoutEx(deletedAsset));
                        }
                    }
                }
                if (atlasName != "" && !ignore)
                {
                   var a = Resources.Load<UIAtlas>("UI/Atlas/" + atlasName);
                   if (a != null)
                   {
                       NGUISettings.atlas = a;
                       if (!deleteTextrue)
                       {
                           if (EditorUtility.DisplayDialog("删除文件", "是否同时删除同名图集", "是"))
                           {
                               string pathN = _AtlasPath + atlasName;
                               string pathTex = _SeperateAlphaFlag ? _AtlasRootTexturePath + atlasName : pathN;
                               AssetDatabase.DeleteAsset(pathTex + ".png");
                               AssetDatabase.DeleteAsset(pathN + "_RGB.png");
                               AssetDatabase.DeleteAsset(pathN + "_Alpha.png");
                               AssetDatabase.DeleteAsset(pathN + ".prefab");
                               AssetDatabase.DeleteAsset(pathN + ".mat");
                               AssetDatabase.SaveAssets();
                               AssetDatabase.Refresh();
                           }
                       }
                       else
                       {
                           if (path != null && path.Count > 0)
                           {
                               string pathTex = _SeperateAlphaFlag ? _AtlasRootTexturePath + a.name + ".png" : _AtlasPath + a.name + ".png";
                               a.spriteMaterial.mainTexture = AssetDatabase.LoadAssetAtPath<Texture>(pathTex);
                               List<UIAtlasMaker.SpriteEntry> sprites = new List<UIAtlasMaker.SpriteEntry>();
                               UIAtlasMaker.ExtractSprites(a, sprites);
                               EditorUtility.ClearProgressBar();
                               int deleteState = System.Array.FindIndex(sprites.ToArray(), (s) => path.Contains(s.name));
                               if (deleteState != -1)
                               {
                                   if (EditorUtility.DisplayDialog("删除图片", "是否同时删除图集中的图片", "是"))
                                   {
                                       for (int i = sprites.Count; i > 0; )
                                       {
                                           UIAtlasMaker.SpriteEntry ent = sprites[--i];
                                           if (path.Contains(ent.name))
                                               sprites.RemoveAt(i);
                                       }
                                       UIAtlasMaker.UpdateAtlas(a, sprites);
                                       path.Clear();
                                       NGUIEditorTools.RepaintSprites();
                                       AssetDatabase.SaveAssets();
                                       AssetDatabase.Refresh();
                                       if (_SeperateAlphaFlag)
                                       {
                                           Seperate(pathTex, a);
                                       }
                                   }
                               }
                               else
                               {
                                   path.Clear();
                               }
                           }
                       }
                   }
                }
            }
        }

        /// <summary>
        /// 检查资源移动
        /// </summary>
        /// <param name="movedAssets"></param>
        /// <param name="movedFromAssetPaths"></param>
        private static void CheckMove(string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (movedAssets != null && movedFromAssetPaths != null && movedFromAssetPaths.Length > 0 
                && movedFromAssetPaths.Length > 0 && movedFromAssetPaths.Length == movedAssets.Length)
            {
                for (int i = 0; i < movedAssets.Length; i++)
                {
                    if ((movedAssets[i].Contains(".jpg") || movedAssets[i].Contains(".png")) && movedAssets[i].Contains(_AtlasTexturesPath) 
                        && !movedAssets[i].Contains(_IgnorPath) && !movedAssets[i].Contains(_AtlasRootTexturePath))
                    {
                        Texture2D tex = AssetDatabase.LoadAssetAtPath(movedAssets[i], typeof(Texture2D)) as Texture2D;
                        if (tex != null)
                        {
                            if (tex.width*tex.height > _TextureMax * _TextureMax)
                            {
                                if (EditorUtility.DisplayDialog("图片大爆炸", tex.name + "  图片尺寸:" + tex.width + "x" + tex.height, "取消操作"))
                                {
                                    AssetDatabase.MoveAsset(movedAssets[i], movedFromAssetPaths[i]);
                                }
                            }
                            else
                            {
                                TextureInfo info = new TextureInfo()
                                {
                                    texture2D = tex,
                                    path = movedAssets[i],
                                };
                                if (!_TextureInfo.Contains(info))
                                {
                                    _TextureInfo.Add(info);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 检查导入图片尺寸
        /// </summary>
        private static void CheckTextures()
        {
            if (_TextureInfo != null && _TextureInfo.Count > 0)
            {
                foreach (TextureInfo texture in _TextureInfo.ToArray())
                {
                    if (texture.path.Contains(_AtlasTexturesPath) && AssetDatabase.LoadMainAssetAtPath(texture.path) != null)
                    {
                        texture.texture2D = AssetDatabase.LoadMainAssetAtPath(texture.path) as Texture2D;
                        if (texture.texture2D != null)
                        {
                            if (texture.texture2D.width *texture.texture2D.height > _TextureMax *_TextureMax)
                            {
                                EditorUtility.DisplayDialog("图片大爆炸", GetFileName(texture.path) + "  图片尺寸:" + texture.texture2D.width + "x" + texture.texture2D.height, "取消操作");
                                AssetDatabase.DeleteAsset(texture.path);
                                _TextureInfo.Remove(texture);
                            }
                        }
                    }
                }
            }
        }

        private static void RefreshBorder(List<string> listStr)
        {
            UIAtlas a = _CurrentAtlas;
            if (a != null)
            {
                List<UIAtlasMaker.SpriteEntry> enS = new List<UIAtlasMaker.SpriteEntry>();
                UIAtlasMaker.ExtractSprites(a, enS);
                for (int i = enS.Count; i > 0; )
                {
                    UIAtlasMaker.SpriteEntry ent = enS[--i];
                    if (listStr.Contains(ent.name))
                    {
                        SetBorder(ref ent);
                    }
                }
                UIAtlasMaker.UpdateAtlas(a, enS);
                NGUIEditorTools.RepaintSprites();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// 检查图集尺寸
        /// </summary>
        private static void CheckAtlas(List<string> listStr, string filePath)
        {
            UIAtlas a = _CurrentAtlas;
            if (a != null)
            {
                Texture t = a.texture;
                if (t != null)
                {
                    if (t.width > _AtlasMax || t.height > _AtlasMax)
                    {
                        if (EditorUtility.DisplayDialog("图集大爆炸", "图集尺寸大于" + _AtlasMax + "x" + _AtlasMax, "取消操作"))
                        {
                            List<UIAtlasMaker.SpriteEntry> enS = new List<UIAtlasMaker.SpriteEntry>();
                            UIAtlasMaker.ExtractSprites(a, enS);
                            for (int i = enS.Count; i > 0; )
                            {
                                UIAtlasMaker.SpriteEntry ent = enS[--i];
                                if (listStr.Contains(ent.name))
                                {
                                    enS.RemoveAt(i);
                                }
                            }
                            UIAtlasMaker.UpdateAtlas(a, enS);
                            NGUIEditorTools.RepaintSprites();
                            for (int i = 0; i < listStr.Count; i++)
                            {
                                AssetDatabase.DeleteAsset(filePath + "/" + listStr[i] + ".png");
                            }
                            listStr.Clear();
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 刷新图集
        /// </summary>
        private static void UpdateAtlasAM(List<UIAtlasMaker.SpriteEntry> sprites, bool keepSprites)
        {
            if (sprites.Count > 0)
            {
                if (keepSprites) UIAtlasMaker.ExtractSprites(_CurrentAtlas, sprites);
                UIAtlasMaker.UpdateAtlas(_CurrentAtlas, sprites);
            }
            else
            {
                UIAtlasMaker.UpdateAtlas(_CurrentAtlas, sprites);
            }
            if (keepSprites)
            {
                NGUIEditorTools.UpgradeTexturesToSprites(_CurrentAtlas);
            }
            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// 创建图集图片
        /// </summary>
        private static bool UpdateTextureAM(UIAtlas atlas)
        {
            Texture2D tex = atlas.texture as Texture2D;
            string oldPath = (tex != null) ? AssetDatabase.GetAssetPath(tex.GetInstanceID()) : "";
            string newPath = _SeperateAlphaFlag ? _AtlasRootTexturePath + atlas.name + ".png" : NGUIEditorTools.GetSaveableTexturePath(atlas);
            if (System.IO.File.Exists(newPath))
            {
#if !UNITY_4_1 && !UNITY_4_0 && !UNITY_3_5
                if (!AssetDatabase.IsOpenForEdit(newPath))
                {
                    Debug.LogError(newPath + " is not editable. Did you forget to do a check out?");
                    return false;
                }
#endif
                System.IO.FileAttributes newPathAttrs = System.IO.File.GetAttributes(newPath);
                newPathAttrs &= ~System.IO.FileAttributes.ReadOnly;
                System.IO.File.SetAttributes(newPath, newPathAttrs);
            }
            bool newTexture = (tex == null || oldPath != newPath);
            if (newTexture)
            {
                tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
               // tex.alphaIsTransparency = true;
            }
            else
            {
                tex = NGUIEditorTools.ImportTexture(oldPath, true, false, false);
            }
            byte[] bytes = tex.EncodeToPNG();
            System.IO.File.WriteAllBytes(newPath, bytes);
            bytes = null;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            return true;
        }

        /// <summary>
        /// 通过名称创建图集
        /// </summary>
        /// <param name="atlas"></param>
        /// <param name="sprites"></param>
        private static void CreateAtlasByName(string atlas)
        {
            string path = _AtlasPath + atlas + ".prefab";
            if (!string.IsNullOrEmpty(path))
            {
                NGUISettings.currentPath = System.IO.Path.GetDirectoryName(path);
                GameObject go = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
                if (go == null)
                {
                    string matPath = path.Replace(".prefab", ".mat");
                    Material mat = AssetDatabase.LoadAssetAtPath(matPath, typeof(Material)) as Material;
                    if (mat == null)
                    {
                       // Shader shader = Shader.Find(NGUISettings.atlasPMA ? "Unlit/Premultiplied Colored" : "Unlit/Transparent Colored");
                        Shader shader = Shader.Find("Unlit/Transparent Colored");
                        mat = new Material(shader);
                        AssetDatabase.CreateAsset(mat, matPath);
                        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                        mat = AssetDatabase.LoadAssetAtPath(matPath, typeof(Material)) as Material;
                    }

                    string atlasName = path.Replace(".prefab", "");
                    atlasName = atlasName.Substring(path.LastIndexOfAny(new char[] { '/', '\\' }) + 1);
                    go = new GameObject(atlasName);
                    Object prefab = PrefabUtility.CreatePrefab(path, go);
                    (prefab as GameObject).AddComponent<UIAtlas>().spriteMaterial = mat;
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                    Object.DestroyImmediate(go);
                    _CurrentAtlas = (prefab as GameObject).GetComponent<UIAtlas>();
                    NGUISettings.atlas = _CurrentAtlas;
                    UpdateTextureAM(_CurrentAtlas);
                }
                _IsCreate = false;
            }
        }

        /// <summary>
        /// 读取UI数据表
        /// </summary>
        private static void LoadData()
        {
            object obj = AssetDatabase.LoadMainAssetAtPath("Assets/Editor/Excel/UIData.txt");
            if (obj != null)
            {
                TextAsset text = obj as TextAsset;
                Hashtable table = text.text.ToJsonTable();
                if (table != null && table.Count > 0)
                {
                    var list = table.GetList("sprite");
                    if (list != null && list.Count > 0)
                    {
                        for (int i = 0; i < list.Count - 1; i++)
                        {
                            var tmpLT = new Hashtable();
                            var tmpL0 = (ArrayList)list[0];
                            var tmpLi = (ArrayList)list[i + 1];
                            for (int j = 0; j < tmpL0.Count; j++)
                            {
                                tmpLT[tmpL0[j]] = tmpLi[j];
                            }
                            _SpriteList.Add(tmpLT);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置sprite's border
        /// </summary>
        private static void SetBorder(ref UIAtlasMaker.SpriteEntry se)
        {
            Vector4 border = GetBorder(se.name);
            se.borderLeft = (int)border.x;
            se.borderRight = (int)border.y;
            se.borderTop = (int)border.z;
            se.borderBottom = (int)border.w;
        }

        /// <summary>
        /// 返回指定路径字符串的文件名和扩展名
        /// </summary>
        private static string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <summary>
        /// 返回指定路径字符串的文件名(图集名称)
        /// </summary>
        private static string GetLocalFileName(string path)
        {
            string[] str = path.Split('/');
            if (str != null && str.Length > 1)
            {
                return str[str.Length - 2];
            }
            return null;
        }

        /// <summary>
        /// 返回图片名称（含扩展名）
        /// </summary>
        private static string GetFileNameByIm(string path)
        {
            string[] str = path.Split('/');
            if (str != null && str.Length > 1)
            {
                return str[str.Length - 1];
            }
            return null;
        }

        /// <summary>
        /// 获取不带后缀的文件名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string GetNameWithoutEx(string path)
        {
            string[] str = path.Split('/');
            if (str != null && str.Length > 1)
            {
                string s = str[str.Length - 1];
                string[] strs = s.Split('.');
                if (strs != null && strs.Length > 1)
                {
                    return strs[0];
                }
            }
            return null;
        }

        /// <summary>
        /// 获取sprite's border
        /// </summary>
        private static Vector4 GetBorder(string name)
        {
            Vector4 v4 = Vector4.zero;
            if (_SpriteList == null || _SpriteList.Count == 0)
            {
                LoadData();
            }
            if (_SpriteList != null && _SpriteList.Count > 0)
            {
                for (int i = 0; i < _SpriteList.Count; i++)
                {
                    Hashtable hash = _SpriteList[i];
                    if (name.ToInt() == hash.GetInt("id"))
                    {
                        v4.x = hash.GetInt("borderX");
                        v4.y = hash.GetInt("borderY");
                        v4.w = hash.GetInt("borderW");
                        v4.z = hash.GetInt("borderZ");
                    }
                }
            }
            return v4;
        }

        /// <summary>
        /// 设置图集材质球的shader
        /// </summary>
        /// <param name="atlas"></param>
        private static void SetAtlasShader(UIAtlas atlas)
        {
            NGUISettings.atlas = atlas;
            if (atlas.spriteMaterial == null)
            {
                return;
            }
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            string path = _AtlasPath + atlas.name;
            Shader shader = Shader.Find("Unlit/Transparent Masked");
            if (atlas.spriteMaterial.shader != shader)
            {
                atlas.spriteMaterial.shader = shader;
            }
            Texture t = AssetDatabase.LoadAssetAtPath<Texture>(path + "_RGB.png");
            if (t != null && atlas.spriteMaterial.GetTexture("_MainTex") != t)
            {
                atlas.spriteMaterial.SetTexture("_MainTex", t);
                if (atlas.spriteMaterial.mainTexture != t)
                {
                    atlas.spriteMaterial.mainTexture = t;
                }
            }
            t = AssetDatabase.LoadAssetAtPath<Texture>(path + "_Alpha.png");
            if (t != null && atlas.spriteMaterial.GetTexture("_Mask") != t)
            {
                atlas.spriteMaterial.SetTexture("_Mask", t);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 获取RGB图片的路径
        /// </summary>
        /// <param name="texPath"></param>
        /// <returns></returns>
        private static string GetRGBTexPath(string texPath)
        {
            return GetTexPath(texPath, "_RGB.");
        }

        /// <summary>
        /// 获取Alpha图片的路径
        /// </summary>
        /// <param name="texPath"></param>
        /// <returns></returns>
        private static string GetAlphaTexPath(string texPath)
        {
            return GetTexPath(texPath, "_Alpha.");
        }

        /// <summary>
        /// 获取图片的路径
        /// </summary>
        /// <param name="texPath"></param>
        /// <param name="texRole"></param>
        /// <returns></returns>
        private static string GetTexPath(string texPath, string texRole)
        {
            string result = texPath.Replace(".", texRole);
            string postfix = GetFilePostfix(texPath);
            return result.Replace(postfix, ".png");
        }

        /// <summary>
        /// including '.' eg ".tga", ".dds"
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static string GetFilePostfix(string filepath)
        {
            string postfix = "";
            int idx = filepath.LastIndexOf('.');
            if (idx > 0 && idx < filepath.Length)
                postfix = filepath.Substring(idx, filepath.Length - idx);
            return postfix;
        }

        /// <summary>
        /// 是否分离
        /// </summary>
        /// <param name="tex"></param>
        /// <returns></returns>
        private static bool HasAlphaChannel(Texture2D tex)
        {
            for (int i = 0; i < tex.width; ++i)
                for (int j = 0; j < tex.height; ++j)
                {
                    Color color = tex.GetPixel(i, j);
                    float alpha = color.a;
                    if (alpha < 1.0f - 0.001f)
                    {
                        return true;
                    }
                }
            return false;
        }
        #endregion
        */
    }
}