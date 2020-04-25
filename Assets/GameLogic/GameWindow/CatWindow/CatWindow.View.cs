using Game.DataModel;
using Msg.ClientMessage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    partial class CatWindow
    {
        private List<CatDataVO> _catDataVOs;
        private UIList _catList;
        private Text _catCountText;
        private GameObject _hint;
        private Dropdown _dropDown;
        private int _sortType;
        private int _openType;

        public void InitView()
        {
            _catList = Find<UIList>("List");
            _catList.SetRenderHandler(RenderHandler);
            _catList.SetSelectHandler(SelectHandler);
            _catList.SetPointerHandler(PointerHandler);
            _catCountText = Find<Text>("CatCount/Text");
            _hint = Find("Hint");
            _dropDown = Find<Dropdown>("Dropdown");
            _dropDown.onValueChanged.AddListener(OnSortTypeChange);

            _sortType = CatSortConst.Rarity;
            _openType = CatOpenType.Normal;
        }

        private void SelectHandler(UIListItem item, int index)
        {
            CatDataVO vo = item.dataSource as CatDataVO;
            if (vo == null)
                return;
        }

        private void OnSortTypeChange(int index)
        {
            _sortType = index;
            OnCatDataChang(_catType);
        }

        private void PointerHandler(UIListItem item, int index)
        {
            CatDataVO vo = item.dataSource as CatDataVO;
            if (vo == null)
                return;
            item.GetComp<TweenScl>("ItemObj").PlayBack();
            CatDatas catDatas = new CatDatas();
            catDatas.mCatDataVOs = _catDataVOs;
            catDatas.mIndex = index;
            catDatas.mOpenType = _openType;
            KUIWindow.OpenWindow<CatInfoWindow>(catDatas);
        }

        private void RenderHandler(UIListItem item, int index)
        {
            CatDataVO vo = item.dataSource as CatDataVO;
            if (vo == null)
                return;
            item.GetComp<Image>("ItemObj/Icon").overrideSprite = KIconManager.Instance.GetCatIcon(vo.mCatXDM.Icon);
            KUIImage frameImage = item.GetComp<KUIImage>("ItemObj/Frame");
            frameImage.gameObject.SetActive(vo.mCatXDM.Rarity != CatRarityConst.N);
            frameImage.ShowSprite(vo.mCatXDM.Rarity - 1);
            Text stateText = item.GetComp<Text>("ItemObj/State/Text");
            if (_openType == CatOpenType.Normal)
            {
                item.GetGameObject("ItemObj/State").SetActive(vo.mCatInfo.State != CatStateConst.None);
                switch (vo.mCatInfo.State)
                {
                    case CatStateConst.None:
                        break;
                    case CatStateConst.Cattery:
                        stateText.text = KLocalization.GetLocalString(54108);
                        break;
                    case CatStateConst.Explore:
                        stateText.text = KLocalization.GetLocalString(54110);
                        break;
                    case CatStateConst.Foster:
                        stateText.text = KLocalization.GetLocalString(54109);
                        break;
                }
            }
            else
            {
                item.GetGameObject("ItemObj/State").SetActive(SpaceDataModel.Instance.OnIsExhibition(vo.mCatInfo.Id));
                stateText.text = KLocalization.GetLocalString(54168);
            }
            #region ÐÇ¼¶ µÈ¼¶
            //KUIImage colorImage = item.GetComp<KUIImage>("ItemObj/Fish");
            //switch (vo.mCatXDM.MainColor)
            //{
            //    case CatColorConst.Red:
            //        colorImage.ShowSprite(0);
            //        break;
            //    case CatColorConst.Yellow:
            //        colorImage.ShowSprite(1);
            //        break;
            //    case CatColorConst.Blue:
            //        colorImage.ShowSprite(3);
            //        break;
            //    case CatColorConst.Green:
            //        colorImage.ShowSprite(2);
            //        break;
            //    case CatColorConst.Purple:
            //        colorImage.ShowSprite(5);
            //        break;
            //    case CatColorConst.Brown:
            //        colorImage.ShowSprite(4);
            //        break;
            //}
            //var fish = item.GetComp<Transform>("ItemObj/Fish");
            //KUIImage[] starImages = new KUIImage[fish.childCount];
            //for (int i = 0; i < starImages.Length; i++)
            //    starImages[i] = fish.GetChild(i).GetComponent<KUIImage>();
            //for (int i = 0; i < starImages.Length; i++)
            //    starImages[i].ShowGray(i >= vo.mCatInfo.Star);
            //item.GetGameObject("ItemObj/Lock").SetActive(vo.mCatInfo.Locked);
            //KUIImage flagImage = item.GetComp<KUIImage>("ItemObj/Flag");
            //flagImage.ShowSprite(vo.mCatXDM.Rarity - 1);
            //item.GetComp<Text>("ItemObj/Grade").text = vo.mCatInfo.Level.ToString();
            #endregion
        }

        public void RefreshView(int type)
        {
            _openType = int.Parse(data.ToString());
            _dropDown.options.Clear();
            _dropDown.options.Add(new Dropdown.OptionData(KLocalization.GetLocalString(54162)));
            _dropDown.options.Add(new Dropdown.OptionData(KLocalization.GetLocalString(54161)));
            _dropDown.captionText.text = _dropDown.options[_sortType].text;
            OnCatDataChang(type);
        }

        private void OnCatDataChang(int type)
        {
            if (_catDataVOs != null)
                _catDataVOs.Clear();
            _catDataVOs = new List<CatDataVO>();
            _catDataVOs = CatDataModel.Instance.GetCatDataByType(type);
            _catDataVOs.Sort(OnCatSort);
            _catList.DataArray = _catDataVOs;
            _hint.SetActive(_catList.DataArray.Count <= 0);
            _catCountText.text = KLocalization.GetLocalString(54122) + _catList.DataArray.Count.ToString();
        }

        private int OnCatSort(CatDataVO v1, CatDataVO v2)
        {
            int index = 0;
            switch (_sortType)
            {
                case CatSortConst.Rarity:
                    index = v2.mCatXDM.Rarity.CompareTo(v1.mCatXDM.Rarity);
                    if (index != 0)
                        return index;
                    index = v2.mCatInfo.Star.CompareTo(v1.mCatInfo.Star);
                    if (index != 0)
                        return index;
                    index = v2.mCatInfo.Level.CompareTo(v1.mCatInfo.Level);
                    if (index != 0)
                        return index;
                    index = v1.mCatXDM.MainColor.CompareTo(v2.mCatXDM.MainColor);
                    if (index != 0)
                        return index;
                    break;
                case CatSortConst.Color:
                    index = v1.mCatXDM.MainColor.CompareTo(v2.mCatXDM.MainColor);
                    if (index != 0)
                        return index;
                    index = v2.mCatInfo.Star.CompareTo(v1.mCatInfo.Star);
                    if (index != 0)
                        return index;
                    index = v2.mCatInfo.Level.CompareTo(v1.mCatInfo.Level);
                    if (index != 0)
                        return index;
                    index = v2.mCatXDM.Rarity.CompareTo(v1.mCatXDM.Rarity);
                    if (index != 0)
                        return index;
                    break;
            }
            index = v1.mCatInfo.CatCfgId.CompareTo(v2.mCatInfo.CatCfgId);
            if (index != 0)
                return index;
            index = v1.mCatInfo.Id.CompareTo(v2.mCatInfo.Id);
            return index;
        }
    }

    public class CatDatas
    {
        public List<CatDataVO> mCatDataVOs;
        public int mIndex;
        public int mOpenType;
    }
}
