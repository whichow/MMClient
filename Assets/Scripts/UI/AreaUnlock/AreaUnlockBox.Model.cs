// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "AreaUnlockBox.Model" company=""></copyright>
// <summary></summary>
// ***********************************************************************

namespace Game.UI
{
    public partial class AreaUnlockBox
    {
        #region WindowData

        public class Data
        {
            public int grade;
            public int star;
            public int cost;
            public int costType;
            public System.Action onConfirm;
            public System.Action onCancel;
        }

        #endregion

        #region Field

        public readonly static Data DefaultData = new Data();

        private Data _myData;

        #endregion

        #region Method

        public void InitModel()
        {
            _myData = new Data();
        }

        public void RefreshModel()
        {
            var passData = data as Data;
            if (passData != null)
            {
                _myData.grade = passData.grade;
                _myData.star = passData.star;
                _myData.cost = passData.cost;
                _myData.costType = passData.costType;
                _myData.onConfirm = passData.onConfirm;
                _myData.onCancel = passData.onCancel;
            }
            else
            {
                _myData.grade = 0;
                _myData.star = 0;
                _myData.cost = 0;
                _myData.costType = 0;
                _myData.onConfirm = null;
                _myData.onCancel = null;
            }
        }

        #endregion
    }
}
