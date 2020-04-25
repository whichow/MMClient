using Game.DataModel;
using Game.Match3;
using System.Collections.Generic;

namespace Game.UI
{
    public class LevelInfoData
    {
        public LevelXDM kLevel;

        public LevelInfoData(int id)
        {
            kLevel = XTable.LevelXTable.GetByID(id);
        }

        public LevelInfoData(LevelXDM level)
        {
            kLevel = level;
        }
    }

    public partial class LevelInfoWindow
    {
        public enum SortType
        {
            star,
            level,
            color,
            rarity
        }

        public LevelXDM levelData;
        public M3LevelData m3Data;
        public KItemProp[] propList;
        public int[] propShopList;

        public void InitModel()
        {
            levelData = ((LevelInfoData)data).kLevel;
            m3Data = M3LevelConfigMgr.Instance.GetLevelConfigData(levelData.ChessboardID);

            var props = levelData.ChooseProp;
            propList = new KItemProp[props.Count];
            propShopList = new int[props.Count];
            for (int i = 0; i < props.Count; i++)
            {
                propList[i] = KItemManager.Instance.GetProp(props[i]);
                propShopList[i] = levelData.ChooseShopId[i];
            }
        }

        public void RefreshModel()
        {
            if (data != null)
            {
                levelData = ((LevelInfoData)data).kLevel;
                m3Data = M3LevelConfigMgr.Instance.GetLevelConfigData(levelData.ChessboardID);
                var props = levelData.ChooseProp;
                propList = new KItemProp[props.Count];
                for (int i = 0; i < props.Count; i++)
                {
                    propList[i] = KItemManager.Instance.GetProp(props[i]);
                }
            }
        }

    }
}
