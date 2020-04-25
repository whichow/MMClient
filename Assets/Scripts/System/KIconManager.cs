// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KIconManager" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.DataModel;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class KIconManager : SingletonUnity<KIconManager>
    {
        #region Method

        public Sprite GetItemIcon(string iconName)
        {
            return GetIcon(iconName, "ItemIcons");
        }

        public Sprite GetItemIcon(int itemID)
        {
            ItemXDM cfg = XTable.ItemXTable.GetByID(itemID);
            if (cfg != null)
                return GetItemIcon(cfg.Icon);
            return null;
        }

        public Sprite GetMoneyIcon(int itemID)
        {
            var item = KItemManager.Instance.GetItem(itemID);
            if (item != null)
            {
                return GetItemIcon(item.iconName);
            }
            return null;
        }

        public Sprite GetSkillIcon(int itemID)
        {
            var item = KItemManager.Instance.GetItem(itemID);
            if (item != null)
            {
                return GetIcon(item.iconName, "SkillIcons");
            }
            return null;
        }

        public Sprite GetPropIcon(int itemID)
        {
            var item = KItemManager.Instance.GetItem(itemID);
            if (item != null)
            {
                return GetItemIcon(item.iconName);
            }
            return null;
        }

        public Sprite GetBuildingIcon(string iconName)
        {
            return GetIcon(iconName, "BuildingIcons");
        }

        public Sprite GetBackGroundIcon(string iconName)
        {
            return GetIcon(iconName, "BuildingSuitIcons");
        }

        public Sprite GetHeadIcon(string iconName)
        {
            if (string.IsNullOrEmpty(iconName))
            {
                iconName = "Icon_Touxiang1_01";
            }
            return GetIcon(iconName, "HeadIcons");
        }

        public Sprite GetHeadIcon(int iconId)
        {
            if (iconId > 0)
                return GetHeadIcon(XTable.ItemXTable.GetByID(iconId).Icon);
            return null;
        }

        public Sprite GetCatIcon(string iconName)
        {
            return GetIcon(iconName, "CatIcons");
        }

        public Sprite GetCatFull(string iconName)
        {
            return GetIcon(iconName, "CatBigIcons");
        }

        public Sprite GetMatch3ElementIcon(string iconName)
        {
            return GetIcon(iconName, "ElementIcons");
        }

        #endregion

        public void LoadAllIcons()
        {
            _textureSet.Clear();
            _spriteDictionary.Clear();
            var atlasNames = new string[] {
                "ItemIcons",
                "BuildingIcons",
                "BuildingSuitIcons",
                "CatIcons",
                "CatBigIcons",
                "ElementIcons",
                "SkillIcons",
                "HeadIcons"
            };
            foreach (var atlasName in atlasNames)
            {
                GameObject atlasObj;
                if (KAssetManager.Instance.TryGetGlobalPrefab(atlasName, out atlasObj))
                {
                    var atlas = atlasObj.GetComponent<KUIAtlas>();
                    foreach (var sprite in atlas.sprites)
                    {
                        _spriteDictionary.Add(sprite.name, sprite);
                        _textureSet.Add(sprite.texture);
                    }
                }
            }
        }

        private HashSet<Texture2D> _textureSet = new HashSet<Texture2D>();
        private Dictionary<string, Sprite> _spriteDictionary = new Dictionary<string, Sprite>();

        private Sprite GetIcon(string iconName, string atlasName)
        {
            Sprite sprite;
            if (_spriteDictionary.TryGetValue(iconName, out sprite))
            {
                if (!sprite.texture)
                {
                    Debug.Log("" + sprite.name);
                }
                return sprite;
            }

            GameObject atlasObj;
            if (KAssetManager.Instance.TryGetGlobalPrefab(atlasName, out atlasObj))
            {
                var atlas = atlasObj.GetComponent<KUIAtlas>();
                sprite = atlas.GetSprite(iconName);
                if (sprite)
                {
                    _spriteDictionary.Add(iconName, sprite);
                    _textureSet.Add(sprite.texture);
                }
            }
            return sprite;
        }
    }
}

