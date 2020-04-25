// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "CollisionHighlight" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class CollisionHighlight : MonoBehaviour
    {
        private const int MaxSize = 10;
        private const int MaxSizeSquare = 100;

        #region Static

        public static CollisionHighlight Instance;

        #endregion

        #region Field

        [SerializeField]
        private Color bonusColor;
        private Color[] _bonusColorPattern;

        private Color clearColor = Color.clear;
        private Color[] _clearColorPattern;

        [SerializeField]
        private Color collisionColor;
        private Color[] _collisionColorPattern;

        [SerializeField]
        public Color normalColor;
        private Color[] _normalColorPattern;

        [SerializeField]
        private float lerpColorSpeed = 2f;
        private bool _lerpColorsRoutineDone = true;

        private Vector2[] _clearUVs;
        private Color[] _currColors;
        private Color[] _targetColors;

        private float _selfHeight = -0.2f;

        private Mesh _mesh;
        private MeshFilter _filter;
        private MeshRenderer _renderer;
        //private Vector3 originalPos;

        private Vector2[] _uvPattern00 = new Vector2[] { new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(1f, 0f) };
        //private Vector2[] _uvPattern01 = new Vector2[] { new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 0f), new Vector2(0.5f, 0f) };
        //private Vector2[] _uvPattern05 = new Vector2[] { new Vector2(0f, 0.5f), new Vector2(0.5f, 1f), new Vector2(1f, 0.5f), new Vector2(0.5f, 0f) };
        //private Vector2[] _uvPattern13 = new Vector2[] { new Vector2(0f, 0.5f), new Vector2(0f, 1f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 1f) };
        //private Vector2[] _uvPattern14 = new Vector2[] { new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(0f, 1f), new Vector2(0f, 0.5f) };
        //private Vector2[] _uvPattern15 = new Vector2[] { new Vector2(0.5f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0.5f, 1f), new Vector2(0f, 1f) };

        #endregion

        #region Method

        public void HideCollisions()
        {
            Debug.Log("HideCollisions`````````````");
            _renderer.enabled = false;

            for (int i = 0; i < this._targetColors.Length; i++)
            {
                this._targetColors[i] = Color.clear;
            }

            if (this._lerpColorsRoutineDone)
            {
                StartCoroutine(this.LerpColors());
            }
        }

        private IEnumerator LerpColors()
        {
            this._lerpColorsRoutineDone = false;
            yield return null;
            do
            {
                var done = true;
                for (int i = 0; i < this._currColors.Length; i++)
                {
                    if (this._currColors[i] != this._targetColors[i])
                    {
                        var a = this._currColors[i].a;
                        this._currColors[i] = Color.Lerp(this._currColors[i], this._targetColors[i], Time.deltaTime * this.lerpColorSpeed);
                        this._currColors[i].a = Mathf.Lerp(a, this._targetColors[i].a, Time.deltaTime * this.lerpColorSpeed);
                        done = false;
                    }
                }
                _mesh.colors = this._currColors;

                if (done)
                {
                    this._lerpColorsRoutineDone = true;
                    break;
                }
                else
                {
                    yield return null;
                }
            } while (true);
        }

        private void RebuildHighlightMesh()
        {
            var vertices = new Vector3[MaxSizeSquare * 4];
            var triangles = new int[MaxSizeSquare * 6];

            this._clearUVs = new Vector2[vertices.Length];
            this._currColors = new Color[vertices.Length];
            this._targetColors = new Color[vertices.Length];

            for (int i = 0; i < this._currColors.Length; i++)
            {
                this._currColors[i] = Color.clear;
            }

            float uw_2 = MapConfig.UnitWidth_2;
            float uh_2 = MapConfig.UnitHeight_2;

            for (int j = 0; j < MaxSizeSquare; j++)
            {
                int vectorI = j * 4;
                int trianglesI = j * 6;

                var vector = MapHelper.GridToPosition(j % MaxSize, j / MaxSize);

                vertices[vectorI] = new Vector3(vector.x, vector.y);
                vertices[vectorI + 1] = new Vector3(vector.x + uw_2, vector.y + uh_2);
                vertices[vectorI + 2] = new Vector3(vector.x + uw_2 + uw_2, vector.y);
                vertices[vectorI + 3] = new Vector3(vector.x + uw_2, vector.y - uh_2);

                triangles[trianglesI] = vectorI;
                triangles[trianglesI + 1] = vectorI + 1;
                triangles[trianglesI + 2] = vectorI + 2;
                triangles[trianglesI + 3] = vectorI;
                triangles[trianglesI + 4] = vectorI + 2;
                triangles[trianglesI + 5] = vectorI + 3;

                _uvPattern00.CopyTo(this._clearUVs, vectorI);
            }

            _mesh.vertices = vertices;
            _mesh.colors = this._currColors;
            _mesh.uv = this._clearUVs;
            _mesh.triangles = triangles;
            //_mesh.SetTriangles(triangles, 0);
            _mesh.RecalculateBounds();
        }

        public void ShowCollisions(MapObject mapObject, List<MapNode> nodes, bool bonusNodes = false)
        {
            //if (mapObject.data.Settings.ObjectType != MapObject.Type.FreeWalker)
            {
                if (mapObject == null)
                {
                    Debug.LogWarning("CollisionHighlight  设置位置点为 Null");
                    return;
                }
                Vector3 position = mapObject.transform.position;
                position.z = position.z + _selfHeight;
                //Vector2[] array = this._clearUVs.Clone() as Vector2[];

                this.transform.position = position;

                int x = mapObject.mapSize.x;
                int y = mapObject.mapSize.y;
                Int2 coordinate = mapObject.mapGrid;

                //if (nodes.Count != (x * y))
                //{
                //    this.HideCollisions();
                //}
                //else
                {
                    for (int i = 0; i < MaxSizeSquare; i++)
                    {
                        int xx = i % MaxSize;
                        int yy = i / MaxSize;

                        int index = i * 4;

                        if (xx < x && yy < y)
                        {
                            if (Map.NodeIsAvailable(mapObject, new Int2(xx + coordinate.x, yy + coordinate.y)))
                            {
                                this._normalColorPattern.CopyTo(this._targetColors, index);
                            }
                            else
                            {
                                this._collisionColorPattern.CopyTo(this._targetColors, index);
                            }
                        }
                        else
                        {
                            this._clearColorPattern.CopyTo(this._targetColors, index);
                        }
                    }

                    if (this._lerpColorsRoutineDone)
                    {
                        this.StartCoroutine(this.LerpColors());
                    }

                    //_mesh.uv = array;
                    _renderer.enabled = true;
                }
            }
        }

        #endregion

        #region Unity

        private void Awake()
        {
            Instance = this;

            _mesh = new Mesh();
            _filter = GetComponent<MeshFilter>();
            _filter.sharedMesh = _mesh;
            _renderer = GetComponent<MeshRenderer>();
            _renderer.enabled = false;

            this.clearColor = this.normalColor;
            this.clearColor.a = 0f;
            this._normalColorPattern = new Color[] { this.normalColor, this.normalColor, this.normalColor, this.normalColor };
            this._collisionColorPattern = new Color[] { this.collisionColor, this.collisionColor, this.collisionColor, this.collisionColor };
            this._clearColorPattern = new Color[] { this.clearColor, this.clearColor, this.clearColor, this.clearColor };
            this._bonusColorPattern = new Color[] { this.bonusColor, this.bonusColor, this.bonusColor, this.bonusColor };
            //originalPos = transform.localPosition;
        }

        private void Start()
        {
            this.RebuildHighlightMesh();
        }

        private void OnDestroy()
        {
            Instance = null;
            Destroy(this._mesh);
        }

        #endregion
    }
}

