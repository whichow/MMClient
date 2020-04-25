/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.5
 * 
 * Author:          Coamy
 * Created:	        2019/3/15 12:00:00
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using Game.UI;

namespace Game.Build
{
    /// <summary>
    /// 地板
    /// </summary>
    class BuildingSurface : Building, IFunCommon
    {

        #region IFunCommon

        public bool IsSell
        {
            get
            {
                return false;
            }
        }

        public bool IsRotate
        {
            get
            {
                return false;
            }
        }

        public bool IsRecovery
        {
            get
            {
                return false;
            }
        }

        public void OnSell()
        {
        }

        public void OnRotate()
        {
        }

        public void OnRecovery()
        {
           
        }

        public void OnRotateConfirm()
        {
        }

        #endregion

        #region override

        //protected override void OnFocus(bool focus)
        //{
        //    base.OnFocus(focus);
        //    //GameCamera.Instance.Show(entityView.centerNode.position);
        //}

        //protected override void OnTap()
        //{
        //    base.OnTap();
        //}

        #endregion

        #region Unity  

        private void Start()
        {
            entityView.ShowModel();
        }

        #endregion

    }
}
