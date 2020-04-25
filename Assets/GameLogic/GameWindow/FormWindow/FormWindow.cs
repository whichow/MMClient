using Game.DataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    partial class FormWindow : KUIWindow
    {
        private Toggle[] _toggles;
        public int _formType { get; private set; }
        public FormWindow() :
            base(UILayer.kNormal, UIMode.kSequenceHide)
        {
            uiPath = "FormWindow";
            uiAnim = UIAnim.kAnim1;
            hasMask = true;
        }

        public override void Awake()
        {
            _toggles = new Toggle[4];
            for (int i = 0; i < 4; i++)
                _toggles[i] = Find<Toggle>("Right/ToggleGroup/Tog" + (i + 1));
            foreach (Toggle tog in _toggles)
                tog.onValueChanged.AddListener((bool blSelect) => { if (blSelect) OnFormTypeChange(tog); });
            _formType = FormTypeConst.Head;
            InitView();
        }

        private void OnFormTypeChange(Toggle tog)
        {
            switch (tog.name)
            {
                case "Tog1":
                    _formType = FormTypeConst.Head;
                    break;
                case "Tog2":
                    _formType = FormTypeConst.Clothes;
                    break;
                case "Tog3":
                    _formType = FormTypeConst.Trousers;
                    break;
                case "Tog4":
                    _formType = FormTypeConst.Shoes;
                    break;
            }
            RefreshView(_formType);
        }

        public override void OnEnable()
        {
            _unitRenderTexture = UnitRenderTexture.Get(_rawImage);
            _unitRenderTexture.SetPlayer(PlayerDataModel.Instance.mPlayerData.mPlayerID);
            _avatar = _unitRenderTexture.GetPlayerAvatar();
            if (_curForm != null)
                _curForm.Clear();
            _curForm = new Dictionary<int, ItemXDM>();
            for (int i = 0; i < SpaceDataModel.Instance.mLstFashionId.Count; i++)
            {
                ItemXDM itemXDM = XTable.ItemXTable.GetByID(SpaceDataModel.Instance.mLstFashionId[i]);
                _curForm.Add(itemXDM.EquipType, itemXDM);
            }
            OnFormTypeChange(_toggles[_formType - 1]);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            _unitRenderTexture.Dispose();
        }

        private void OnBackBtnClick()
        {
            CloseWindow(this);
        }
    }
}
