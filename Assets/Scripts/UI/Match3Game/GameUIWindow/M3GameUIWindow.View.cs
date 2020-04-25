using Game.DataModel;
using Game.Match3;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class M3GameUIWindow
    {


        private Transform signTransform;
        private KUIList targetParentList;
        private Button backBtn;
        private Text stepText;
        private Text levelText;
        private GameObject flyEffectRoot;
        public GameObject bonuseStartObj;
        public RectTransform bonusPos;
        public RectTransform canvasRect;
        public Text scoreText;
        public Text timerText;
        public GameObject stepBoard;
        public GameObject TimerBoard;
        public GameObject collectBoard;
        public GameObject scoreBoard;
        public Text targetScoreText;
        public Dictionary<int, M3TargetItem> itemDic;
        public List<KUIImage> starObjList;
        public Canvas canvas;
        private float barLength;
        private Image scoreBar;
        private Image energyBar;
        private Image skillIcon;
        private GameObject skillObj;
        private Button skillBtn;
        private KUIItemPool layoutElementPool;
        private Image highLightMask = null;
        private GameObject gameRefreshTip;

        public void InitView()
        {
            InitMainView();
            InitPropView();
            RefreshView();
        }

        private void InitMainView()
        {
            starObjList = new List<KUIImage>();
            targetParentList = transform.Find("GameTargetWindow/targetParent").GetComponent<KUIList>();
            stepText = transform.Find("GameTargetWindow/AddBack/Text").GetComponent<Text>();
            flyEffectRoot = transform.Find("FlyEffect").gameObject;
            levelText = transform.Find("GameTargetWindow/levelText").GetComponent<Text>();
            backBtn = transform.Find("RT/Button").GetComponent<Button>();
            backBtn.onClick.AddListener(OnBackBtnClick);
            signTransform = transform.Find("Center/effect_1_pos").transform;
            collectBoard = transform.Find("GameTargetWindow/targetParent").gameObject;
            scoreBoard = transform.Find("GameTargetWindow/scoreParent").gameObject;
            targetScoreText = scoreBoard.transform.Find("score").GetComponent<Text>();

            bonuseStartObj = transform.Find("Center/bonusStartEffectPos").gameObject;
            bonusPos = transform.Find("Center/bonusPoint").GetComponent<RectTransform>();
            scoreText = transform.Find("GameTargetWindow/PrograssBarBack/score").GetComponent<Text>();
            stepBoard = transform.Find("GameTargetWindow/AddBack").gameObject;
            TimerBoard = transform.Find("GameTargetWindow/TimerBoard").gameObject;
            timerText = TimerBoard.transform.Find("Text").GetComponent<Text>();
            scoreBar = transform.Find("GameTargetWindow/PrograssBarBack/PrograssBar").GetComponent<Image>();
            barLength = scoreBar.rectTransform.sizeDelta.x;
            skillObj = transform.Find("LB/catSkillBtn").gameObject;
            skillBtn = transform.Find("LB/catSkillBtn/Button").GetComponent<Button>();
            energyBar = transform.Find("LB/catSkillBtn/bar").GetComponent<Image>();
            gameRefreshTip = Find("GameNone").gameObject;
            skillIcon = transform.Find("LB/catSkillBtn/Button").GetComponent<Image>();
            skillBtn.onClick.AddListener(OnSkillBtnClick);
            for (int i = 0; i < 3; i++)
            {
                starObjList.Add(scoreBar.transform.parent.Find("Star_" + (i + 1).ToString()).GetComponent<KUIImage>());
            }
            canvas = KUIRoot.Instance.GetComponent<Canvas>();
            canvasRect = canvas.GetComponent<RectTransform>();
        }

        public void InitPropView()
        {
            layoutElementPool = transform.Find("R/GamePropWindow/ScrollRect").GetComponent<KUIItemPool>();
            highLightMask = transform.Find("highLight").GetComponent<Image>();

            if (layoutElementPool && layoutElementPool.elementTemplate)
            {
                layoutElementPool.elementTemplate.gameObject.AddComponent<GamePropItem>();
            }
            highLightMask.GetComponent<Canvas>().sortingOrder = stepText.canvas.sortingOrder + maskSort;
            highLightMask.gameObject.SetActive(false);
        }

        public void RefreshView()
        {
            RefreshMainView();
            RefreshPropView();
        }
        private void RefreshMainView()
        {
            InitGameMode();
            InitGameTarget();
            InitScoreBar();
            InitSkill();
        }

        private void InitSkill()
        {
            if (M3GameManager.Instance.catManager.GameCat != null)
            {
                skillObj.SetActive(true);
                skillIcon.overrideSprite = M3GameManager.Instance.catManager.GameCat.GetSkillSprite();
            }
            else
            {
                skillObj.SetActive(false);
            }
        }

        private void RefreshPropView()
        {
            layoutElementPool.Clear();
            if (LevelDataModel.Instance.CurrLevel != null)
            {
                var propsID = LevelDataModel.Instance.CurrLevel.BattleProp;
                for (int i = 0; i < LevelDataModel.Instance.CurrLevel.BattleProp.Count; i++)
                {
                    var element = layoutElementPool.SpawnElement();
                    var propItem = element.GetComponent<GamePropItem>();
                    propItem.ShowProp(propList[i], propShopIDList[i]);
                }
            }
        }



        private void InitGameTarget()
        {
            var target = modeManager.target;


            if (target == GameTargetEnum.Collection)
            {
                collectBoard.SetActive(true);
                scoreBoard.SetActive(false);
                M3TargetItem item = null;
                int index = 0;
                var dic = mapData.LevelTaskElementDic;
                foreach (var t in dic)
                {
                    var tmp = targetParentList.GetItem(index);
                    item = (M3TargetItem)tmp;
                    itemDic.Add(t.Key, item);
                    item.UpdateData(XTable.ElementXTable.GetByID(t.Key).Icon, t.Value);
                    index++;
                }
                M3GameEvent.AddEvent(M3FightEnum.EliminateElement, OnTargetAdd);
            }
            else if (target == GameTargetEnum.Score)
            {
                collectBoard.SetActive(false);
                scoreBoard.SetActive(true);
                targetScoreText.text = modeManager.GetScore().ToString();
            }
            if (!M3Config.isEditor)
                levelText.text = LevelDataModel.Instance.CurrLevel.Name;

        }



        public void PlayBonus()
        {
            PlaySignEffect(bonusEffectName);
        }
        private void InitGameMode()
        {
            var mode = modeManager.mode;
            if (mode == GameModeEnum.StepMode)
            {
                stepBoard.SetActive(true);
                TimerBoard.SetActive(false);
                stepText.text = M3GameManager.Instance.modeManager.GameModeCtrl.totalSetps.ToString();

                M3GameEvent.AddEvent(M3FightEnum.DoStep, OnDoStep);

            }
            else if (mode == GameModeEnum.TimeMode)
            {
                stepBoard.SetActive(false);
                TimerBoard.SetActive(true);
                timerText.text = M3Supporter.Instance.TransformTimer(mapData.time);
                M3GameEvent.AddEvent(M3FightEnum.DoTimer, OnDoTimer);
            }
        }
        private void UpdateScore(object[] args)
        {
            int length = args.Length;
            if (length > 0)
            {
                scoreText.text = args[0].ToString();
                scoreBar.fillAmount = (int)args[0] / (float)modeManager.starScore[2];
            }
        }


        private void PlaySignEffect(object[] args)
        {
            int combCount = (int)args[0];
            if (combCount >= 0 && combCount <= 4)
            {
                PlaySignEffect(impossibleEffectName);
            }
            else if (combCount >= 5 && combCount <= 6)
            {
                PlaySignEffect(coolEffectName);

            }
            else if (combCount >= 7 && combCount <= 8)
            {
                PlaySignEffect(wonderfulEffectName);

            }
            else if (combCount >= 9 && combCount <= 10)
            {
                PlaySignEffect(fantasicEffectName);

            }
            else if (combCount >= 11)
            {
                PlaySignEffect(impossibleEffectName);

            }
        }

        public void PlaySignEffect(string name)
        {
            var obj = M3FxManager.Instance.LoadM3Prefab(name, signTransform.gameObject);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            GameObject.Destroy(obj, 2);
        }

        private void AddEnergy(object[] args)
        {
            if (args.Length > 0)
            {
                float rate = (float)args[0];
                KTweenUtils.DoImageFillAmount(energyBar, rate, 0.5f);
            }
        }
        private void OnTargetAdd(object[] args)
        {
            int id = (int)args[0];
            int num = (int)args[1];
            Vector3 pos = (Vector3)args[2];
            Element ele = (Element)args[3];
            if (itemDic.ContainsKey(id))
            {
                PlayTargetFly(M3Supporter.Instance.WordToScenePoint(pos, canvasRect, canvas),
                    itemDic[id].transform.position, itemDic[id].iconSprite, delegate ()
                    {

                        itemDic[id].ReduceNum(num);
                    });
            }
        }
        private void PlayTargetFly(Vector3 fromPos, Vector3 targetPos, Sprite icon, Action action)
        {
            GameObject obj;
            if (KAssetManager.Instance.TryGetMatchPrefab("M3UIElementEffect", out obj))
            {
                obj = KPool.Spawn(obj, fromPos, Vector3.zero, Vector3.one, flyEffectRoot.transform);
                obj.GetComponent<Image>().overrideSprite = icon;
            }
            if (obj != null)
            {
                KTweenUtils.MoveTo(obj.transform, targetPos, 0.6f, delegate
                {
                    if (action != null)
                        action();
                    KPool.Despawn(obj);
                });
                KTweenUtils.ScaleTo(obj.transform, new Vector3(0.6f, 0.6f, 0), 0.6f);
            }
        }
        public void PlayBottleLiziEffect(Vector3 fromPos, float speed, Action action = null)
        {
            fromPos = M3Supporter.Instance.WordToScenePoint(fromPos, canvasRect, canvas);

            GameObject obj = M3FxManager.Instance.PlayM3CommonEffect((int)MatchEffectType.EnergyLizi, flyEffectRoot, fromPos);

            TransformUtils.SetLayer(obj, "UI");
            if (obj != null)
            {

                //    Vector3 from = fromPos;
                //    Vector3 to = skillBtn.transform.position;
                //    Debug.Log(from);
                //    Debug.Log(to);
                //    Vector3 vec1 = new Vector3(to.y, to.x, to.z);
                //    Vector3 vec2 = new Vector3(from.x, from.y, from.z);
                //    Vector3[] vecs = new Vector3[] {
                //    from,
                //    vec1,
                //    vec2,
                //    to,
                //};
                //    BezierManager.PutPath(obj.transform, vecs, 0.05f, 0.6f, delegate ()
                //    {
                //        if (action != null)
                //            action();

                //        var go = M3FxManager.Instance.PlayM3CommonEffect((int)MatchEffectType.EnergyQuan, skillBtn.gameObject, new Vector3(0, 0, -1));
                //        TransformUtils.SetLayer(go, "UI");

                //        GameObject.Destroy(obj);
                //    });
                KTweenUtils.MoveTo(obj.transform, skillBtn.transform.position, 1f, delegate
                {
                    if (action != null)
                        action();

                    var go = M3FxManager.Instance.PlayM3CommonEffect((int)MatchEffectType.EnergyQuan, skillBtn.gameObject, new Vector3(0, 0, -1));
                    TransformUtils.SetLayer(go, "UI");

                    GameObject.Destroy(obj);
                });

            }

        }
        private void OnSkillBtnClick()
        {
            //M3GameEvent.DispatchEvent(M3FightEnum.OnClickSkill);
            //M3GameManager.Instance.catManager.OnTryToUseSkill(null);

            KCat cat = M3GameManager.Instance.catManager.GameCat;
            if (cat != null && cat.skillId > 0)
            {
                OpenWindow<SkillInfoWindow>(cat.skillId);
            }
        }

        private void InitScoreBar()
        {
            if (modeManager.starScore == null)
                return;
            for (int i = 0; i < modeManager.starScore.Length; i++)
            {
                if (modeManager.starScore[i] == 0)
                    return;
            }
            float s1 = (float)modeManager.starScore[0] / modeManager.starScore[2];
            float s2 = (float)modeManager.starScore[1] / modeManager.starScore[2];

            for (int i = 0; i < starObjList.Count; i++)
            {
                starObjList[i].ShowGray(true);
                starObjList[i].transform.GetChild(0).gameObject.SetActive(false);
            }

            starObjList[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(s1 * barLength, starObjList[0].GetComponent<RectTransform>().anchoredPosition.y);
            starObjList[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(s2 * barLength, starObjList[1].GetComponent<RectTransform>().anchoredPosition.y);
            starObjList[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(barLength, starObjList[2].GetComponent<RectTransform>().anchoredPosition.y);
            scoreBar.fillAmount = (float)modeManager.score / modeManager.starScore[2];
            scoreText.text = modeManager.score.ToString();
        }

        public void ShowStarGet(int star)
        {
            starObjList[star - 1].ShowGray(false);
            starObjList[star - 1].transform.GetChild(0).gameObject.SetActive(true);
            SkeletonGraphic sg = starObjList[star - 1].transform.GetChild(0).GetComponent<SkeletonGraphic>();
            if (sg != null)
            {
                starObjList[star - 1].transform.GetChild(0).GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "idle", false);//.AnimationName = "idle";
                starObjList[star - 1].transform.GetChild(0).GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "animation", false);//AnimationName = "animation";
            }
        }

        public void ShowGameRefreshTips(bool value)
        {
            gameRefreshTip.SetActive(value);
        }
    }
}