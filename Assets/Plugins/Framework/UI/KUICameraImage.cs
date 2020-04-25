// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KUICameraImage" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Custom/Camera Image", 12)]
public class KUICameraImage : MaskableGraphic
{
    [SerializeField]
    Texture m_Texture;
    [SerializeField] Rect m_UVRect = new Rect(0f, 0f, 1f, 1f);

    private WebCamTexture _webCam;

    protected KUICameraImage()
    {
        useLegacyMeshGeneration = false;
    }

    /// <summary>
    /// Returns the texture used to draw this Graphic.
    /// </summary>
    public override Texture mainTexture
    {
        get
        {
            if (m_Texture == null)
            {
                if (material != null && material.mainTexture != null)
                {
                    return material.mainTexture;
                }
                return s_WhiteTexture;
            }

            return m_Texture;
        }
    }

    /// <summary>
    /// Texture to be used.
    /// </summary>
    public Texture texture
    {
        get
        {
            return m_Texture;
        }
        set
        {
            if (m_Texture == value)
                return;

            m_Texture = value;
            SetVerticesDirty();
            SetMaterialDirty();
        }
    }

    /// <summary>
    /// UV rectangle used by the texture.
    /// </summary>
    public Rect uvRect
    {
        get
        {
            return m_UVRect;
        }
        set
        {
            if (m_UVRect == value)
                return;
            m_UVRect = value;
            SetVerticesDirty();
        }
    }

    /// <summary>
    /// Adjust the scale of the Graphic to make it pixel-perfect.
    /// </summary>

    public override void SetNativeSize()
    {
        Texture tex = mainTexture;
        if (tex != null)
        {
            int w = Mathf.RoundToInt(tex.width * uvRect.width);
            int h = Mathf.RoundToInt(tex.height * uvRect.height);
            rectTransform.anchorMax = rectTransform.anchorMin;
            rectTransform.sizeDelta = new Vector2(w, h);
        }
    }

    protected override void Start()
    {
        StartCoroutine(StartCamera());
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (_webCam != null)
        {
            _webCam.Play();
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_webCam != null)
        {
            _webCam.Stop();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_webCam != null)
        {
            _webCam.Stop();
            _webCam = null;
        }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        Texture tex = mainTexture;
        vh.Clear();
        if (tex != null)
        {
            var r = GetPixelAdjustedRect();
            var v = new Vector4(r.x, r.y, r.x + r.width, r.y + r.height);
            var scaleX = tex.width * tex.texelSize.x;
            var scaleY = tex.height * tex.texelSize.y;
            {
                var color32 = color;
                vh.AddVert(new Vector3(v.x, v.y), color32, new Vector2(m_UVRect.xMin * scaleX, m_UVRect.yMin * scaleY));
                vh.AddVert(new Vector3(v.x, v.w), color32, new Vector2(m_UVRect.xMin * scaleX, m_UVRect.yMax * scaleY));
                vh.AddVert(new Vector3(v.z, v.w), color32, new Vector2(m_UVRect.xMax * scaleX, m_UVRect.yMax * scaleY));
                vh.AddVert(new Vector3(v.z, v.y), color32, new Vector2(m_UVRect.xMax * scaleX, m_UVRect.yMin * scaleY));

                vh.AddTriangle(0, 1, 2);
                vh.AddTriangle(2, 3, 0);
            }
        }
    }

    private IEnumerator StartCamera()
    {
        yield return null;
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            color = Color.white;
            ChangeCam(false);
        }
    }

    private WebCamTexture SetWebCam(string deviceName)
    {
        if (!string.IsNullOrEmpty(deviceName))
        {
            if (_webCam != null)
            {
                _webCam.Stop();
            }
            Vector2 size = rectTransform.sizeDelta;
            _webCam = new WebCamTexture(deviceName, (int)size.x, (int)size.y);
            _webCam.wrapMode = TextureWrapMode.Repeat;
            texture = _webCam;
            _webCam.Play();
        }

        return _webCam;
    }

    public WebCamTexture ChangeCam(bool isFrontFacing)
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        string deviceName = "";
        foreach (var item in devices)
        {
            if (item.isFrontFacing == isFrontFacing)
            {
                deviceName = item.name;
                break;
            }
        }
        return SetWebCam(deviceName);
    }

    public WebCamTexture ChangeCam()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        string deviceName = "";
        foreach (var item in devices)
        {
            if (_webCam == null)
            {
                if (!item.isFrontFacing)
                {
                    deviceName = item.name;
                    break;
                }
            }
            else
            {
                if (_webCam.deviceName != item.name)
                {
                    deviceName = item.name;
                    break;
                }
            }
        }
        return SetWebCam(deviceName);
    }

    public Texture2D GetTexture2D()
    {
        Texture2D tex = null;
        var camTex = texture as WebCamTexture;
        if (camTex != null)
        {
            camTex.Pause();
            tex = new Texture2D(camTex.width, camTex.height, TextureFormat.RGB24, false);
            tex.SetPixels32(camTex.GetPixels32());
            tex.Apply();
            camTex.Play();
        }
        return tex;
    }

}

