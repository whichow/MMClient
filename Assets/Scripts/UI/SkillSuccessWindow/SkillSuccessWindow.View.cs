//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//namespace Game.UI
//{
//    public partial class SkillSuccessWindow 
//    {
//        private Button _btnBack;
//        private Image _imageIcon;
//        private Text _textOldSkillLvl;
//        private Text _textNewSKilLvl;
//        private Text _textSkillName;


//        public void InitView()
//        {
//            _btnBack = Find<Button>("BgBlack");
//            _btnBack.onClick.AddListener(OnBackBtnClick);

//            _imageIcon = Find<Image>("BgBlack/SkillIcon");
//            _textOldSkillLvl = Find<Text>("BgBlack/ImageLevel/LVfront/Text");
//            _textNewSKilLvl = Find<Text>("BgBlack/ImageLevel/LVback/Text (1)");
//            _textSkillName = Find<Text>("BgBlack/SkillName");

//        }


//        public void RefreshView()
//        {
//            _imageIcon.overrideSprite = GetSkillIcon();
//            _textOldSkillLvl.text = GetOldSKillLvl();
//            _textNewSKilLvl.text = GetNewSkillLvl();
//            _textSkillName.text = GetSkillName();

//        }
//    }
//}
