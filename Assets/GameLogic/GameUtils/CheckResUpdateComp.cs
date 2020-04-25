/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/28 15:02:51
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using K.AB;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game
{
    /// <summary>
    /// ABDataManager
    /// </summary>
    public class CheckResUpdateComp : MonoBehaviour
    {
        public RectTransform _barRT;
        public Text _msgTxt;
        public Text _verTxt;

        public void Start()
        {
            _barRT.sizeDelta = new Vector2(1, 50);
            _barRT.DOSizeDelta(new Vector2(800, 50), 1);
            _verTxt.text = "";

            ABDataManager.Instance.ShowMsgHandler = ShowMsgHandler;
            ABDataManager.Instance.ProgressHandler = ProgressHandler;
        }

        public void Update()
        {
            if (!string.IsNullOrEmpty(AppConfig.GameVersion) && string.IsNullOrEmpty(_verTxt.text))
            {
                _verTxt.text = string.Format("{0} v{1} {2}", AppConfig.IsDebugMod ? "Debug" : "Release", AppConfig.GameVersion, AppConfig.SvnVersion);
            }
        }

        private void ShowMsgHandler(string msg)
        {
            _msgTxt.text = msg;
        }

        private void ProgressHandler(float p)
        {
            _barRT.DOKill();
            _barRT.sizeDelta = new Vector2(800 * p, 50);
        }

    }
}
