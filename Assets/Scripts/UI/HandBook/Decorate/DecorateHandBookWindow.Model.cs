// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "MessageBox.Model" company=""></copyright>
// <summary></summary>
// ***********************************************************************

namespace Game.UI
{
    partial class DecorateHandBookWindow
    {
        public enum PageType
        {
            letter,//字母
            infrastructure,//基建
            trees,//植物
            water,//水上
            others,//其他类
        }

        #region WindowData

        public class ItemInformationData
        {
            public string _title;
        }
        private PageType _pageType;
        public PageType pageType
        {
            get { return _pageType; }
            private set
            {
                //if (_pageType != value)
                //{
                    _pageType = value;
                //}
            }
        }
        #endregion

        #region Field

        private ItemInformationData _messageData;

        #endregion

        #region Method

        public void InitModel()
        {
            _messageData = new ItemInformationData();
        }

        public void RefreshModel()
        {
            pageType = PageType.letter;
            _tg_letter.isOn = true;
        }
        public KHandBookManager.HandBookConfiger[] GetTorAData()
        {
            KHandBookManager.HandBookConfiger[] currentList;
            switch (pageType)
            {
                case PageType.letter:
                    currentList = KHandBookManager.Instance.GetDecorateDatas(PageType.letter);
                    break;
                case PageType.infrastructure:
                    currentList = KHandBookManager.Instance.GetDecorateDatas(PageType.infrastructure);
                    break;
                case PageType.trees:
                    currentList = KHandBookManager.Instance.GetDecorateDatas(PageType.trees);
                    break;
                case PageType.water:
                    currentList = KHandBookManager.Instance.GetDecorateDatas(PageType.water);
                    break;
                case PageType.others:
                    currentList = KHandBookManager.Instance.GetDecorateDatas(PageType.others);
                    break;
                default:
                    currentList = null;
                    break;
            }
//            UnityEngine.Debug.Log("当前页签 3：" + pageType + "长度：" + currentList.Length);k
            //Debug.Log("当前页签：" + pageType + "数据长度：" + Catdata.Length);
            return currentList;
        }
        /*
        private void RefreshToggle()
        {

            switch (pageType)
            {
                case PageType.letter:
                    _tgl_AllCat.isOn = true;
                    _tgl_NCat.isOn = false;
                    _tgl_RCat.isOn = false;
                    _tgl_SRCat.isOn = false;
                    _tgl_SSRCat.isOn = false;
                    break;
                case PageType.infrastructure:
                    _tgl_AllCat.isOn = false;
                    _tgl_NCat.isOn = true;
                    _tgl_RCat.isOn = false;
                    _tgl_SRCat.isOn = false;
                    _tgl_SSRCat.isOn = false;
                    break;
                case PageType.trees:
                    _tgl_AllCat.isOn = false;
                    _tgl_NCat.isOn = false;
                    _tgl_RCat.isOn = true;
                    _tgl_SRCat.isOn = false;
                    _tgl_SSRCat.isOn = false;
                    break;
                case PageType.water:
                    _tgl_AllCat.isOn = false;
                    _tgl_NCat.isOn = false;
                    _tgl_RCat.isOn = false;
                    _tgl_SRCat.isOn = true;
                    _tgl_SSRCat.isOn = false;
                    break;
                case PageType.others:
                    _tgl_AllCat.isOn = false;
                    _tgl_NCat.isOn = false;
                    _tgl_RCat.isOn = false;
                    _tgl_SRCat.isOn = false;
                    _tgl_SSRCat.isOn = true;
                    break;
                default:
                    break;
            }
        }*/
        #endregion
    }
}

