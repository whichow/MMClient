/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/28 17:10:42
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TextAssetsPrefab : MonoBehaviour
    {
        #region Field   

        public TextAsset[] tas;

        private Dictionary<string, TextAsset> _taDic;

        #endregion

        #region Method

        public TextAsset GetTextAsset(string name)
        {
            if (_taDic == null)
            {
                _taDic = new Dictionary<string, TextAsset>(tas.Length);
                foreach (var sprite in tas)
                {
                    _taDic.Add(sprite.name, sprite);
                }
            }

            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            TextAsset ret;
            _taDic.TryGetValue(name, out ret);
            return ret;
        }

        public TextAsset[] GetAll()
        {
            return tas;
        }

        #endregion

    }
}
