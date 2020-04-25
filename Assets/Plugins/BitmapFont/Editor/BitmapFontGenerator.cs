// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "BitmapFontGenerater" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

public static class BitmapFontGenerater
{
    static string DEFAULT_SHADER = "GUI/Text Shader";

    [MenuItem("Assets/Create/Bitmap Font")]
    public static void GenerateBitmapFont()
    {
        Object[] texts = Selection.GetFiltered(typeof(TextAsset), SelectionMode.DeepAssets);
        Object[] textures = Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);

        if (texts.Length < 1)
        {
            Debug.LogError("BitmapFont Create Error -- XML File is not selected.");
            return;
        }

        if (textures.Length < 1)
        {
            Debug.LogError("BitmapFont Create Error -- Texture is not selected.");
            return;
        }

        Generate((TextAsset)texts[0], (Texture2D)textures[0]);
    }

    private static void Generate(TextAsset textAsset, Texture2D texture)
    {
        var xml = new XmlDocument();
        xml.LoadXml(textAsset.text);

        var commonNode = xml.GetElementsByTagName("common")[0];
        var charNodes = xml.GetElementsByTagName("chars")[0].ChildNodes;

        float textureW = GetFloatValue(commonNode, "scaleW");
        float textureH = GetFloatValue(commonNode, "scaleH");

        var charInfos = new CharacterInfo[charNodes.Count];
        for (int i = 0; i < charNodes.Count; i++)
        {
            var charNode = charNodes[i];
            if (charNode.Attributes != null)
            {
                charInfos[i].index = GetIntValue(charNode, "id");
                float x = GetFloatValue(charNode, "x");
                float y = GetFloatValue(charNode, "y");
                float w = GetFloatValue(charNode, "width");
                float h = GetFloatValue(charNode, "height");
                float xo = GetFloatValue(charNode, "xoffset");
                float yo = GetFloatValue(charNode, "yoffset");

                var vertRect = new Rect(xo, -yo, w, -h);
                charInfos[i].minX = (int)vertRect.xMin;
                charInfos[i].maxX = (int)vertRect.xMax;
                charInfos[i].minY = (int)vertRect.yMax;
                charInfos[i].maxY = (int)vertRect.yMin;


                float charX = x / textureW;
                float charY = 1f - (y + h) / textureH;
                float charW = w / textureW;
                float charH = h / textureH;

                // UnFlipped.
                charInfos[i].uvBottomLeft = new Vector2(charX, charY);
                charInfos[i].uvBottomRight = new Vector2(charX + charW, charY);
                charInfos[i].uvTopLeft = new Vector2(charX, charY + charH);
                charInfos[i].uvTopRight = new Vector2(charX + charW, charY + charH);

                charInfos[i].advance = GetIntValue(charNode, "xadvance");
            }
        }

        string rootPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(textAsset));
        string exportPath = rootPath + "/" + Path.GetFileNameWithoutExtension(textAsset.name);
        string fontName = textAsset.name;
        string fontPath = rootPath + "/" + fontName + ".fontsettings";

        var material = GenerateMaterial(texture);
        var font = GenerateFont(fontName, fontPath, material);
        font.characterInfo = charInfos;
        AssetDatabase.AddObjectToAsset(material, fontPath);
        AssetDatabase.Refresh();

        // Save m_LineSpacing.
        XmlNode info = xml.GetElementsByTagName("info")[0];
        SerializedObject serializedFont = new SerializedObject(font);
        SerializedProperty serializedLineSpacing = serializedFont.FindProperty("m_LineSpacing");
        serializedLineSpacing.floatValue = Mathf.Abs(float.Parse(GetValue(info, "size")));
        serializedFont.ApplyModifiedProperties();
    }

    private static Material GenerateMaterial(Texture2D texture)
    {
        var shader = Shader.Find(DEFAULT_SHADER);
        var material = new Material(shader)
        {
            name = "FontMaterial",
            mainTexture = texture,
        };
        return material;
    }

    private static Font GenerateFont(string fontName, string fontPath, Material material)
    {
        Font font = LoadAsset(fontPath, new Font(fontName));
        font.material = material;
        SaveAsset(font, fontPath);
        return font;
    }

    private static void SaveAsset(Object obj, string path)
    {
        Object existingAsset = AssetDatabase.LoadMainAssetAtPath(path);
        if (existingAsset != null)
        {
            EditorUtility.CopySerialized(obj, existingAsset);
            AssetDatabase.SaveAssets();
        }
        else
        {
            AssetDatabase.CreateAsset(obj, path);
        }
    }

    private static T LoadAsset<T>(string path, T defaultAsset) where T : Object
    {
        T existingAsset = AssetDatabase.LoadMainAssetAtPath(path) as T;
        if (existingAsset == null)
        {
            existingAsset = defaultAsset;
        }
        return existingAsset;
    }

    private static string GetValue(XmlNode node, string name)
    {
        return node.Attributes.GetNamedItem(name).InnerText;
    }

    private static int GetIntValue(XmlNode node, string name)
    {
        var text = node.Attributes.GetNamedItem(name).InnerText;
        int result;
        int.TryParse(text, out result);
        return result;
    }

    private static float GetFloatValue(XmlNode node, string name)
    {
        var text = node.Attributes.GetNamedItem(name).InnerText;
        float result;
        float.TryParse(text, out result);
        return result;
    }
}
