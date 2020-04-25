// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "BuildingView" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.UI;
using Spine.Unity;
using UnityEngine;

namespace Game.Build
{
    [ExecuteInEditMode]
    public class BuildingView : MonoBehaviour
    {
        #region Field  

        /// <summary>
        /// 
        /// </summary>
        private SkeletonAnimation _mainAnimation;
        /// <summary>
        /// 实例
        /// </summary>
        private Building _entity;
        /// <summary>
        /// 实例数据
        /// </summary>
        private KItemBuilding _entityData;

        /// <summary>
        /// 
        /// </summary>
        private Transform _tentNode;
        /// <summary>
        /// 模型节点
        /// </summary>
        Transform _model;

        /// <summary>
        /// 静态模型
        /// </summary>
        private Transform _spriteModel;

        private int _rotateValue;
        /// <summary>
        /// 静态模型原始缩放
        /// </summary>
        private Vector3 _spriteModelScale;
        private Transform _rotateNode;
        public Vector2 ModelHeight { get; private set; }
        private bool _isPump = true;

        private TweenColorBuilding _tweenColorBuilding;
        private Renderer _buildingRenderer;
        private MaterialPropertyBlock _materialPropertyBlock;
        private Color _red;
        private Color _white;

        /// <summary>
        /// 旋转模型
        /// </summary>
        private GameObject _rotateModel;
        /// <summary>
        /// 记录当前加载的模型（旋转与非旋转）
        /// </summary>
        private GameObject _currentPrefabModel;

        private GameObject _currentPrefabNode =null;
        /// <summary>
        /// 树木类型
        /// </summary>
        private enum TreeType
        {
            none,
            /// <summary> 小树</summary>
            small = 1203,
            /// <summary> 大树</summary>
            bigTree = 1204,
        }

        #endregion

        #region Property

        /// <summary>
        /// 
        /// </summary>
        public Building entity
        {
            get { return _entity; }
            set
            {
                if (_entity != value)
                {
                    Init(value);
                }
            }
        }
        private int _rotationValue = 0;
        public int rotationValue {
            get
            {
                return _rotationValue;
            }

            set
            {
                _rotationValue = value;
            }
        }
        private TreeType treeType;
        /// <summary>
        /// 气泡顶点
        /// </summary>
        public Transform bubbleNode { get; private set; }

        public Transform progressNode { get; private set; }
        /// <summary>
        /// 中心点
        /// </summary>
        public Transform centerNode { get; private set; }
        /// <summary>
        /// 模型节点
        /// </summary>
        public Transform modelNode { get; private set; }
        /// <summary>
        /// 旋转模型
        /// </summary>
        public Transform rotatemodelNode { get; private set; }
        public Transform gmPoint { get; private set; }
        public Vector2[] polygonColliderVec { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public GameObject modelObj
        {
            get;
            private set;
        }

        #endregion
        public BuildingView()
        {


            //     Color Red;
            //private Color White;
            //materialPropertyBlock.SetColor("_ColorOffSet",Color.red);
        }

        #region Method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Init(Building entity)
        {
            _entity = entity;
            _entityData = entity.entityData;

            var mapSize = _entityData.mapSize;
            bubbleNode = new GameObject("Bubble").transform;
            bubbleNode.SetParent(this.transform);
            bubbleNode.localPosition = MapHelper.GridToPosition(0, mapSize.y);

            progressNode = new GameObject("ProgressNode").transform;
            progressNode.SetParent(this.transform);
            progressNode.localPosition = MapHelper.GridToPosition(0, mapSize.y) + new Vector3(0, 0.5f, 0);

            centerNode = new GameObject("Center").transform;
            centerNode.SetParent(this.transform, false);
            rotationValue =  _entity.RotateDir;
            centerNode.localPosition = MapHelper.GridCenterToPosition(mapSize);
        }

        /// <summary>
        /// 显示城建元素已完成显示帐篷和气球状态
        /// </summary>
        /// <param name="isIdle">true 播放idle动画，false 飞起球动画</param>
        public void ShowTent(bool isIdle)
        {
            if (modelNode)
            {
                modelNode.gameObject.SetActive(false);
            }

            if (_tentNode)
            {
                Destroy(_tentNode.gameObject);
            }

            _tentNode = new GameObject("Tent").transform;

            _tentNode.SetParent(this.transform, false);
            _tentNode.transform.localPosition = Vector3.zero;

            GameObject prefab;
            if (KAssetManager.Instance.TryGetBuildingPrefab("Item/MB01_123", out prefab))
            {
                var tentObj = Instantiate(prefab);
                tentObj.transform.SetParent(_tentNode, false);
                _currentPrefabModel = tentObj;
                tentObj.transform.localPosition = new Vector3(0,0,-0.00001f); //设置层级在建筑物前面
                var skeletonAnim = tentObj.GetComponentInChildren<SkeletonAnimation>();
                if (skeletonAnim)
                {
                    skeletonAnim.AnimationName = null;
                    skeletonAnim.loop = isIdle;
                    skeletonAnim.AnimationName = (isIdle ? "idle" : "complete");  //idle  气球原地飘动动画 ，complete 完成后飞起球动画
                }
            }

            if (!isIdle)
            {
                Destroy(_tentNode.gameObject, 1.5f);
            }

            GetColliderPoints();
        }

        /// <summary>
        ///  显示城建正在建设状态
        /// </summary>
        public void ShowBuild()
        {
            if (modelNode)
            {
                modelNode.gameObject.SetActive(false);
            }

            if (_tentNode)
            {
                Destroy(_tentNode.gameObject);
            }

            _tentNode = new GameObject("Tent").transform;
            _tentNode.SetParent(this.transform, false);
            _tentNode.transform.localPosition = Vector3.zero;

            GameObject prefab;
            if (KAssetManager.Instance.TryGetBuildingPrefab("Item/MB01_121", out prefab))
            {
                var tentObj = Instantiate(prefab);
                _currentPrefabModel = tentObj;
               // _currentPrefabModel = tentObj;
                tentObj.transform.SetParent(_tentNode, false);

                var skeletonAnim = tentObj.GetComponentInChildren<SkeletonAnimation>();
                if (skeletonAnim)
                {
                    skeletonAnim.AnimationName = null;
                    skeletonAnim.loop = true;
                    skeletonAnim.AnimationName = "idle";  //idle
                }
            }
        }

        /// <summary>
        /// 显示城建元素图片
        /// </summary>
        public void ShowModel()
        {
            //Debug.Log("ShowModel");
            if (!modelNode)
            {
                modelNode = new GameObject("Model").transform;
                modelNode.SetParent(this.transform, false);
                var mapSize = _entityData.mapSize;
                Vector2 pos = MapHelper.GridToPosition(mapSize.x, 0);
                modelNode.localPosition = pos;

                GameObject prefab;
                if (KAssetManager.Instance.TryGetBuildingPrefab(_entityData.model, out prefab))
                {
                    modelObj = Instantiate(prefab);
                    _currentPrefabModel = modelObj;
                    modelObj.transform.SetParent(modelNode, false);
                    modelObj.transform.localPosition = -pos;
                    _buildingRenderer = modelObj.GetComponentInChildren<Renderer>();
                    _mainAnimation = modelObj.GetComponentInChildren<SkeletonAnimation>();
                    if (_mainAnimation)
                    {
                        if (_mainAnimation.state != null)
                        {
                            _mainAnimation.state.Complete += OnAnimationComplete;
                        }
                         
                        if (_entityData.cfgId == (int)TreeType.bigTree)
                        {
                            _mainAnimation.skeleton.SetSkin("shuzhuang");
                            treeType = TreeType.bigTree;
                        }
                        if (_entityData.cfgId == (int)TreeType.small)
                        {
                            _mainAnimation.skeleton.SetSkin("shu");
                            treeType = TreeType.small;
                        }
                        //    if (_entityData.model == "MB01_204")
                        //        _mainAnimation.state.sk52216


                        PlayAnimation(_entity.idleAnimation, true);
                    }
                    else
                    {
                        _spriteModel = modelObj.transform.Find("Model");
                        if (_spriteModel)
                            _spriteModelScale = _spriteModel.localScale;
                        else
                            _spriteModelScale = Vector3.one;
                    }
                    getNode();
                    //GetColliderPoints();
                }
            }
            else
            {
                modelNode.gameObject.SetActive(true);
                _currentPrefabModel = modelObj;
            }
            _currentPrefabNode = modelNode.gameObject;
            initShaderColor();
            initBuildingTweenHeight();
            RotateModel(true);

        }

        private void HideModel()
        {
            modelNode.gameObject.SetActive(false);
        }
        private void GetColliderPoints()
        {
            PolygonCollider2D polygonCollider2D = _currentPrefabModel.GetComponentInChildren<PolygonCollider2D>();
            if (polygonCollider2D)
            {
                Vector3 local = polygonCollider2D.transform.localScale;
                if (treeType == TreeType.small)
                {
                    polygonColliderVec = polygonCollider2D.GetPath(0);
                }
                else if(treeType == TreeType.bigTree)
                {
                    polygonColliderVec = polygonCollider2D.GetPath(1);
                }
                else
                    polygonColliderVec = polygonCollider2D.points;
                Vector3 vector3;
                for (int i = 0; i < polygonColliderVec.Length; i++)
                {
                    vector3 = polygonColliderVec[i];
                    vector3.x *= local.x;
                    vector3.y *= local.y;
                    vector3.z *= local.z;
                    vector3 += polygonCollider2D.transform.localPosition;
                    polygonColliderVec[i] = vector3;
                }
                polygonCollider2D.enabled = false;
            }
        }
        /// <summary>
        /// 显示城建元素图片
        /// </summary>
        public void ToggleRotateModel(bool toggleState)
        {
            if (toggleState)
            {

                if (!_rotateModel)
                {
                    rotatemodelNode = new GameObject("RotateModel").transform;
                    rotatemodelNode.SetParent(this.transform, false);
                    GameObject prefab;
                    if (KAssetManager.Instance.TryGetBuildingPrefab("Item/" + _entityData.roModel, out prefab))
                    {
                        _rotateModel = Instantiate(prefab);
                        _currentPrefabModel = _rotateModel;
                        _rotateModel.transform.SetParent(rotatemodelNode, false);
                    }
                }
                else
                {
                    _currentPrefabModel = _rotateModel;
                    rotatemodelNode.gameObject.SetActive(true);
                }
                //GetColliderPoints();
                if (_rotateModel)
                {
                    _currentPrefabNode = rotatemodelNode.gameObject;
                    modelNode.gameObject.SetActive(false);
                }
                else
                {
                    //Vector3 rot = modelNode.transform.localRotation.eulerAngles;
                    //rot.y = 180;
                    //modelNode.transform.localScale
                    //modelNode.transform.localScale.y = 180;

                    modelNode.transform.localScale = new Vector3(-1,1,1);
                }
            }
            else
            {
                if (_rotateModel)
                    rotatemodelNode.gameObject.SetActive(false);
                if (modelNode)
                {

                    modelNode.gameObject.SetActive(true);
                    _currentPrefabModel = modelObj;
                    modelNode.transform.localScale = new Vector3(1, 1, 1);
                }
            }
            //ShowModel();

            //ColorChangeSet();
        }


        /// <summary>
        /// 获取气泡节点位置和模型、大小
        /// </summary>
        private void getNode()
        {
            _model = modelObj.transform.Find(_entityData.model);
            if (!_model)
                _model = modelObj.transform.GetChild(0);
            if (_model)
            {
                PolygonCollider2D polygonCollider2D = _model.GetComponent<PolygonCollider2D>();
                if (polygonCollider2D)
                    ModelHeight = polygonCollider2D.bounds.size;
            }
            gmPoint = modelObj.transform.Find("Point");
            if (!gmPoint)
                gmPoint = centerNode;
            if (!_rotateNode)
            {
                Transform rotateTrans = modelObj.transform.Find("RotateNode");
                if (rotateTrans)
                {
                    _rotateNode = Instantiate(rotateTrans).transform;
                    _rotateNode.SetParent(modelNode, false);
                }
                //if (_rotateValue != 0)
                //    RotateModel(_rotateValue);
            }

        }
        /// <summary>
        /// 点击事件
        /// </summary>
        public void TouchModel(bool touch)
        {
            if (_mainAnimation)
            {
                if (touch)
                {
                    if (_mainAnimation.state.Data.SkeletonData != null && !string.IsNullOrEmpty(_entity.touchAnimation))
                    {
                        var animation = _mainAnimation.state.Data.SkeletonData.FindAnimation(_entity.touchAnimation);
                        if (animation != null)
                        {
                            PlayAnimation(_entity.touchAnimation, false);
                        }
                        else
                        {
                            Pump();
                        }
                    }
                    else
                    {
                        Pump();
                    }
                }
                else
                {
                    PlayAnimation(_entity.idleAnimation, true);
                }
            }
            else
            {
                if (touch)
                {
                    Pump();
                }
                else
                {
                    OnPumpComplete();
                }
            }
        }

        /// <summary>
        /// 获取模型副本
        /// </summary>
        /// <returns></returns>
        public GameObject GetModel()
        {
            if (modelObj)
            {
                return Instantiate(modelObj);
            }
            return null;
        }

        public void ShowByState(Building.State state)
        {
            //switch (state)
            //{
            //    case Building.State.kBuild:
            //        ShowTent(true);
            //        break;
            //    case Building.State.
            //}

        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="loop"></param>
        public void PlayAnimation(string animation, bool loop)
        {
            if (_mainAnimation)
            {
                if (!string.IsNullOrEmpty(animation))
                {
                    _mainAnimation.AnimationName = null;
                    _mainAnimation.loop = loop;
                    _mainAnimation.AnimationName = animation;
                }
            }
        }

        private void OnAnimationComplete(Spine.TrackEntry trackEntry)
        {
            if (!entity.IsToggleIdle)
                return;
            if (trackEntry.Loop)
            {
                return;
            }
            if (entity.AnimationFinish != null)
            {
                entity.AnimationFinish();
                return;
            }

            var animName = trackEntry.Animation.Name;
            if (animName == _entity.touchAnimation)
            {
                PlayAnimation(_entity.idleAnimation, true);
            }

        }
        /// <summary>
        /// 缩放动画
        /// </summary>
        private void Pump()
        {
            StartScale();

            //if (_isPump && _spriteModel)
            //{
            //    _spriteModel.localScale = _spriteModelScale;
            //    Vector3 scale = _spriteModelScale;
            //    scale.x  *= 0.1f;
            //    KTweenUtils.DOPunchScale(_spriteModel, scale, 0.6f, 4, 1, OnPumpComplete);
            //    _isPump = false;


            //}
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnPumpComplete()
        {
            if (_spriteModel)
            {
                _spriteModel.localScale = _spriteModelScale;
                _isPump = true;
            }

        }


        public void RotateModel(bool isInit =false)
        {
            IFunCommon funCommon = entity as IFunCommon;
            MapObject mapObject = _entity.GetComponent<MapObject>();
            if (mapObject != null)
            {
                if (funCommon != null && funCommon.IsRotate)
                {
                    if (!isInit)
                        rotationValue = rotationValue > 0 ? 0 : 1;

                    mapObject = _entity.GetComponent<MapObject>();
                    _entity.RotateDir = rotationValue;
                    if (!isInit)
                        mapObject.RotateMap();
                    ToggleRotateModel(rotationValue > 0);



                    //RotateModel(rotationValue);
                }
                GetColliderPoints();
                buildingTweenHeight.init(_currentPrefabNode);
                mapObject.AddPolygonFromModel();
            }
            else
            {
                //Debug.LogError("建筑创建失败");
            }
        }
        public void RotateModel(int rotateValue)
        {
            //MapObject mapObject = _entity.GetComponent<MapObject>();
            //if (mapObject != null)
            //{

            //    mapObject.RotateMap();
            //    ToggleRotateModel(rotateValue > 0);

            //}
            //GetColliderPoints();
            //mapObject.AddPolygonFromModel();
        }


        ColorChange _colorChange;
        ColorChange _ColorChange
        {
            get
            {

                if (_colorChange == null)
                {
                    if (_model)
                    {
                        Renderer mesh = _model.GetComponent<Renderer>();
                        if (mesh)
                            _colorChange = new ColorChange(ref mesh);
                    }

                }
                return _colorChange;
            }
        }

        #region 修改颜色

        private Color? _color = null;
        private ShaderColor shaderColor;

        private void initShaderColor()
        {
            if (shaderColor == null)
            {
                if (!_buildingRenderer)
                    return;

                shaderColor = new ShaderColor(_buildingRenderer);

                if (_color != null)
                {
                    shaderColor.SetColor((Color)_color);
                }
            }
        }

        //Color color = new Color(0x08,0x00,0x00,0xff);
        public void ColorChangeSet()
        {
            ResetBrightness();
            if (shaderColor != null)
                shaderColor.SetColor(Color.red);
            _color = Color.red;
        }

        public void ColorEditorSet()
        {
            ResetBrightness();
            if (shaderColor != null)
                shaderColor.SetColor(Color.green);
            _color = Color.green;
        }

        public void ColorRecovery()
        {
            SetBrightness();
            if (shaderColor != null)
                shaderColor.SetColor(Color.white);
            _color = Color.white;
        }
        public void SetBrightness(bool isLoop =true)
        {
            if (shaderColor != null)
                shaderColor.SetBrightness( isLoop);
        }
        public void ResetBrightness()
        {
            if (shaderColor != null)
                shaderColor.ResetBrightness();
        }

        //public void SetBrightness()
        //{
        //    if (shaderColor != null)
        //        shaderColor.SetColor(Color.white);

        //}
        #endregion

        #region 缩放
        BuildingTweenHeight buildingTweenHeight;
        void initBuildingTweenHeight()
        {
            buildingTweenHeight = new BuildingTweenHeight();
            buildingTweenHeight.init(_currentPrefabNode);
        }

        public void StartScale()
        {

            if (buildingTweenHeight != null)
            {
                buildingTweenHeight.StartScale();
            }
                
        }
        #endregion
        #endregion

        private void Awake()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();



        }
    }
}
