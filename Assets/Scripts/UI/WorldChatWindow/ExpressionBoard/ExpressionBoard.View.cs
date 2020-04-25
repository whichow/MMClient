
/** 
* 作用：世界聊天的界面人物头像弹出框的控制
* 作者：wsy
*/
using Game.Build;
using Msg.ClientMessage;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class ExpressionBoard
    {
        #region Field
        private Button _blackBack;

        private List<Toggle> _lstTgl_channel;
        private Toggle _tgl_system;
        private Toggle _tgl_world;
        private Toggle _tgl_nearby;
        private Dictionary<Toggle, PageType> _dict_tglAndPgtp = new Dictionary<Toggle, PageType>();

        private KUIGrid _kgrd_commonExpressoin;
        private KUIGrid _kgrd_specialExpression;
        #endregion

        #region Method
        public void InitView()
        {
            _blackBack = Find<Button>("BackGround");
            _blackBack.onClick.AddListener(CloseUI);

            //            GameCamera.Instance.AddBreakAndResponseClickGameObjects(_blackBack.gameObject);
            ToggleGroup _tglGrp = Find<ToggleGroup>("Panel/Tab View/ToggleGroup");
            _lstTgl_channel = new List<Toggle>(_tglGrp.GetComponentsInChildren<Toggle>());
            for (int i = 0; i < _lstTgl_channel.Count; i++)
            {
                _lstTgl_channel[i].onValueChanged.AddListener(this.OnPageChange);
            }
            _tgl_system = Find<Toggle>("Panel/Tab View/ToggleGroup/Toggle_system");
            _tgl_world = Find<Toggle>("Panel/Tab View/ToggleGroup/Toggle_world");
            _tgl_nearby = Find<Toggle>("Panel/Tab View/ToggleGroup/Toggle_nearby");
            _dict_tglAndPgtp.Add(_tgl_system, PageType.kCommonExpression);
            _dict_tglAndPgtp.Add(_tgl_world, PageType.kSpecialExpression);
            _dict_tglAndPgtp.Add(_tgl_nearby, PageType.kDIY);

            _kgrd_commonExpressoin = Find<KUIGrid>("Scroll View_common");
            if (_kgrd_commonExpressoin)
            {
                _kgrd_commonExpressoin.uiPool.itemTemplate.AddComponent<CommonExpressionItem>();
            }
            _kgrd_specialExpression = Find<KUIGrid>("Scroll View_special");
            if (_kgrd_specialExpression)
            {
                _kgrd_specialExpression.uiPool.itemTemplate.AddComponent<SpecialExpressionItem>();
            }
        }
        /// <summary>
        /// View层入口
        /// </summary>
        public void RefreshView()
        {
            //显隐相关项
            _kgrd_commonExpressoin.ClearItems();
            _kgrd_commonExpressoin.gameObject.SetActive(CurrentPageType == PageType.kCommonExpression);
            _kgrd_specialExpression.ClearItems();
            _kgrd_specialExpression.gameObject.SetActive(CurrentPageType == PageType.kSpecialExpression);
            switch (CurrentPageType)
            {
                case PageType.kCommonExpression:
                    RefreshCommonExpressionView();
                    break;
                case PageType.kSpecialExpression:
                    RefreshSpecialExpressionView();
                    break;
                case PageType.kDIY:
                    RefreshDIYExpressionView();
                    break;
            }
        }
        /// <summary>
        /// 展示通用表情
        /// </summary>
        private void RefreshCommonExpressionView()
        {
            List<KEmojiManager.EmojiInfo> lst_allCmnExps = GetCommonExpression();
            CreateCommonExpression(lst_allCmnExps);
        }
        private void CreateCommonExpression(List<KEmojiManager.EmojiInfo> value)
        {
            KEmojiManager.EmojiInfo[] datas = value.ToArray();
            _kgrd_commonExpressoin.uiPool.SetItemDatas(datas);
        }
        /// <summary>
        /// 展示特殊表情
        /// </summary>
        private void RefreshSpecialExpressionView()
        {
            List<Sprite> lst_allCmnExps = GetSpecialExpression();
            CreateSpecialExpression(lst_allCmnExps);
        }
        private void CreateSpecialExpression(List<Sprite> value)
        {
            Sprite[] datas = value.ToArray();
            _kgrd_specialExpression.uiPool.SetItemDatas(datas);
        }
        /// <summary>
        /// 展示自制的表情
        /// </summary>
        private void RefreshDIYExpressionView()
        {

        }
        #endregion
    }
}









