using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.UI
{
    public partial class BagPowerBoxWindow
    {



        public class Data
        {
            public KItem itemdata;
            public System.Action onConfirm;
            public System.Action onCancel;
        }


        private Data _bagPowerBoxData;
        public void InitModel()
        {
            _bagPowerBoxData = new Data();
        }

        public void RefreshModel()
        {
            var passData = data as Data;
            if (passData != null)
            {
                _bagPowerBoxData.itemdata = passData.itemdata;
                _bagPowerBoxData.onConfirm = passData.onConfirm;
                _bagPowerBoxData.onCancel = passData.onCancel;
            }
            else
            {
                _bagPowerBoxData.itemdata = null;
                _bagPowerBoxData.onConfirm = null;
                _bagPowerBoxData.onCancel = null;

            }
        }
    }
}