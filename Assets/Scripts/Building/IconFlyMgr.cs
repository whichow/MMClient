using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Game.UI;

namespace Game.Build
{
    class IconFlyMgr : KUIWindow// : SingletonUnity<IconFlyMgr>
    {
        string _uiPath;
        GameObject _parent;
        GameObject _iconFly;

        Transform _flyParent;
        Sprite _flyIconSpr;
        GameObject _iconPath;

        GameObject _gmTemp;


        public Transform iconPathParent;

        BuildingElementPool<IconFlyItem> _flyPool;
        BuildingElementPool<IconPathItem> _pathPool;

        Vector3[] _targetList;
        int _targetListNum =10;
        float _space = 20;

        MainWindow _mainWindow;
        MainWindow _MainWindow
        {
            get
            {
                if (_mainWindow == null)
                {
                    _mainWindow = KUIWindow.GetWindow<MainWindow>();
                }
                return _mainWindow;
            }
        }
        private static IconFlyMgr _Instance;

        /// <summary>
        /// 
        /// </summary>
        public static IconFlyMgr Instance
        {
            get
            {
                if (_Instance == null)
                {
                    KUIWindow.OpenWindow<IconFlyMgr>();
                    _Instance = KUIWindow.GetWindow<IconFlyMgr>();

                    //new GameObject(typeof(IconFlyMgr).Name, typeof(IconFlyMgr));
                }
                return _Instance;
            }
        }

        public enum GiftType
        {
            KNone = -1,
            KBag,
            KGold,
            KDiamond,
            KFood,
            KCat,
            KBuilding,
        }

        public IconFlyMgr()
          : base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "UIIconFly";
        }
        public void InitView()
        {
            //_uiPath = "UIIconFly";

            _flyPool = new BuildingElementPool<IconFlyItem>();
            _flyPool.NewElementFunSet(initFly);
            _flyPool.onDelElementSet(onDelElementFly);

            _pathPool = new BuildingElementPool<IconPathItem>();
            _pathPool.NewElementFunSet(initPath);
            _pathPool.onDelElementSet(onDelElementPath);

        }
        private void dataInit()
        {
            _targetList = new Vector3[_targetListNum];
            int loopNum = 200;
            int curTargetPointNum = 0;
            int curPointNum = 0;
            Vector3 newPoint =Vector3.zero;
            float range = _space * _targetListNum / 2;
            bool isGetPoint;
            while (true)
            {
                curTargetPointNum++;
                newPoint.x = Random.Range(-range, range);
                newPoint.y = Random.Range(-range, 0)*0.7f;
                isGetPoint = true;
                for (int i = 0; i < curPointNum; i++)
                {
                    if(Vector3.Distance( _targetList[i], newPoint)<_space)
                    {
                        //curIndex++;
          
                        isGetPoint = false;
                    }
                }
                
                if (isGetPoint)
                {
                    _targetList[curPointNum] = newPoint;
                    curPointNum++;
                }
                if (curPointNum >= _targetListNum|| curTargetPointNum>= loopNum)
                {
                    break;
                }

                //if (true)
                //{
                //    curIndex++;
                //}

            }
            
        }

        private IconFlyItem initFly()
        {

            IconFlyItem _flyItem = new IconFlyItem();
            _flyItem.init(_iconFly);
            _flyItem.setParent(_flyParent);

            // _flyItem.SetActive(true);


            return _flyItem;
        }
        private void onDelElementFly(IconFlyItem iconFlyItem)
        {
            iconFlyItem.SetActive(false);
        }
        private IconPathItem initPath()
        {
            IconPathItem pathItem = new IconPathItem();
            //_gmTemp = Instantiate(_fly) as GameObject;
            //_gmTemp.transform.SetParent(_iconPathParent, false);
            pathItem.init(_iconPath);
            pathItem.setParent(iconPathParent);
            pathItem.transform.localPosition = Vector3.zero;
            //pathItem.SetActive(true);
            return pathItem;
        }
        private void onDelElementPath(IconPathItem iconPathItem)
        {

            iconPathItem.SetActive(false);

        }

        public IconFlyItem IconFlyStart(Vector3 pos, Vector3 target, Sprite icon = null,string content = null,bool isUIPos =false)
        {
            IconFlyItem iconFlyItem = _flyPool.GetElementCanUsed();
            iconFlyItem.setPos(pos, isUIPos);
            iconFlyItem.setTargetPos(target, isUIPos);
            iconFlyItem.SetIcon(icon);
            iconFlyItem.SetContent(content);
            iconFlyItem.Play();
            return iconFlyItem;

        }
        public IconFlyItem IconFlyStart(Vector3 pos, Vector3 target, string content = null)
        {
            IconFlyItem iconFlyItem = _flyPool.GetElementCanUsed();
            iconFlyItem.setPos(pos, false);
            iconFlyItem.setTargetPos(target,true);
            iconFlyItem.SetIcon(null);
            iconFlyItem.SetContent(content);
            iconFlyItem.Play();
            return iconFlyItem;

        }
        IEnumerator IconGroupFlyCoroutine(Vector3 pos, Vector3 target, float time, int count, Sprite icon = null)
        {
            int countTemp = 0;
            while (true)
            {
                countTemp++;
                Debug.Log("执行顺序" + countTemp);
                IconFlyStart(pos, target, icon);
                yield return new WaitForSeconds(time);
                if (countTemp >= count)
                    yield break;
            }
        }


        public void IconGroupFlyStart(Vector3 pos, Vector3 target, float time = 0.5f, int count = 1, Sprite icon = null)
        {
            if (gameObject.activeSelf == true)
                StartCoroutine(IconGroupFlyCoroutine(pos, target, time, count, icon));

        }
        /// <summary>
        /// 播放路径动画
        /// </summary>
        /// <param name="pos">动画开始位置</param>
        /// <param name="target">动画目标点</param>
        /// <param name="content"></param>
        /// <param name="scaleAnimType">主界面 动画播放类型</param>
        /// <param name="icon">图标名字</param>
        /// <param name="isPlayPosAnim">是否播放直线动画</param>
        /// <param name="scaleFactor">路径动画x轴缩放系数</param>
        /// <returns></returns>
        public IconPathItem IconPathStart(Vector3 pos, Vector3 target, string content, Vector2 pathTargetPos, MainWindow.ScaleAnimType scaleAnimType, Sprite icon = null, bool isPlayPosAnim = false, bool isUIPos = false)
        {
            IconPathItem iconPathItem = _pathPool.GetElementCanUsed();
            iconPathItem.setPathStartPos(pos, isUIPos);
            iconPathItem.setPathTargetEndPos(pathTargetPos, isUIPos);
            iconPathItem.setFlyPos(target, isUIPos);
            iconPathItem.setScaleAnimType(scaleAnimType);
            iconPathItem.SetIcon(icon);
            iconPathItem.SetContent(content);
            iconPathItem.isPlayPosAnim = isPlayPosAnim;
            iconPathItem.PathPlayTarget();
            return iconPathItem;
        }
        /// <summary>
        /// 播放一组路径动画
        /// </summary>
        /// <param name="pos">起始位置</param>
        /// <param name="time">动画时间间隔</param>
        /// <param name="iconIdLst">要播放的动画数据组，x：iconID，y:数量，z飞向目标的类型 (IconFlyMgr.GiftType)</param>
        /// <returns></returns>
        IEnumerator IconPathGroupCoroutine(Vector3 pos, float time, Int3[] iconIdLst, bool isUIPos = false)
        {

            int count = iconIdLst.Length;
            Vector3 targetPos = Vector3.one;
            Sprite sprite = null;
            GiftType giftType;

            KItem kItem;
            KItem.GiftType kItemGiftType;
            KItem.GiftOtherType giftOtherType;
            MainWindow.ScaleAnimType scaleAnimType = new MainWindow.ScaleAnimType();
            for (int i = 0; i < count; i++)
            {
                //Debug.Log(iconIdLst[i]);
                giftType = (GiftType)iconIdLst[i].z;
                kItem = KItemManager.Instance.GetItem(iconIdLst[i].x);
                sprite = KIconManager.Instance.GetItemIcon(kItem.iconName);
                if (giftType == GiftType.KNone)
                {

                    giftOtherType = (KItem.GiftOtherType)kItem.itemType;

                    if (giftOtherType == KItem.GiftOtherType.kNone)
                    {
                        kItemGiftType = (KItem.GiftType)kItem.itemID;
                        if (kItemGiftType == KItem.GiftType.kGold)
                        {
                            scaleAnimType = MainWindow.ScaleAnimType.kGoldNode;
                            targetPos = _MainWindow.goldNode.position;
                        }
                        else if (kItemGiftType == KItem.GiftType.kDiamont)
                        {
                            scaleAnimType = MainWindow.ScaleAnimType.kDiamontNode;
                            targetPos = _MainWindow.diamontNode.position;
                        }
                        else if (kItemGiftType == KItem.GiftType.kFood)
                        {
                            scaleAnimType = MainWindow.ScaleAnimType.kFoodNode;
                            targetPos = _MainWindow.foodNode.position;
                        }
                        else if (kItemGiftType == KItem.GiftType.kCharmValue)
                        {
                            scaleAnimType = MainWindow.ScaleAnimType.kCharmNode;
                            targetPos = _MainWindow.charmNode.position;
                        }
                        else if (kItemGiftType == KItem.GiftType.kExp)
                        {
                            scaleAnimType = MainWindow.ScaleAnimType.kGradeNode;
                            targetPos = _MainWindow.gradeNode.position;
                        }
                        else
                        {
                            scaleAnimType = MainWindow.ScaleAnimType.kNone;
                            targetPos = Vector3.zero;
                        }
                    }
                    else
                    {
                        scaleAnimType = MainWindow.ScaleAnimType.kShopBagNode;
                        targetPos = _MainWindow.shopBagNode.position;
                    }

                }
                else if (giftType == GiftType.KBuilding)
                {
                    scaleAnimType = MainWindow.ScaleAnimType.kBuildingBagNode;
                    targetPos = _MainWindow.buildingBagNode.position;
                    //KItemManager.Instance.get
                }
                else if (giftType == GiftType.KCat)
                {
                    scaleAnimType = MainWindow.ScaleAnimType.kCatNode;
                    targetPos = _MainWindow.catNode.position;
                }

                IconPathStart(pos, targetPos, iconIdLst[i].y.ToString(),_targetList[ Mathf.Clamp(i,0,_targetListNum)], scaleAnimType, sprite, true, isUIPos);
                yield return new WaitForSeconds(time);
            }
        }
        /// <summary>
        /// 播放一组路径动画
        /// </summary>
        /// <param name="pos">起始位置</param>
        /// <param name="time">动画时间间隔</param>
        /// <param name="iconIdLst">要播放的动画数据组，x：iconID，y:数量，z飞向目标的类型 (IconFlyMgr.GiftType)</param>
        /// <returns></returns>
        public void IconPathGroupStart(Vector3 pos, float time, Int3[] iconIdLst, bool isUIPos = false)
        {
            if (!gameObject)
                return;
            if (gameObject.activeSelf == true)
                StartCoroutine(IconPathGroupCoroutine(pos, time, iconIdLst, isUIPos));
        }
        IEnumerator IconPathGroupCoroutine(Vector3 pos, int iconId, float time, int count, GiftType type, bool isUIPos = false)
        {

 


            Vector3 targetPos = Vector3.one;
            Sprite sprite = null;
            GiftType giftType;

            KItem kItem;
            KItem.GiftType kItemGiftType;
            KItem.GiftOtherType giftOtherType;
            giftType = type;
            kItem = KItemManager.Instance.GetItem(iconId);
            sprite = KIconManager.Instance.GetItemIcon(kItem.iconName);
            MainWindow.ScaleAnimType scaleAnimType = new MainWindow.ScaleAnimType();
            if (giftType == GiftType.KNone)
            {

                giftOtherType = (KItem.GiftOtherType)kItem.itemType;

                if (giftOtherType == KItem.GiftOtherType.kNone)
                {
                    kItemGiftType = (KItem.GiftType)kItem.itemID;
                    if (kItemGiftType == KItem.GiftType.kGold)
                    {
                        scaleAnimType = MainWindow.ScaleAnimType.kGoldNode;
                        targetPos = _MainWindow.goldNode.position;
                    }
                    else if (kItemGiftType == KItem.GiftType.kDiamont)
                    {
                        scaleAnimType = MainWindow.ScaleAnimType.kDiamontNode;
                        targetPos = _MainWindow.diamontNode.position;
                    }
                    else if (kItemGiftType == KItem.GiftType.kFood)
                    {
                        scaleAnimType = MainWindow.ScaleAnimType.kFoodNode;
                        targetPos = _MainWindow.foodNode.position;
                    }
                    else if (kItemGiftType == KItem.GiftType.kCharmValue)
                    {
                        scaleAnimType = MainWindow.ScaleAnimType.kCharmNode;
                        targetPos = _MainWindow.charmNode.position;
                    }
                    else if (kItemGiftType == KItem.GiftType.kExp)
                    {
                        scaleAnimType = MainWindow.ScaleAnimType.kGradeNode;
                        targetPos = _MainWindow.gradeNode.position;
                    }
                    else
                    {
                        scaleAnimType = MainWindow.ScaleAnimType.kNone;
                        targetPos = Vector3.zero;
                    }
                }
                else
                {
                    scaleAnimType = MainWindow.ScaleAnimType.kShopBagNode;
                    targetPos = _MainWindow.shopBagNode.position;
                }

            }
            else if (giftType == GiftType.KBuilding)
            {
                scaleAnimType = MainWindow.ScaleAnimType.kBuildingBagNode;
                targetPos = _MainWindow.buildingBagNode.position;
                //KItemManager.Instance.get
            }
            else if (giftType == GiftType.KCat)
            {
                scaleAnimType = MainWindow.ScaleAnimType.kCatNode;
                targetPos = _MainWindow.catNode.position;
            }
            for (int i = 0; i < count; i++)
            {

                IconPathStart(pos, targetPos, null, _targetList[Mathf.Clamp(i, 0, _targetListNum)], scaleAnimType, sprite, true);
                if(time>0)
                    yield return new WaitForSeconds(time);
            }
        }


        public void IconPathGroupStart(Vector3 pos, int iconId, float time, GiftType type, int count, bool isUIPos = false)
        {

            if (gameObject.activeSelf == true)
                StartCoroutine(IconPathGroupCoroutine(pos, iconId, time, count, type, isUIPos));
        }
        public void flyPoolRecovery(IconFlyItem iconFlyItem)
        {
            _flyPool.DelElement(iconFlyItem);
        }

        public void pathPoolRecovery(IconPathItem iconPathItem)
        {
            _pathPool.DelElement(iconPathItem);
        }

        public void playMainWindowAnim(MainWindow.ScaleAnimType scaleAnimType)
        {
            _MainWindow.playTweenSclAnim(scaleAnimType);
        }





        private void loadAsset()
        {
            _parent = gameObject;
            _parent.transform.SetParent(this.transform, false);

            _flyParent = _parent.transform.Find("IconFly");
            _iconFly = _parent.transform.Find("IconFly/Item").gameObject;
            _iconFly.SetActive(false);

            iconPathParent = _parent.transform.Find("IconPath");
            _iconPath = _parent.transform.Find("IconPath/Item").gameObject;
            _iconPath.SetActive(false);

        }





        public RectTransform canvasTransform
        {
            get;
            private set;
        }

        public override void Awake()
        {
            base.Awake();
            dataInit();
            loadAsset();
            InitView();



        }
        public override void Start()
        {
            base.Start();
            //InitView();
            //canvasTransform = ScreenCoordinateTransform.Instance.uiCanvas //Instance.gameObject.GetComponentInChildren<Canvas>().transform as RectTransform;// ScreenCoordinateTransform.Instance.gameCanvas;
            //canvasTransform = ScreenCoordinateTransform.Instance.uiCamera;
            //canvasTransform = Instance.gameObject.GetComponentInChildren<Canvas>().transform as RectTransform;// ScreenCoordinateTransform.Instance.gameCanvas;
        }
        public class IconFlyItem
        {
            public GameObject gameObject;
            public RectTransform transform;
            private Image _flyIcon;
            private Text _content;
            private Text _iconContent;
            private bool _isSetIconText;
            private Vector3 target;
            TweenPos _tweenPos;

            public void init(GameObject gm)
            {
                this.gameObject = Object.Instantiate(gm);
                gameObject.SetActive(false);
                transform = gameObject.transform as RectTransform;

                _tweenPos = gameObject.GetComponent<TweenPos>();
                if (_tweenPos)
                {
                    _tweenPos.onEndTweenSet(animFinish);
                }
                _content = transform.Find("Content").GetComponent<Text>();
                //_iconContent = transform.Find("Icon/IconContent").GetComponent<Text>();
                _flyIcon = transform.Find("Fly").GetComponent<Image>();
            }

            public void SetIcon(Sprite icon)
            {
                _isSetIconText = icon;
                _flyIcon.gameObject.SetActive(icon);
                if (icon)
                    _flyIcon.overrideSprite = icon;
            }
            public void setParent(Transform parent)
            {
                transform.SetParent(parent);
                transform.localScale = Vector3.one;

            }
            public void SetActive(bool isShow)
            {
                gameObject.SetActive(isShow);
            }

            /// <summary>
            ///设置文本内容
            /// </summary>
            /// <param name="content"></param>
            public void SetContent(string content)
            {
                bool isNullOrEmpty = string.IsNullOrEmpty(content);
                //_iconContent.gameObject.SetActive(isNullOrEmpty);
                _content.gameObject.SetActive(!isNullOrEmpty);
                if (isNullOrEmpty)
                {
                    return;
                }
                    _content.text = content;
            }
            public void setTargetPos(Vector3 target, bool isGamePos = true)
            {
                if (isGamePos)
                {
                    this.target = target;
                }
                else
                {
                    this.target = transform.InverseTransformPoint(target);
                }
                _tweenPos.to = this.target;

            }
            public void setPos(Vector3 pos, bool isGamePos = true)
            {



                if (isGamePos)
                {
                     transform.position = pos;
                   // _tweenPos.to = target;
                    //_tweenPos.to = _tweenPos.transform.InverseTransformPoint(target);
                    //_tweenPos.to = _tweenPos.transform.InverseTransformPoint( target);
                }
                else
                {
                    ScreenCoordinateTransform.Instance.WorldScenePointToUIPiontSet(transform, pos);
                    //new GameObject("dd").transform.position = transform.InverseTransformPoint(target);
                    
                    //_tweenPos.to = Vector3.zero;
                    //transform.InverseTransformPoint(ScreenCoordinateTransform.Instance.WorldScenePointToUIPiontGet(target));// new Vector3(100,100,0); //transform.InverseTransformPoint(target);//ScreenCoordinateTransform.Instance.WorldScenePointToGameUIPointGet(target);
                }
               
                //_tweenPos.to = _tweenPos.transform.InverseTransformPoint(target);
                _tweenPos.RefurbishPos();
            }
            public void Play()
            {
                if (_tweenPos)
                {
                    SetActive(true);
                    _tweenPos.PlayBack();
                }
            }
            private void animFinish()
            {
                Debug.Log("回收" + this.GetType());
                IconFlyMgr.Instance.flyPoolRecovery(this);
            }
        }

        public class IconPathItem
        {
            public GameObject gameObject;
            public RectTransform transform;

            private Image _iconPathImg;
            private Text _content;
            private Text _iconContent;

            TweenPath _tweenPath;

            TweenPathTarget _tweenPathTarget;
            TweenPos _tweenPos;

            private float _moveUnitX;
            private float _moveUnitY;
            private Vector2 _toPos;
            private float _moveUnitHeight;
            private MainWindow.ScaleAnimType scaleAnimType;

            private Vector3 target;
            public bool isPlayPosAnim
            { get; set; }
            private bool _isSetIconText;
            public void init(GameObject gm)
            {
                this.gameObject = Object.Instantiate(gm);
                gameObject.SetActive(false);
                transform = gameObject.transform as RectTransform;
                _tweenPath = gameObject.GetComponent<TweenPath>();
                _tweenPos = gameObject.GetComponent<TweenPos>();
                _tweenPathTarget = gameObject.GetComponent<TweenPathTarget>();
                if (_tweenPath)
                {
                    _tweenPath.onEndTweenSet(pathAnimFinish);
                    _moveUnitX = _tweenPath.MoveUnitX;
                    _moveUnitY = _tweenPath.MoveUnitY;
                }
                if (_tweenPathTarget)
                {
                    _tweenPathTarget.onEndTweenSet(pathAnimFinish);
                    _toPos = _tweenPathTarget.ToPos;
                    _moveUnitHeight = _tweenPathTarget.MoveUnitHeight;
                    //_moveUnitX = _tweenPath.MoveUnitX;
                    //_moveUnitY = _tweenPath.MoveUnitY;
                }
                if (_tweenPos)
                {
                    _tweenPos.onEndTweenSet(posAnimFinish);
                }
                _iconPathImg = transform.Find("Icon").GetComponent<Image>();
                _content = transform.Find("Content").GetComponent<Text>();
                _iconContent = transform.Find("Icon/IconContent").GetComponent<Text>();
            }
            /// <summary>
            ///设置文本内容
            /// </summary>
            /// <param name="content"></param>
            public void SetContent(string content)
            {
                bool isNullOrEmpty = string.IsNullOrEmpty(content);
                _iconContent.gameObject.SetActive(!isNullOrEmpty);
                _content.gameObject.SetActive(!isNullOrEmpty);
                if (isNullOrEmpty)
                {
                    return;
                }

                if ( _isSetIconText)
                {
  
                    _iconContent.text = content;
                }
                else
                {

                    _content.text = content;
                }
            }
            public void setScaleAnimType(MainWindow.ScaleAnimType scaleAnimType)
            {
                this.scaleAnimType = scaleAnimType;
            }
            /// <summary>
            /// 设置图标
            /// </summary>
            /// <param name="sprite"></param>
            public void SetIcon(Sprite sprite)
            {
                if (sprite)
                {
                    _isSetIconText = true;
                    _iconPathImg.gameObject.SetActive(true);
                    _iconPathImg.overrideSprite = sprite;
                }
                else
                {
                    _isSetIconText = false;
                    _iconPathImg.gameObject.SetActive(false);
                }
                _content.gameObject.SetActive(!_isSetIconText);
            }
            ///// <summary>
            ///// 设置位置
            ///// </summary>
            ///// <param name="pos"></param>
            //public void setPathPos(Vector3 pos)
            //{
            //    ScreenCoordinateTransform.Instance.WorldScenePointToGameUIPointSet(transform, pos);
            //    _tweenPath.ResetPos();
            //}
            /// <summary>
            /// 设置曲线路径结束点位置
            /// </summary>
            /// <param name="pos"></param>
            public void setPathStartPos(Vector3 pos, bool isUIPos)
            {
                if (isUIPos)
                {
                    transform.position = pos;
                }
                else
                    ScreenCoordinateTransform.Instance.WorldScenePointToUIPiontSet(transform, pos);
                //Debug.Log("pos:" + pos + "transPos:" + transform.position);
                _tweenPathTarget.ResetPos();
            }
            /// <summary>
            /// 设置路径结束点位置
            /// </summary>
            /// <param name="pos"></param>
            public void setPathTargetEndPos(Vector2 pos, bool isUIPos)
            {


                //if (isUIPos)
                //{
                //    transform.position = pos;
                //}
                //else
                //{
                //    ScreenCoordinateTransform.Instance.WorldScenePointToUIPiontSet(transform, pos);
                //}
                if (pos.x == 0 || pos.y == 0)
                    _tweenPathTarget.ToPos = _toPos;
                _tweenPathTarget.ToPos = pos;

            }
            ///// <summary>
            ///// 设置缩放系数
            ///// </summary>
            ///// <param name="scaleFactor"></param>
            //public void setPosScale(float scaleFactor)
            //{
            //    if (_tweenPath)
            //        _tweenPath.MoveUnitX = _moveUnitX * scaleFactor;
            //}

            /// <summary>
            /// 设置直线动画初始化位置
            /// </summary>
            /// <param name="target"></param>
            public void setFlyPos(Vector3 target, bool isUIPos)
            {

                this.target = target;

            }
            /// <summary>
            /// 设置父节点
            /// </summary>
            /// <param name="parent"></param>

            public void setParent(Transform parent)
            {
                transform.SetParent(parent);
                transform.localScale = Vector3.one;
            }
            /// <summary>
            /// 显示隐藏物体
            /// </summary>
            /// <param name="isShow"></param>
            public void SetActive(bool isShow)
            {
                gameObject.SetActive(isShow);
            }
            /// <summary>
            /// 播放曲线动画
            /// </summary>
            public void PathPlay()
            {
                if (_tweenPath)
                {
                    SetActive(true);
                    _tweenPath.PlayBack();
                }
            }
            /// <summary>
            /// 播放曲线动画
            /// </summary>
            public void PathPlayTarget()
            {
                if (_tweenPathTarget)
                {
                    SetActive(true);
                    _tweenPathTarget.PlayBack();
                }
            }
            /// <summary>
            /// 曲线动画播放完成
            /// </summary>
            private void pathAnimFinish()
            {
                if (isPlayPosAnim)
                {
                    FlyPlay();
                }
                else
                {
                    IconFlyMgr.Instance.pathPoolRecovery(this);
                }
            }
            /// <summary>
            /// 播放直线动画
            /// </summary>
            public void FlyPlay()
            {
                if (_tweenPos)
                {
                    _tweenPos.to = _tweenPos.transform.InverseTransformPoint(target);
                    //Debug.Log("位置" + _tweenPos.transform.position);
                    _tweenPos.SetBeginPos(transform.localPosition);
                    _tweenPos.PlayBack();
                }
            }
            /// <summary>
            /// 直线动画播放完成
            /// </summary>
            private void posAnimFinish()
            {
                //Debug.Log("回收" + this.GetType());
                IconFlyMgr.Instance.playMainWindowAnim(scaleAnimType);
                IconFlyMgr.Instance.pathPoolRecovery(this);
            }

            // flyPool.GetElementCanUsed();
        }
    }
}
