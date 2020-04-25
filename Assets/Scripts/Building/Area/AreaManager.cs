// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "AreaManager" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    /// <summary>
    /// 地图区域管理器
    /// </summary>
    public class AreaManager : MonoBehaviour
    {
        #region Static

        /// <summary>
        /// 
        /// </summary>
        public static AreaManager Instance;

        #endregion

        #region Field

        private Area _selectedArea;
        private List<Area> _allAreas = new List<Area>();

        #endregion

        #region Property

        public List<Area> allAreas
        {
            get { return _allAreas; }
        }

        //public AreaData this[int _index]
        //{
        //    get
        //    {
        //        if (this._allAreas.IsInRange(_index))
        //        {
        //            return this._allAreas[_index];
        //        }
        //        return null;
        //    }
        //}

        #endregion

        #region Method

        public Area GetArea(int areaId)
        {
            return _allAreas.Find(a => a.areaId == areaId);
        }
        public GameObject GetAreaObject(int areaId)
        {
            Area area = _allAreas.Find(a => a.areaId == areaId);
            if (area)
                return area.gameObject;
            else
                return null;
        }
        public void Activate(int areaIndex)
        {
            if (_selectedArea == _allAreas[areaIndex])
            {
                //if (this.onActivate != null)
                //{
                //    this.onActivate();
                //}
            }
            else
            {
                this.Select(areaIndex);
            }
        }
        Area focusArea;
        /// <summary>
        /// 聚焦区域
        /// </summary>
        /// <param name="areaId"></param>
        public void FocusArea(int areaId,System.Action zoomFocus = null)
        {
            focusArea = GetArea(areaId);
            if (focusArea)
            {
                GameCamera.Instance.Show(focusArea.transform.position);

                GameCamera.Instance.zoomFocus(()=> {
                    GameCamera.Disallow(focusArea.gameObject);
                    GameCamera.Unblock(focusArea.gameObject);
                    if (zoomFocus !=null)
                        zoomFocus();
                } );

                GameCamera.Block(focusArea.gameObject, GameCamera.Restrictions.All);
            }
        }
        public void ApplyAreasToMap(bool refresh = true)
        {
            for (int i = 0; i < _allAreas.Count; i++)
            {
                _allAreas[i].ToggleArea(true, false);
                //_allAreas[i].Data = _allAreas[i];
            }

            //this.areasUnlocked.Clear();
            //for (int j = 0; j < _allAreas.Count; j++)
            //{
            //    unlockedAreas[j] |= _allAreas[j].unlockedByDefault;
            //    _allAreas[j].area.Set(j, unlockedAreas[j]);

            //    if (unlockedAreas[j])
            //    {
            //        this.areasUnlocked.Add(_allAreas[j]);
            //    }
            //}
            if (refresh)
            {
                this.RefreshColliders();
            }
        }

        private void Deselect()
        {
            if (_selectedArea != null)
            {
                _selectedArea.ToggleHighlight(false);
                _selectedArea = null;
            }
            //GameCamera.Instance.preClick -= this.OnClick;
        }

        public static Area FindArea(Vector3 position)
        {
            foreach (var area in Instance.allAreas)
            {
                if (area.IsInside(position))
                {
                    return area;
                }
            }
            return null;
        }

        private void OnClick()
        {
            //GameObject screenPointObject = GameCamera.Instance.GetScreenPointObject();
            //if ((_selectedArea != null) && ((screenPointObject == null) || (screenPointObject.transform.parent.gameObject != _selectedArea.gameObject)))
            //{
            //    this.Deselect();
            //}
        }

        public void RefreshColliders()
        {
        }

        private void Select(int areaIndex)
        {
            if (_selectedArea != null)
            {
                this.Deselect();
            }
            _selectedArea = _allAreas[areaIndex];
            _selectedArea.ToggleHighlight(true);

            //GameCamera.Instance.preClick -= this.OnClick;
            //GameCamera.Instance.preClick += this.OnClick;
        }

        private void LoadArea()
        {
            foreach (var area in _allAreas)
            {
                area.LoadData();
                area.unlocked = false;
            }

            AreaDataUpdate();
        }

        private void Load(Hashtable table)
        {
            var areaList = table.GetArrayList("Area");
            if (areaList != null)
            {
                KJson.Resolve(areaList, (t) =>
                 {
                     var tmpArea = new GameObject().AddComponent<Area>();
                     tmpArea.transform.SetParent(transform, false);
                     tmpArea.LoadTable(t);
                     _allAreas.Add(tmpArea);
                 });
            }
        }

        public void AreaDataOtherUpdate(List<int> areaInfo)
        {
            foreach (var area in _allAreas)
            {
                area.unlocked = areaInfo.Contains(area.areaId);
            }
            _allAreas[0].unlocked = true;//强制解锁
            foreach (var area in _allAreas)
            {
                area.ShowLockModel();
            }

            foreach (var area in _allAreas)
            {
                area.setAreaTag();
            }
        }

        public void AreaDataUpdate()
        {
            var areaTable = KDatabase.Database.GetTable(KDatabase.AREA_TABLE_NAME);
            foreach (var area in _allAreas)
            {
                var value = areaTable.GetValue(area.areaId, 0);
                area.unlocked = (value != null);
            }
            _allAreas[0].unlocked = true;//强制解锁
            foreach (var area in _allAreas)
            {
                area.ShowLockModel();
            }

            foreach (var area in _allAreas)
            {
                area.setAreaTag();
            }
        }
        
        #endregion

        #region Unity

        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            TextAsset textAsset;
            KAssetManager.Instance.TryGetExcelAsset("Area", out textAsset);
            if (textAsset)
            {
                var table = textAsset.bytes.ToJsonTable();
                if (table != null)
                {
                    Load(table);
                }
            }
            LoadArea();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDestroy()
        {
            Instance = null;
        }

        private void Update()
        {
            if (Time.frameCount % 120 == 0)
            {
                var index = Random.Range(0, _allAreas.Count);
                Select(index);
            }
        }

        #endregion
    }
}
