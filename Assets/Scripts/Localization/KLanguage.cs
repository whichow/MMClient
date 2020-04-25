// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KLanguage" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public class KLanguage
    {
        #region Field

        private Dictionary<int, string> _entryDictionary = new Dictionary<int, string>();
        private Dictionary<string, int> _idDictionary = new Dictionary<string, int>();

        #endregion

        #region Property

        public string name
        {
            get;
            set;
        }

        public string iconName
        {
            get;
            set;
        }

        public string database
        {
            get;
            set;
        }

        #endregion

        #region Method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public void LoadEntries(ArrayList list)
        {
            _entryDictionary.Clear();
            if (list != null && list.Count > 0)
            {
                var tmpL0 = (ArrayList)list[0];
                var idIndex = tmpL0.IndexOf("Id");
                var textIndex = tmpL0.IndexOf("Text");

                for (int i = 1; i < list.Count; i++)
                {
                    var tmpLi = (ArrayList)list[i];
                    int key = (int)tmpLi[idIndex];
                    string value = (string)tmpLi[textIndex];
#if UNITY_EDITOR
                    if (_entryDictionary.ContainsKey(key))
                    {
                        Debuger.LogErrorFormat("[语言表冲突] {0} {1} - {2}", key, value, _entryDictionary[key]);
                        continue;
                    }
#endif
                    _entryDictionary.Add(key, value);
                }
            }
        }

        public void SetAsDefault()
        {
            _idDictionary.Clear();
            foreach (var kvPair in _entryDictionary)
            {
#if UNITY_EDITOR
                if (_idDictionary.ContainsKey(kvPair.Value))
                {
                    Debuger.LogWarningFormat("[主语言表冲突] {0} {1} - {2}", kvPair.Value, kvPair.Key, _idDictionary[kvPair.Value]);
                }
#endif
                _idDictionary[kvPair.Value] = kvPair.Key;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entry"></param>
        public void Add(int id, string entry)
        {
            _entryDictionary.Add(id, entry);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Contains(int id)
        {
            return _entryDictionary.ContainsKey(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool TryGet(int id, out string entry)
        {
            return _entryDictionary.TryGetValue(id, out entry);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool TryGetId(string entry, out int id)
        {
            return _idDictionary.TryGetValue(entry, out id);
        }

        #endregion
    }
}