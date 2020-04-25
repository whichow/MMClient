// ***********************************************************************
// Company          : 
// Author           : KimCh
// Copyright(c)     : 
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KUIAtlas : MonoBehaviour
{
    #region Field   

    public Sprite[] sprites;

    private Dictionary<string, Sprite> _spriteDictionary;

    #endregion

    #region Method

    public Sprite GetSprite(string name)
    {
        if (_spriteDictionary == null)
        {
            _spriteDictionary = new Dictionary<string, Sprite>(this.sprites.Length);
            foreach (var sprite in this.sprites)
            {
                _spriteDictionary.Add(sprite.name, sprite);
            }
        }

        if (string.IsNullOrEmpty(name))
        {
            return null;
        }

        Sprite ret;
        _spriteDictionary.TryGetValue(name, out ret);

        if (!ret)
            Debug.LogError(this.name + " --sprite is null-- " + name);
        else if (!ret.texture)
            Debug.LogError(this.name + " --texture is null-- " + name);

        return ret;
    }

    #endregion
}
