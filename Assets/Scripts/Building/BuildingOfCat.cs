using Game.DataModel;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    class BuildingOfCat
    {
        #region field
        GameObject _model;
        Vector3 _pos;
        Transform _parentNode;

        BuildingCattery building;

        GameObject catNode;

        private Dictionary<KCat, GameObject> _catModels;

        private List<GameObject> _otherCatModels;

        float[] _catPosY;
        #endregion  
        #region Method
        public BuildingOfCat(BuildingCattery building)
        {
            this.building = building;
            _parentNode = building.transform.Find("View");

            _catModels = new Dictionary<KCat, GameObject>();

            getRandom();
            //getCatModels();
        }

        public void setCatModels(int catId)
        {

            //model = KCattery.Instance.get
        }

        private void initCatModel()
        {

        }

        private void getRandom()
        {
            _catPosY = new float[4];
            for (int i = 0; i < _catPosY.Length; i++)
            {

                float posY = -0.1f * UnityEngine.Random.Range(1, 5);
                float posYTemp = System.Array.Find(_catPosY, pos => { return pos == posY; });

                if (posYTemp >= 0)
                {
                    _catPosY[i] = posY;
                }
                else
                {
                    i--;
                }
            }
        }
        public void refurbishCatModel()
        {
            getCatModels();
            if (!catNode)
            {
                catNode = new GameObject("catNode");
                catNode.transform.SetParent(_parentNode);
                catNode.transform.position = building.entityView.centerNode.position;
                catNode.layer = _parentNode.gameObject.layer;
            }
            if (building.isOneSelf)
                initOneSelfCatModel();
            else
                initOtherCatModel();


        }
        private void initOneSelfCatModel()
        {
            int index = 0;
            foreach (var item in _catModels)
            {
                if (System.Array.Find(building.CatLst, itemCat => { return itemCat.catId == item.Key.catId; }) == null)
                {
                    item.Value.SetActive(false);
                }
                else
                {
                    item.Value.SetActive(true);
                    item.Value.transform.SetParent(catNode.transform);
                    item.Value.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                    item.Value.transform.localPosition = new Vector3(index * 0.3f, _catPosY[index], -Mathf.Pow(_catPosY[index] * 0.1f, 2));
                }

                index++;
            }
        }



        public void HideCats()
        {
            foreach (var item in _catModels.Values)
            {
                item.SetActive(false);
            }
        }

        private void getCatModels()
        {


            if (building.isOneSelf)
            {

                oneSelfCatInit();
            }
            else
            {
                otherPlayerCatInit();
            }

        }
        #endregion

        #region 自己猫舍实例化
        private void oneSelfCatInit()
        {
            GameObject gm;
            for (int i = 0; i < building.CatLst.Length; i++)
            {
                if (building.CatLst[i] != null)
                {
                    if (!_catModels.TryGetValue(building.CatLst[i], out gm))
                        _catModels.Add(building.CatLst[i], CatUtils.GetModel(building.CatLst[i].shopId));
                }
                else
                {
                    //throw new System.NullReferenceException("猫 id不存在猫列表里。服务器数据同步存在问题");
                    Debug.LogError("猫 id不存在猫列表里。服务器数据同步存在问题");
                }
            }
        }
        #endregion
        #region 其他玩家 猫舍实例化

        private void initOtherCatModel()
        {
            for (int i = 0; i < _otherCatModels.Count; i++)
            {
                GameObject gm = _otherCatModels[i];
                gm.SetActive(true);
                gm.transform.SetParent(catNode.transform);
                gm.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                gm.transform.localPosition = new Vector3(i * 0.3f, _catPosY[i], -Mathf.Pow(_catPosY[i], 2) - 1);
            }

        }
        private void otherPlayerCatInit()
        {
            GameObject gm;
            if (building.viewBuildingInfo.CatHouseData == null)
                return;
            IList<int> catList = building.viewBuildingInfo.CatHouseData.CatIds;
            _otherCatModels = new List<GameObject>();
            for (int i = 0; i < catList.Count; i++)
            {

                _otherCatModels.Add(this.GetModel(catList[i]));
                //}
                //else
                //{
                //    //throw new System.NullReferenceException("猫 id不存在猫列表里。服务器数据同步存在问题");
                //    Debug.LogError("猫 id不存在猫列表里。服务器数据同步存在问题");
                //}
            }
        }
        private string getCatName(int catCfg)
        { 
            var _catItem = XTable.CatXTable.GetByID(catCfg);
            if (_catItem != null)
            {
                //rarity = _catItem.rarity;
                //mainColor = _catItem.mainColor;
                //title = _catItem.itemName;
                return _catItem.Model;
            }
            return "";

        }
        public GameObject GetModel(int catCfg)
        {
            var modelName = getCatName(catCfg);
            return CatUtils.GetModel(catCfg);
        }

        #endregion
        #region unity
        private void Awake()
        {

        }
        #endregion
    }
}
