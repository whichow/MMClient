/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/6 16:03:16
 * Description:     资源被预设引用反查工具
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class PluginMenu
{
    #region Plugins Menu

    [MenuItem("Tools/资源引用分析", false, 2)]
    public static void OpenRes2Check()
    {
        ResourceCheckTool.Open();
    }

    #endregion
}

public class ResourceCheckTool : EditorWindow
{
    #region Member variables

    private List<string> m_SelectPathList;
    private List<string> m_SelectGUIDList;
    private List<string> m_PrefabList;
    private List<string> M_ResNotBeRefs;
    private Dictionary<string, List<string>> m_DicResPrefabs;
    private string m_strCurrentPerfabPath;
    private string m_strUIPerfabPath = "Assets/Res/";
    private Vector2 m_scrollPos;

    #endregion

    #region Public Method

    public static void Open()
    {
        GetWindowWithRect(typeof(ResourceCheckTool), new Rect(0, 0, 700, 800), true);
    }

    void OnGUI()
    {
        if (null != m_SelectPathList && 0 != m_SelectPathList.Count)
        {
            GUILayout.Space(10);
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("重新分析 - UI", GUILayout.Height(50)))
            {
                __Init();
                __GetSelectItem();
                __GetAllPrefabs(m_strCurrentPerfabPath);
                __CheckEveryPrefab();
                m_strCurrentPerfabPath = m_strUIPerfabPath;
            }
            GUILayout.Space(10);

            GUI.backgroundColor = Color.grey;
            m_scrollPos = GUILayout.BeginScrollView(m_scrollPos, true, true, GUILayout.Height(720));
            GUILayout.Space(10);

            List<string> lists = null;
            GUILayout.Label("当前预设资源路径为：");
            GUILayout.Label(m_strCurrentPerfabPath);

            if (0 == m_SelectPathList.Count)
            {
                GUILayout.Label("------------------------------------------------------------------------------");
                GUILayout.Label("当前分析的资源为：");
                GUILayout.Label("此资源尚未被任何预设引用过，可以考虑删除！");
            }

            if (m_DicResPrefabs != null)
            {
                for (int nInedx = 0; nInedx != m_SelectPathList.Count; ++nInedx)
                {
                    if (m_DicResPrefabs.TryGetValue(m_SelectGUIDList[nInedx], out lists))
                    {
                        if (null != lists && 0 != lists.Count)
                        {
                            GUILayout.Label("------------------------------------------------------------------------------");
                            GUILayout.Label("当前分析的资源为：");
                            GUILayout.Label(m_SelectPathList[nInedx]);
                            GUILayout.Label("当前分析的资源引用信息为：(" + lists.Count + "个Prefab)");

                            foreach (string str in lists)
                            {
                                GUILayout.Label(str);
                            }
                        }
                        else
                        {
                            M_ResNotBeRefs.Add(m_SelectPathList[nInedx]);
                        }
                    }
                }
            }

            if (0 != M_ResNotBeRefs.Count)
            {
                GUILayout.Label("------------------------------------------------------------------------------");
                GUILayout.Label("以下为完全没被引用过的资源：");
                M_ResNotBeRefs.Sort();
                foreach (string str in M_ResNotBeRefs)
                {
                    GUILayout.Label(str);
                }
                M_ResNotBeRefs.Clear();
            }

            GUILayout.Label("------------------------------------------------------------------------------");

            GUILayout.EndScrollView();
        }
        else
        {
            GUILayout.Space(20);
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("分析 - UI", GUILayout.Height(50)))
            {
                __Init();
                __GetSelectItem();
                __GetAllPrefabs(m_strUIPerfabPath);
                __CheckEveryPrefab();
                m_strCurrentPerfabPath = m_strUIPerfabPath;
            }
            GUILayout.Space(30);

            GUILayout.Label(" 当前没有资源被选中，选择你需要分析的资源然后点【分析】", new GUIStyle()
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleCenter,
                normal = new GUIStyleState() { textColor = Color.yellow }
            });

            GUILayout.Space(10);
        }

    }

    #endregion

    #region private Method

    private void __Init()
    {
        if (null == m_SelectPathList)
        {
            m_SelectPathList = new List<string>();
        }

        m_SelectPathList.Clear();

        if (null == m_SelectGUIDList)
        {
            m_SelectGUIDList = new List<string>();
        }

        m_SelectGUIDList.Clear();

        if (null == m_PrefabList)
        {
            m_PrefabList = new List<string>();
        }
        m_PrefabList.Clear();

        if (null == m_DicResPrefabs)
        {
            m_DicResPrefabs = new Dictionary<string, List<string> > ();
        }
        m_DicResPrefabs.Clear();

        if (null == M_ResNotBeRefs)
        {
            M_ResNotBeRefs = new List<string>();
        }
        M_ResNotBeRefs.Clear();

        //m_strUIPerfabPath = "Assets/Res/";
    }

    private void __GetSelectItem()
    {
        var objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object o in objs)
        {
            var path = AssetDatabase.GetAssetPath(o);

            // 过滤掉meta文件和文件夹
            if (path.Contains(".meta") || path.Contains(".") == false)
            {
                continue;
            }

            m_SelectPathList.Add(path);
            m_SelectGUIDList.Add(AssetDatabase.AssetPathToGUID(path));

            //Debug.Log("path = " + path);
            //Debug.Log("GUID = " + AssetDatabase.AssetPathToGUID(path));
        }
    }

    private void __GetAllPrefabs(string strPrefabsPath)
    {
        if (null == m_SelectPathList || 0 == m_SelectPathList.Count)
        {
            Debug.Log("请在Project中选择需要分析的资源！");
            return;
        }

        //var dirArr = Directory.GetDirectories(strPrefabsPath);
        //for (int i = 0; i < dirArr.Length; i++)
        //{
        //    var pathArr = __GetFiles(dirArr[i]);
        //    for (int j = 0; j < pathArr.Length; j++)
        //    {
        //        var filePath = pathArr[j];
        //        // Debug.Log("filePath: "+ "[" + i + "]" + "[" + j + "] = " + filePath);
        //        m_PrefabList.Add(filePath);
        //    }
        //}

        var paths = __GetFiles(strPrefabsPath);
        for (int j = 0; j < paths.Length; j++)
        {
            var filePath = paths[j];
            if (Path.GetExtension(filePath) == ".prefab")
            {
                // Debug.Log("filePath: " +  "[" + j + "] = " + filePath);
                m_PrefabList.Add(filePath);
            }
        }

        Debug.Log(" m_PrefabList.Count = " + m_PrefabList.Count);
    }

    private void __CheckEveryPrefab()
    {
        if (null == m_SelectGUIDList || 0 == m_SelectGUIDList.Count)
        {
            return;
        }

        if (null == m_SelectPathList || 0 == m_SelectPathList.Count)
        {
            return;
        }

        if (null == m_PrefabList || 0 == m_PrefabList.Count)
        {
            return;
        }

        m_DicResPrefabs.Clear();
        int nLen = m_SelectPathList.Count;
        for (int nInedx = 0; nInedx != nLen; ++nInedx)
        {
            string strFilePath = m_SelectPathList[nInedx];
            string strFileGUID = m_SelectGUIDList[nInedx];
            List<string> list = null;
            if (!m_DicResPrefabs.TryGetValue(strFileGUID, out list))
            {
                list = new List<string>();
                list.Clear();
                m_DicResPrefabs.Add(strFileGUID, list);
            }

            if (null != list)
            {
                if(__CheckByGUID(strFilePath, strFileGUID, ref list) == 2)
                {
                    return;
                }
            }
        }
    }

    private int __CheckByGUID(string strFilePath, string strGUID, ref List<string> list)
    {
        if (null == strGUID || null == list)
        {
            return 0;
        }

        if (null == m_PrefabList || 0 == m_PrefabList.Count)
        {
            return 0;
        }

        int nLen = m_PrefabList.Count;
        string strPrefabFile = null;
        try
        {
            strFilePath = Path.GetFileName(strFilePath);
            for (int nIndex = 0; nIndex != nLen; ++nIndex)
            {
                strPrefabFile = m_PrefabList[nIndex];
                if (EditorUtility.DisplayCancelableProgressBar("搜索预设引用关系: " + strFilePath, "搜索中..." + strPrefabFile, nIndex / (float)nLen))
                {
                    EditorUtility.ClearProgressBar();
                    return 2;
                }
                FileStream fs = new FileStream(strPrefabFile, FileMode.Open, FileAccess.Read);
                byte[] buff = new byte[fs.Length];
                fs.Read(buff, 0, (int)fs.Length);
                fs.Close();
                string strText = Encoding.Default.GetString(buff);

                int nStar = 0;
                int nCount = 0;
                while (-1 != nStar)
                {
                    nStar = strText.IndexOf(strGUID, nStar);
                    if (-1 != nStar)
                    {
                        nCount++;
                        nStar++;
                    }
                }

                if (0 != nCount)
                {
                    strText = m_PrefabList[nIndex] + " 在此资源中被引用 " + nCount + " 次";
                    list.Add(strText);
                }
            }
            EditorUtility.ClearProgressBar();
        }
        catch (System.Exception ex)
        {
            Debug.Log("__CheckByGUID Error");
            return -1;
        }
        return 1;
    }

    /// <summary>
    /// 获取目录下的所有对象路径，去掉了.meta
    /// </summary>
    /// <param name="path">目录路径</param>
    /// <param name="recursive">是否递归获取</param>
    /// <returns></returns>
    private string[] __GetFiles(string path, bool recursive = true)
    {
        var resultList = new List<string>();
        var dirArr = Directory.GetFiles(path, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        for (int i = 0; i < dirArr.Length; i++)
        {
            if (Path.GetExtension(dirArr[i]) != ".meta")
            {
                resultList.Add(dirArr[i].Replace('\\', '/'));
            }
        }
        return resultList.ToArray();
    }

    #endregion


}
