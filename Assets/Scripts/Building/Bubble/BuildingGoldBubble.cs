using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Game.Build
{
    public class BuildingGoldBubble : Bubble //: KUIWindow/*Singleton<BuildingStateMgr>*/
    {
        #region Field
        Image image;
        GameObject _timeProgress;
        GameObject _bubble;
        GameObject _parentNode;
        Image _bubbleIcon;
        Button _bubbleOnclick;

        RectTransform _bubbleRect;

        private Transform _targetTrans;


        public void refurbish(Building building, System.Action onClick = null)
        {
            _bubbleOnclick.onClick.RemoveAllListeners();
            _bubbleOnclick.onClick.AddListener(() => { if (onClick != null) onClick(); });


        }

        public void IconSet(string iconNmae)
        {
            if (_bubbleIcon)
            {
                Sprite sprite = KIconManager.Instance.GetBuildingIcon(iconNmae);
                if (sprite)
                    _bubbleIcon.overrideSprite = sprite;
            }
        }
        public void GoldIconSet(int iconId)
        {
            if (_bubbleIcon)
            {
                Sprite sprite = KIconManager.Instance.GetItemIcon(iconId);
                if (sprite)
                    _bubbleIcon.overrideSprite = sprite;
            }
        }

        public void BubblePosSet(Vector3 pos)
        {
            //if (rootNodeRect)
            //{
            //    _bubble.anchoredPosition = pos;
            //}
        }
        protected override void Start()
        {
            
        }


 

        
        #endregion

        #region Unity 
        public void OnEnable()
        {
 
        }
        protected override void Awake()
        {
            base.Awake();
            _bubble = this.gameObject.transform.Find("Icon").gameObject ;
            //_bubble.SetActive(false);
            _bubbleOnclick = _bubble.GetComponent<Button>();
            _bubbleIcon = _bubble.GetComponent<Image>();
            _bubbleRect = this.GetComponent<RectTransform>();
        }
        public override void FollowTarget(Transform target)
        {
            _targetTrans = target;
            LateUpdate();
        }

        protected override void LateUpdate()
        {
            if (_targetTrans)
            {
                var anchoredPosition = BubbleManager.Instance.ScreenPointToLocalPointInRectangle(_targetTrans.position);
                _bubbleRect.anchoredPosition = anchoredPosition;
            }
        }


        IEnumerator posUpdata()
        {
            while (true)
            {
                //posUpdateAll();
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
