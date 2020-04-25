using Game;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LocationTextTools
{
    #region Plugins Menu

    [MenuItem("Tools/UI本地化语言组件/Adjust Single Prefab")]
    public static void AdjustSinglePrefab()
    {
        GameObject go = Selection.activeGameObject;
        if (go == null) return;

        LoadLanguageJson();
        OnSwitch(go);
    }

    [MenuItem("Tools/UI本地化语言组件/Adjust All Prefabs")]
    public static void AdjustAllPrefabs()
    {
        //创建canvas
        GameObject objCanvas = new GameObject("Canvas", typeof(Canvas));
        DirectoryInfo abResOrignal = new DirectoryInfo(UIPrefabDir);
        FileInfo[] files = abResOrignal.GetFiles();
        if (files.Length == 0) return;

        LoadLanguageJson();
        string filterExtension = ".meta";
        //GameObject uiObject;
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = files[i];
            if (file.Extension.Contains(filterExtension))
                continue;

            EditorUtility.DisplayProgressBar("Adjust all prefabs location text", "调整中..." + file.FullName, i/(float)files.Length);
            //创建prefab
            GameObject objPrefab = AssetDatabase.LoadAssetAtPath(UIPrefabDir + "\\" + file.FullName.Split('\\')[file.FullName.Split('\\').Length - 1], typeof(GameObject)) as GameObject;
            OnSwitch(objPrefab);
        }

        GameObject.DestroyImmediate(objCanvas);
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    #endregion

    private static string UIPrefabDir = "Assets\\Res\\UI";
    private static string LangJsonPath = "Assets\\Res\\Table\\language.txt";

    private static KLanguage _cnLanguages;
    private static KLanguage _enLanguages;

    private static void LoadLanguageJson()
    {
        TextAsset asset = AssetDatabase.LoadAssetAtPath<TextAsset>(LangJsonPath);
        Hashtable table = asset.bytes.ToJsonTable();
        var langList = table.GetArrayList("language");
        if (langList != null && langList.Count > 0)
        {
            _cnLanguages = new KLanguage();
            _cnLanguages.LoadEntries(table.GetArrayList("zh_CN"));
            _cnLanguages.SetAsDefault();

            _enLanguages = new KLanguage();
            _enLanguages.LoadEntries(table.GetArrayList("en_US"));
            _enLanguages.SetAsDefault();
        }
    }

    private static void OnSwitch(GameObject orgPrefab)
    {
        if (orgPrefab)
        {
            GameObject objInsPre = GameObject.Instantiate(orgPrefab);
            // objInsPre.transform.SetParent(objCanvas.transform, false);

            bool isChange = false;
            Text[] instChilds = objInsPre.GetComponentsInChildren<Text>(true);
            foreach (var value in instChilds)
            {
                if (value is LocationText) continue;

                isChange = true;

                GameObject obj = value.gameObject;
                //创建物体继承text的数值
                GameObject objTmp = GameObject.Instantiate(obj);
                //销毁原有text组件，添加locationtext组件
                Component.DestroyImmediate(obj.GetComponent<Text>());
                obj.AddComponent<LocationText>();
                //把原有text属性赋值给locationtext组件
                LocationText textNew = obj.GetComponent<LocationText>();
                Text textTmp = objTmp.GetComponent<Text>();

                textNew.text = textTmp.text;
                textNew.font = textTmp.font;
                textNew.fontStyle = textTmp.fontStyle;
                textNew.fontSize = textTmp.fontSize;
                textNew.lineSpacing = textTmp.lineSpacing;

                textNew.alignment = textTmp.alignment;
                textNew.alignByGeometry = textTmp.alignByGeometry;
                textNew.horizontalOverflow = textTmp.horizontalOverflow;
                textNew.verticalOverflow = textTmp.verticalOverflow;
                textNew.resizeTextForBestFit = textTmp.resizeTextForBestFit;
                textNew.color = textTmp.color;
                textNew.material = textTmp.material;
                textNew.raycastTarget = textTmp.raycastTarget;
                //销毁用来复制属性的物体
                GameObject.DestroyImmediate(objTmp);

                var transform = obj.transform;
                string path = transform.name;
                while (transform.parent)
                {
                    transform = transform.parent;
                    path = transform.name + "/" + path;
                }

                Debug.Log("Text组件需要替换: " + path);
            }

            LocationText[] locationTexts = objInsPre.GetComponentsInChildren<LocationText>(true);
            foreach (var value in locationTexts)
            {
                //value.enabled = true;
                //value.supportRichText = true;
                if (value.gameObject.transform.parent != null)
                {
                    GameObject parent = value.gameObject.transform.parent.gameObject;
                    if (parent.GetComponent<InputField>() != null)
                    {
                        if (parent.GetComponent<InputField>().textComponent != value)
                        {
                            isChange = true;
                            parent.GetComponent<InputField>().textComponent = value;
                            value.supportRichText = false;

                            var transform = value.transform;
                            string path = transform.name;
                            while (transform.parent)
                            {
                                transform = transform.parent;
                                path = transform.name + "/" + path;
                            }

                            Debug.Log("LocationText组件InputField修改: " + path);
                        }
                    }
                }

                if (SetIDLanguage(value))
                {
                    isChange = true;

                    var transform = value.transform;
                    string path = transform.name;
                    while (transform.parent)
                    {
                        transform = transform.parent;
                        path = transform.name + "/" + path;
                    }

                    Debug.Log("LocationText组件IDLanguage修改: " + path);
                }

            }

            if (isChange)
            {
                //自动修改Prefab
                PrefabUtility.ReplacePrefab(objInsPre, orgPrefab, ReplacePrefabOptions.Default);
            }

            GameObject.DestroyImmediate(objInsPre);
        }
    }

    private static bool SetIDLanguage(LocationText locationText)
    {
        bool b = false;
        int id = GetIDByContent(locationText.text);
        if (id == -100)
        {
            id = 0;
        }
        if (locationText.IDLanguage != id)
        {
            b = true;
            locationText.IDLanguage = id;
        }

        if (locationText.IDLanguage == 0)
        {
            var transform = locationText.transform;
            string path = transform.name;
            while (transform.parent)
            {
                transform = transform.parent;
                path = transform.name + "/" + path;
            }
            Write(path + " => " + locationText.text + "\r\n");
        }
        return b;
    }

    private static int GetIDByContent(string value)
    {
        if (string.IsNullOrEmpty(value) || IsNum(value))
        {
            return -100;
        }
        value = value.Replace(" ", "");

        int id = 0;
        _cnLanguages.TryGetId(value, out id);
        if (id == 0)
            _enLanguages.TryGetId(value, out id);
        return id;
    }

    private static void Write(string value)
    {
        FileStream fs = new FileStream(Application.dataPath + "/TextNotInLanguage.txt", FileMode.Append);
        //获得字节数组
        byte[] data = System.Text.Encoding.Default.GetBytes(value);
        //开始写入
        fs.Write(data, 0, data.Length);
        //清空缓冲区、关闭流
        fs.Flush();
        fs.Close();
    }

    private static bool IsChinese(string str)
    {
        if (System.Text.RegularExpressions.Regex.IsMatch(str, @"[\u4e00-\u9fa5]"))
            return true;
        else return false;
    }

    private static bool IsNum(string str)
    {
        if (System.Text.RegularExpressions.Regex.IsMatch(str, @"^[0-9]*$"))
            return true;
        else return false;
    }

}