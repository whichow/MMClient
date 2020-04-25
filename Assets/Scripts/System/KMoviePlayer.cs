// ***********************************************************************
// Company          : 
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
using UnityEngine;
using System.Collections;
using Callback = System.Action<int, string, object>;

namespace Game
{
    /// <summary>
    /// KMoviePlayer
    /// </summary>
    public class KMoviePlayer : MonoBehaviour
    {
        #region MODEL

        #endregion

        #region FIELD

        //网络视频地址  
        private string _movieURL;

        //视频下载本地存储地址  
        private string _moviePath;
        //
        private Callback _playCallback;

        #endregion

        #region PROPERTY        

        private string movieURL
        {
            set
            {
                _movieURL = value;
                _moviePath = System.IO.Path.Combine(Application.persistentDataPath, System.IO.Path.GetFileName(value));

            }
        }

        private Callback playCallback
        {
            set
            {
                _playCallback = value;
            }
        }

        #endregion

        #region METHOD   

        #endregion

        #region UNITY


        /// <summary>Starts this instance.</summary>
        private IEnumerator Start()
        {
#if UNITY_IOS

			if(!_movieURL.Contains("://"))
			{
				Screen.orientation = ScreenOrientation.Landscape;
			}

			Handheld.PlayFullScreenMovie(_movieURL, Color.black, FullScreenMovieControlMode.CancelOnInput);
			yield return null;
#else

            var file = new System.IO.FileInfo(_moviePath);
            if (!System.IO.File.Exists(_moviePath))
            {
                WWW _www = new WWW(_movieURL);
                yield return _www;

                if (string.IsNullOrEmpty(_www.error))
                {
                    //print("视频加载完成");
                    //获取www的字节  
                    byte[] bytes = _www.bytes;
                    System.IO.File.WriteAllBytes(_moviePath, bytes);
                }
                else
                {
                    Destroy(this.gameObject);
                    yield break;
                }
            }

            yield return null;
            //文件存在 直接播放视频  
            //print("文件存在 直接播放视频");
            //Handheld.PlayFullScreenMovie(_moviePath, Color.black, FullScreenMovieControlMode.CancelOnInput);
            //Destroy(this.gameObject);
#endif
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus)
            {
                //				print("Finish play movie");
                if (!_movieURL.Contains("://"))
                {
                    Screen.orientation = ScreenOrientation.Portrait;
                }

                if (_playCallback != null)
                {
                    _playCallback(0, null, null);
                    _playCallback = null;
                }

                Instance = null;
                Destroy(this.gameObject);
            }
            else
            {
                //				print("Start play movie");
            }
        }

        #endregion

        #region STATIC

        private static KMoviePlayer Instance;

        public static void PlayMovie(string movieURL, Callback callback = null)
        {
#if UNITY_EDITOR
            if (callback != null)
            {
                callback(0, null, null);
            }
#else
            if (!Instance)
            {
				Instance = new GameObject().AddComponent<KMoviePlayer>();
				Instance.movieURL = movieURL;
				Instance.playCallback = callback;
            }
#endif
        }

        #endregion
    }
}