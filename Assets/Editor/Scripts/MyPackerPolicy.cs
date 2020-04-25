// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 2017-11-18
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "MyPackerPolicy" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Sprites;
using System.Collections.Generic;

// DefaultPackerPolicy will pack rectangles no matter what Sprite mesh type is unless their packing tag contains.
class MyPackerPolicy : IPackerPolicy
{
    protected class Entry
    {
        public string atlasName;
        public Sprite sprite;
        public AtlasSettings settings;
        public SpritePackingMode packingMode;
    }

    public virtual int GetVersion() { return 1; }

    public virtual bool AllowSequentialPacking { get; }

    protected virtual string TagPrefix { get { return "[TIGHT]"; } }

    protected virtual bool AllowTightWhenTagged { get { return true; } }

    public void OnGroupAtlases(BuildTarget target, PackerJob job, int[] textureImporterInstanceIDs)
    {
        Debug.Log("Begin pack atlas ");

        List<Entry> entries = new List<Entry>();

        foreach (int instanceID in textureImporterInstanceIDs)
        {
            TextureFormat desiredFormat;
            ColorSpace colorSpace;
            int compressionQuality;
            var ti = EditorUtility.InstanceIDToObject(instanceID) as TextureImporter;
            ti.ReadTextureImportInstructions(target, out desiredFormat, out colorSpace, out compressionQuality);

            var tis = new TextureImporterSettings();
            ti.ReadTextureSettings(tis);

            var sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(ti.assetPath).Select(x => x as Sprite).Where(x => x != null).ToArray();
            foreach (Sprite sprite in sprites)
            {
                //在这里设置每个图集的参数
                Entry entry = new Entry
                {
                    sprite = sprite
                };
                entry.settings.format = desiredFormat;
                entry.settings.colorSpace = colorSpace;
                entry.settings.compressionQuality = compressionQuality;

                entry.settings.filterMode = Enum.IsDefined(typeof(FilterMode), ti.filterMode) ? ti.filterMode : FilterMode.Bilinear;
                entry.settings.maxWidth = 1024;
                entry.settings.maxHeight = 1024;
                entry.atlasName = ParseAtlasName(ti.spritePackingTag);
                entry.packingMode = GetPackingMode(ti.spritePackingTag, tis.spriteMeshType);

                entries.Add(entry);
            }
        }

        // First split sprites into groups based on atlas name
        var atlasGroups =
            from e in entries
            group e by e.atlasName;

        foreach (var atlasGroup in atlasGroups)
        {
            int page = 0;
            // Then split those groups into smaller groups based on texture settings
            var settingsGroups =
                from t in atlasGroup
                group t by t.settings;

            foreach (var settingsGroup in settingsGroups)
            {
                string atlasName = atlasGroup.Key;
                if (settingsGroups.Count() > 1)
                {
                    //atlasName += string.Format(" (Group {0})", page);
                }

                job.AddAtlas(atlasName, settingsGroup.Key);
                foreach (Entry entry in settingsGroup)
                {
                    job.AssignToAtlas(atlasName, entry.sprite, entry.packingMode, SpritePackingRotation.None);
                }

                page++;
            }
        }

        Debug.Log("End pack atlas ");
    }

    protected bool IsTagPrefixed(string packingTag)
    {
        packingTag = packingTag.Trim();
        if (packingTag.Length < TagPrefix.Length)
        {
            return false;
        }
        return (packingTag.Substring(0, TagPrefix.Length) == TagPrefix);
    }

    private string ParseAtlasName(string packingTag)
    {
        string name = packingTag.Trim();
        //if (IsTagPrefixed(name))
        //{
        //    name = name.Substring(TagPrefix.Length).Trim();
        //}
        return (name.Length == 0) ? "(unnamed)" : name;
    }

    private SpritePackingMode GetPackingMode(string packingTag, SpriteMeshType meshType)
    {
        if (meshType == SpriteMeshType.Tight)
            //if (IsTagPrefixed(packingTag) == AllowTightWhenTagged)
                return SpritePackingMode.Tight;
        return SpritePackingMode.Rectangle;
    }
}
