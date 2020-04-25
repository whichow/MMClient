// ***********************************************************************
// Company          : 
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
namespace Game
{
    using ToolGood.Words;
    using UnityEngine;

    /// <summary>
    /// 敏感词过滤 已忽略大小写 全半角 简繁体差异 
    /// </summary>
    public class BadWords
    {
        private static IllegalWordsSearch _WordsSearch;

        public static IllegalWordsSearch WordsSearch
        {
            get
            {
                if (_WordsSearch == null)
                {
                    var textAsset = Resources.Load<TextAsset>("Configs/UnsafeConfig");
                    if (textAsset)
                    {
                        var splits = textAsset.text.Split('|');
                        _WordsSearch = new IllegalWordsQuickSearch();
                        _WordsSearch.SetKeywords(splits);
                    }
                }
                return _WordsSearch;
            }
        }

        public static bool ContainsAny(string text)
        {
            if (WordsSearch != null)
            {
                return WordsSearch.ContainsAny(text);
            }
            return false;
        }

    }
}