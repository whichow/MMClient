using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Game.Build
{
    public class BuildingStateView : MonoBehaviour //: KUIWindow/*Singleton<BuildingStateMgr>*/
    {
        #region Field
        Image image;
        GameObject _timeProgress;
        GameObject _bubble;
        GameObject _parentNode;
        Image _cropbackIcon;

        public BuildingTimeCounDown  buildingTimeCounDown;
        public Image bubbleNew;
        public GameObject rootNode;
        public RectTransform rootNodeRect;
        public Image iconNode;

        BuildingElementPool<GameObject> _bubbleElementPool;
        BuildingElementPool<GameObject> _ProgressElementPool;
        Dictionary<Building, BuildingStateView> _bubbleElemetDict;
        Dictionary<Building, BuildingStateView> _ProgressElemetDict;
        //string uiPath;
        #endregion

        #region Constructor

        public BuildingStateView()
            //: base(UILayer.kNormal, UIMode.kNone)
        {
            //uiPath = "CropUp";


            _bubbleElementPool = new BuildingElementPool<GameObject>();
            _bubbleElementPool.NewElementFunSet(
                () =>
                {
                    GameObject newGm = Object.Instantiate(_bubble);
                    newGm.transform.SetParent(_parentNode.transform, false);
                    newGm.SetActive(true);
                    return newGm;
                }
                );
            _ProgressElementPool = new BuildingElementPool<GameObject>();
            _ProgressElementPool.NewElementFunSet(
                () =>
                {
                GameObject newGm = Object.Instantiate(_timeProgress);
                newGm.transform.SetParent(_parentNode.transform, false);
                newGm.SetActive(true);
                return newGm;

                }

                );

            _bubbleElemetDict =new Dictionary<Building, BuildingStateView>();
            _ProgressElemetDict =new Dictionary<Building, BuildingStateView>();

            //LoadAseet();
        }



        public BuildingStateView BubbleShow(Building building,System.Action onClick =null)
        {
            BuildingStateView buildingStateView;
            if (!_bubbleElemetDict.TryGetValue(building, out buildingStateView))
            {
                buildingStateView = new BuildingStateView();
                GameObject gm  = _bubbleElementPool.GetElementCanUsed();
                buildingStateView.rootNodeRect = gm.GetComponent<RectTransform>();
                buildingStateView.rootNode = gm;
                buildingStateView.bubbleNew = gm.GetComponent<Image>();
                //gm.GetComponent<RectTransform>().anchoredPosition = WorldToUI(building.gameObject.transform.position);
                //Debug.Log(WorldToUI(building.gameObject.transform));
                _bubbleElemetDict.Add(building, buildingStateView);
                Button bt = buildingStateView.rootNode.GetComponent<Button>();
                bt.onClick.RemoveAllListeners();
                bt.onClick.AddListener(() => {if (onClick != null) onClick(); });

            }
            buildingStateView.rootNode.SetActive(true);
            buildingStateView.rootNode.GetComponent<RectTransform>().anchoredPosition = BuildingStateMgr.Instance.WorldToUI(building.gameObject.transform.position);

            return buildingStateView;
        }
        public void BubbleHide(Building building)
        {
            BuildingStateView  buildingStateView;
            if (_bubbleElemetDict.TryGetValue(building, out buildingStateView))
            {
                //gm.transform.SetParent(_parentNode.transform, true);
                buildingStateView.rootNode.SetActive(false);
                _bubbleElementPool.DelElement(buildingStateView.rootNode);
                _bubbleElemetDict.Remove(building) ;
     
            }

        }
        public void IconSet(string iconNmae)
        {
            if (bubbleNew)
            {
                Sprite sprite = KIconManager.Instance.GetBuildingIcon(iconNmae);
                if(sprite)
                    bubbleNew.overrideSprite = sprite;
            }
        }
        public void GoldIconSet(int iconId)
        {
            if (bubbleNew)
            {
                Sprite sprite = KIconManager.Instance.GetItemIcon(iconId);
                if (sprite)
                    bubbleNew.overrideSprite = sprite;
            }
        }
        public void BubblePosSet(Vector3 pos)
        {
            if (rootNodeRect)
            {
                rootNodeRect.anchoredPosition = pos;
            }
        }
        public BuildingStateView ProgressShow(Building building, System.Action onClick=null)
        {

            BuildingStateView  buildingStateView;
            if (!_ProgressElemetDict.TryGetValue(building, out buildingStateView))
            {
                buildingStateView = new BuildingStateView();
                GameObject gm = _ProgressElementPool.GetElementCanUsed();
                buildingStateView.rootNodeRect = gm.GetComponent<RectTransform>();
                buildingStateView.rootNode = gm;
                _ProgressElemetDict.Add(building, buildingStateView);
                buildingStateView.buildingTimeCounDown = gm.GetComponent<BuildingTimeCounDown>();
            }
            buildingStateView.rootNode.SetActive(true);
            buildingStateView.rootNode.GetComponent<RectTransform>().anchoredPosition = BuildingStateMgr.Instance.WorldToUI(building.gameObject.transform.position);
            return buildingStateView;
        }
        public void ProgressHide(Building building)
        {
            BuildingStateView buildingStateView;
            if (_ProgressElemetDict.TryGetValue(building, out buildingStateView))
            {
                buildingStateView.rootNode.SetActive(false);
                _bubbleElementPool.DelElement(buildingStateView.rootNode);   //释放对象池
                _ProgressElemetDict.Remove(building);

            }
        }


        public void posUpdate(Building building,GameObject gm)
        {
            gm.GetComponent<RectTransform>().anchoredPosition = BuildingStateMgr.Instance.WorldToUI(building.gameObject.transform.position);
        }
        private void posUpdateAll()
        {

            if (_bubbleElemetDict.Count > 0)
            {
                foreach (var item in _bubbleElemetDict)
                {
                    posUpdate(item.Key, item.Value.rootNode);
                }
            }

            if (_ProgressElemetDict.Count > 0)
            {
                foreach (var item in _ProgressElemetDict)
                {
                    posUpdate(item.Key, item.Value.rootNode);
                }
            }
        }

        public void LoadAseet()
        {
            //Object node;
           // KAssetManager.Instance.TryGetUIAsset(uiPath, out node);
            //_parentNode = Object.Instantiate(node) as GameObject;
            _parentNode.transform.SetParent(KUIRoot.uiRootCanvas.transform,false);
            _parentNode.SetActive(true);
            _timeProgress = _parentNode.transform.Find("TimeProgress").gameObject;
            _timeProgress.SetActive(false);
            _bubble = _parentNode.transform.Find("CropUp").gameObject;
            _bubble.SetActive(false);

            _cropbackIcon = _parentNode.transform.Find("CropUp/Cropback").GetComponent<Image>();
            buildingTimeCounDown = _timeProgress.GetComponent<BuildingTimeCounDown>();
        }

 

        
        #endregion

        #region Unity 
        public void OnEnable()
        {
            _parentNode = gameObject;
            _timeProgress = this.transform.Find("TimeProgress").gameObject;
            _timeProgress.SetActive(false);
            _bubble = this.transform.Find("Bubble").gameObject;
            _bubble.SetActive(false);

            _cropbackIcon = this.transform.Find("Bubble/Cropback").GetComponent<Image>();
        }
        public void Awake()
        {


        }
        public void Start()
        {
        }
        IEnumerator posUpdata()
        {
            while (true)
            {
                posUpdateAll();
                yield return 0;
            }
        }
        public void Update()
        {
            //posUpdateAll();
        }

        #endregion

    }
}
