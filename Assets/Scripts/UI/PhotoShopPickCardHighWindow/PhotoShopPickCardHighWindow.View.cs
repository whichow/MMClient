using Msg.ClientMessage;
using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class PhotoShopPickCardHighWindow
    {
        private Button _buttonBack;
        private Button _buttonCamera;
        private Button _buttonPhotoLimit;
        private Button _buttonChangeCam;
        private GameObject _goImage;
        private KUICameraImage _cameraImageTexture;
        private SkeletonGraphic _cardFx;

        private KItem kShopitemHight;
        private KCat cat;

        private float m_drawCardTime = 0;

        public void InitView()
        {
            _buttonBack = Find<Button>("Back/ButtonBack");
            _buttonBack.onClick.AddListener(this.OnCloseBtnClick);

            _buttonCamera = Find<Button>("Back/Photo");
            _buttonCamera.onClick.AddListener(this.PhotoDrawCard);

            _buttonPhotoLimit = Find<Button>("Back/PhotoLimit/ButtonImage");
            _buttonPhotoLimit.onClick.AddListener(this.ApplyForPhotoLimit);

            _buttonChangeCam = Find<Button>("Back/ChangeCam");
            _buttonChangeCam.onClick.AddListener(ChangeCamHandler);
            
            _cameraImageTexture = Find<KUICameraImage>("Back/WebCam/Image");
            _goImage = transform.Find("Back/PhotoLimit/ButtonImage/Image").gameObject;
            _cardFx = Find<SkeletonGraphic>("Back/Fx/Fx_Card_03");
        }

        private void PhotoDrawCard()
        {
            if (Time.time - m_drawCardTime < 3)
            {
                return;
            }

            m_drawCardTime = Time.time;
            windowdata = data as PhtotShopPickCardWindowData;
            if (windowdata._type == (int)DrawCardType.Mid)
            {
                KUser.DrawCard(2, 1, OnMindDrawCard);
            }
            else
            {
                KUser.DrawCard(3, 1, OnHighDrawCard);
            }
        }

        private void OnHighDrawCard(int code, string message, object data)
        {
            if (code == 0)
            {
                Debug.Log("高级抽卡");
                var list = data as ArrayList;
                if (list != null)
                {
                    kShopitemHight = new KItem();
                    foreach (var item in list)
                    {
                        if (item is S2CDrawResult)
                        {
                            var result = (S2CDrawResult)item;
                            if (result.Cats != null)
                            {
                                for (int i = 0; i < result.Cats.Count; i++)
                                {
                                    cat = KCatManager.Instance.GetCat(result.Cats[i].Id);
                                }
                            }
                            if (result.Buildings != null)
                            {
                                for (int i = 0; i < result.Buildings.Count; i++)
                                {
                                    kShopitemHight = KItemManager.Instance.GetItem(result.Buildings[i].CfgId);
                                }
                            }
                        }
                    }
                }
                else
                {
                    kShopitemHight = null;
                }
                if (cat != null)
                {
                    SaveTexture();
                    StartCoroutine(StartFx(cat, null));
                    cat = null;
                    return;
                }
                if (kShopitemHight != null)
                {
                    StartCoroutine(StartFx(null, kShopitemHight));
                    kShopitemHight = null;
                    return;
                }
                else
                {
                    Debug.Log("高级抽卡失败");
                }
            }
        }

        private void OnMindDrawCard(int code, string message, object data)
        {
            if (code == 0)
            {
                Debug.Log("中级抽卡");
                var list = data as ArrayList;
                if (list != null)
                {
                    kShopitemMid = new KItem();
                    foreach (var item in list)
                    {
                        if (item is S2CDrawResult)
                        {
                            var result = (S2CDrawResult)item;
                            if (result.Cats != null)
                            {
                                for (int i = 0; i < result.Cats.Count; i++)
                                {
                                    //kShopitemMid = KItemManager.Instance.GetItem(result.Cats[i].CatCfgId);
                                    cat = KCatManager.Instance.GetCat(result.Cats[i].Id);
                                }
                            }
                            if (result.Buildings != null)
                            {
                                for (int i = 0; i < result.Buildings.Count; i++)
                                {
                                    kShopitemMid = KItemManager.Instance.GetItem(result.Buildings[i].CfgId);
                                }
                            }
                        }
                    }
                }
                else
                {
                    kShopitemMid = null;
                }
                if (cat != null)
                {
                    SaveTexture();
                    StartCoroutine(StartFx(cat, null));
                    cat = null;
                    return;
                }
                if (kShopitemMid != null)
                {
                    StartCoroutine(StartFx(null, kShopitemMid));
                }
                else
                {
                    Debug.Log("中级抽卡失败");
                }
            }
        }

        private IEnumerator StartFx(KCat cat, KItem itemCard)
        {
            // cardFx.AnimationName = null;
            //cardFx.AnimationName = "start";
            _cardFx.AnimationState.SetAnimation(0, "start", false);
            yield return new WaitForSeconds(1.7f);
            OpenWindow<PhotoShopGotBuildWindow>(new PhotoShopGotBuildWindow.Data
            {
                cat = cat,
                item = itemCard,
                type = 0,
            });
            cat = null;
        }

        private void ApplyForPhotoLimit()
        {
            Debug.Log("获取手机照相权限");

            RefreshView();
        }

        private void ChangeCamHandler()
        {
            _cameraImageTexture.ChangeCam();
        }

        public void RefreshView()
        {
            //if (Application.HasUserAuthorization(UserAuthorization.WebCam))
            //{
            //    _goImage.SetActive(true);
            //}
            //else
            //{
            //    _goImage.SetActive(false);
            //}
        }

        private void SaveTexture()
        {
            string filename = PlayerDataModel.Instance.mPlayerData.mPlayerID + "-" + cat.catId + ".jpg";
            Texture2D t = _cameraImageTexture.GetTexture2D();
            if (t != null)
            {
                //string filename = ImageUtils.SaveTakePicture(t);
                if (!string.IsNullOrEmpty(filename))
                {
                    ImageUtils.UploadPicture(t, filename, (url) =>
                    {
                        Debuger.Log(url);
                    });
                }
                GameObject.Destroy(t);
            }
            else
            {
                //StartCoroutine(ScreenCapture(filename));
            }
        }

        private IEnumerator ScreenCapture(string filename)
        {
            yield return null;

            //测试截屏保存
            //捕抓摄像机图像并转换成字符数组
            Camera camera = KUIRoot.Instance.uiCamera;
            Rect rect = new Rect(0, 0, Screen.width, Screen.height);
            RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
            camera.targetTexture = rt;
            camera.Render();

            RenderTexture.active = rt;
            Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(rect, 0, 0);
            screenShot.Apply();

            //string filename = ImageUtils.SaveTakePicture(screenShot);
            if (!string.IsNullOrEmpty(filename))
            {
                ImageUtils.UploadPicture(screenShot, filename, (url) =>
                {
                    Debuger.Log(url);
                });
            }

            camera.targetTexture = null;
            RenderTexture.active = null;
            GameObject.Destroy(rt);
        }

    }
}
