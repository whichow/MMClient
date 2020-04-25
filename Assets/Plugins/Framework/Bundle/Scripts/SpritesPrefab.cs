/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/28 17:14:11
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SpritesPrefab : MonoBehaviour
    {
        #region Field   

        public Sprite[] sprites;

        private Dictionary<string, Sprite> _spriteDic;

        #endregion

        #region Method

        public Sprite GetTextAsset(string name)
        {
            if (_spriteDic == null)
            {
                _spriteDic = new Dictionary<string, Sprite>(sprites.Length);
                foreach (var sprite in sprites)
                {
                    _spriteDic.Add(sprite.name, sprite);
                }
            }

            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            Sprite ret;
            _spriteDic.TryGetValue(name, out ret);
            return ret;
        }

        public Sprite[] GetAll()
        {
            return sprites;
        }

        #endregion

    }
}
