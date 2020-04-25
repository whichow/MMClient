/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.5
 * 
 * Author:          Coamy
 * Created:	        2019/3/15 13:26:00
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    class BuildingSurfaceManager : SingletonUnity<BuildingSurfaceManager>
    {
        #region 字段

        private GameObject m_SurfaceParent;
        private GameObject m_SurfaceOtherParent;

        /// <summary>
        /// 地板实体列表
        /// </summary>
        private Dictionary<Int2, BuildingSurface> m_surfaceDic = new Dictionary<Int2, BuildingSurface>();
        private Dictionary<Int2, BuildingSurface> m_otherPlayerSurfaceDic = new Dictionary<Int2, BuildingSurface>();

        /// <summary>
        /// 当前编辑的地板列表
        /// </summary>
        private Dictionary<Int2, BuildingSurface> m_editSurfaceDic = new Dictionary<Int2, BuildingSurface>();

        /// <summary>
        /// 当前编辑的地板配置表数据
        /// </summary>
        private KItemBuilding m_curSurfaceCO;
        /// <summary>
        /// 是否是清除地板
        /// </summary>
        private bool m_isClearSurface;

        /// <summary>
        /// 地板服务器数据
        /// </summary>
        private IList<BuildingInfo> m_serverSurfaceInfos;
        private IList<BuildingInfo> m_otherSurfaceInfos;
        /// <summary>
        /// 向服务器发送更新的临时数据
        /// </summary>
        private IList<BuildingInfo> m_reqEditlist;

        #endregion

        #region 数据同步

        /// <summary>
        /// 同步服务器地板数据
        /// </summary>
        /// <param name="buildingInfos"></param>
        public void OnSyncSurfaceDataHandler(IList<BuildingInfo> surfaces)
        {
            m_serverSurfaceInfos = surfaces;
        }

        public void OnOtherSuifaceInfos(IList<BuildingInfo> surfaces)
        {
            m_otherSurfaceInfos = surfaces;
        }

        /// <summary>
        /// 应用编辑结果
        /// </summary>
        public void OnSurfaceUpdate()
        {
            SurfaceDataUpdate();
            SurfaceEntityUpdate();
            m_reqEditlist.Clear();
            m_editSurfaceDic.Clear();
            CancelSurfacePrev();
        }

        public void OnShowSurface()
        {
            DestroyOtherEntityAll();
            m_SurfaceParent.gameObject.SetActive(true);
        }

        /// <summary>
        /// 删除自己建筑元素
        /// </summary>
        /// <param name="buildingId"></param>
        public void DestroyOtherEntityAll()
        {

            //foreach (var item in _otherPlayerentities.Values)
            //{

            //}
            Destroy(m_SurfaceOtherParent);
            //buildingOtherParent = null;
            m_SurfaceOtherParent = new GameObject("SurfaceOther");
            if (m_otherPlayerSurfaceDic != null)
                m_otherPlayerSurfaceDic.Clear();
            //buildingInfosServer.Clear();
            if (m_otherSurfaceInfos != null)
                m_otherSurfaceInfos.Clear();

        }

        /// <summary>
        /// 地板数据更新
        /// </summary>
        private void SurfaceDataUpdate()
        {
            foreach (var item in m_reqEditlist)
            {
                int index = GetServerSurfaceInfoIndex(item.X, item.Y);
                if (!m_isClearSurface)
                {
                    if (index > -1)
                    {
                        m_serverSurfaceInfos[index].CfgId = item.CfgId;
                    }
                    else
                    {
                        m_serverSurfaceInfos.Add(item);
                    }
                }
                else
                {
                    m_serverSurfaceInfos.RemoveAt(index);
                }
            }
        }

        /// <summary>
        /// 地板实体更新
        /// </summary>
        private void SurfaceEntityUpdate()
        {
            foreach (var item in m_editSurfaceDic)
            {
                BuildingSurface surface;
                if (m_surfaceDic.TryGetValue(item.Key, out surface))
                {
                    surface.DelBuilding();
                    m_surfaceDic[item.Key] = item.Value;
                }
                else
                {
                    m_surfaceDic.Add(item.Key, item.Value);
                }
                Vector3 pos = item.Value.entityView.transform.position;
                pos.z += 1;
                item.Value.entityView.transform.position = pos;
            }
        }

        private int GetServerSurfaceInfoIndex(int x, int y)
        {
            int index = -1;
            for (int i = 0; i < m_serverSurfaceInfos.Count; i++)
            {
                var sInfo = m_serverSurfaceInfos[i];
                if (sInfo.X == x && sInfo.Y == y)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        #endregion

        /// <summary>
        /// 初始化地板
        /// </summary>
        public void InitSurface()
        {
            StartCoroutine(InitEntityAll());
        }

        IEnumerator InitEntityAll()
        {
            if (m_serverSurfaceInfos == null) m_serverSurfaceInfos = new List<BuildingInfo>();

            Debuger.Log("初始化所有地板:" + m_serverSurfaceInfos.Count);
            float timeCur = Time.realtimeSinceStartup;
            List<BuildingInfo> screenBuilding = new List<BuildingInfo>();
            List<BuildingInfo> notScreenBiulding = new List<BuildingInfo>();
            foreach (var item in m_serverSurfaceInfos)
            {
                //Debug.Log(MapHelper.GridToPosition(item.X, item.Y));
                if (ScreenCoordinateTransform.Instance.IsSceneScreenCoord(MapHelper.GridToPosition(item.X, item.Y)))
                    screenBuilding.Add(item);
                else
                    notScreenBiulding.Add(item);
            }
            for (int i = 0; i < screenBuilding.Count; i++)
            {
                AddBuildingEntity(screenBuilding[i]);
            }
            for (int i = 0; i < notScreenBiulding.Count; i++)
            {
                AddBuildingEntity(notScreenBiulding[i]);
                //yield return new WaitForSeconds(Time.deltaTime / 200);
            }
            yield return new WaitForSeconds(0);
        }

        /// <summary>
        /// 实例化 其它玩家地板
        /// </summary>
        public void InitOtherSurface()
        {
            m_SurfaceParent.gameObject.SetActive(false);
            StartCoroutine(InitOtherSurface(0f));
        }

        IEnumerator InitOtherSurface(float time = 0)
        {
            Debuger.Log("初始化其他玩家地板" + m_otherSurfaceInfos.Count);
            Building building;
            for (int i = 0; i < m_otherSurfaceInfos.Count; i++)
            {
                building = AddBuildingEntity(m_otherSurfaceInfos[i], false);
                //if (!BuildingManager.Instance.IsOneSelf)
                //{
                //    building.viewBuildingInfo = m_otherSurfaceInfos[i];
                //}
                yield return new WaitForSeconds(time);
            }
        }

        /// <summary>
        /// 添加地板实体
        /// </summary>
        /// <param name="buildingInfo"></param>
        /// <param name="isOneSelf"></param>
        /// <returns></returns>
        private Building AddBuildingEntity(BuildingInfo buildingInfo, bool isOneSelf = true)
        {
            KItemBuilding itemBuilding = KItemManager.Instance.GetBuilding(buildingInfo.CfgId);
            Building.Date data = new Building.Date()
            {
                kItemBuilding = itemBuilding,
                dir = buildingInfo.Dir,
                initType = Building.InitType.Server
            };
            return CreateEntity(data, new Int2(buildingInfo.X, buildingInfo.Y), isOneSelf);
        }

        /// <summary>
        /// 进入编辑模式
        /// </summary>
        /// <param name="itemBuilding"></param>
        public void EnterEditorMod(KItemBuilding itemBuilding)
        {
            this.m_curSurfaceCO = itemBuilding;
            this.m_editSurfaceDic.Clear();
            this.m_isClearSurface = string.IsNullOrEmpty(m_curSurfaceCO.model);
            Map.Instance.EditorMode();
            GameCamera.Instance.SurfaceEditor(true);

            BubbleConfirm.Data data = new BubbleConfirm.Data()
            {
                onConfirm = this.EditFinalize,
                onCancel = this.EditCancel,
            };
            BubbleManager.Instance.ShowConfirm(this.transform, data);
            BuildingManager.Instance.ShowMainWindowMenu(false);
        }

        /// <summary>
        /// 完成编辑
        /// </summary>
        private void EditFinalize()
        {
            if (m_reqEditlist == null)
            {
                m_reqEditlist = new List<BuildingInfo>();
            }
            foreach (var item in m_editSurfaceDic)
            {
                item.Value.entityView.ColorRecovery();
                item.Value.entityView.ResetBrightness();
                m_reqEditlist.Add(new BuildingInfo()
                {
                    CfgId = item.Value._buildingData.cfgId,
                    X = item.Key.x,
                    Y = item.Key.y,
                });
            }
            if (m_isClearSurface)
            {
                GameApp.Instance.GameServer.SurfaceUpdateRequest(null, m_reqEditlist);
            }
            else
            {
                GameApp.Instance.GameServer.SurfaceUpdateRequest(m_reqEditlist, null);
            }
        }

        /// <summary>
        /// 取消编辑
        /// </summary>
        private void EditCancel()
        {
            CancelSurfacePrev();
        }

        /// <summary>
        /// 取消地板预览
        /// </summary>
        public void CancelSurfacePrev()
        {
            Map.Instance.NormalMode();
            GameCamera.Instance.SurfaceEditor(false);
            BubbleManager.Instance.HideConfirm();
            BuildingManager.Instance.ShowMainWindowMenu(true);

            foreach (var item in m_editSurfaceDic.Values)
            {
                if (m_isClearSurface)
                {
                    item.entityView.gameObject.SetActive(true);
                }
                else
                {
                    item.DelBuilding();
                }
            }
            m_editSurfaceDic.Clear();
            m_curSurfaceCO = null;
        }

        /// <summary>
        /// 创建地板预览
        /// </summary>
        /// <param name="pos"></param>
        private void CreateSurfacePrev(Int2 pos)
        {
            if (!m_editSurfaceDic.ContainsKey(pos) && Map.NodeIsAvailable(m_curSurfaceCO, pos))
            {
                if (!m_isClearSurface)
                {
                    Building.Date data = new Building.Date()
                    {
                        kItemBuilding = m_curSurfaceCO,
                        dir = 0,
                        initType = Building.InitType.Create
                    };
                    CreateSurfacePrev(data, pos, true);
                }
                else
                {
                    BuildingSurface surface;
                    if (m_surfaceDic.TryGetValue(pos, out surface))
                    {
                        surface.entityView.gameObject.SetActive(false);
                        m_editSurfaceDic.Add(pos, surface);
                    }
                }
            }
        }

        /// <summary>
        /// 创建地板预览
        /// </summary>
        /// <param name="data">城建元素数据</param>
        /// <param name="screenPos">屏幕坐标</param>
        /// <returns></returns>
        private BuildingSurface CreateSurfacePrev(Building.Date data, Int2 pos, bool isUsePos)
        {
            if ((Building.Category)data.kItemBuilding.type != Building.Category.kSurface)
            {
                return null;
            }

            Vector3 touchPoint;
            if (isUsePos)
                touchPoint = MapHelper.GridToPosition(pos);
            else
                touchPoint = MapHelper.AlignToGrid(GameCamera.Instance.ScreenPointToNavCoord(Input.mousePosition));
            touchPoint.z -= 1;

            BuildingSurface entity = Building.CreateEntity<BuildingSurface>(data) as BuildingSurface;
            entity.CurrCategory = (Building.Category)data.kItemBuilding.type;
            entity.transform.localPosition = touchPoint;
            entity.transform.SetParent(m_SurfaceParent.transform, false);

            var mapItem = entity.gameObject.AddComponent<MapObject>();
            mapItem.mapSize = data.kItemBuilding.mapSize;

            entity.entityView.ColorEditorSet();

            m_editSurfaceDic.Add(pos, entity);

            return entity;
        }

        /// <summary>
        /// 创建网络地板元素
        /// </summary>
        /// <param name="entityData"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private BuildingSurface CreateEntity(Building.Date data, Int2 pos, bool isOneSelf = true)
        {
            if ((Building.Category)data.kItemBuilding.type != Building.Category.kSurface)
            {
                return null;
            }

            Vector3 touchPoint = MapHelper.GridToPosition(pos);

            BuildingSurface entity = Building.CreateEntity<BuildingSurface>(data) as BuildingSurface;
            entity.CurrCategory = (Building.Category)data.kItemBuilding.type;
            entity.transform.localPosition = touchPoint;
            if (isOneSelf)
            {
                entity.transform.SetParent(m_SurfaceParent.transform, false);
                if (!m_surfaceDic.ContainsKey(pos))
                {
                    m_surfaceDic.Add(pos, entity);
                }
            }
            else
            {
                entity.transform.SetParent(m_SurfaceOtherParent.transform, false);
                if (!m_otherPlayerSurfaceDic.ContainsKey(pos))
                {
                    m_otherPlayerSurfaceDic.Add(pos, entity);
                }
            }

            var mapItem = entity.gameObject.AddComponent<MapObject>();
            mapItem.mapSize = data.kItemBuilding.mapSize;

            return entity;
        }


        #region Unity

        void Start()
        {
            m_SurfaceParent = new GameObject("SurfaceSelf");
            m_SurfaceOtherParent = new GameObject("SurfaceOther");

            Vector3 pos = Vector3.zero;
            pos.z = 10;
            m_SurfaceParent.transform.position = pos;
            m_SurfaceOtherParent.transform.position = pos;

            GameCamera.OnSurfaceDragProgress = CreateSurfacePrev;
        }

        #endregion

    }
}
