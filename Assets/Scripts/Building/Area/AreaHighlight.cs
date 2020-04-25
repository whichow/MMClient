// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "AreaHighlight" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class AreaHighlight : MonoBehaviour
    {
        #region Static

        public static AreaHighlight Instance;

        #endregion

        private GameObject _gameObject;

        //private FloatLinearTweener _alphaTweener;

        private Vector2[] _clearUVs;

        private float nodeHalfSize;
        private float nodeSize;
        private readonly Dictionary<Vector3, int> nodeToGeometry = new Dictionary<Vector3, int>();

        private Vector2[] uvPattern00 = new Vector2[] { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero };
        private Vector2[] uvPattern01 = new Vector2[] { new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 0f), new Vector2(0.5f, 0f) };
        private Vector2[] uvPattern02 = new Vector2[] { new Vector2(0f, 0.5f), new Vector2(0f, 0f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0f) };
        private Vector2[] uvPattern03 = new Vector2[] { new Vector2(0.5f, 0f), new Vector2(0f, 0f), new Vector2(0.5f, 0.5f), new Vector2(0f, 0.5f) };
        private Vector2[] uvPattern04 = new Vector2[] { new Vector2(0f, 0f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0f), new Vector2(0f, 0.5f) };
        private Vector2[] uvPattern05 = new Vector2[] { new Vector2(0.5f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f), new Vector2(1f, 0.5f) };
        private Vector2[] uvPattern06 = new Vector2[] { new Vector2(1f, 1f), new Vector2(1f, 0.5f), new Vector2(0.5f, 1f), new Vector2(1f, 1f) };
        private Vector2[] uvPattern07 = new Vector2[] { new Vector2(1f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(1f, 1f), new Vector2(0.5f, 1f) };
        private Vector2[] uvPattern08 = new Vector2[] { new Vector2(1f, 1f), new Vector2(0.5f, 1f), new Vector2(1f, 0.5f), new Vector2(0.5f, 0.5f) };
        private Vector2[] uvPattern09 = new Vector2[] { new Vector2(0.5f, 0.5f), new Vector2(1f, 0.5f), new Vector2(0.5f, 0f), new Vector2(1f, 0f) };
        private Vector2[] uvPattern10 = new Vector2[] { new Vector2(0.5f, 0f), new Vector2(1f, 0f), new Vector2(0.5f, 0.5f), new Vector2(1f, 0.5f) };
        private Vector2[] uvPattern11 = new Vector2[] { new Vector2(1f, 0f), new Vector2(0.5f, 0f), new Vector2(1f, 0.5f), new Vector2(0.5f, 0.5f) };
        private Vector2[] uvPattern12 = new Vector2[] { new Vector2(1f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(1f, 0f), new Vector2(0.5f, 0f) };
        private Vector2[] uvPatternClear = new Vector2[] { Vector2.one, Vector2.one, Vector2.one, Vector2.one };
        private List<Vector2> uvs = new List<Vector2>();

        #region Field

        private Mesh _mesh;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        #endregion

        #region Property

        #endregion

        #region Method

        public void HideHighlight()
        {
            //_alphaTweener.PlayReverse();
        }

        private void RebuildHighlightGridMesh(Vector3 center, int width, int height)
        {
            float unitW = MapConfig.UnitWidth;
            float unitH = MapConfig.UnitHeight;
            float unitW_2 = MapConfig.UnitWidth_2;
            float unitH_2 = MapConfig.UnitHeight_2;

            var nodesInArea = new Vector3[width * height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    nodesInArea[i * height + j] = new Vector3((i + j) * unitW_2, (j - i) * unitH_2);
                }
            }

            var vertices = new Vector3[(width * height) * 4];
            var triangles = new int[(width * height) * 6];


            _clearUVs = new Vector2[vertices.Length];

            this.nodeToGeometry.Clear();

            for (int i = 0; i < nodesInArea.Length; i++)
            {
                //Vector3 point = nodesInArea[i].position;
                //this.nodeToGeometry.Add(nodesInArea[i].position, i);
                Vector3 point = nodesInArea[i];

                int vIndex = i * 4;
                int tIndex = i * 6;

                vertices[vIndex + 0] = new Vector3(point.x - unitW_2, point.y, 0f);
                vertices[vIndex + 1] = new Vector3(point.x + unitW_2, point.y, 0f);
                vertices[vIndex + 2] = new Vector3(point.x, point.y + unitH_2, 0f);
                vertices[vIndex + 3] = new Vector3(point.x, point.y - unitH_2, 0f);

                triangles[tIndex + 0] = vIndex + 0;
                triangles[tIndex + 1] = vIndex + 2;
                triangles[tIndex + 2] = vIndex + 1;

                triangles[tIndex + 3] = vIndex + 0;
                triangles[tIndex + 4] = vIndex + 1;
                triangles[tIndex + 5] = vIndex + 3;

                this.uvPattern01.CopyTo(_clearUVs, vIndex);
            }

            _mesh.vertices = vertices;
            _mesh.uv = _clearUVs;
            _mesh.SetTriangles(triangles, 0);
            _mesh.RecalculateBounds();

            this.uvs.Clear();
        }

        private void SetToFade()
        {
            //var tw = DG.Tweening.DOTween.ToAlpha(() => _meshRenderer.material.color, (c) => _meshRenderer.material.color = c, 0.1f, 2);

            //FloatLinearTweener.EventHandler onApplyValue = (value) =>
            //{
            //    var color = _meshRenderer.material.color;
            //    color.a = value;
            //    _meshRenderer.material.color = color;
            //};

            //Tweener.EventHandler onFinished = (direction) =>
            //{
            //    if (!direction)
            //    {
            //        _meshRenderer.enabled = false;
            //        _mesh.uv = this._clearUVs;
            //    }
            //    SetToFade();
            //};

            //_alphaTweener = _gameObject.AddComponent<FloatLinearTweener>();
            //_alphaTweener.Set(0f, 0f, 1f, 2f, 2f, onApplyValue, onFinished);
            //_alphaTweener.ResetToBeginning();
        }

        public void ShowHighlight(Area area, List<MapNode> nodes)
        {
            if (nodes != null)
            {
                this.transform.position = area.transform.position;
                Debug.Log("ShowHighlight" + nodes.Count);
                Vector2[] array = _clearUVs.Clone() as Vector2[];
                for (int i = 0; i < nodes.Count; i++)
                {
                    var p = nodes[i].mapPoint;
                    Vector3 r, l, t, b, tl, tr, bl, br;
                    r = l = t = b = tl = tr = bl = br = nodes[i].position;
                    r.x += 1000;
                    l.x -= 1000;
                    b.z -= 1000;
                    t.z += 1000;
                    bl.x -= 1000;
                    bl.z -= 1000;
                    br.x += 1000;
                    br.z -= 1000;
                    tl.x -= 1000;
                    tl.z += 1000;
                    tr.x += 1000;
                    tr.z += 1000;

                    if (this.nodeToGeometry.ContainsKey(nodes[i].position))
                    {
                        if (nodes.Find((n) => n.position == t) == null)
                        {
                            if (nodes.Find((n) => n.position == l) == null)
                            {
                                this.uvPattern06.CopyTo(array, this.nodeToGeometry[nodes[i].position] * 4);
                            }
                            else if (nodes.Find((n) => n.position == r) == null)
                            {
                                this.uvPattern07.CopyTo(array, this.nodeToGeometry[nodes[i].position] * 4);
                            }
                            else
                            {
                                this.uvPattern03.CopyTo(array, this.nodeToGeometry[nodes[i].position] * 4);
                            }
                        }
                        else if (nodes.Find((n) => n.position == b) == null)
                        {
                            if (nodes.Find((n) => n.position == l) == null)
                            {
                                this.uvPattern05.CopyTo(array, this.nodeToGeometry[nodes[i].position] * 4);
                            }
                            else if (nodes.Find((n) => n.position == r) == null)
                            {
                                this.uvPattern08.CopyTo(array, this.nodeToGeometry[nodes[i].position] * 4);
                            }
                            else
                            {
                                this.uvPattern01.CopyTo(array, this.nodeToGeometry[nodes[i].position] * 4);
                            }
                        }
                        else if (nodes.Find((n) => n.position == l) == null)
                        {
                            this.uvPattern02.CopyTo(array, this.nodeToGeometry[nodes[i].position] * 4);
                        }
                        else if (nodes.Find((n) => n.position == r) == null)
                        {
                            this.uvPattern04.CopyTo(array, this.nodeToGeometry[nodes[i].position] * 4);
                        }
                        else if (nodes.Find((n) => n.position == tr) == null)
                        {
                            this.uvPattern11.CopyTo(array, this.nodeToGeometry[nodes[i].position] * 4);
                        }
                        else if (nodes.Find((n) => n.position == tl) == null)
                        {
                            this.uvPattern10.CopyTo(array, this.nodeToGeometry[nodes[i].position] * 4);
                        }
                        else if (nodes.Find((n) => n.position == br) == null)
                        {
                            this.uvPattern12.CopyTo(array, this.nodeToGeometry[nodes[i].position] * 4);
                        }
                        else if (nodes.Find((n) => n.position == bl) == null)
                        {
                            this.uvPattern09.CopyTo(array, this.nodeToGeometry[nodes[i].position] * 4);
                        }
                        else
                        {
                            this.uvPattern00.CopyTo(array, this.nodeToGeometry[nodes[i].position] * 4);
                        }
                    }
                }

                _mesh.uv = array;
                _meshRenderer.enabled = true;
                //this._alphaTweener.PlayForward();
            }
        }

        #endregion

        #region Unity

        private void Awake()
        {
            Instance = this;

            //Map.onGridUpdate += this.RebuildHighlightGridMesh;

            _gameObject = this.gameObject;
            _mesh = new Mesh();
            _meshFilter = this.GetComponent<MeshFilter>();
            _meshFilter.sharedMesh = _mesh;
            _meshRenderer = this.GetComponent<MeshRenderer>();
            //_meshRenderer.enabled = false;

            this.SetToFade();
        }

        private void Start()
        {
            RebuildHighlightGridMesh(Vector3.zero, 10, 10);
            SetToFade();
        }

        private void OnDestroy()
        {
            Instance = null;
            //Map.onGridUpdate -= this.RebuildHighlightGridMesh;
            Destroy(_mesh);
        }

        private void OnDrawGizmos()
        {

        }

        #endregion


    }
}
