// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class KLoading : MonoBehaviour
    {
        #region STATIC 
        private static GameObject m_loadingGO = null;

        private static GameObject LoadingGO
        {
            set
            {
                if (!m_loadingGO)
                {
                    m_loadingGO = value;
                    DontDestroyOnLoad(m_loadingGO);
                }
            }
        }

        /// <summary>Gets the load progress.</summary>
        public static float LoadingProgress
        {
            get
            {
                return _AsyncR != null ? _AsyncR.progress : 1f;
            }
        }

        /// <summary>load game assets.</summary>
        public static void LoadAssets()
        {
            DG.Tweening.DOTween.KillAll(false);
            DG.Tweening.DOTween.Clear(false);

            if (m_loadingGO == null)
            {
                KLaunch.LoadLevel("loadingScene");
            }
            else
            {
                m_loadingGO.SetActive(true);
            }
            _State = 1;

            Application.targetFrameRate = 60;
            Time.fixedDeltaTime = 0.02f;
        }

        /// <summary>unload game assets.</summary>
        public static void UnloadAssets()
        {
            DG.Tweening.DOTween.KillAll(false);
            DG.Tweening.DOTween.Clear(false);

            if (m_loadingGO == null)
            {
                KLaunch.LoadLevel("loadingScene");
            }
            else
            {
                m_loadingGO.SetActive(true);
            }
            _State = 11;

            //KLaunch.LoadLevel("buildingScene");
            //KAssetManager.Instance.UnloadMatch3Assets(true);

            Application.targetFrameRate = 30;
            Time.fixedDeltaTime = 0.05f;
        }

        public static void LoadCompleted()
        {
            if (m_loadingGO)
            {
                m_loadingGO.SetActive(false);
            }
        }

        #endregion

        private static int _State;
        private static IAsyncR _AsyncR;

        private GameObject _window;
        private Image _progress;

        #region UNITY

        private void Awake()
        {
            LoadingGO = gameObject;
        }

        private void Start()
        {

            GameObject prefab;
            if (KAssetManager.Instance.TryGetUIPrefab("LoadingWindow", out prefab))
            {
                _window = Instantiate(prefab);
                var wTransform = _window.transform;
                wTransform.SetParent(transform.Find("Canvas"), false);
                wTransform.SetAsFirstSibling();
                _progress = wTransform.Find("Back/Progress").GetComponent<Image>();
            }
        }

        private void Update()
        {
            switch (_State)
            {
                case 1:
                    KAssetManager.Instance.UnloadBuildingAssets(false);
                    //KAssetManager.Instance.UnloadCharacterAssets(true);
                    _State = 2;
                    break;
                case 2:
                    _AsyncR = KAssetManager.Instance.LoadMatch3Assets();
                    _State = 3;
                    break;
                case 3:
                    if (_AsyncR.done)
                    {
                        _AsyncR = null;
                        _State = 0;
                        //if (kloadingmanager.instance != null && klevelmanager.instance.currlevelid != kuser.testlevelid)
                        //{
                        //    kloadingmanager.instance.hideloading();
                        //}
                        KLaunch.LoadLevel("matchScene");
                    }
                    break;

                case 11:
                    _progress.fillAmount = 0.7f;
                    _progress.DOFillAmount(1, 2);

                    KAssetManager.Instance.UnloadMatch3Assets(true);
                    //KNGUIUtils.UnloadAtlas("game");
                    _State = 12;
                    break;
                case 12:
                    _State = 0;
                    KLaunch.LoadLevel("buildingScene");
                    break;
            }

            if (_AsyncR != null)
            {
                if (_progress)
                {
                    _progress.fillAmount = LoadingProgress;
                }
            }
        }

#if DEBUG_MY

        private void OnGUI()
        {
            GUI.Label(new Rect(Screen.width - 90, Screen.height - 50, 80, 40), "载入中.." + Mathf.RoundToInt(LoadingProgress * 100));
        }

#endif

        #endregion
    }
}
