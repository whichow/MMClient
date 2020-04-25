///** 
// *FileName:     M3Map.cs 
// *Author:       HeJunJie 
// *Version:      1.0 
// *UnityVersion：5.6.2f1
// *Date:         2018-01-09 
// *Description:    
// *History: 
//*/
//using System.Collections.Generic;
//using UnityEngine;

//namespace Game.Match3
//{
//    public class M3Map : MonoBehaviour
//    {
//        #region Field   

//        public TextAsset[] maps;

//        private Dictionary<string, TextAsset> _spriteDictionary;

//        #endregion

//        #region Method

//        public TextAsset GetMap(string name)
//        {
//            if (_spriteDictionary == null)
//            {
//                _spriteDictionary = new Dictionary<string, TextAsset>(this.maps.Length);
//                foreach (var sprite in this.maps)
//                {
//                    _spriteDictionary.Add(sprite.name, sprite);
//                }
//            }

//            if (string.IsNullOrEmpty(name))
//            {
//                return null;
//            }

//            TextAsset ret;
//            _spriteDictionary.TryGetValue(name, out ret);
//            return ret;
//        }

//        public TextAsset[] GetAllMaps()
//        {
//            if (_spriteDictionary == null)
//            {
//                _spriteDictionary = new Dictionary<string, TextAsset>(this.maps.Length);
//                foreach (var sprite in this.maps)
//                {
//                    _spriteDictionary.Add(sprite.name, sprite);
//                }
//            }
//            TextAsset[] ta = new TextAsset[_spriteDictionary.Count];
//            int index = 0;
//            foreach (var item in _spriteDictionary)
//            {
//                ta[index] = item.Value;
//                index++;
//            }
//            return ta;
//        }
//        #endregion

//    }
//}