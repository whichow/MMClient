// ***********************************************************************
// 作用：表情，超链接系统的资源加载等
// 作者：wsy
// ***********************************************************************
using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    using Callback = System.Action<int, string, object>;

    public class KEmojiManager : KGameModule
    {
        #region FIELD
        /// <summary>
        /// 大型动态表情的识别标志
        /// </summary>
        public const string SpecialExpressionMarker = "#E";
        /// <summary>
        /// 存放最后一次的频道
        /// </summary>
        /// <param name="value"></param>
        public void RecordLastPageType(ExpressionBoard.PageType value)
        {
            _lastPageType = value;
        }
        private ExpressionBoard.PageType _lastPageType = ExpressionBoard.PageType.kCommonExpression;
        /// <summary>
        /// 获取最后一次频道
        /// </summary>
        public ExpressionBoard.PageType LastOpenPageType
        {
            get
            {
                return _lastPageType;
            }
        }
        public Dictionary<string, EmojiInfo> Dict_AllCommonEmoji
        {
            get
            {
                return _dict_emojiIndex;
            }
        }
        private Dictionary<string, EmojiInfo> _dict_emojiIndex = new Dictionary<string, EmojiInfo>();
        public struct EmojiInfo
        {
            public string name;
            public string key;
            public float x;
            public float y;
            public float size;
            public int len;
        }

        public delegate void AddSpecialExpressionDelegate(string param);
        public AddSpecialExpressionDelegate event_AddSpecialExpression;

        public delegate void AddCommonExpressionDelegate(string param);
        public AddCommonExpressionDelegate event_AddCommonExpression;


        #endregion

        #region PROPERTY

        #endregion

        #region METHOD
        #endregion

        #region Unity
        private void Awake()
        {
            Instance = this;
        }

        public override void Load()
        {
            //Debug.Log("-><color=#9400D3>" + "[日志] [KEmojiManager] [Load] 表情系统解析表情配置" + "</color>");
            if (_dict_emojiIndex == null || _dict_emojiIndex.Count <= 0)
            {
                //load emoji data, and you can overwrite this segment code base on your project.
                TextAsset emojiContent = Resources.Load<TextAsset>("Configs/emoji");
                string[] lines = emojiContent.text.Split('\n');
                for (int i = 1; i < lines.Length; i++)
                {
                    if (!string.IsNullOrEmpty(lines[i]))
                    {
                        string[] strs = lines[i].Split('\t');
                        EmojiInfo info;
                        info.name = strs[0];
                        info.key = strs[1];
                        info.x = float.Parse(strs[3]);
                        info.y = float.Parse(strs[4]);
                        info.size = float.Parse(strs[5]);
                        info.len = 0;
                        //Debug.Log("-><color=#9400D3>" + "[日志] [EmojiText] [OnPopulateMesh] 获取字典ID：" + strs[0] + "</color>");
                        _dict_emojiIndex.Add(strs[0], info);
                    }
                }
            }
        }

        #endregion

        #region STATIC
        public static KEmojiManager Instance;
        #endregion
    }
}
