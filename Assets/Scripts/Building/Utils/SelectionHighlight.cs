// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 2017-11-17
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "SelectionHighlight" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using UnityEngine;

namespace Game.Build
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class SelectionHighlight : MonoBehaviour
    {
        private const int MaxSize = 1;
        private const int MaxSizeSquare = 1;

        #region Static

        public static SelectionHighlight Instance;

        #endregion

        #region Field  

        public Color normalColor;
        public Color clearColor;

        [SerializeField]
        private float lerpColorSpeed = 2f;
        private bool _lerpColorsRoutineDone = true;

        private Vector2[] _clearUVs;
        private Color[] _currentColors;
        private Color[] _targetColors;

        private float _selfHeight = 0.03f;
        private int _lastSizeX = -1;
        private int _lastSizeY = -1;

        private Mesh _mesh;
        private MeshFilter _filter;
        private MeshRenderer _renderer;

        private Vector2[] _uvPattern00 = new Vector2[] {
            new Vector2(.1f, .1f), new Vector2(.1f, .9f), new Vector2(.9f, .9f), new Vector2(.9f, .1f),
            new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(1f, 0f)};
        //private Vector2[] _uvPattern01 = new Vector2[] { new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 0f), new Vector2(0.5f, 0f) };
        //private Vector2[] _uvPattern05 = new Vector2[] { new Vector2(0f, 0.5f), new Vector2(0.5f, 1f), new Vector2(1f, 0.5f), new Vector2(0.5f, 0f) };
        //private Vector2[] _uvPattern13 = new Vector2[] { new Vector2(0f, 0.5f), new Vector2(0f, 1f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 1f) };
        //private Vector2[] _uvPattern14 = new Vector2[] { new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(0f, 1f), new Vector2(0f, 0.5f) };
        //private Vector2[] _uvPattern15 = new Vector2[] { new Vector2(0.5f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0.5f, 1f), new Vector2(0f, 1f) };

        #endregion

        #region Method

        public void HideSelection()
        {
            _renderer.enabled = false;
            if (this._targetColors == null)
                return;
            for (int i = 0; i < this._targetColors.Length; i++)
            {
                this._targetColors[i] = Color.clear;
            }

            if (this._lerpColorsRoutineDone)
            {
                StartCoroutine(this.LerpColors());
            }
        }

        public void ShowSelection(MapObject mapObject)
        {
            var position = mapObject.transform.position;
            position.z = _selfHeight;
            this.transform.position = position;

            int x = mapObject.mapSize.x;
            int y = mapObject.mapSize.y;
            if (_lastSizeX != x || _lastSizeY != y)
            {
                _lastSizeX = x;
                _lastSizeY = y;
                RebuildHighlightMesh(x, y);
            }

            for (int i = 0; i < this._targetColors.Length; i++)
            {
                this._targetColors[i] = normalColor;
            }

            if (this._lerpColorsRoutineDone)
            {
                this.StartCoroutine(this.LerpColors());
            }

            _renderer.enabled = true;
        }

        private IEnumerator LerpColors()
        {
            this._lerpColorsRoutineDone = false;
            yield return null;
            do
            {
                var done = true;
                for (int i = 0; i < this._currentColors.Length; i++)
                {
                    if (this._currentColors[i] != this._targetColors[i])
                    {
                        var a = this._currentColors[i].a;
                        this._currentColors[i] = Color.Lerp(this._currentColors[i], this._targetColors[i], Time.deltaTime * this.lerpColorSpeed);
                        this._currentColors[i].a = Mathf.Lerp(a, this._targetColors[i].a, Time.deltaTime * this.lerpColorSpeed);
                        done = false;
                    }
                }
                _mesh.colors = this._currentColors;

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

        private void RebuildHighlightMesh(int sizeX, int sizeY)
        {
            var vertices = new Vector3[8];
            var triangles = new int[24];
            this._clearUVs = new Vector2[vertices.Length];
            this._currentColors = new Color[vertices.Length];
            this._targetColors = new Color[vertices.Length];

            for (int i = 0; i < this._currentColors.Length; i++)
            {
                this._currentColors[i] = clearColor;
            }

            float uw_2 = MapConfig.UnitWidth_2;
            float uh_2 = MapConfig.UnitHeight_2;

            float wOffset = uw_2 * 0.3f;
            float hOffset = uh_2 * 0.3f;

            for (int j = 0; j < 1; j++)
            {
                vertices[0] = new Vector3(0, 0f);
                vertices[1] = new Vector3(uw_2 * sizeY, uh_2 * sizeY);
                vertices[2] = new Vector3(uw_2 * (sizeX + sizeY), uh_2 * (sizeY - sizeX));
                vertices[3] = new Vector3(uw_2 * sizeX, -uh_2 * sizeX);

                vertices[4] = new Vector3(-wOffset, 0f);
                vertices[5] = new Vector3(uw_2 * sizeY, uh_2 * sizeY + hOffset);
                vertices[6] = new Vector3(uw_2 * (sizeX + sizeY) + wOffset, uh_2 * (sizeY - sizeX));
                vertices[7] = new Vector3(uw_2 * sizeX, -uh_2 * sizeX - hOffset);

                triangles[0] = 0;
                triangles[1] = 5;
                triangles[2] = 1;
                triangles[3] = 0;
                triangles[4] = 3;
                triangles[5] = 7;
                triangles[6] = 0;
                triangles[7] = 4;
                triangles[8] = 5;
                triangles[9] = 0;
                triangles[10] = 7;
                triangles[11] = 4;
                triangles[12] = 2;
                triangles[13] = 1;
                triangles[14] = 5;
                triangles[15] = 2;
                triangles[16] = 7;
                triangles[17] = 3;
                triangles[18] = 2;
                triangles[19] = 5;
                triangles[20] = 6;
                triangles[21] = 2;
                triangles[22] = 6;
                triangles[23] = 7;

                this._uvPattern00.CopyTo(this._clearUVs, 0);
            }

            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _mesh.colors = this._currentColors;
            _mesh.uv = this._clearUVs;
            _mesh.RecalculateBounds();
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
        }

        private void OnDestroy()
        {
            Instance = null;
            Destroy(this._mesh);
        }

        #endregion
    }
}

