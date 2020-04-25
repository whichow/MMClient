/** 
 *FileName:     #SCRIPTFULLNAME# 
 *Author:       #AUTHOR# 
 *Version:      #VERSION# 
 *UnityVersion：#UNITYVERSION#
 *Date:         #DATE# 
 *Description:    
 *History: 
*/
using Game.Match3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.UI
{
    public partial class M3GameUIWindow
    {
        M3GameModeManager modeManager;
        M3LevelData mapData;
        public List<int> propList;
        public List<int> propShopIDList;


        public string bonusEffectName = "FX_SX_Other69_bonustime";
        public string fantasicEffectName = "FX_SX_Other69_fantasic";
        public string coolEffectName = "FX_SX_Other69_cool";
        public string goodEffectName = "FX_SX_Other69_good";
        public string impossibleEffectName = "FX_SX_Other69_impossible";
        public string wonderfulEffectName = "FX_SX_Other69_wonderful";



        public void InitModel()
        {
            itemDic = new Dictionary<int, M3TargetItem>();
            modeManager = M3GameManager.Instance.modeManager;
            mapData = M3GameManager.Instance.level;
            if (LevelDataModel.Instance.CurrLevel != null)
            {
                propList = LevelDataModel.Instance.CurrLevel.BattleProp;
                propShopIDList = LevelDataModel.Instance.CurrLevel.BattleShopId;
            }
            M3GameEvent.AddEvent(M3FightEnum.Comb, PlaySignEffect);
            M3GameEvent.AddEvent(M3FightEnum.UpdateScore, UpdateScore);
            M3GameEvent.AddEvent(M3FightEnum.OnAddEnergy, AddEnergy);
        }
        public void RefreshPropModel()
        {
            //if (KLevelManager.Instance.currLevel != null)
            //    propList = KLevelManager.Instance.currLevel.FightProps;
        }
    }
}
