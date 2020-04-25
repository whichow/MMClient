/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/23 17:33:19
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using Game.DataModel;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Game
{
    public class ImageUtils
    {
        /// <summary>
        /// 照片服务器上传
        /// </summary>
        public static string PHOTO_SERVER_UPLOAD_URL = XTable.GlobalXTable.PhotoServerUploadUrl;
        //public const string PHOTO_SERVER_UPLOAD_URL = "http://47.74.186.77:8080/mm/upload/upload_file.php";
        //public const string PHOTO_SERVER_UPLOAD_URL = "http://192.168.0.16:8000/upload_files";

        /// <summary>
        /// 照片服务器下载地址
        /// </summary>
        public static string PHOTO_SERVER_DOWNLOAD_URL = XTable.GlobalXTable.PhotoServerDownloadUrl;
        //public const string PHOTO_SERVER_DOWNLOAD_URL = "http://47.74.186.77:8080/mm/upload/upload_files/";

        public static string PersistentDataPath
        {
            get
            {
                string path;
#if UNITY_EDITOR
                path = "file:///" + Application.persistentDataPath;
#elif UNITY_STANDALONE
                path = "file:///" + Application.persistentDataPath;
#elif UNITY_IPHONE
                path = "file:///" + Application.persistentDataPath;
#elif UNITY_ANDROID
                path = "file://" + Application.persistentDataPath;
                //www时Application.persistentDataPath返回的路径不会自动添加协议，需要手动添加，协议是file://而不是jar:// 
                //因为导出包是选择SD卡，该目录会在SD卡中暴露出来，无需root权限，属于本地文件，因此需要本地文件协议file://读取。
#endif
                return path;
            }
        }


        #region 加载图片

        /// <summary>
        /// 加载本地图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="img"></param>
        /// <param name="action"></param>
        public static void LoadLocalImage(string fileName, Image img, Action<Texture2D> action = null)
        {
            string url = PersistentDataPath + "/" + fileName;
            LoadImage(url, img, action);
        }

        /// <summary>
        /// 加载网络图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="img"></param>
        public static void LoadNetImage(string fileName, Image img, Action<Texture2D> action = null)
        {
            string url = PHOTO_SERVER_DOWNLOAD_URL + fileName;
            LoadImage(url, img, action);
        }

        public static void LoadImage(string url, Image img, Action<Texture2D> action = null)
        {
            LoadTexture(url, (texture) =>
            {
                if (texture != null)
                {
                    Rect rect = new Rect(0, 0, texture.width, texture.height);
                    img.overrideSprite = Sprite.Create(texture, rect, img.rectTransform.pivot);
                }
                else
                {
                    img.overrideSprite = null;
                }
                action?.Invoke(texture);
            });
        }

        public static void LoadLocalTexture(string fileName, Action<Texture2D> action = null)
        {
            string url = PersistentDataPath + "/" + fileName;
            LoadTexture(url, action);
        }

        public static void LoadNetTexture(string fileName, Action<Texture2D> action = null)
        {
            string url = XTable.GlobalXTable.PhotoServerDownloadUrl + fileName;
            LoadTexture(url, action);
        }

        public static void LoadTexture(string url, Action<Texture2D> action = null)
        {
            GameApp.Instance.StartCoroutine(LoadNetImageCoroutine(url, (texture) =>
            {
                action?.Invoke(texture);
            }));
        }

        private static IEnumerator LoadNetImageCoroutine(string url, Action<Texture2D> callback)
        {
            UnityWebRequest uwr = new UnityWebRequest(url);
            uwr.downloadHandler = new DownloadHandlerTexture();
            yield return uwr.SendWebRequest();
            Texture2D texture = null;
            if (string.IsNullOrEmpty(uwr.error))
                texture = DownloadHandlerTexture.GetContent(uwr);
            callback?.Invoke(texture);
            uwr.Dispose();
        }

        #endregion

        #region 保存文件

        /// <summary>
        /// 保存拍照图片
        /// </summary>
        /// <param name="texture"></param>
        public static string SaveTakePicture(Texture2D texture)
        {
            if (texture == null)
            {
                Debug.LogError("要保存的图片为空!");
                return "";
            }

            string savePath = Application.persistentDataPath + "/CardPhoto/";
            string fileName = DateTime.Now.ToLocalTime() + ".jpg";
            fileName = fileName.Replace(" ", "");
            fileName = fileName.Replace("/", "");
            fileName = fileName.Replace(":", "");
            SaveFile(texture.EncodeToJPG(), savePath, fileName);
            return fileName;
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="savePath"></param>
        /// <param name="fileName"></param>
        public static void SaveFile(byte[] data, string savePath, string fileName)
        {
            string path = savePath + fileName;

            Debug.Log(savePath);

            try
            {
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                FileStream stream = new FileStream(path, FileMode.Create);
                stream.Write(data, 0, data.Length);
                stream.Flush();
                stream.Close();

                if (!File.Exists(path))
                {
                    Debug.LogError("---- 写入文件失败 " + path);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError(">>> 写入文件出错 " + path + "\n" + ex);
            }
        }

        #endregion

        #region 上传图片

        public static void UploadPicture(Texture2D texture, string filename, Action<string> callback)
        {
            if (texture == null)
            {
                Debug.LogError("要上传的图片为空!");
                return;
            }

            string serverUrl = PHOTO_SERVER_UPLOAD_URL;
            GameApp.Instance.StartCoroutine(TransmitPicture(texture, serverUrl, filename, callback));
        }

        public static IEnumerator TransmitPicture(Texture2D texture, string serverUrl, string filename, Action<string> callback)
        {
            byte[] data = texture.EncodeToJPG();
            WWWForm form = new WWWForm();                                       // WWWForm是一个辅助类，该类用于生成表单数据，                                                                           //然后WWW类就可以将该表单数据post到web服务器上了
            form.AddBinaryData("file", data, filename, "image/jpeg");           // 添加二进制文件到表单，使用该函数可以上传文件或者图片到Web服务器                                                                           //第一个参数相当于一个"key"，类似于html中表单的fieldname，服务器端根据这个"key"得到文件流                                                                           //所以在php文件中用_FILES["picture"]可以得到该文件                                                                            //上传的内容就是字节数组bs的内容                                                                            //第三个参数filename用于告诉服务器当保存上传文件时使用什么文件名                                                                           //最后一个参数是档案格式，此处我上传的是jpg格式，所以用image/jpg                                                                           //如果bs使用的是png格式图片，就使用image/png
            WWW www = new WWW(serverUrl, form);                                 // 该构造函数创建并发送一个post请求 //form包含着要被post到web服务器的表单数据（form data）

            yield return www;
            if (www.error != null)
            {
                Debug.LogError(www.error);
                yield return null;
            }
            else
            {
                Debug.Log(www.text); // 服务器要返回图片url
                callback?.Invoke(filename);
                yield return null;
            }
        }

        #endregion

    }
}
