// ***********************************************************************
// 作用：世界聊天和附近聊天的子物体控制类
//作者：wsy
// ***********************************************************************
using Game.Build;
using Msg.ClientMessage;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    public class CommonExpressionItem : KUIItem, IPointerClickHandler
    {
        public KEmojiManager.EmojiInfo _data_expression { get; private set; }
        private KUIImage _img_expression;



        private void Awake()
        {
            _img_expression = Find<KUIImage>("Image");
        }
        #region Method
        protected override void Refresh()
        {
            ShowItem((KEmojiManager.EmojiInfo)data);
        }
        public void ShowItem(KEmojiManager.EmojiInfo value)
        {
            _data_expression = value;
            string str = _data_expression.name.Replace(" ", "");
            string[] strArray = str.Split(new string[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
            strArray[0] = strArray[0];
            if (strArray[0] != string.Empty)
            {
                _img_expression.overrideSprite = Resources.Load<Sprite>("Textures/CommonExpression/" + strArray[0]);
            }
        }
        #endregion
        #region Interface
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            //KUIWindow.GetWindow<ChatWindow>().AddCommonExpression(_data_expression.key);
            if (KEmojiManager.Instance.event_AddCommonExpression != null)
            {
                KEmojiManager.Instance.event_AddCommonExpression(_data_expression.key);
            }
        }
        #endregion
    }
}