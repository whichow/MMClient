using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public class DynamicFontTextureRebuildTracker : MonoBehaviour
    {
        private class FontUpdateNode
        {
            private bool m_FontTextureRebuilt = false;
            private Font m_FontRebuilt = null;

            public FontUpdateNode(Font font)
            {
                m_FontRebuilt = font;
                Validate();
            }

            public void Validate()
            {
                if (null == m_FontRebuilt)
                {
                    m_FontTextureRebuilt = false;

                    Debug.LogWarning("You need a actual font to validate!");
                    return;
                }

                m_FontTextureRebuilt = true;
            }

            public void Invalidate()
            {
                m_FontTextureRebuilt = false;
            }

            public bool NeedUpdate
            {
                get { return m_FontTextureRebuilt && (null != m_FontRebuilt); }
            }

            public Font font
            {
                get { return m_FontRebuilt; }
            }
        }

        private System.Reflection.MethodInfo m_RebuildForFont = null;
        private List<FontUpdateNode> m_FontUpdateList = new List<FontUpdateNode>();

        private static DynamicFontTextureRebuildTracker m_Instance = null;

        void Awake()
        {
            if (null != m_Instance)
            {
                Debug.LogError("There is only one DynamicFontTextureRebuildTracker instance allowed!");
                Destroy(gameObject);
                return;
            }

            m_Instance = this;
        }

        // Use this for initialization
        void Start()
        {
            Font.textureRebuilt += OnFontTextureRebuilt;

            System.Type fontUpdateTrackerType = typeof(UnityEngine.UI.FontUpdateTracker);
            m_RebuildForFont = fontUpdateTrackerType.GetMethod("RebuildForFont", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Debug.Log("Get RebuildForFont method is: " + m_RebuildForFont);
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (null == m_RebuildForFont)
            {
                return;
            }

            for (int i = 0; i < m_FontUpdateList.Count; i++)
            {
                FontUpdateNode node = m_FontUpdateList[i];
                if (node.NeedUpdate)
                {
                    Font font = node.font;
                    m_RebuildForFont.Invoke(null, new object[] { font });

                    // Log rebuild.
                    Texture fontTexture = font.material.mainTexture;
                    Debuger.LogWarning(string.Format("Texture of dynamic font \"{0}\" is enlarged to {1}x{2}.", font.name, fontTexture.width, fontTexture.height));

                    node.Invalidate();
                }
            }
        }

        void OnDestroy()
        {
            Font.textureRebuilt -= OnFontTextureRebuilt;
        }

        private void OnFontTextureRebuilt(Font font)
        {
            bool findThisFont = false;
            for (int i = 0; i < m_FontUpdateList.Count; i++)
            {
                FontUpdateNode node = m_FontUpdateList[i];
                if (node.font == font)
                {
                    node.Validate();
                    findThisFont = true;
                    break;
                }
            }

            if (!findThisFont)
            {
                m_FontUpdateList.Add(new FontUpdateNode(font));
            }
        }

        //void OnGUI()
        //{
        //    if (GUI.Button(new Rect(30.0f, 50.0f, 200.0f, 50.0f), "Force Update Text"))
        //    {
        //        for (int i = 0; i < m_FontUpdateList.Count; i++)
        //        {
        //            Font font = m_FontUpdateList[i].font;
        //            m_RebuildForFont.Invoke(null, new object[] { font });
        //            Debug.Log(string.Format("Force rebuild text for font \"{0}\".", font.name));
        //        }

        //        Debug.Log("Force rebuild all text ok!");
        //    }
        //}
    }
}