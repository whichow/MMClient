using Game.Build;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class BuildingStateItem : KUIWindow
    {
        #region Field
        Image image;

        Build.IFunction iFunction;
        Build.Building building;
        #endregion

        #region Constructor

        public BuildingStateItem()
            : base(UILayer.kNormal, UIMode.kNone)
        {
            uiPath = "CropUp";
        }

        #endregion

        #region Unity 

        public override void Awake()
        {
            base.Awake();
            image = Find<Image>("CropUp");



        }
        public override void OnEnable()
        {
            base.OnEnable();
            iFunction = data as Build.IFunction;
            building = data as Build.Building;
            Vector2 vector2 = GameCamera.Instance.GetSceneCoord(building.gameObject);
            Debug.Log("---------------"+ vector2);
            this.gameObject.GetComponent<RectTransform>().anchoredPosition = vector2;// +new Vector2(100,160);
            // this.gameObject.GetComponent<RectTransform>().anchoredPosition = 
            //this.gameObject.GetComponent<RectTransform>().pivot =vector2;
            //this.transform.parent = building.transform;
            //this.transform.localPosition = Vector3.zero;
            //this.transform.localScale = Vector3.one;


        }
        public override void UpdatePerSecond()
        {
            
        }

        #endregion

    }
}
