using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    class BuildingFxManager :SingletonUnity<BuildingFxManager>
    {
        private GameObject grassGameObject;
        private Transform grassTrans;
        private GameObject waterGameObject;
        private Transform waterTrans;
        private KFx waterFx;
        private KFx grassFx;

        private GameObject _unlockingGameObj;
        private KFx _unlockingKFx;
        private string _grassName = "Fx/Grass";
        private string _waterName = "Fx/Water";
        private string _unlockingName = "Fx/Unlocking";
        public void loadAsset()
        {
            //grassGameObject;
            GameObject unlockingPrefab;
            KAssetManager.Instance.TryGetBuildingPrefab(_unlockingName, out unlockingPrefab);
            if (unlockingPrefab)
            {
               
                _unlockingGameObj = new GameObject("Unlocking",typeof(KFx));
                _unlockingKFx = _unlockingGameObj.GetComponent<KFx>();
                _unlockingKFx.isStartFirst = false;
                //GameObject unlockingPrefabTemp;
                //unlockingPrefabTemp = Instantiate(unlockingPrefab);
                //unlockingPrefabTemp.name = "InitUnlocking";
                //unlockingPrefabTemp.transform.SetParent(_unlockingGameObj.transform,false);
                _unlockingKFx.fxPrefab = unlockingPrefab;
                _unlockingKFx.soundFxClips = new AudioClip[0];
                _unlockingKFx.freeType = KFx.FreeType.Disable;
                _unlockingGameObj.transform.SetParent(this.transform, false);
            }
          



        }
        public void playunlockingFx(Vector3 pos)
        {
            if (_unlockingGameObj)
            {
                _unlockingGameObj.SetActive(true);
                _unlockingGameObj.transform.position = pos;
            }
            ////if (!waterTrans)
            ////    loadAsset();
            ////waterTrans.position = ScreenCoordinateTransform.Instance.sceneCamera.ScreenToWorldPoint(Input.mousePosition);
            //if (!Instance.waterGameObject)
            //    return;
            //Vector3 pos = ScreenCoordinateTransform.Instance.sceneCamera.ScreenToWorldPoint(Input.mousePosition);
            //pos.z = -36;
            //waterTrans.position = pos;
            ////Instance.waterGameObject.SetActive(false);
            ////Instance.waterGameObject.SetActive(true);
            //waterFx.Reset();
        }
        private void Start()
        {
            loadAsset();
        }
    }
}
