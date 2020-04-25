/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/9 18:45:01
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using Game.DataModel;
using UnityEngine;

namespace Game
{
    public class PlayerAvatar : RoleAvatar
    {
        #region Member
        private AvatarChange m_avatarChange;
        #endregion
        public const string ROLE_COSTUME = "costume";
        public const string ROLE_APPLAUSE = "applause";
        public const string ROLE_RANKING = "ranking";

        #region C&D
        public PlayerAvatar(int modID) : base(modID)
        {
            int gender = 0;
            if (modID == PlayerDataModel.Instance.mPlayerData.mPlayerID)
                gender = SpaceDataModel.Instance.mGender;
            else
                gender = SpaceDataModel.Instance.mSpaceOtherDataVO.mGender;
            string resName = gender == 1 ? "base_boy" : "base_girl";
            Init(resName);
        }
        #endregion

        #region Public
        public void ChangePart(string resName)
        {
            if (m_roleGO != null)
                m_avatarChange.ChangePart(resName);
        }
        public void SetDefaultPart(EAvatarPart part)
        {
            if (part == EAvatarPart.none)
                m_avatarChange.SetDefaultAll();
            else
                m_avatarChange.SetDefaultPart(part);
        }
        #endregion

        #region Private
        protected override void Load(string resName)
        {
            KAssetManager.Instance.TryGetCharacterPrefab(resName, OnLoaded);
        }

        protected override void OnLoaded(GameObject prefab)
        {
            base.OnLoaded(prefab);

            m_avatarChange = new AvatarChange(m_roleGO);
            if (ModID == PlayerDataModel.Instance.mPlayerData.mPlayerID)
            {
                for (int i = 0; i < SpaceDataModel.Instance.mLstFashionId.Count; i++)
                {
                    ItemXDM itemXDM = XTable.ItemXTable.GetByID(SpaceDataModel.Instance.mLstFashionId[i]);
                    m_avatarChange.ChangePart(itemXDM.Model);
                }
            }
            else
            {
                for (int i = 0; i < SpaceDataModel.Instance.mSpaceOtherDataVO.mFashionId.Count; i++)
                {
                    ItemXDM itemXDM = XTable.ItemXTable.GetByID(SpaceDataModel.Instance.mSpaceOtherDataVO.mFashionId[i]);
                    m_avatarChange.ChangePart(itemXDM.Model);
                }
            }
            //此处根据实际数据传入资源名
            //m_avatarChange.ChangePart("mj_c6532");
            //m_avatarChange.ChangePart("mp_c6532");
            //m_avatarChange.ChangePart("ms_c6532");
        }
        #endregion

    }
}
