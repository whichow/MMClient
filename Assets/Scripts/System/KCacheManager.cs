// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KCacheManager" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class KCacheManager : MonoBehaviour
    {
        private class CacheInfo
        {
            /// <summary>
            /// 0初始 1 loading 2 complete
            /// </summary>
            public int state;
            /// <summary>
            /// 重试次数 
            /// </summary>
            public int retry = 2;

            public string url;
            public string path;

            public Sprite sprite;
            public Texture2D texture;
            public Action<Sprite> callback;
        }

        private string _cachePath;
        private WWW _www;
        private Texture2D _defaultTexture;
        private Dictionary<string, CacheInfo> _cacheTable = new Dictionary<string, CacheInfo>();

        private string cachePath
        {
            get
            {
                if (string.IsNullOrEmpty(_cachePath))
                {
                    _cachePath = Path.Combine(Application.persistentDataPath, "Textures");
                    if (!Directory.Exists(_cachePath))
                    {
                        Directory.CreateDirectory(_cachePath);
                    }
                }
                return _cachePath;
            }
        }

        public void GetSprite(string url, Action<Sprite> callback)
        {
            //url = "http://q.qlogo.cn/qqapp/1104400419/C53D64A12E0107614CFAA7661132E0F6/40";
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            CacheInfo ti;
            if (_cacheTable.TryGetValue(url, out ti))
            {
                if (callback != null)
                {
                    callback(ti.sprite);
                }
            }
            else
            {
                ti = new CacheInfo
                {
                    url = url,
                    path = Path.Combine(cachePath, ((uint)url.GetHashCode()).ToString()),
                    callback = callback,
                };

                var color = new Color32(145, 150, 207, 255);
                ti.texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                ti.texture.SetPixels32(new Color32[] { color, color, color, color });
                ti.texture.Apply();

                if (File.Exists(ti.path))
                {
                    ti.state = 2;
                    ti.texture.LoadImage(File.ReadAllBytes(ti.path));
                    ti.sprite = Sprite.Create(ti.texture, new Rect(0, 0, ti.texture.width, ti.texture.height), new Vector2(0.5f, 0.5f));
                    if (ti.callback != null)
                    {
                        ti.callback(ti.sprite);
                    }
                }
                else
                {
                    ti.state = 0;
                }
                _cacheTable.Add(url, ti);
            }
        }

        public Texture2D GetTexture(string url)
        {
            //url = "http://q.qlogo.cn/qqapp/1104400419/C53D64A12E0107614CFAA7661132E0F6/40";
            if (string.IsNullOrEmpty(url))
            {
                return _defaultTexture;
            }

            CacheInfo ti;
            if (_cacheTable.TryGetValue(url, out ti))
            {
                return ti.texture;
            }
            else
            {
                ti = new CacheInfo
                {
                    url = url,
                    path = Path.Combine(cachePath, ((uint)url.GetHashCode()).ToString()),
                    texture = new Texture2D(2, 2, TextureFormat.ARGB32, false)
                };
                var color = new Color32(145, 150, 207, 255);
                ti.texture.SetPixels32(new Color32[] { color, color, color, color });
                ti.texture.Apply();

                if (File.Exists(ti.path))
                {
                    ti.state = 2;
                    ti.texture.LoadImage(File.ReadAllBytes(ti.path));
                }
                else
                {
                    ti.state = 0;
                }
                _cacheTable.Add(url, ti);
                return ti.texture;
            }
        }

        public void Clear()
        {
        }

        #region UNITY

        private void Start()
        {
            _defaultTexture = Resources.Load<Texture2D>("Textures/default");
        }

        private void Update()
        {
            if (_www != null)
            {
                if (_www.isDone)
                {
                    var ti = _cacheTable[_www.url];
                    if (string.IsNullOrEmpty(_www.error))
                    {
                        _www.LoadImageIntoTexture(ti.texture);
                        ti.sprite = Sprite.Create(ti.texture, new Rect(0, 0, ti.texture.width, ti.texture.height), new Vector2(0.5f, 0.5f));
                        if (ti.callback != null)
                        {
                            ti.callback(ti.sprite);
                        }
                        ti.state = 2;
                        ti.callback = null;
                        if (!string.IsNullOrEmpty(ti.path))
                        {
                            File.WriteAllBytes(ti.path, ti.texture.EncodeToPNG());
                        }
                    }
                    else
                    {
                        ti.retry -= 1;
                        if (ti.retry > 0)
                        {
                            ti.state = 0;
                        }
                    }
                    _www.Dispose();
                    _www = null;
                }
            }
            else
            {
                foreach (var pair in _cacheTable)
                {
                    var value = pair.Value;
                    if (value.state == 0)
                    {
                        value.state = 1;
                        _www = new WWW(pair.Key);
                        break;
                    }
                }
            }
        }

        #endregion

        #region STATIC

        private static KCacheManager _Instance;
        public static KCacheManager Instance
        {
            get
            {
                if (!_Instance)
                {
                    _Instance = new GameObject("CacheManager").AddComponent<KCacheManager>();
                    _Instance.transform.parent = GameObject.Find("Launch").transform;
                }
                return _Instance;
            }
        }

        #endregion
    }
}
