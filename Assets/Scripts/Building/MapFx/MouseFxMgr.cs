using System;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Build
{
    class MouseFxMgr : SingletonUnity<MouseFxMgr>
    {
        public GameObject grassGameObject;
        public Transform grassTrans;
        public GameObject waterGameObject;
        public Transform waterTrans;
        public KFx waterFx;
        public KFx grassFx;
        private string _grassName = "Fx/Grass";
        private string _waterName = "Fx/Water";
        public void loadAsset()
        {
            //grassGameObject;
            GameObject grassPrefab;
            KAssetManager.Instance.TryGetBuildingPrefab(_grassName, out grassPrefab);
            if (grassPrefab)
            {
                Instance.grassGameObject = Instantiate(grassPrefab);
                //grassGameObject = Instance.grassGameObject;
                Instance.grassGameObject.SetActive(true);
                Instance.grassTrans = grassGameObject.transform;
                Instance.grassGameObject.transform.SetParent(this.transform,false);
                Instance.grassFx = grassGameObject.GetComponent<KFx>();
            }
            GameObject waterPrefab;
            KAssetManager.Instance.TryGetBuildingPrefab(_waterName, out waterPrefab);
            if (waterPrefab)
            {
                Instance.waterGameObject = Instantiate(waterPrefab);
                Instance.waterGameObject.SetActive(true);
                Instance.waterGameObject.transform.SetParent(this.transform,false);
                Instance.waterTrans = waterGameObject.transform;
                Instance.waterFx = waterGameObject.GetComponent<KFx>();
            }
        }
        public void playWaterFx()
        {
            //if (!waterTrans)
            //    loadAsset();
            //waterTrans.position = ScreenCoordinateTransform.Instance.sceneCamera.ScreenToWorldPoint(Input.mousePosition);
            if (!Instance.waterGameObject)
                return;
            Vector3 pos =ScreenCoordinateTransform.Instance.sceneCamera.ScreenToWorldPoint(Input.mousePosition);
            pos.z = -36;
            waterTrans.position = pos;
            //Instance.waterGameObject.SetActive(false);
            //Instance.waterGameObject.SetActive(true);
            waterFx.Reset();
        }
        public void playGrassFx()
        {
            //if (!grassTrans)
            //    loadAsset();
            // grassTrans.position = ScreenCoordinateTransform.Instance.sceneCamera.ScreenToWorldPoint(Input.mousePosition);
            if (!Instance.grassGameObject)
                return;
            Vector3 pos = ScreenCoordinateTransform.Instance.sceneCamera.ScreenToWorldPoint(Input.mousePosition);
            pos.z = -36;
            Instance.grassTrans.position = pos;
            Instance.grassFx.Reset();
            //Instance.grassGameObject.SetActive(false);
            //Instance.grassGameObject.SetActive(true);
            //grassFx.Reset();
        }
        private void Start()
        {
            loadAsset();
        }
    }
}
