// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "LoginWindow" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Game.UI;
using Game.DataModel;

namespace Game
{
    public class LoginWindow : MonoBehaviour
    {
        #region Field
#if DEBUG_MY
        public InputField inputField;
        public Dropdown dropdown;
        private List<string> serverList;// = new List<string> { "(国外) 47.74.186.77:35000", "(外网) 47.101.193.60:35000", "(内网) 192.168.0.16:45000", "(内网) 192.168.0.250:45000" };
#endif

        private Button _loginBtn;
        private Text _tipsText;
        private Text _progressText;
        private Image _progressImage;
        private Text _userAgreeText;
        private GameObject _tipObject;
        private IAsyncR _asyncR;

        #endregion

        private void OnLoginBtnClick()
        {
#if !UNITY_EDITOR
            if (AppConfig.HotUpdateRes && !GameApp.Instance.HotFixLuaSucc)
            {
                _userAgreeText.text = "HotFixLua Fail!";
                _userAgreeText.color = Color.red;
                Debuger.LogError("HotFixLua Fail!");
                return;
            }
#endif

#if DEBUG_MY
            Debuger.Log("[C#]>>>>>OnLoginBtnClick");

            var server = serverList[dropdown.value];
            KConfig.ServerURL = server.Substring(5);

            string openId = inputField.text;
            if (!string.IsNullOrEmpty(openId))
            {
                PlayerPrefs.SetInt("server_index", dropdown.value);
                PlayerPrefs.SetString("account_postfix", openId);
                PlayerPrefs.Save();
            }
            else
            {
                openId = "test001";
                PlayerPrefs.SetString("account_postfix", openId);
                PlayerPrefs.Save();
            }
#endif
            HideLogin();
            var gameFsm = KFramework.FsmManager.GetFsm<GameFsm>();
            if (gameFsm != null)
            {
                gameFsm.SendEvent(this, LoginState.kStartLogin, "guest");
            }
        }

        public void HideLogin()
        {
#if DEBUG_MY
            dropdown.gameObject.SetActive(false);
            inputField.gameObject.SetActive(false);
#endif
            _loginBtn.gameObject.SetActive(false);
            _userAgreeText.gameObject.SetActive(false);
            _tipObject.SetActive(true);
        }

        public void ShowLogin()
        {
#if DEBUG_MY
            dropdown.gameObject.SetActive(true);
            inputField.gameObject.SetActive(true);
#endif
            _tipObject.SetActive(false);
            _userAgreeText.gameObject.SetActive(true);
            _loginBtn.gameObject.SetActive(true);

            SetState(0f, "", "");

            GameApp.Instance.InitGameNet();
        }

        public void SetState(float progress, string progressHint, string tips)
        {
            _progressImage.fillAmount = progress;
            _progressText.text = progressHint;
            _tipsText.text = tips;
        }

        public void LoadAssets(IAsyncR asyncR)
        {
            _asyncR = asyncR;
        }

        private void InitView()
        {
#if DEBUG_MY
            XTable.GlobalXTable.Load(()=> {
                serverList = XTable.GlobalXTable.ServerList;
                dropdown.AddOptions(serverList);
                dropdown.value = PlayerPrefs.GetInt("server_index", 0);
                inputField.text = PlayerPrefs.GetString("account_postfix", "");
            });
#endif          
            GameObject wPrefab;
            if (KAssetManager.Instance.TryGetUIPrefab("LoginWindow", out wPrefab))
            {
                var wTransform = Instantiate(wPrefab).transform;
                wTransform.SetParent(transform.Find("Canvas"), false);
                wTransform.SetAsFirstSibling();

                _loginBtn = wTransform.Find("Panel/Guest").GetComponent<Button>();
                _loginBtn.onClick.AddListener(this.OnLoginBtnClick);
                _loginBtn.gameObject.SetActive(false);

                _tipObject = wTransform.Find("Panel/Progress").gameObject;
                _tipsText = wTransform.Find("Panel/Progress/Tips").GetComponent<Text>();
                _progressText = wTransform.Find("Panel/Progress/Text").GetComponent<Text>();
                _progressImage = wTransform.Find("Panel/Progress/Mask").GetComponent<Image>();
                _userAgreeText = wTransform.Find("Panel/UserAgree").GetComponent<Text>();
            }
        }

        #region Unity

        public static LoginWindow Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        // Use this for initialization
        private void Start()
        {
            InitView();
        }

        // Update is called once per frame
        private void Update()
        {
        }

        #endregion
    }
}
