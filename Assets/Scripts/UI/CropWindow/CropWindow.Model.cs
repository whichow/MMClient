// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "CropWindow.Model" company=""></copyright>
// <summary></summary>
// ***********************************************************************

namespace Game.UI
{
    partial class CropWindow
    {
        public KItemCrop[] GetCrops()
        {
            return KItemManager.Instance.GetCrops();
        }
    }
}