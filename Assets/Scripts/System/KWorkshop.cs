// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KWorkshop" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    using Callback = System.Action<int, string, object>;

    /// <summary>
    /// 手工作坊
    /// </summary>
    public class KWorkshop : KGameModule
    {

        public enum MakeSateType
        {
            None,
            Lock,
            Make,
            Makeing,
            MakeReady,
            MakeCancel,
            MakeFinish,
        }
        #region Slot

        public class Slot
        {
            /// <summary>
            /// 打造完成的时间戳
            /// </summary>
            private int _completeTimestamp;

            public int slotId
            {
                get { return slotIndex; }
            }
            /// <summary>
            /// 槽位顺序
            /// </summary>
            public int slotOrder
            {
                get;
                set;
            }
            public int slotIndex
            {
                get;
                private set;
            }

            public int formulaId
            {
                get;
                set;
            }

            public bool unlock
            {
                get;
                set;
            }

            public KItem.ItemInfo unlockCost
            {
                get;
                set;
            }

            /// <summary>
            /// 剩余时间(s)
            /// </summary>
            public int remainTime
            {
                get { return _completeTimestamp - KLaunch.Timestamp; }
                set
                {
                    _completeTimestamp = KLaunch.Timestamp + value;
                }
            }

            public int totalTime
            {
                get
                {
                    if (formulaId > 0)
                    {
                        var formula = KItemManager.Instance.GetFormula(formulaId);
                        return formula.costTime;
                    }
                    return 1;
                }
            }

            /// <summary>
            /// -1 locked,0 noting,> 0 ing
            /// </summary>
            public int state
            {
                get
                {
                    if (!unlock)
                    {
                        return -1;
                    }
                    else
                    {
                        return formulaId;
                    }
                }
            }

            public Sprite GetFormulaIcon()
            {
                if (formulaId > 0)
                {
                    var formula = KItemManager.Instance.GetFormula(formulaId);
                    if (formula != null)
                    {
                        return KIconManager.Instance.GetBuildingIcon(formula.GetBuildindIcon());
                    }
                }
                return null;
            }

            public MakeSateType MakeSate { get; set; }

            public Slot(int index)
            {
                slotIndex = index;
            }

            public void Make(int formulaId, int remainSecond)
            {
                this.formulaId = formulaId;
                remainTime = remainSecond;
            }

            public void Cancel()
            {
                formulaId = 0;
            }

            public void SpeedUp()
            {
                remainTime = 0;
            }

            public void Collect()
            {
                formulaId = 0;
            }
        }

        #endregion

        #region Field

        public const int kMaxSlotCount = 4;

        private Slot[] _slots;
        private List<int> _makedItemIds = new List<int>();

        #endregion

        #region Method
        /// <summary>
        /// 初始化锻造状态
        /// </summary>
        public void MakeSateInit()
        {
            bool state = false;
            for (int i = 0; i < _slots.Length; i++)
            {
                if (!_slots[i].unlock)
                {
                    _slots[i].MakeSate = MakeSateType.Lock;
                    continue;
                }
                if (_slots[i].remainTime > 0 && !state)
                {
                    _slots[i].MakeSate = MakeSateType.Makeing;
                    state = true;
                }
                else
                {
                    if (_slots[i].formulaId > 0)
                    {
                        if (!state)
                            _slots[i].MakeSate = MakeSateType.MakeFinish;
                        else
                            _slots[i].MakeSate = MakeSateType.MakeReady;
                    }
                    else
                        _slots[i].MakeSate = MakeSateType.Make;
                }
            }
        }

        public Slot GetSlot(int index)
        {
            if (index > 0 && index <= kMaxSlotCount)
            {
                return System.Array.Find(_slots, slot => slot.slotIndex == index);
            }
            return null;
        }

        public Slot[] GetSlot()
        {
            System.Array.Sort(_slots, (x, y) => { return x.slotOrder.CompareTo(y.slotOrder); });
            MakeSateInit();

            return _slots;// System.Array.FindAll(_slots, s => s.unlock == true);
        }

        /// <summary>
        /// 获取打造完的物品
        /// </summary>
        /// <returns></returns>
        public KItemBuilding[] GetMakedItem()
        {
            if (_makedItemIds.Count > 0)
            {
                var ret = new KItemBuilding[_makedItemIds.Count];
                for (int i = 0; i < _makedItemIds.Count; i++)
                {
                    ret[i] = KItemManager.Instance.GetBuilding(_makedItemIds[i]);
                }
                return ret;
            }
            return null;
        }

        public void GetInfos(Callback callback)
        {
            KUser.WorkshopGets((code, message, data) =>
            {
                if (code == 0)
                {
                    OnGetInfosCallback(code, message, data);
                }
                if (callback != null)
                {
                    callback(code, message, data);
                }
            });
        }

        public void BuySlot(int slotIndex, Callback callback)
        {
            KUser.WorkshopBuySlot(slotIndex, (code, message, data) =>
            {
                if (code == 0)
                {
                    OnGetInfosCallback(code, message, data);
                    OnBuySlotCallback(code, message, data);
                }
                if (callback != null)
                {
                    callback(code, message, data);
                }
            });
        }

        public void Make(int slotIndex, int formulaId, Callback callback)
        {
            KUser.WorkshopMakeFormula(slotIndex, formulaId, (code, message, data) =>
             {
                 if (code == 0)
                 {
                     OnGetInfosCallback(code, message, data);
                     OnMakeCallback(code, message, data);
                 }
                 if (callback != null)
                 {
                     callback(code, message, data);
                 }
             });
        }

        public void MakeCancel(int slotIndex, Callback callback)
        {
            KUser.WorkshopMakeCancel(slotIndex, (code, message, data) =>
            {
                if (code == 0)
                {
                    OnGetInfosCallback(code, message, data);
                    OnMakeCancelCallback(code, message, data);
                }
                if (callback != null)
                {
                    callback(code, message, data);
                }
            });
        }

        public void Collect(int slotIndex, Callback callback)
        {
            KUser.WorkshopCollect(slotIndex, (code, message, data) =>
            {
                if (code == 0)
                {
                    OnGetInfosCallback(code, message, data);
                    OnCollectCallback(code, message, data);
                }
                if (callback != null)
                {
                    callback(code, message, data);
                }
            });
        }

        public void SpeedUp(int slotIndex, Callback callback)
        {
            KUser.WorkshopSpeedUp(slotIndex, (code, message, data) =>
             {
                 if (code == 0)
                 {
                     OnGetInfosCallback(code, message, data);
                     OnSpeedUpCallback(code, message, data);
                 }
                 if (callback != null)
                 {
                     callback(code, message, data);
                 }
             });
        }
        public void Process(object data)
        {
            var protoData = data;

            if (protoData is Msg.ClientMessage.S2CGetMakingFormulaBuildingsResult)
            {
                var originData = (Msg.ClientMessage.S2CGetMakingFormulaBuildingsResult)protoData;

                for (int j = 0; j < originData.Buildings.Count; j++)
                {
                    Msg.ClientMessage.MakingFormulaBuildingInfo make = originData.Buildings[j];
                    var slotIndex = make.SlotId;
                    var slot = GetSlot(slotIndex);
                    slot.formulaId = make.FormulaId;
                    slot.remainTime = make.RemainSeconds;
                    slot.slotOrder = j;
                    slot.unlock = true;
                }

                //foreach (var item in originData.Buildings)
                //{
                //    var slotIndex = item.SlotId;
                //    var slot = GetSlot(slotIndex);
                //    slot.formulaId = item.FormulaId;
                //    slot.remainTime = item.RemainSeconds;
                //    slot.unlock = true;
                //}
                _makedItemIds.Clear();
                _makedItemIds.AddRange(originData.MakedBuildings);
            }
        }
        private void OnGetInfosCallback(int code, string message, object data)
        {
            var protoDatas = data as ArrayList;
            if (protoDatas != null)
            {
                for (int i = 0; i < protoDatas.Count; i++)
                {
                    var protoData = protoDatas[i];

                    if (protoData is Msg.ClientMessage.S2CGetMakingFormulaBuildingsResult)
                    {
                        var originData = (Msg.ClientMessage.S2CGetMakingFormulaBuildingsResult)protoData;

                        for (int j = 0; j < originData.Buildings.Count; j++)
                        {
                            Msg.ClientMessage.MakingFormulaBuildingInfo make = originData.Buildings[j];
                            var slotIndex = make.SlotId;
                            var slot = GetSlot(slotIndex);
                            slot.formulaId = make.FormulaId;
                            slot.remainTime = make.RemainSeconds;
                            slot.slotOrder = j;
                            slot.unlock = true;
                        }

                        //foreach (var item in originData.Buildings)
                        //{
                        //    var slotIndex = item.SlotId;
                        //    var slot = GetSlot(slotIndex);
                        //    slot.formulaId = item.FormulaId;
                        //    slot.remainTime = item.RemainSeconds;
                        //    slot.unlock = true;
                        //}
                        _makedItemIds.Clear();
                        _makedItemIds.AddRange(originData.MakedBuildings);
                    }
                }
            }
        }

        private void OnBuySlotCallback(int code, string message, object data)
        {
            var protoDatas = data as ArrayList;
            if (protoDatas != null)
            {
                for (int i = 0; i < protoDatas.Count; i++)
                {
                    var protoData = protoDatas[i];

                    if (protoData is Msg.ClientMessage.S2CBuyMakeBuildingSlotResult)
                    {
                        var originData = (Msg.ClientMessage.S2CBuyMakeBuildingSlotResult)protoData;
                        var slotIndex = originData.SlotId;
                        var slot = GetSlot(slotIndex);
                        slot.unlock = true;
                    }
                }
            }
        }

        private void OnMakeCallback(int code, string message, object data)
        {
        }

        private void OnMakeCancelCallback(int code, string message, object data)
        {
            var protoDatas = data as ArrayList;
            if (protoDatas != null)
            {
                for (int i = 0; i < protoDatas.Count; i++)
                {
                    var protoData = protoDatas[i];

                    if (protoData is Msg.ClientMessage.S2CCancelMakingFormulaBuildingResult)
                    {
                        var originData = (Msg.ClientMessage.S2CCancelMakingFormulaBuildingResult)protoData;
                        var slotIndex = originData.SlotId;
                        var slot = GetSlot(slotIndex);
                        slot.Cancel();
                    }
                }
            }
        }

        private void OnSpeedUpCallback(int code, string message, object data)
        {
            var protoDatas = data as ArrayList;
            if (protoDatas != null)
            {
                for (int i = 0; i < protoDatas.Count; i++)
                {
                    var protoData = protoDatas[i];

                    if (protoData is Msg.ClientMessage.S2CSpeedupMakeBuildingResult)
                    {
                        var originData = (Msg.ClientMessage.S2CSpeedupMakeBuildingResult)protoData;
                        var slotIndex = originData.SlotId;
                        var slot = GetSlot(slotIndex);
                        slot.SpeedUp();
                    }
                }
            }
        }

        private void OnCollectCallback(int code, string message, object data)
        {
            var protoDatas = data as ArrayList;
            if (protoDatas != null)
            {
                for (int i = 0; i < protoDatas.Count; i++)
                {
                    var protoData = protoDatas[i];

                    if (protoData is Msg.ClientMessage.S2CGetCompletedFormulaBuildingResult)
                    {
                        var originData = (Msg.ClientMessage.S2CGetCompletedFormulaBuildingResult)protoData;

                        //var slotIndex = originData.SlotId;
                        //var slot = GetSlot(slotIndex);
                        //slot.Collect();
                    }
                }
            }
        }

        private void Load(Hashtable table)
        {
            var taskList = table.GetArrayList("workingshop");
            if (taskList != null && taskList.Count > 0)
            {
                var tmpT = new Hashtable();
                var tmpL0 = (ArrayList)taskList[0];
                for (int i = 1; i < taskList.Count; i++)
                {
                    var tmpLi = (ArrayList)taskList[i];
                    for (int j = 0; j < tmpL0.Count; j++)
                    {
                        tmpT[tmpL0[j]] = tmpLi[j];
                    }

                    var slotIndex = tmpT.GetInt("slot");
                    var slot = GetSlot(slotIndex);
                    slot.unlockCost = KItem.ItemInfo.Convert(tmpT.GetArrayList("cost"));
                }
            }
        }

        #endregion

        #region Unity

        public static KWorkshop Instance;

        private void Awake()
        {
            Instance = this;
        }

        // Use this for initialization
        public override void Load()
        {
            _slots = new Slot[kMaxSlotCount];
            for (int i = 0; i < kMaxSlotCount; i++)
            {
                _slots[i] = new Slot(i + 1)
                {
                };
            }

            TextAsset textAsset;
            KAssetManager.Instance.TryGetExcelAsset("workingshop", out textAsset);
            if (textAsset)
            {
                var table = textAsset.bytes.ToJsonTable();
                if (table != null)
                {
                    Load(table);
                }
            }
        }

        #endregion
    }
}
