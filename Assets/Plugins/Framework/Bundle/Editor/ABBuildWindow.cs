// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace K.AB
{
    public class ABBuildWindow : EditorWindow
    {
        #region Menu
        public static ABBuildWindow Instance;

        [MenuItem("Tools/Builder Window", false, 1)]
        static void Open()
        {
            Instance = GetWindow<ABBuildWindow>("ABBuildWindow", true);
        }

        public static void BuildAssetBundles(bool isHot = false)
        {
            var abConfig = LoadAssetAtPath<ABBuildConfig>(CONFIG_SAVE_PATH);
            if (abConfig == null)
            {
                return;
            }

            ABBuilder builder = new ABBuilder5x();

            builder.Begin();

            for (int i = 0; i < abConfig.filters.Count; i++)
            {
                var filter = abConfig.filters[i];
                if (filter.valid)
                {
                    if (filter.filter == "t:GameObject")
                    {
                        builder.AddRootTargets("t:GameObject", new string[] { filter.path });
                    }
                    else if (filter.filter == "t:TextAsset")
                    {
                        builder.AddRootTargets("t:TextAsset", new string[] { filter.path });
                    }
                    else if (filter.filter == "t:AudioClip")
                    {
                        builder.AddRootTargets("t:AudioClip", new string[] { filter.path });
                    }
                }
            }

            builder.Build();
            builder.End(isHot);

            Debug.Log("保存资源: " + (isHot ? ABBuilder.HotSavePath : ABDefine.SavePath));
        }

        private static T LoadAssetAtPath<T>(string path) where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }

        #endregion 

        #region Field

        const string CONFIG_SAVE_PATH = "Assets/Editor/ABConfig.asset";

        private ABBuildConfig _abBuildConfig;
        private ReorderableList _abFilterList;
        private Vector2 _scrollPosition = Vector2.zero;
        private Vector3 _progress = Vector3.zero;


        #endregion

        #region Method 

        private void InitConfig()
        {
            _abBuildConfig = LoadAssetAtPath<ABBuildConfig>(CONFIG_SAVE_PATH);
            if (_abBuildConfig == null)
            {
                _abBuildConfig = ScriptableObject.CreateInstance<ABBuildConfig>();
            }
        }

        private void Add()
        {
            string path = SelectFolder();
            if (!string.IsNullOrEmpty(path))
            {
                var filter = new ABFilter();
                filter.path = path;
                _abBuildConfig.filters.Add(filter);
            }
        }

        private void OnAdd(ReorderableList list)
        {
            Add();
        }

        private void Remove(ABFilter filter)
        {
            if (filter != null)
            {
                _abBuildConfig.filters.Remove(filter);
            }
        }

        private void OnRemove(ReorderableList list)
        {
            Remove(list.list[list.index] as ABFilter);
        }

        private void Build(bool isHot = false)
        {
            Save();
            BuildAssetBundles(isHot);
        }

        private void Save()
        {
            if (LoadAssetAtPath<ABBuildConfig>(CONFIG_SAVE_PATH) == null)
            {
                AssetDatabase.CreateAsset(_abBuildConfig, CONFIG_SAVE_PATH);
            }
            else
            {
                EditorUtility.SetDirty(_abBuildConfig);
            }
        }

        private void ClearAB()
        {
            //ABUtils.DeleteDirectory(ABBuilder.BUILD_PATH);
        }

        private string SelectFolder()
        {
            string dataPath = Application.dataPath;
            string selectedPath = EditorUtility.OpenFolderPanel("Path", dataPath, "");

            if (!string.IsNullOrEmpty(selectedPath))
            {
                if (selectedPath.StartsWith(dataPath))
                {
                    return selectedPath.Substring(dataPath.Length - 6);
                }
                else
                {
                    ShowNotification(new GUIContent("只能选择资源目录"));
                }
            }

            return null;
        }

        public void ShowProgress(float value, float min, float max)
        {
            _progress = new Vector3(min, max, value);
        }

        #endregion

        private void InitFilterList()
        {
            _abFilterList = new ReorderableList(_abBuildConfig.filters, typeof(ABFilter));
            _abFilterList.draggable = true;
            _abFilterList.elementHeight = 22;
            _abFilterList.drawElementCallback = OnListElementGUI;
            _abFilterList.drawHeaderCallback = OnListHeaderGUI;
            _abFilterList.onAddCallback = OnAdd;
            _abFilterList.onRemoveCallback = OnRemove;
        }

        private void OnListElementGUI(Rect rect, int index, bool isactive, bool isfocused)
        {
            const float space = 5;

            var filter = _abBuildConfig.filters[index];
            rect.y += 1;

            Rect r = rect;
            r.width = 18;
            r.height = 18;
            filter.valid = EditorGUI.Toggle(r, filter.valid);

            r.xMin = r.xMax + space;
            r.xMax = rect.xMax - 300;
            GUI.enabled = false;

            filter.path = EditorGUI.TextField(r, filter.path);
            GUI.enabled = true;

            r.xMin = r.xMax + space;
            r.width = 50;
            if (GUI.Button(r, "Select"))
            {
                var path = SelectFolder();
                if (path != null)
                {
                    filter.path = path;
                }
            }

            //r.xMin = r.xMax + space;
            //r.width = 50;
            //filter.buildType = (BuildType)EditorGUI.EnumPopup(r, "", filter.buildType);

            r.xMin = r.xMax + space;
            r.xMax = rect.xMax;            
            filter.filter = EditorGUI.TextField(r, filter.filter);
        }

        private void OnListHeaderGUI(Rect rect)
        {
            EditorGUI.LabelField(rect, "Asset Filter");
            //EditorGUI.Popup(rect, "Asset Filter", 0, new string[] { "t:GameObject", "t:TextAsset" }); //.LabelField(rect, "Asset Filter",,,);
        }

        private void OnProgressGUI(Rect rect)
        {
            EditorGUI.Slider(rect, 0.5f, 0f, 1f);
        }

        #region Unity 

        private void OnGUI()
        {
            if (_abBuildConfig == null)
            {
                InitConfig();
            }

            if (_abFilterList == null)
            {
                InitFilterList();
            }

            bool execBuild = false;
            bool exeBuildHot = false;
            //tool bar
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                if (GUILayout.Button("Add", EditorStyles.toolbarButton))
                {
                    Add();
                }
                GUILayout.Space(5f);
                if (GUILayout.Button("Save", EditorStyles.toolbarButton))
                {
                    Save();
                }
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("ClearAB", EditorStyles.toolbarButton))
                {
                    ClearAB();
                }
                GUILayout.Space(15f);
                if (GUILayout.Button("BuildHotAB", EditorStyles.toolbarButton))
                {
                    exeBuildHot = true;
                }
                GUILayout.Space(15f);
                if (GUILayout.Button("BuildAB", EditorStyles.toolbarButton))
                {
                    execBuild = true;
                }
            }
            GUILayout.EndHorizontal();

            //context
            GUILayout.BeginVertical();
            {
                GUILayout.Space(5);
                GUILayout.HorizontalSlider(_progress.z, _progress.x, _progress.y);
                GUILayout.Space(6);

                //Filter item list
                _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
                {
                    _abFilterList.DoLayoutList();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();

            //set dirty
            if (GUI.changed)
            {
                EditorUtility.SetDirty(_abBuildConfig);
            }

            if (exeBuildHot)
            {
                Build(true);
            }
            else if (execBuild)
            {
                Build();
            }
        }

        #endregion

    }
}