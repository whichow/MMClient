// ***********************************************************************
// 作用：世界聊天和附近聊天的子物体控制类
// 作者：wsy
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
    public class SpecialExpressionItem : KUIItem, IPointerClickHandler
    {
        private KUIImage _img_expression;
        private Sprite _data_sprite;
        private void Awake()
        {
            _img_expression = Find<KUIImage>("Image");
        }
        #region Method
        protected override void Refresh()
        {
            ShowItem(data as Sprite);
        }
        public void ShowItem(Sprite value)
        {
            _data_sprite = value;
            _img_expression.overrideSprite = value;
        }
        #endregion
        #region Interface
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            //KUIWindow.GetWindow<ChatWindow>().AddSpecialExpression(KEmojiManager.SpecialExpressionMarker + _data_sprite.name);
            if (KEmojiManager.Instance.event_AddSpecialExpression != null)
            {
                KEmojiManager.Instance.event_AddSpecialExpression(KEmojiManager.SpecialExpressionMarker + _data_sprite.name);
            }
            KUIWindow.CloseWindow<ExpressionBoard>();
        }
        #endregion
    }
}