/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/21 14:14:05
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    public class WebRequest : Singleton<WebRequest>
    {
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="actionResult"></param>
        public void Get(string url, Action<UnityWebRequest> actionResult)
        {
            GameApp.Instance.StartCoroutine(_Get(url, actionResult));
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="downloadFilePathAndName">储存文件的路径和文件名 like 'Application.persistentDataPath+"/unity3d.html"'</param>
        /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求对象</param>
        /// <returns></returns>
        public void DownloadFile(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult)
        {
            GameApp.Instance.StartCoroutine(_DownloadFile(url, downloadFilePathAndName, actionResult));
        }

        /// <summary>
        /// 请求图片
        /// </summary>
        /// <param name="url">图片地址,like 'http://www.my-server.com/image.png '</param>
        /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的图片</param>
        /// <returns></returns>
        public void GetTexture(string url, Action<Texture2D> actionResult)
        {
            GameApp.Instance.StartCoroutine(_GetTexture(url, actionResult));
        }

        /// <summary>
        /// 请求AssetBundle
        /// </summary>
        /// <param name="url">AssetBundle地址,like 'http://www.my-server.com/myData.unity3d'</param>
        /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AssetBundle</param>
        /// <returns></returns>
        public void GetAssetBundle(string url, Action<AssetBundle> actionResult)
        {
            GameApp.Instance.StartCoroutine(_GetAssetBundle(url, actionResult));
        }

        /// <summary>
        /// 请求服务器地址上的音效
        /// </summary>
        /// <param name="url">没有音效地址,like 'http://myserver.com/mysound.wav'</param>
        /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AudioClip</param>
        /// <param name="audioType">音效类型</param>
        /// <returns></returns>
        public void GetAudioClip(string url, Action<AudioClip> actionResult, AudioType audioType = AudioType.WAV)
        {
            GameApp.Instance.StartCoroutine(_GetAudioClip(url, actionResult, audioType));
        }

        /// <summary>
        /// 向服务器提交post请求
        /// </summary>
        /// <param name="url">服务器请求目标地址,like "http://www.my-server.com/myform"</param>
        /// <param name="lstformData">form表单参数</param>
        /// <param name="actionResult">处理返回结果的委托,处理请求对象</param>
        /// <returns></returns>
        public void Post(string url, List<IMultipartFormSection> lstformData, Action<UnityWebRequest> actionResult)
        {
            //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            //formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
            //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));
            GameApp.Instance.StartCoroutine(_Post(url, lstformData, actionResult));
        }

        /// <summary>
        /// 通过PUT方式将字节流传到服务器
        /// </summary>
        /// <param name="url">服务器目标地址 like 'http://www.my-server.com/upload' </param>
        /// <param name="contentBytes">需要上传的字节流</param>
        /// <param name="actionResult">处理返回结果的委托</param>
        /// <returns></returns>
        public void UploadByPut(string url, byte[] contentBytes, Action<bool> actionResult)
        {
            GameApp.Instance.StartCoroutine(_UploadByPut(url, contentBytes, actionResult, ""));
        }



        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url">请求地址,like 'http://www.my-server.com/ '</param>
        /// <param name="actionResult">请求发起后处理回调结果的委托</param>
        /// <returns></returns>
        IEnumerator _Get(string url, Action<UnityWebRequest> actionResult)
        {
            using (UnityWebRequest uwr = UnityWebRequest.Get(url))
            {
                yield return uwr.SendWebRequest();
                actionResult?.Invoke(uwr);
                uwr.Dispose();
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="downloadFilePathAndName">储存文件的路径和文件名 like 'Application.persistentDataPath+"/unity3d.html"'</param>
        /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求对象</param>
        /// <returns></returns>
        IEnumerator _DownloadFile(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult)
        {
            var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
            uwr.downloadHandler = new DownloadHandlerFile(downloadFilePathAndName);
            yield return uwr.SendWebRequest();
            actionResult?.Invoke(uwr);
            uwr.Dispose();
        }

        /// <summary>
        /// 请求图片
        /// </summary>
        /// <param name="url">图片地址,like 'http://www.my-server.com/image.png '</param>
        /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的图片</param>
        /// <returns></returns>
        IEnumerator _GetTexture(string url, Action<Texture2D> actionResult)
        {
            UnityWebRequest uwr = new UnityWebRequest(url);
            DownloadHandlerTexture handler = new DownloadHandlerTexture(true);
            uwr.downloadHandler = handler;
            yield return uwr.SendWebRequest();
            Texture2D t = null;
            if (!(uwr.isNetworkError || uwr.isHttpError))
            {
                t = handler.texture;
            }
            actionResult?.Invoke(t);
            uwr.Dispose();
        }

        /// <summary>
        /// 请求AssetBundle
        /// </summary>
        /// <param name="url">AssetBundle地址,like 'http://www.my-server.com/myData.unity3d'</param>
        /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AssetBundle</param>
        /// <returns></returns>
        IEnumerator _GetAssetBundle(string url, Action<AssetBundle> actionResult)
        {
            UnityWebRequest uwr = new UnityWebRequest(url);
            DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(uwr.url, uint.MaxValue);
            uwr.downloadHandler = handler;
            yield return uwr.SendWebRequest();
            AssetBundle bundle = null;
            if (!(uwr.isNetworkError || uwr.isHttpError))
            {
                bundle = handler.assetBundle;
            }
            actionResult?.Invoke(bundle);
            uwr.Dispose();
        }

        /// <summary>
        /// 请求服务器地址上的音效
        /// </summary>
        /// <param name="url">音效地址,like 'http://myserver.com/mysound.wav'</param>
        /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AudioClip</param>
        /// <param name="audioType">音效类型</param>
        /// <returns></returns>
        IEnumerator _GetAudioClip(string url, Action<AudioClip> actionResult, AudioType audioType = AudioType.WAV)
        {
            using (var uwr = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
            {
                yield return uwr.SendWebRequest();
                if (!(uwr.isNetworkError || uwr.isHttpError))
                {
                    actionResult?.Invoke(DownloadHandlerAudioClip.GetContent(uwr));
                }
                uwr.Dispose();
            }
        }

        /// <summary>
        /// 向服务器提交post请求
        /// </summary>
        /// <param name="url">服务器请求目标地址,like "http://www.my-server.com/myform"</param>
        /// <param name="lstformData">form表单参数</param>
        /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求对象</param>
        /// <returns></returns>
        IEnumerator _Post(string url, List<IMultipartFormSection> lstformData, Action<UnityWebRequest> actionResult)
        {
            //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            //formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
            //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));
            UnityWebRequest uwr = UnityWebRequest.Post(url, lstformData);
            yield return uwr.SendWebRequest();
            actionResult?.Invoke(uwr);
            uwr.Dispose();
        }

        /// <summary>
        /// 通过PUT方式将字节流传到服务器
        /// </summary>
        /// <param name="url">服务器目标地址 like 'http://www.my-server.com/upload' </param>
        /// <param name="contentBytes">需要上传的字节流</param>
        /// <param name="actionResult">处理返回结果的委托</param>
        /// <param name="contentType">设置header文件中的Content-Type属性</param>
        /// <returns></returns>
        IEnumerator _UploadByPut(string url, byte[] contentBytes, Action<bool> actionResult, string contentType = "application/octet-stream")
        {
            UnityWebRequest uwr = new UnityWebRequest();
            UploadHandler uploader = new UploadHandlerRaw(contentBytes);
            // Sends header: "Content-Type: custom/content-type";
            uploader.contentType = contentType;
            uwr.uploadHandler = uploader;
            yield return uwr.SendWebRequest();
            bool b = true;
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                b = false;
            }
            actionResult?.Invoke(b);
            uwr.Dispose();
        }

    }
}
