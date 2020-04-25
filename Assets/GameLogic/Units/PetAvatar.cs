/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/28 14:12:59
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using Game.DataModel;

namespace Game
{
    public class PetAvatar : RoleAvatar
    {
        #region Member

        #endregion

        #region C&D
        public PetAvatar(int modID) : base(modID)
        {
            var cat = XTable.CatXTable.GetByID(modID);
            string resName = cat.Model;
            Init(resName);
        }
        #endregion

        #region Public

        #endregion

        #region Private
        protected override void Load(string resName)
        {
            KAssetManager.Instance.TryGetPetPrefab(resName, OnLoaded);
        }
        #endregion
        
    }
}
