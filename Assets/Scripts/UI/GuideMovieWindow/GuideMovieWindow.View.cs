// ***********************************************************************
// Assembly         : Unity
// Author           : LiMuChen
// Created          : 
//
// Last Modified By : LiMuChen
// Last Modified On : 
// ***********************************************************************
// <copyright file= "GuideMovieWindow.View" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    partial class GuideMovieWindow
    {
        #region Field
        private Transform[] transMovie;
        private Button _btnClose;
        #endregion

        #region Method

        public void InitView()
        {
            var movieParent = Find<Transform>("MovieGroup");
            transMovie = new Transform[movieParent.childCount];
            for (int i = 0; i < transMovie.Length; i++)
            {
                transMovie[i] = Find<Transform>("MovieGroup/Guide_" + (i + 1));
            }
            _btnClose = Find<Button>("ButtonClose");
            _btnClose.onClick.AddListener(OnCloseBtnClikc);
        }
   
        public void RefreshView()
        {
            for (int i = 0; i < transMovie.Length; i++)
            {
                transMovie[i].gameObject.SetActive(false);
            }
            int index = (_guideMovie.action.LevelImage - 1);
            if (_guideMovie.action.LevelImage != 0 && transMovie[index] != null)
            {
                transMovie[index].gameObject.SetActive(true);
            }
        }
  
     
      
        #endregion
    }
}

