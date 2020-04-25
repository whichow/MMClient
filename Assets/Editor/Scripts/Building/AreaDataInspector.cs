using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.Build
{
    [CustomEditor(typeof(AreaData))]
    public class AreaDataInspector : Editor
    {
        #region Field

        #endregion

        #region Property

        /// <summary>
        /// 
        /// </summary>
        private AreaData areaData
        {
            get { return this.target as AreaData; }
        }

        /// <summary>
        /// Gets the game object.
        /// </summary>
        private GameObject gameObject
        {
            get { return areaData.gameObject; }
        }

        /// <summary>
        /// Gets the transform.
        /// </summary>
        private Transform transform
        {
            get { return areaData.transform; }
        }

        #endregion

        #region Method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void DrawRhombus(int x, int y)
        {
            float unitW_2 = MapConfig.UnitWidth_2;
            float unitH_2 = MapConfig.UnitHeight_2;

            var center = new Vector3((x + y) * unitW_2, (y - x) * unitH_2);

            var points = new Vector3[]
            {
                new Vector3(center.x - unitW_2, center.y, 0f),
                new Vector3(center.x, center.y + unitH_2, 0f),
                new Vector3(center.x + unitW_2, center.y, 0f),
                new Vector3(center.x, center.y - unitH_2, 0f),
                new Vector3(center.x - unitW_2, center.y, 0f),
            };

            Handles.DrawPolyLine(points);
        }

        public void SnapToNavGrid(Vector3 p)
        {
            var grid = MapHelper.PositionToGrid(p);

            this.areaData.gridRect.x = grid.x;
            this.areaData.gridRect.y = grid.y;

            this.transform.position = MapHelper.GridToPosition(grid);
        }
        /// <summary>
        /// 
        /// </summary>
        private void RecalculateBounds()
        {
            int x = this.areaData.gridRect.x;
            int y = this.areaData.gridRect.y;
            int w = this.areaData.gridRect.z;
            int h = this.areaData.gridRect.w;

            float uw_2 = MapConfig.UnitWidth_2;
            float uh_2 = MapConfig.UnitHeight_2;

            Vector2[] points = new Vector2[4]
            {
                Vector2.zero,
                new Vector2(uw_2 * h , uh_2 * h ),
                new Vector2(uw_2 * (w + h) ,uh_2 * (h - w)),
                new Vector2(uw_2 * w , uh_2 * (-w)),
            };

            var polygon = this.areaData.GetComponent<PolygonCollider2D>();
            if (polygon != null)
            {
                polygon.SetPath(0, points);
            }
        }

        private void ResetGrids()
        {
            int w = 20;
            int h = 20;

            float uw_2 = MapConfig.UnitWidth_2;
            float uh_2 = MapConfig.UnitHeight_2;

            Vector2[] points = new Vector2[4]
            {
                new Vector2(-uw_2 * w , uh_2 * h ),
                new Vector2(uw_2 * w , uh_2 * h ),
                new Vector2(uw_2 * w  ,-uh_2 * h),
                new Vector2(-uw_2 * w , -uh_2 * h),
            };

            var polygon = this.areaData.GetComponent<PolygonCollider2D>();
            if (polygon != null)
            {
                polygon.SetPath(0, points);
            }

            areaData.gridRect = new Int4();
            areaData.delGrids = new Int2[0];
            areaData.addGrids = new Int2[0];
        }

        private void RecalculateGrids()
        {
            var polygon = this.areaData.GetComponent<PolygonCollider2D>();
            if (polygon != null)
            {
                var grid = MapHelper.PositionToGrid(polygon.transform.position);

                var list = new List<Int2>();
                for (int i = -30; i < 30; i++)
                {
                    for (int j = -30; j < 30; j++)
                    {
                        var tmpG = new Int2(grid.x + i, grid.y + j);
                        var tmpP = MapHelper.GridToPosition(tmpG);
                        if (polygon.OverlapPoint(tmpP))
                        {
                            list.Add(tmpG);
                        }
                    }
                }

                var list2 = new List<Int2>();
                for (int i = 0; i < list.Count; i++)
                {
                    var tmpG = list[i];
                    var tmpGs = new Int2[3] { new Int2(tmpG.x - 1, tmpG.y), new Int2(tmpG.x, tmpG.y - 1), new Int2(tmpG.x - 1, tmpG.y - 1) };
                    for (int j = 0; j < tmpGs.Length; j++)
                    {
                        var tmpP = MapHelper.GridToPosition(tmpGs[j]);
                        if (!polygon.OverlapPoint(tmpP))
                        {
                            if (!list2.Contains(tmpGs[j]))
                            {
                                list2.Add(tmpGs[j]);
                            }
                        }
                    }
                }
                list.AddRange(list2);

                //areaData.gridRect = new Int4(0, 0, 0, 0);
                //areaData.delGrids = new Int2[0];
                areaData.addGrids = list.ToArray();
            }
        }

        #endregion

        #region Unity

        private void OnEnable()
        {
        }

        private void OnSceneGUI()
        {
            //var guiS = new GUIStyle(GUI.skin.label);
            //guiS.fontSize = 48;
            //Handles.Label(this.transform.position, this.transform.name, guiS);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();
            this.serializedObject.Update();

            EditorUtils.BeginArea("区域编辑");

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("生成格子"))
            {
                RecalculateGrids();
            }

            if (GUILayout.Button("重置区域"))
            {
                ResetGrids();
            }

            GUILayout.EndHorizontal();
            //EditorUtils.DrawTips();

            EditorUtils.EndArea();

            this.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndChangeCheck();
        }

        #endregion
    }
}
