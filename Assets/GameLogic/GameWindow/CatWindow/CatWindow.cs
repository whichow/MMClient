using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    partial class CatWindow : KUIWindow
    {
        private Button _closeBtn;
        private Toggle[] _toggles;
        public int _catType { get; private set; }

        public CatWindow() :
            base(UILayer.kNormal, UIMode.kSequenceHide)
        {
            uiPath = "CatWindow";
            uiAnim = UIAnim.kAnim1;
            hasMask = true;
        }

        public override void Awake()
        {
            _closeBtn = Find<Button>("Close");
            _closeBtn.onClick.AddListener(OnCloseBtnClick);
            _toggles = new Toggle[5];
            for (int i = 0; i < 5; i++)
                _toggles[i] = Find<Toggle>("ToggleGroup/Tog" + (i + 1));
            foreach (Toggle tog in _toggles)
                tog.onValueChanged.AddListener((bool blSelect) => { if (blSelect) OnBagTypeChange(tog); });
            _catType = CatRarityConst.All;
            InitView();
        }

        private void OnBagTypeChange(Toggle tog)
        {
            switch (tog.name)
            {
                case "Tog1":
                    _catType = CatRarityConst.All;
                    break;
                case "Tog5":
                    _catType = CatRarityConst.SSR;
                    break;
                case "Tog4":
                    _catType = CatRarityConst.SR;
                    break;
                case "Tog3":
                    _catType = CatRarityConst.R;
                    break;
                case "Tog2":
                    _catType = CatRarityConst.N;
                    break;
            }
            RefreshView(_catType);
        }

        public void OnCloseBtnClick()
        {
            CloseWindow(this);
        }

        public override void OnEnable()
        {
            OnBagTypeChange(_toggles[_catType]);
        }
    }
}
