using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Build
{
    public class BuildWindow : EditorWindow
    {
        #region Static  

        public static BuildWindow Instance;

        /// <summary>Creates the window.</summary>
        //[MenuItem("游戏/编辑器")]
        public static void CreateWindow()
        {
            if (!BuildWindow.Instance)
            {
                EditorWindow.GetWindow<BuildWindow>("Editor").Show();
            }
        }

        #endregion

        #region Field

        /// <summary>
        /// 
        /// </summary>
        private bool _showGrid;
        /// <summary>
        /// 
        /// </summary>
        private string _showMessage = "";

        #endregion

        #region Property

        internal bool showGrid
        {
            get { return _showGrid; }
            private set
            {
                if (_showGrid != value)
                {
                    _showGrid = value;
                    _showMessage = "显示网络";
                    EditorPrefs.SetBool("showGrid", value);
                }
            }
        }

        #endregion

        #region Method

        private void DrawMapGrid(int w, int h)
        {
            float uw_2 = MapConfig.UnitWidth_2;
            float uh_2 = MapConfig.UnitHeight_2;

            var center = new Vector3(((w + h) / 2) * uw_2, ((h - w) / 2) * uh_2);

            for (int i = 0; i < w; i++)
            {
                Vector3 p1 = new Vector3(i * uw_2, (-i - 1) * uh_2) - center;
                Vector3 p2 = new Vector3((i + h) * uw_2, (-i - 1 + h) * uh_2) - center;
                Handles.DrawLine(p1, p2);
            }

            for (int j = 0; j < h; j++)
            {
                Vector3 p1 = new Vector3(j * uw_2, (j - 1) * uh_2) - center;
                Vector3 p2 = new Vector3(j * uw_2 + w * uw_2, (j - w - 1) * uh_2) - center;
                Handles.DrawLine(p1, p2);
            }
        }

        private void DrawAssets(Rect screenRect)
        {
            GUILayout.BeginArea(screenRect, EditorUtils.TextAreaStyle);
            GUILayout.Space(4f);

            GUILayout.BeginHorizontal();
            GUILayout.Label("当前主题");
            GUILayout.Space(36f);

            GUILayout.EndHorizontal();

            GUILayout.Space(4f);

            if (GUILayout.Button("创建区域"))
            {
                Selection.activeGameObject = new GameObject("Area_001", typeof(AreaData));
                _showMessage = "创建区域";
            }

            GUILayout.EndArea();
        }

        private void DrawMessage(Rect screenRect)
        {
            GUILayout.BeginArea(screenRect);
            EditorGUILayout.HelpBox(this._showMessage, MessageType.Info);
            GUILayout.EndArea();
        }

        private void DrawSearch(Rect screenRect)
        {
            GUILayout.BeginArea(screenRect, EditorUtils.TextAreaStyle);
            GUILayout.Space(4f);
            GUILayout.BeginHorizontal();

            //string text = GUILayout.TextField(this._searchText, BabaEditor.searchTextField, GUILayout.MaxWidth(180f));

            //if (GUILayout.Button(GUIContent.none, string.IsNullOrEmpty(text) ? BabaEditor.searchCancelButtonEmpty : BabaEditor.searchCancelButton))
            //{
            //    text = string.Empty;
            //}

            //if (text != this._searchText)
            //{
            //    // 可以用正则 递增匹配 增加内存
            //    this._searchList.Clear();

            //    if (!string.IsNullOrEmpty(text))
            //    {
            //        if (this._globalPrefabs != null && this._globalPrefabs.Count > 0)
            //        {
            //            foreach (string key in this._globalPrefabs.Keys)
            //            {
            //                if (key.StartsWith(text, true, null))
            //                {
            //                    this._searchList.Add(key);
            //                }
            //            }
            //        }
            //        if (this._chapterPrefabs != null && this._chapterPrefabs.Count > 0)
            //        {
            //            foreach (string key in this._chapterPrefabs.Keys)
            //            {
            //                if (key.StartsWith(text, true, null))
            //                {
            //                    this._searchList.Add(key);
            //                }
            //            }
            //        }
            //    }

            //    this._searchText = text;
            //    this._searchArray = this._searchList.ToArray();
            //}

            GUILayout.EndHorizontal();

            GUILayout.Space(2f);
            //scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            //int index = GUILayout.SelectionGrid(-1, this._searchArray, 1, GUILayout.MaxWidth(166f));
            //GUILayout.EndScrollView();

            //if (index != -1)
            //{
            //    string prefabKey = this._searchArray[index];
            //    if (this.CreateObject(prefabKey))
            //    {
            //        this._showMessage = string.Format("Msg: {0} success !", prefabKey);
            //    }
            //    else
            //    {
            //        this._showMessage = string.Format("Msg: {0} not exists !", prefabKey);
            //    }

            //    if (!this._historyList.Contains(prefabKey))
            //    {
            //        if (this._historyList.Count >= _historyMax)
            //        {
            //            this._historyList.Dequeue();
            //        }
            //        this._historyList.Enqueue(prefabKey);

            //        this._historyArray = this._historyList.ToArray();
            //    }
            //}

            GUILayout.EndArea();
        }

        private void DrawHistory(Rect screenRect)
        {
            //int index = GUILayout.SelectionGrid(-1, this._historyArray, 8);
            //if (index != -1)
            //{
            //    string prefabKey = this._historyArray[index];
            //    if (this.CreateObject(prefabKey))
            //    {
            //        this._showMessage = string.Format("Msg: {0} success !", prefabKey);
            //    }
            //    else
            //    {
            //        this._showMessage = string.Format("Msg: {0} not exists !", prefabKey);
            //    }
            //}
        }

        private void DrawFunction(Rect screenRect)
        {
            GUILayout.BeginArea(screenRect, EditorUtils.TextAreaStyle);
            GUILayout.Space(2f);
            this.showGrid = EditorGUILayout.Toggle("显示网格", _showGrid);
            //this.showHistory = EditorGUILayout.Toggle("显示历史", this._showHistory);
            //this.showReplace = EditorGUILayout.Toggle("关闭弯曲", this._showReplace);
            GUILayout.EndArea();
        }

        private void BeginArea(Rect sceenRect)
        {
            GUILayout.BeginArea(sceenRect, GUI.skin.box);
        }

        private void EndArea()
        {
            GUILayout.EndArea();
        }

        #endregion

        #region Unity

        public void OnEnable()
        {
            BuildWindow.Instance = this;
            SceneView.onSceneGUIDelegate = this.OnSceneFunc;

            _showGrid = EditorPrefs.GetBool("showGrid");
            //this._showHistory = EditorPrefs.GetBool("showHistory");
            //this._showReplace = EditorPrefs.GetBool("showReplace");

            //InitRes((Chapter)EditorPrefs.GetInt("currChapter", (int)Chapter.Chapter1), true);
        }

        public void OnDisable()
        {
            SceneView.onSceneGUIDelegate = null;
            BuildWindow.Instance = null;
        }

        public void OnGUI()
        {
            GUILayout.Label("\n  说明：\n\n        编辑器窗口 编辑时请不要关闭！", GUILayout.MinWidth(this.position.width), GUILayout.MinHeight(this.position.height));
        }

        private void OnSceneFunc(SceneView sceneView)
        {
            if (_showGrid)
            {
                Handles.color = new Color32(22, 99, 3, 180);
                Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
                this.DrawMapGrid(240, 240);
            }

            Handles.BeginGUI();

            Vector2 size = sceneView.position.size;

            this.BeginArea(new Rect(0f, 0f, 200f, size.y));

            this.DrawAssets(new Rect(3f, 4f, 195f, 92f));

            this.DrawSearch(new Rect(3f, 98f, 195f, size.y - 224f));

            this.DrawFunction(new Rect(3f, size.y - 124f, 195f, 64f));

            this.DrawMessage(new Rect(3f, size.y - 58f, 195f, 58f));

            this.EndArea();

            //if (this._showHistory)
            //{
            //    this.BeginArea(new Rect(202f, size.y - 124f, size.x - 204f, 120f));

            //    this.DrawHistory(new Rect(2, 2, size.x - 208f, 140));

            //    this.EndArea();
            //}

            Handles.EndGUI();
        }

        #endregion
    }
}
