/** 
*FileName:     #SCRIPTFULLNAME# 
*Author:       #AUTHOR# 
*Version:      #VERSION# 
*UnityVersion：#UNITYVERSION#
*Date:         #DATE# 
*Description:    
*History: 
*/
using Game.Match3;
using System;

namespace Game.UI
{
    public partial class M3GameUIWindow : KUIWindow
    {
        public int skillBtnSort = 30;

        public int maskSort = 50;



        public M3GameUIWindow() :
                base(UILayer.kBackground, UIMode.kSequenceHide)
        {
            uiPath = "GameUIWindow";
        }

        public override void Awake()
        {
            base.Awake();

            InitModel();
            InitView();


        }

        public override void OnEnable()
        {
            base.OnEnable();


        }

        private void OnDoStep(object[] args)
        {
            stepText.text = args[0].ToString();
        }
        private void OnDoTimer(object[] args)
        {
            timerText.text = args[0].ToString();
        }
        private void OnBackBtnClick()
        {
            if (!M3GameManager.Instance.modeManager.IsStepModeLevelEnd())
            {
                if (GuideManager.Instance == null || !GuideManager.Instance.IsGuideing)
                    KUIWindow.OpenWindow<GamePauseWindow>();
            }
        }
        public override void OnDisable()
        {
            base.OnDisable();

        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            M3GameEvent.RemoveEvent(M3FightEnum.Comb, PlaySignEffect);
            M3GameEvent.RemoveEvent(M3FightEnum.UpdateScore, UpdateScore);
            M3GameEvent.RemoveEvent(M3FightEnum.OnAddEnergy, AddEnergy);
        }
        public void SetPropHightLightMask(bool value, Action action)
        {

            if (value)
            {
                //M3GameManager.Instance.propManager.current.SetHightLight(true);
                //highLightMask.gameObject.SetActive(value);
                //KTweenUtils.DoImageFade(highLightMask, 0, 0.7f, 0.5f, action);
            }
            else
            {
                //M3GameManager.Instance.propManager.current.SetHightLight(false);
                //KTweenUtils.DoImageFade(highLightMask, 0.7f, 0, 0.5f, delegate () { if (action != null) action(); highLightMask.gameObject.SetActive(value); });

            }
        }

        public void SetSkillHightLightMask(bool value, Action action)
        {
            if (value)
            {

                highLightMask.gameObject.SetActive(value);
                KTweenUtils.DoImageFade(highLightMask, 0, 0.7f, 0, action);
            }
            else
            {
                KTweenUtils.DoImageFade(highLightMask, 0.7f, 0, 0,
                    delegate () { if (action != null) action(); highLightMask.gameObject.SetActive(value); });

            }
        }
    }
}
