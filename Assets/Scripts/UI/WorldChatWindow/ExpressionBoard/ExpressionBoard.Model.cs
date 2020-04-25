using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
/** 
* 作用：世界聊天的界面人物头像弹出框的控制
* 作者：wsy
*/
namespace Game.UI
{
    public partial class ExpressionBoard
    {
        public enum PageType
        {
            kCommonExpression = 1,
            kSpecialExpression = 2,
            kDIY = 3,
        }
        public PageType CurrentPageType { get; private set; }

        private void InitializationModel()
        {

        }
        private void ResetModel()
        {
            CurrentPageType = KEmojiManager.Instance.LastOpenPageType;
            foreach (var item in _dict_tglAndPgtp)
            {
                if (item.Value.Equals(CurrentPageType))
                {
                    item.Key.isOn = true;
                }
            }
        }
        private PageType GetOnToggle()
        {
            foreach (var toggle in _lstTgl_channel)
            {
                if (toggle.isOn)
                {
                    return _dict_tglAndPgtp[toggle];
                }
            }
            return PageType.kCommonExpression;
        }
        private List<KEmojiManager.EmojiInfo> GetCommonExpression()
        {
            List<KEmojiManager.EmojiInfo> _lst_allCommonExpression = new List<KEmojiManager.EmojiInfo>();
            foreach (KEmojiManager.EmojiInfo item in KEmojiManager.Instance.Dict_AllCommonEmoji.Values)
            {
                _lst_allCommonExpression.Add(item);
            }
            return _lst_allCommonExpression;
        }
        private List<Sprite> GetSpecialExpression()
        {
            GameObject bigExpressionOverView;
            List<Sprite> allBigExpression = new List<Sprite>();
            if (KAssetManager.Instance.TryGetUIPrefab("BigExpressionOverView", out bigExpressionOverView))
            {
                KUIImage expressionImage = bigExpressionOverView.GetComponent<KUIImage>();
                if (expressionImage != null)
                {
                    for (int i = 0; i < expressionImage.sprites.Length; i++)
                    {
                        allBigExpression.Add(expressionImage.sprites[i]);
                    }
                }
                else
                {
                    Debug.Log("-><color=#9400D3>" + "[异常] [ExpressionBoard] [GetSpecialExpression] 获取大型表情的预设体的解析过程异常" + "</color>");
                }
            }
            return allBigExpression;
        }
    }
}

