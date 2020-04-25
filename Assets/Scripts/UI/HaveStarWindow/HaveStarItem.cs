/** 
 *FileName:     DiscoveryItem.cs 
 *Author:       LiMuChen 
 *Version:      1.0 
 *UnityVersion：5.6.3f1
 *Date:         2017-10-19 
 *Description:    
 *History: 
*/
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    using Game.DataModel;
    using UnityEngine.EventSystems;

    public class HaveStarItem : KUIItem, IPointerClickHandler
    {

        #region Field
        private Transform[] _tansStarList;
        private Text _textLevelNum;
        private LevelXDM _level;
        #endregion

        #region Method

        public void ShowLevel(LevelXDM level)
        {

            if (level != null)
            {
                _level = level;
                for (int i = 0; i < _tansStarList.Length; i++)
                {
                    if (level.CurrStar > i)
                    {
                        _tansStarList[i].GetComponent<Image>().material = null;
                    }
                    else
                    {
                        _tansStarList[i].GetComponent<Image>().material = Resources.Load<Material>("Materials/UIGray");
                    }
                }
                _textLevelNum.text = level.LevelIndex.ToString();
            }


        }


        #endregion

        #region Action


        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            KUIWindow.CloseWindow<HaveStarWindow>();
            KUIWindow.OpenWindow<LevelInfoWindow>(new LevelInfoData(_level.ID));
        }

        #endregion

        #region Unity

        private void Awake()
        {
            var star = Find<Transform>("Item/Star");
            _tansStarList = new Transform[star.childCount];
            for (int i = 0; i < _tansStarList.Length; i++)
            {
                _tansStarList[i] = Find<Transform>("Item/Star/Start" + (i + 1));
            }
            _textLevelNum = Find<Text>("Item/Text");

        }
        void Update()
        {

        }




        #endregion
    }
}

