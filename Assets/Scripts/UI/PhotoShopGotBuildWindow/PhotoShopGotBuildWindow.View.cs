using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Game.UI
{
    public partial class PhotoShopGotBuildWindow
    {
        private Button _buttonBack;
        private Button _buttonConfirm;
        private Button _buttonAgain;

        private Button _buttonFacebook;
        private Button _buttonWechat;
        private Button _buttonQQ;

        private KUIImage _rarityImage;

        private Text _blueBackText;
        private Text _purpleBackText;
        private Text _diamondBackText;

        private Transform _modelParent;
        private Text _textCatName;
        private Text _textCatColor;
        private Text _textCatCoin;
        private Text _textCatExplore;
        private Text _textCatMatch;
        private GameObject _goCatInfor;
        private GameObject _goBuildInfor;
        private Text _textBuildName;
        private KCat cat;
        private Transform _modelBuildParent;
        private Transform _modelBuildParent1;
        private Text _textBuildCharm;
        private Text _textBuildSuit;
        private Transform _transSuit;
   
        //Fx
        private SkeletonAnimation _animationLevel;
        private SkeletonAnimation[] _animations;
        private Transform _transFx;

        public void InitView()
        {
            _buttonBack = Find<Button>("ButtonBack");
            _buttonBack.onClick.AddListener(this.OnCloseBtnClick);

            _buttonConfirm = Find<Button>("ButtonConfirm");
            _buttonConfirm.onClick.AddListener(this.OnCloseBtnClick);

            _buttonAgain = Find<Button>("ButtonAgain");
            _buttonAgain.onClick.AddListener(this.OnAgainBtnClick);

            _buttonFacebook = Find<Button>("Back/Fback/Button");
            _buttonFacebook.onClick.AddListener(this.OnFaceBookBtnClick);

            _buttonWechat = Find<Button>("Back/WxBack/Button");
            _buttonWechat.onClick.AddListener(this.OnWeChatBtnClick);

            _buttonQQ = Find<Button>("Back/QqBack/Button");
            _buttonQQ.onClick.AddListener(this.OnQQBtnClick);

            _blueBackText = Find<Text>("Back/BlueBack/Text");
            _purpleBackText = Find<Text>("Back/PurpleBack/Text");
            _diamondBackText = Find<Text>("Back/DiamondBack/Text");
            _modelBuildParent = Find<Transform>("BuildModelParent");
            _modelParent = Find<Transform>("Model");
            _textCatName = Find<Text>("Back/InfoBack/InfoBackAdd/CatInfor/CatName");
            _textCatColor = Find<Text>("Back/InfoBack/InfoBackAdd/CatInfor/Color");
            _textBuildName = Find<Text>("Back/InfoBack/InfoBackAdd/BuildInfor/Name");
            _textCatCoin = Find<Text>("Back/InfoBack/InfoBackAdd/CatInfor/Property/TextCreatGold");
            _textCatExplore = Find<Text>("Back/InfoBack/InfoBackAdd/CatInfor/Property/TexExplore");
            _textCatMatch = Find<Text>("Back/InfoBack/InfoBackAdd/CatInfor/Property/TextFoster");
            _goCatInfor = transform.Find("Back/InfoBack/InfoBackAdd/CatInfor").gameObject;
            _goBuildInfor = transform.Find("Back/InfoBack/InfoBackAdd/BuildInfor").gameObject;
            _rarityImage = Find<KUIImage>("Back/InfoBack/InfoBackAdd/Level");
            _textBuildCharm = Find<Text>("Back/InfoBack/InfoBackAdd/BuildInfor/InfoBase01/Property/TextCreatGold");
            _textBuildSuit = Find<Text>("Back/InfoBack/InfoBackAdd/BuildInfor/InfoBase01 (1)/TextCreatGold");
            _transSuit = Find<Transform>("Back/InfoBack/InfoBackAdd/BuildInfor/InfoBase01 (1)");
            _modelBuildParent1 = Find<Transform>("BuidModelParent1");
            //Fx
            _animationLevel = Find<SkeletonAnimation>("Back/InfoBack/InfoBackAdd/Level/Fx_Card_02_level");
            _transFx = Find<Transform>("Fx");
            _animations = new SkeletonAnimation[_transFx.childCount];
            for (int i = 0; i < _animations.Length; i++)
            {
                _animations[i] = Find<SkeletonAnimation>("Fx/Fx_"+ (i + 1));
            }

        }

        public override void OnDisable()
        {
            if (_modelParent.childCount > 0)
            {
                Object.Destroy(_modelParent.GetChild(0).gameObject);
            }
            if (_modelBuildParent.childCount > 0)
            {
                Object.Destroy(_modelBuildParent.GetChild(0).gameObject);
            }
            if (_modelBuildParent1.childCount>0)
            {
                Object.Destroy(_modelBuildParent1.GetChild(0).gameObject);
            }
        }
  
        public void RefreshView()
        {
        
            //_modelParent.localScale = new Vector3(55, 55, 40);
            _blueBackText.text = GetBlueBlack();
            _purpleBackText.text = GetPurpoleBlack();
            _diamondBackText.text = GetDiamBlack();
            if (_modelParent.childCount > 0)
            {
                Object.Destroy(_modelParent.GetChild(0).gameObject);
            }
            if (_photoShopCardData.type==1)
            {
                _buttonConfirm.gameObject.SetActive(false);
                _buttonAgain.gameObject.SetActive(false);
                _blueBackText.transform.parent.gameObject.SetActive(false);
                _purpleBackText.transform.parent.gameObject.SetActive(false);
                _diamondBackText.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                _buttonConfirm.gameObject.SetActive(true);
                _buttonAgain.gameObject.SetActive(true);
                _blueBackText.transform.parent.gameObject.SetActive(true);
                _purpleBackText.transform.parent.gameObject.SetActive(true);
                _diamondBackText.transform.parent.gameObject.SetActive(true);
            }
            for (int i = 0; i < _animations.Length; i++)
            {
                _animations[i].gameObject.SetActive(false);
            }
            if (_modelBuildParent.childCount > 0)
            {
                Object.Destroy(_modelBuildParent.GetChild(0).gameObject);
            }
            if (_modelBuildParent1.childCount > 0)
            {
                Object.Destroy(_modelBuildParent1.GetChild(0).gameObject);
            }
            _animationLevel.AnimationName = null;
            if (cat != null)
            {
               
                if (cat.rarity==2)
                {
                    StartCoroutine(Animtion(3));
                    //_animationLevel.Reset();
                    _animationLevel.GetComponent<Renderer>().sortingOrder = _textCatMatch.canvas.sortingOrder + 1;
                    _animationLevel.loop = false;
                    _animationLevel.AnimationName = "tex_r";
                }
                else if (cat.rarity == 3)
                {
                    StartCoroutine(Animtion(1));
                    //_animationLevel.Reset();
                    _animationLevel.GetComponent<Renderer>().sortingOrder = _textCatMatch.canvas.sortingOrder + 1;
                    _animationLevel.loop = false;
                    _animationLevel.AnimationName = "tex_sr";
                }
                else if (cat.rarity == 4)
                {
                    StartCoroutine(Animtion(2));
                    //_animationLevel.Reset();
                    _animationLevel.GetComponent<Renderer>().sortingOrder = _textCatMatch.canvas.sortingOrder + 1;
                    _animationLevel.loop = false;
                    _animationLevel.AnimationName = "tex_ssr";
                }
            }
            else
            {
                if (kShopitem.rarity == 2)
                {
                    StartCoroutine(Animtion(3));
                    //_animationLevel.Reset();
                    _animationLevel.GetComponent<Renderer>().sortingOrder = _textCatMatch.canvas.sortingOrder + 1;
                    _animationLevel.loop = false;
                    _animationLevel.AnimationName = "tex_r";
                }
                else if (kShopitem.rarity == 3)
                {
                    StartCoroutine(Animtion(1));
                    //_animationLevel.Reset();
                    _animationLevel.GetComponent<Renderer>().sortingOrder = _textCatMatch.canvas.sortingOrder + 1;
                    _animationLevel.loop = false;
                    _animationLevel.AnimationName = "tex_sr";
                }
                else if (kShopitem.rarity == 4)
                {
                    StartCoroutine(Animtion(2));
                    //_animationLevel.Reset();
                    _animationLevel.GetComponent<Renderer>().sortingOrder = _textCatMatch.canvas.sortingOrder + 1;
                    _animationLevel.loop = false;
                    _animationLevel.AnimationName = "tex_ssr";
                }
            }
            if (cat != null)
            {
                _textCatColor.text = "颜色：" + cat.GetColorText();
                _textCatName.text = "昵称：" + cat.name;
                _textCatCoin.text = "产金：" + cat.initCoinAbility;
                _textCatExplore.text = "探索：" + cat.initExploreAbility;
                _textCatMatch.text = "三消：" + cat.initMatchAbility;
                _goCatInfor.SetActive(true);
                _goBuildInfor.SetActive(false);

                StartCoroutine(ShowCat());
            }
            else
            {
                //_modelBuildParent.localScale = new Vector3(100,100,100);
                if (kShopitem is KItemBuilding)
                {
                    KItemBuilding build = (KItemBuilding)kShopitem;
                    _textBuildName.text = build.itemName;
                    _textBuildCharm.text = "+" + build.charm;
                    if (build.suitID != 0)
                    {
                        _textBuildSuit.text = KItemManager.Instance.GetItem(build.suitID).itemName;
                        _transSuit.gameObject.SetActive(true);
                    }
                    else
                    {
                        _transSuit.gameObject.SetActive(false);
                    }
                    _goCatInfor.SetActive(false);
                    _goBuildInfor.SetActive(true);
                    var modelObj = build.model;
                    if (modelObj != string.Empty)
                    {
                        GameObject modelPrefab;
                        if (KAssetManager.Instance.TryGetBuildingPrefab(modelObj, out modelPrefab))
                        {
                            modelPrefab = Object.Instantiate(modelPrefab);
                            if (modelPrefab.transform.childCount > 0)
                            {
                                /* Vector3  vec=*/
                                modelPrefab.transform.GetChild(0).localScale = Vector3.one;
                                modelPrefab.transform.GetChild(0).localPosition = Vector3.zero;
                                //vec.x = Mathf.Max(0.001f, vec.x);
                                //vec.y = Mathf.Max(0.001f, vec.y);
                                //modelPrefab.transform.localScale = new Vector3(1f/vec.x,1f/vec.y,1f);
                            }
                            if (modelPrefab.GetComponentInChildren<SkeletonAnimation>()==null)
                            {
                                modelPrefab.transform.SetParent(_modelBuildParent, false);
                            }
                            else
                            {
                                modelPrefab.transform.SetParent(_modelBuildParent1, false);
                            }

                            TransformUtils.SetLayerSameWithParent(modelPrefab);
                            modelPrefab.GetComponentInChildren<Renderer>().sortingOrder = _textCatName.canvas.sortingOrder + 1;
                        }
                        else
                        {
                            Debug.Log("加载建筑失败" + modelObj);
                            return;
                        }
                    }
                }
            }
        }

        public IEnumerator ShowCat()
        {
            yield return null;

            var modelObj = cat.model;
            if (modelObj != string.Empty)
            {
                CatUtils.GetModel(cat.shopId, CallBalck);
            }
        }

        private void CallBalck(GameObject go)
        {
            if (go != null)
            {
                go.layer = _modelParent.gameObject.layer;
                go.transform.SetParent(_modelParent, false);
                go.GetComponent<Renderer>().sortingOrder = _textCatName.canvas.sortingOrder + 2;
            }
            else
            {
                Debug.Log("加载猫咪失败");
                return;
            }
        }
        private IEnumerator Animtion(int type)
        {
   
            if (type==1)
            {
                _animations[0].gameObject.SetActive(true);
                //_animations[0].Reset();
                _animations[0].GetComponent<Renderer>().sortingOrder = _textCatMatch.canvas.sortingOrder + 3;
                _animations[0].loop = false;
                _animations[0].AnimationName = null;
                _animations[0].AnimationName = "start";
                //yield return new WaitForSeconds(0.6f);
                _animations[2].gameObject.SetActive(true);
                //_animations[2].Reset();
                _animations[2].GetComponent<Renderer>().sortingOrder = _textCatMatch.canvas.sortingOrder + 3;
                _animations[2].loop = true;
                _animations[2].AnimationName = null;
                _animations[2].AnimationName = "lizi";
                _animations[3].gameObject.SetActive(true);
                //_animations[3].Reset();
                _animations[3].GetComponent<Renderer>().sortingOrder = _textCatMatch.canvas.sortingOrder +1;
                _animations[3].loop = true;
                _animations[3].AnimationName = null;
                _animations[3].AnimationName = "loop_SR";

            }
            else if (type==2)
            {
                _animations[5].gameObject.SetActive(true);
                //_animations[5].Reset();
                _animations[5].GetComponent<Renderer>().sortingOrder = _textCatMatch.canvas.sortingOrder + 3;
                _animations[5].loop = false;
                _animations[5].AnimationName = null;
                _animations[5].AnimationName = "start_zi";
                //yield return new WaitForSeconds(0.6f);
                _animations[1].gameObject.SetActive(true);
                //_animations[1].Reset();
                _animations[1].GetComponent<Renderer>().sortingOrder = _textCatMatch.canvas.sortingOrder + 3;
                _animations[1].loop = false;
                _animations[1].AnimationName = null;
                _animations[1].AnimationName = "zixian";
                //yield return new WaitForSeconds(0.6f);
                _animations[6].gameObject.SetActive(true);
                //_animations[6].Reset();
                _animations[6].GetComponent<Renderer>().sortingOrder = _textCatMatch.canvas.sortingOrder + 3;
                _animations[6].loop = true;
                _animations[6].AnimationName = null;
                _animations[6].AnimationName = "lizi_zi";
                _animations[4].gameObject.SetActive(true);
                //_animations[4].Reset();
                _animations[4].GetComponent<Renderer>().sortingOrder = _textCatMatch.canvas.sortingOrder+1;
                _animations[4].loop = true;
                _animations[4].AnimationName = null;
                _animations[4].AnimationName = "loop_SSR";
            }
            else if (type==3)
            {
                _animations[0].gameObject.SetActive(true);
                //_animations[0].Reset();
                _animations[0].GetComponent<Renderer>().sortingOrder = _textCatMatch.canvas.sortingOrder + 3;
                _animations[0].loop = false;
                _animations[0].AnimationName = null;
                _animations[0].AnimationName = "start";
                _animations[7].gameObject.SetActive(true);
                //_animations[4].Reset();
                _animations[7].GetComponent<Renderer>().sortingOrder = _textCatMatch.canvas.sortingOrder + 1;
                _animations[7].loop = true;
                _animations[7].AnimationName = null;
                _animations[7].AnimationName = "loop_R";
            }
            yield return null;
        }


    }



}
