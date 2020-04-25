using Game.Build;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class FunctionWindow : KUIWindow
    {
        #region Field
        Button _speedUp;
        Button _gradeUp;
        Button _collect;
        Button _infomation;

        /// <summary>
        /// 猫舍钻石数量
        /// </summary>
        Text _catcoin;

        // Text _catcoin;
        Text _makeCost;

        /// <summary>
        /// 加速图标
        /// </summary>
        Transform _iconUpSleep;

        public static GameObject[] FunctionBtLst;

        public class Data
        {
            public object[] dataList;
            public List<KUIItem> itemList;
            public GameObject gm;
            public object type;
            public System.Func<Data, int, GameObject, KUIItem> instance;
            public System.Action<int> fun;
        }

        List<KUIItem> CatPos1 = new List<KUIItem>();
        List<KUIItem> UnusedCatPos1 = new List<KUIItem>();

        //List<GameObject> CatPos = new List<GameObject>();
        //List<GameObject> UnusedCatPos = new List<GameObject>();

        List<KUIItem> MakeLst1 = new List<KUIItem>();
        List<KUIItem> MakeingLst1 = new List<KUIItem>();
        List<KUIItem> MakeReadyLst1 = new List<KUIItem>();

        private int _moneyCost;
        //List<GameObject> MakeLst = new List<GameObject>();
        //List<GameObject> MakeingLst = new List<GameObject>();
        //List<GameObject> MakeReadyLst = new List<GameObject>();
        //List<GameObject> MakeFinishLst = new List<GameObject>();

        public static GameObject[] CurrFunctionBtLst
        {
            get
            {
                if (_function)
                {
                    GameObject[] FunBtLst = new GameObject[_function.childCount];
                    for (int i = 0; i < _function.childCount; i++)
                    {
                        FunBtLst[i] = _function.GetChild(i).gameObject;
                    }
                    return FunBtLst;
                }
                return new GameObject[0];

            }
        }

        public enum CatFunEnum
        {
            CatUnusedPos = 0, 
            CatInfo = 1,
            Collect = 2,
            UpGrade = 4,
            Start = 13,
            Stop = 14,

        }
        public enum ManualWorkShopFunEnum
        {
            None,
            UpSleep = 3,
            MakeFinish = 6,
            ReadyMake = 7,
            Make = 8,
            Add = 9,
            Cancel = 10,
            Formula = 11,
        }
        public enum OtherFunEnum
        {
            IntoBtnFun = 5,
            UpSleep = 3,
            BuildingInfo = 12,
        }
        Text _speedUpMoneyCost;

        Text _titleText;
        static Transform _function;

        #endregion


        #region Method

        /// <summary>
        /// 通过数据列表数据，实例按钮。 猫舍，手工作坊按钮初始化。将对象添加的缓存中，避免反复实例化
        /// </summary>
        /// <param name="data">数据</param>
        private void buttonInit(Data data)
        {
            if (data == null)
                return;
            KUIItem item;
            int maxNum = Mathf.Max(data.dataList.Length, data.itemList.Count);
            for (int i = 0; i < maxNum; i++)
            {
                if (i >= data.itemList.Count)           //如果缓存池没有可用对象，创建对象，否则直接提取缓存池对象
                {
                    item = data.instance(data, i, null);
                    //item.SetData(data.dataList[i]);
                    data.itemList.Add(item);
                    item.gameObject.SetActive(true);

                }
                else
                {
                    item = data.itemList[i];
                    if (i >= data.dataList.Length)
                    {
                        item.gameObject.SetActive(false);
                    }
                    else
                    {
                        item.gameObject.SetActive(true);
                        data.instance(data, i, item.gameObject);
                    }
                }

            }
        }



        #region 猫舍功能按钮
        /// <summary>
        /// 猫舍金币刷新
        /// </summary>
        /// <returns></returns>
        IEnumerator CoinRefurbishFun()
        {
            while (true)
            {
                if (_icat.Coin >= _icat.CoinMax)
                {
                    _catcoin.text = _icat.CoinMax.ToString();
                    //yield break;
                }
                else
                    _catcoin.text = _icat.Coin.ToString();
                yield return new WaitForSeconds(1);
            }
        }

        /// <summary>
        /// 实例化猫舍按钮
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="index">数据索引</param>
        /// <param name="initGm">要初始化的物体节点，null--创建物体</param>
        /// <returns></returns>
        private KUIItem instanceCatItem(Data data, int index, GameObject initGm)
        {
            GameObject gmTemp;
            CatItem item;
            if (!initGm)
            {
                gmTemp = Object.Instantiate(data.gm);
                item = gmTemp.AddComponent<CatItem>();
                gmTemp.transform.parent = _function;
                gmTemp.transform.localScale = Vector3.one;
                gmTemp.transform.SetAsLastSibling();
                gmTemp.SetActive(true);
            }
            else
            {
                gmTemp = initGm;
                item = initGm.GetComponent<CatItem>();

            }
            item.CatEvent = (indx) => data.fun(indx);
            item.setCatType(data.type);
            item.SetData(data.dataList[index]);
            //item.setCatType(data);
            gmTemp.transform.SetAsLastSibling();
            //gmTemp.GetComponent<Button>().onClick.RemoveAllListeners();
            return item;
        }
        /// <summary>
        /// 猫舍功能按钮初始化
        /// </summary>
        private void catFunSet()
        {
            _icat = data as ICatFunction;
            if (_icat == null)
                return;
            _catcoin.text = _icat.Coin.ToString();
            int index = 0;
            if (_icat != null)
            {
                //如果不在加速状态  ，显示猫槽  ，收集功能
                if (_iFunction.supportGradeUp || _icat.IsGradeMax)
                {
                    //Debug.Log("猫槽总数" + _icat.CatStorage + "/---/" + _icat.CatLst.Length);
                    catLstInit(_icat.CatLst);

                    //添加正在使用的槽
                    Data usedCatData = new Data
                    {
                        dataList = _usedCat,
                        itemList = CatPos1,
                        type = CatFunEnum.CatInfo,
                        gm = FunctionBtLst[(int)CatFunEnum.CatInfo],
                        instance = instanceCatItem,
                        fun = this.CatInfoShow,
                    };
                    buttonInit(usedCatData);

                    //未使用卡槽数
                    Data unUsedCatData = new Data
                    {
                        dataList = _unUsedCat,
                        itemList = UnusedCatPos1,
                        type = CatFunEnum.CatUnusedPos,
                        gm = FunctionBtLst[(int)CatFunEnum.CatUnusedPos],
                        instance = instanceCatItem,
                        fun = (indx) => _icat.AddCat(),
                    };
                    buttonInit(unUsedCatData);


                    if (_icat.ProduceGoldRemainSeconds == -1)
                    {
                        //猫舍收集开始
                        index = (int)CatFunEnum.Start;
                        GameObject start = FunctionBtLst[index];
                        start.SetActive(true);
                        start.GetComponent<Button>().onClick.RemoveAllListeners();
                        start.GetComponent<Button>().onClick.AddListener(() => _icat.CatCollectStart());
                        start.transform.SetAsLastSibling();
                    }
                    else if (_icat.ProduceGoldRemainSeconds > 0)
                    {
                        //猫舍收集终止
                        index = (int)CatFunEnum.Stop;
                        GameObject stop = FunctionBtLst[index];
                        stop.SetActive(true);
                        stop.GetComponent<Button>().onClick.RemoveAllListeners();
                        stop.GetComponent<Button>().onClick.AddListener(() => _icat.CatCollectStop());
                        stop.transform.SetAsLastSibling();
                    }
                    else
                    {
                        //猫舍收集
                        index = (int)CatFunEnum.Collect;
                        GameObject Collect = FunctionBtLst[index];
                        Collect.SetActive(true);
                        Collect.GetComponent<Button>().onClick.RemoveAllListeners();
                        Collect.GetComponent<Button>().onClick.AddListener(() => _icat.CatCollect());
                        Collect.transform.SetAsLastSibling();
                    }
                }


                //猫舍升级
                index = (int)CatFunEnum.UpGrade;
                GameObject UpGrade = FunctionBtLst[index];
                if (_iFunction.supportGradeUp)
                {
                    UpGrade.SetActive(true);
                    UpGrade.GetComponent<Button>().onClick.RemoveAllListeners();
                    UpGrade.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        this.CatHouseUpGrade();

                    });
                    FunctionBtLst[index].transform.SetAsLastSibling();
                }
                else
                {
                    FunctionBtLst[index].GetComponent<Button>().onClick.RemoveAllListeners();
                    FunctionBtLst[index].SetActive(false);
                }
                //猫舍加速
                index = (int)OtherFunEnum.UpSleep;
                GameObject UpSleep = FunctionBtLst[index];
                if (_iFunction.supportSpeedUp)
                {
                    UpSleep.SetActive(true);
                    UpSleep.GetComponent<Button>().onClick.RemoveAllListeners();
                    UpSleep.GetComponent<Button>().onClick.AddListener(() => OnCatSpeedUp());
                    UpSleep.transform.SetAsLastSibling();
                }
                else
                {
                    UpSleep.GetComponent<Button>().onClick.RemoveAllListeners();
                    UpSleep.SetActive(false);
                }
            }
        }
        #endregion
        #region 手工作坊按钮


        /// <summary>
        /// 手工作坊按钮初始化
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="index">数据索引</param>
        /// <param name="initGm">要初始化的物体节点，null--创建物体</param>
        /// <returns></returns>
        private KUIItem instanceMakeItem(Data data, int index, GameObject initGm)
        {
            GameObject gmTemp;
            MakeItem item;
            if (!initGm)
            {
                gmTemp = Object.Instantiate(data.gm);
                item = gmTemp.AddComponent<MakeItem>();
                gmTemp.transform.SetParent(_function);
                gmTemp.transform.localScale = Vector3.one;
                gmTemp.SetActive(true);
            }
            else
            {
                gmTemp = initGm;
                item = initGm.GetComponent<MakeItem>();
            }
            gmTemp.transform.SetAsLastSibling();

            item.MakeEvent = data.fun;

            item.MakeTypeSet(data.type);

            item.SetData(data.dataList[index]);

            if ((ManualWorkShopFunEnum)data.type == ManualWorkShopFunEnum.UpSleep)
                item.getmoneyCostSet(getmoneyCost);
            else
                item.getmoneyCostSet(null);
            //gmTemp.GetComponent<Button>().onClick.RemoveAllListeners();
            return item;
        }



        private void ManualWorkShopSet()
        {
            _iMWShop = data as IManualWorkShopFunction;

            int index = 0;
            if (_iMWShop != null)
            {
                makeLstInit(_iMWShop.CurrSlot);
                Debug.Log("手工作坊锻造总数" + _iMWShop.CurrSlot.Length);





                //添加正在锻造的槽
                Data data = new Data
                {
                    dataList = _makeingLstData,
                    itemList = MakeingLst1,
                    type = ManualWorkShopFunEnum.UpSleep,
                    gm = FunctionBtLst[(int)ManualWorkShopFunEnum.UpSleep],
                    instance = instanceMakeItem,
                    fun = this.OnMakeSpeedUp,
                };
                buttonInit(data);

                //添加准备锻造的槽
                Data data1 = new Data
                {
                    dataList = _makeReadyLstData,
                    itemList = MakeReadyLst1,
                    type = ManualWorkShopFunEnum.ReadyMake,
                    gm = FunctionBtLst[(int)ManualWorkShopFunEnum.ReadyMake],
                    instance = instanceMakeItem,
                    fun = null,
                };
                buttonInit(data1);
                //添加可以锻造的槽
                Data data2 = new Data
                {
                    dataList = _makeLstData,
                    itemList = MakeLst1,
                    type = ManualWorkShopFunEnum.Make,
                    gm = FunctionBtLst[(int)ManualWorkShopFunEnum.Make],
                    instance = instanceMakeItem,
                    fun = this.IMWShopMake,
                };
                buttonInit(data2);

                //猫槽空位添加
                index = (int)ManualWorkShopFunEnum.Add;
                GameObject itemGm;
                if (_unlockLstData.Length < _iMWShop.CurrSlot.Length)
                {

                    //锻造槽添加
                    itemGm = FunctionBtLst[index];
                    itemGm.SetActive(true);
                    itemGm.transform.Find("Cost").GetComponent<Text>().text = this.IMWShopCurrSlot(_unlockLstData.Length).ToString();
                    itemGm.transform.SetAsLastSibling();
                    itemGm.GetComponent<Button>().onClick.RemoveAllListeners();
                    itemGm.GetComponent<Button>().onClick.AddListener(() => this.IMWShopBuySlot(_unlockLstData.Length));
                }
                else
                {
                    FunctionBtLst[index].SetActive(false);
                }

                //锻造  ---配方
                index = (int)ManualWorkShopFunEnum.Formula;
                itemGm = FunctionBtLst[index];
                itemGm.SetActive(true);
                itemGm.transform.SetAsLastSibling();
                itemGm.GetComponent<Button>().onClick.RemoveAllListeners();
                itemGm.GetComponent<Button>().onClick.AddListener(this.IMWShopFormulaGet);
            }


        }

        private int getmoneyCost()
        {
            return _moneyCost;
        }

        #endregion
        /// <summary>
        /// 刷新UI界面
        /// </summary>
        public void RefurbishData()
        {
            if (!gameObject || !gameObject.activeInHierarchy)
                return;
            //Debug.Log("刷新数据");
            _iconUpSleep = FunctionBtLst[(int)OtherFunEnum.UpSleep].transform.Find("Icon");
            if (_iconUpSleep)
                _iconUpSleep.gameObject.SetActive(false);

            for (int i = 0; i < CurrFunctionBtLst.Length; i++)
            {
                CurrFunctionBtLst[i].SetActive(false);
            }
            if (_iFunction == null)
                return;
            switch (_iFunction.functionTypeType)
            {

                case Build.Building.Category.kCatHouse:
                    {
                        catFunSet();
                        break;
                    }
                case Build.Building.Category.kManualWorkShop:
                    {
                        ManualWorkShopSet();
                        break;
                    }
                case Build.Building.Category.kExpeditionZone:
                case Build.Building.Category.kTree:
                case Build.Building.Category.kFosterCare:
                case Build.Building.Category.kTakePhotos:
                    {
                        FunctionBtLst[(int)OtherFunEnum.IntoBtnFun].SetActive(true);
                        FunctionBtLst[(int)OtherFunEnum.IntoBtnFun].GetComponent<Button>().onClick.RemoveAllListeners();
                        FunctionBtLst[(int)OtherFunEnum.IntoBtnFun].GetComponent<Button>().onClick.AddListener(OnIntoView);


                        break;
                    }
                case Build.Building.Category.kFarm:
                    {
                        GameObject gm = FunctionBtLst[(int)OtherFunEnum.UpSleep];
                        if (_iFunction.supportSpeedUp)
                        {
                            gm.GetComponent<Button>().onClick.AddListener(this.OnSpeedUp);
                            gm.SetActive(true);
                        }
                        else
                        {
                            gm.SetActive(false);
                        }

                        break;
                    }
                case Build.Building.Category.kLifePool:
                case Build.Building.Category.kSurface:
                    {
                        break;
                    }

                default: break;
            }

            //刷新金币数据显示
            RefurbishCost();
            //显示建筑物信息
            FunctionBtLst[(int)OtherFunEnum.BuildingInfo].SetActive(true);  //xian
            FunctionBtLst[(int)OtherFunEnum.BuildingInfo].transform.SetAsLastSibling();

        }


        public void RefurbishCost()
        {
            if (_iFunction != null)
            {
                if (_iFunction.supportSpeedUp)
                {
                    int moneyCost = 0;
                    int moneyType = 0;
                    _iFunction.SpeedUpInfo(ref moneyCost, ref moneyType);

                    if (_iFunction.functionTypeType == Building.Category.kFarm || _iFunction.functionTypeType == Building.Category.kCatHouse)
                    {
                        _speedUpMoneyCost.text = moneyCost.ToString();


                    }
                    else if (_iFunction.functionTypeType == Building.Category.kManualWorkShop)
                    {
                        _moneyCost = moneyCost;
                    }
                }
            }
        }
        #endregion

        #region Unity 

        public override void Awake()
        {
            base.Awake();

            _titleText = Find<Text>("Title");
            _speedUp = Find<Button>("Function/SpeedUp");
            _gradeUp = Find<Button>("Function/GradeUp");
            _collect = Find<Button>("Function/Collect");
            _infomation = Find<Button>("Function/Info");
            _speedUpMoneyCost = Find<Text>("Function/SpeedUp/Cost");
            //_iconUpSleep = FunctionBtLst[(int)OtherFunEnum.UpSleep].transform.Find("Icon");
            _function = Find<Transform>("Function");

            _infomation.onClick.AddListener(this.OnInfo);
            FunctionBtLst = new GameObject[_function.childCount];
            for (int i = 0; i < _function.childCount; i++)
            {
                FunctionBtLst[i] = _function.GetChild(i).gameObject;
            }

            _catcoin = FunctionBtLst[(int)CatFunEnum.Collect].transform.Find("Cost").GetComponent<Text>();

        }
        public override void OnEnable()
        {
            base.OnEnable();

            //Debug.Log("显示");
            _iFunction = data as Game.Build.IFunction;

            if (_iFunction != null)
            {
                _titleText.text = _iFunction.title;
                _infomation.gameObject.SetActive(_iFunction.supportInfomation);
                if (_iFunction.supportInfomation)
                {
                }
                _speedUp.gameObject.SetActive(_iFunction.supportSpeedUp);
                if (_iFunction.supportSpeedUp)
                {

                }
                _gradeUp.gameObject.SetActive(_iFunction.supportGradeUp);
                if (_iFunction.supportGradeUp)
                {

                }
                _collect.gameObject.SetActive(_iFunction.supportCollect);
                if (_iFunction.supportCollect)
                {

                }


                RefurbishData();
                if (_iFunction.functionTypeType == Build.Building.Category.kCatHouse)
                {
                    StopAllCoroutine();
                    StartCoroutine(CoinRefurbishFun());
                }
            }
            var mainWindow = KUIWindow.GetWindow<MainWindow>();

            mainWindow.FoldMajor();


        }

        public override void UpdatePerSecond()
        {
            RefurbishCost();
        }
        #endregion
    }
}
