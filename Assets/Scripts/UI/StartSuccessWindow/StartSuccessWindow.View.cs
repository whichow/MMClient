//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//namespace Game.UI
//{
//    public partial class StartSuccessWindow
//    {
//        private Button _btnBack;
//        private Button _btnOK;

//        private Image _imageCatIcon;
//        private Text _textOldGrad;
//        private Text _textNewGrad;
//        private Text _textOldCoin;
//        private Text _textNewCoin;
//        private Text _textOldExplore;
//        private Text _textNewExplore;
//        private Text _textOldMath;
//        private Text _textNewMath;
//        private KUIImage[] _starImages;
//        private KUIImage[] _newStarImages;
//        private KUIImage[] _nowstartImages;
//        public void InitView()
//        {
//            _btnBack = Find<Button>("BgBlack");
//            _btnBack.onClick.AddListener(OnBackBtnClick);
//            _btnOK = Find<Button>("BgBlack/ButtonOK");
//            _btnOK.onClick.AddListener(OnBackBtnClick);


//            _imageCatIcon = Find<Image>("BgBlack/Cat");
//            _textOldGrad = Find<Text>("BgBlack/Level/Front");
//            _textNewGrad = Find<Text>("BgBlack/Level/Back");
//            _textOldCoin = Find<Text>("BgBlack/AddCoin/Front");
//            _textNewCoin = Find<Text>("BgBlack/AddCoin/Back");
//            _textOldExplore = Find<Text>("BgBlack/AddExplore/Front");
//            _textNewExplore = Find<Text>("BgBlack/AddExplore/Back");
//            _textOldMath = Find<Text>("BgBlack/AddMath/Front");
//            _textNewMath = Find<Text>("BgBlack/AddMath/Back");



//            var fish = transform.Find("BgBlack/StarUp/Fish");
//            _starImages = new KUIImage[fish.childCount];
//            for (int i = 0; i < _starImages.Length; i++)
//            {
//                _starImages[i] = fish.GetChild(i).GetComponent<KUIImage>();
//            }
//            var newFish = transform.Find("BgBlack/StarUp/Fish1");
//            _newStarImages = new KUIImage[newFish.childCount];
//            for (int i = 0; i < _newStarImages.Length; i++)
//            {
//                _newStarImages[i] = newFish.GetChild(i).GetComponent<KUIImage>();
//            }
//            var nowFish = transform.Find("BgBlack/Ribbon/Fish");
//            _nowstartImages = new KUIImage[nowFish.childCount];
//            for (int i = 0; i < _nowstartImages.Length; i++)
//            {
//                _nowstartImages[i] = nowFish.GetChild(i).GetComponent<KUIImage>();
//            }

//        }



//        public void RefreshView()
//        {
//            _imageCatIcon.overrideSprite = GetCatIcon();
//            _textOldGrad.text = windowData.oldCatMaxGrad.ToString();
//            _textNewGrad.text = GetNewCatMaxGrad();
//            _textOldCoin.text = windowData.oldCatAddCoin.ToString();
//            _textNewCoin.text = GetNewCatAddCoin();
//            _textOldExplore.text = windowData.oldCatExplore.ToString();
//            _textNewExplore.text = GetNewCatAddExplore();
//            _textOldMath.text = windowData.oldCatMatch.ToString();
//            _textNewMath.text = GetNewCatAddMatch();
//            ShowOldStar(GetOldStart());
//            ShowNewStar(GetNewStart());

//        }

//        public void ShowOldStar(int star)
//        {
//            for (int i = 0; i < _starImages.Length; i++)
//            {
//                _starImages[i].ShowGray(i >= star);
//            }
//        }
//        public void ShowNewStar(int star)
//        {
//            for (int i = 0; i < _newStarImages.Length; i++)
//            {
//                _newStarImages[i].ShowGray(i >= star);
//            }
//            for (int i = 0; i < _nowstartImages.Length; i++)
//            {
//                _nowstartImages[i].ShowGray(i >= star); 
//            }
//        }

//    }
//}
