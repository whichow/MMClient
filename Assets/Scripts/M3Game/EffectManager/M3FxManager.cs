using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    public class M3FxManager : Singleton<M3FxManager>
    {
        #region Field

        public GameObject crashFxParent;
        public GameObject scoreFxParent;
        public readonly string EffectConfigUrl = "MatchEffectConfig";

        private Dictionary<string, GameObject> effectCacheDic;

        #endregion

        #region Method

        public void Init()
        {
            TextAsset tex;
            KAssetManager.Instance.TryGetExcelAsset(EffectConfigUrl, out tex);
            M3Effect.LoadTable(tex.text.ToJsonTable());
            crashFxParent = GameObject.Find("GameScreen/Board/CrashFxParent");
            scoreFxParent = GameObject.Find("GameScreen/Board/ScoreFxParent");
            effectCacheDic = new Dictionary<string, GameObject>();
        }

        public void PlayAnimator(Animator animator, string currentName, Action callBack)
        {
            if (animator == null)
                return;
            animator.Play(currentName, -1, 0);
            float time = 0;
            var clips = animator.runtimeAnimatorController.animationClips;
            foreach (var item in clips)
            {
                if (item.name.Equals(currentName))
                {
                    time = item.length;
                }
            }
            if (time != 0)
                M3GameManager.Instance.StartCoroutine(AnimCoroutine(time, callBack));
        }

        IEnumerator AnimCoroutine(float time, Action callBack)
        {
            yield return new WaitForSeconds(time);
            if (callBack != null)
            {
                callBack();
            }
        }

        public void PlayerHammerEffect(float x, float y)
        {
            var tmpObj = PlayM3CommonEffect((int)MatchEffectType.Hammer, crashFxParent);
            tmpObj.transform.localPosition = new Vector3(x, y, 0);
            tmpObj.SetActive(true);
            GameObject.Destroy(tmpObj, M3EffectConfig.ItemBoomCrashDestroyTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public void PlayCashStarEffect(int x, int y, float delay = 0)
        {
            var tmpObj = PlayM3CommonEffect((int)MatchEffectType.CrashStar, crashFxParent);
            //tmpObj.GetComponent<SpriteRenderer>().sortingOrder = 999;
            tmpObj.transform.localPosition = new Vector3(x, y, 0);
            tmpObj.SetActive(true);
            GameObject.Destroy(tmpObj, M3EffectConfig.ItemBoomCrashDestroyTime);
        }

        public void PlayBottleCrashEffect(int x, int y, float delay = 0)
        {
            var tmpObj = PlayM3CommonEffect((int)MatchEffectType.BottleEffect1, crashFxParent);
            tmpObj.transform.localPosition = new Vector3(x, y, 0);
            tmpObj.SetActive(true);
            GameObject.Destroy(tmpObj, M3EffectConfig.ItemBoomCrashDestroyTime);
        }

        public void PlayFinishEffect(int x, int y, float delay = 0)
        {
            var tmpObj = PlayM3CommonEffect((int)MatchEffectType.FinishEffect, crashFxParent);
            tmpObj.transform.localPosition = new Vector3(y * M3Config.DistancePerUnit, -x * M3Config.DistancePerUnit, 0);
            tmpObj.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            tmpObj.SetActive(true);
        }

        public void PlayFinishQuan(float x, float y, float delay = 0)
        {
            var tmpObj = PlayM3CommonEffect((int)MatchEffectType.FinishQuanEffect, crashFxParent);
            tmpObj.transform.localPosition = new Vector3(x, -y, 0);
            tmpObj.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            tmpObj.SetActive(true);
        }

        public void PlayZombieLine(Vector3 vector3)
        {
            var tmpObj = PlayM3CommonEffect((int)MatchEffectType.ZombieLine, crashFxParent);
            tmpObj.transform.localPosition = vector3;
        }

        private IEnumerator CrashItemDelay(float time, MEventDelegate callBack)
        {
            yield return new WaitForSeconds(time);
            if (callBack != null)
                callBack();
        }

        public void Special(GameObject obj, ElementSpecial special)
        {
            if (special == ElementSpecial.Row)
            //KAssetManager.Instance.TryGetMatchPrefab("Flag_Row", out effectObj);
            {
                PlayM3CommonEffect((int)MatchEffectType.Flag_Row, obj);
            }
            else if (special == ElementSpecial.Column)
                PlayM3CommonEffect((int)MatchEffectType.Flag_Col, obj);
            else if (special == ElementSpecial.Area)
                PlayM3CommonEffect((int)MatchEffectType.Flag_Area, obj);
        }

        public GameObject PlayM3CommonEffect(int effectID, GameObject parent, float delayTime = 0)
        {
            var effect = M3Effect.GetEffectConfig(effectID);
            if (effect == null)
                Debug.Log("特效配置错误: ID " + effectID);
            GameObject effectObj = LoadM3Prefab(effect.modelName);
            if (effectObj == null)
            {
                Debug.Log("特效加载失败，ID: " + effectID);
                return null;
            }
            return PlayCommonEffect(effectObj, parent, Vector3.zero, effect.speed, effect.duration, delayTime, effect.skin, effect.animName);
        }

        public GameObject PlayM3CommonEffect(string effectName, GameObject parent, float speed, float destroyTime, float delayTime = 0)
        {
            GameObject effectObj = LoadM3Prefab(effectName);
            if (effectObj == null)
            {
                Debug.Log("特效加载失败，Name: " + effectName);
                return null;
            }
            return PlayCommonEffect(effectObj, parent, Vector3.zero, speed, destroyTime, delayTime); ;
        }

        public GameObject PlayM3CommonEffect(int effectID, GameObject parent, Vector3 pos, float delayTime = 0)
        {
            var effect = M3Effect.GetEffectConfig(effectID);
            if (effect == null)
                Debug.Log("特效配置错误: ID " + effectID);
            GameObject effectObj = LoadM3Prefab(effect.modelName);
            if (effectObj == null)
            {
                Debug.Log("特效加载失败，ID: " + effectID);
                return null;
            }
            return PlayCommonEffect(effectObj, parent, pos, effect.speed, effect.duration, delayTime, effect.skin, effect.animName);
        }

        private GameObject PlayCommonEffect(GameObject effectObj, GameObject parent, Vector3 pos, float speed, float destroyTime, float delayTime = 0, string skin = null, string animName = null)
        {
            var tmpObj = GameObject.Instantiate(effectObj);
            tmpObj.transform.SetParent(parent.transform, false);
            tmpObj.transform.localPosition = pos;
            Animator anim = tmpObj.GetComponentInChildren<Animator>();
            if (skin != null)
            {

            }
            if (anim != null)
            {
                if (animName != "null")
                {
                    anim.speed = speed;
                    anim.Play(animName, -1, 0);
                }
                else
                {
                    anim.speed = speed;
                }
            }
            if (destroyTime >= 0)
            {
                GameObject.Destroy(tmpObj, destroyTime);
            }
            return tmpObj;
        }

        public void PlayConveyorAnimation(Transform transform, Vector3 vector3, float conveyorTime, Action action)
        {
            KTweenUtils.LocalMoveTo(transform, vector3, conveyorTime, action);
        }

        public void PlayCoomJump(Transform transform, Vector3 to, Action action)
        {
            KTweenUtils.LocalMoveTo(transform, to, 0.4f, action);
        }

        public void PlayFishCollectAnimation(Transform transform, Action action = null)
        {
            KTweenUtils.LocalMoveTo(transform, new Vector3(transform.localPosition.x, transform.localPosition.y - 0.7f * M3Config.DistancePerUnit, transform.localPosition.z), M3Config.ElementDisapperTime);
            KTweenUtils.ScaleTo(transform, new Vector3(0.5f, 0.5f, 1), M3Config.ElementDisapperTime);
        }

        public void PlayFishAbsorbAnimation(Transform transform, Action action)
        {
            KTweenUtils.LocalMoveTo(transform, new Vector3(transform.localPosition.x, transform.localPosition.y + 0.4f * M3Config.DistancePerUnit, transform.localPosition.z), 0.2f);
            KTweenUtils.ScaleTo(transform, new Vector3(0.3f, 0.3f, 1), 0.2f, action);
        }

        public void PlayEnergyCornerEffect(int x, int y, Transform transform, Action action = null)
        {
            Vector3 pos = M3Supporter.Instance.GetItemPositionByGrid(x, y);

            GameObject obj = PlayM3CommonEffect((int)MatchEffectType.EnergyCornerFly, crashFxParent, Vector3.one);
            if (obj != null)
            //KTweenUtils.LocalMoveTo(obj.transform, pos, 0.3f, delegate ()
            //{
            //    if (action != null)
            //        action();
            //    PlayM3CommonEffect((int)MatchEffectType.Flag_Energy, transform.gameObject, new Vector3(0, 0, -0.5f));
            //    GameObject.Destroy(obj);
            //});
            {
                Vector3 from = new Vector3(-3, -7, 0);
                obj.transform.localPosition = from;
                Vector3 to = pos;
                Vector3 vec1 = new Vector3(to.y, to.x, 0);
                Vector3 vec2 = new Vector3(from.x, from.y, 0);
                Vector3[] vecs = new Vector3[] {
                from,
                vec1,
                vec2,
                to,
            };
                BezierManager.PutLocalPath(obj.transform, vecs, 0.05f, 0.6f, DG.Tweening.PathType.Linear, delegate ()
                {
                    if (action != null)
                        action();
                    PlayM3CommonEffect((int)MatchEffectType.Flag_Energy, transform.gameObject, new Vector3(0, 0, -0.5f));
                    GameObject.Destroy(obj);
                });
            }
        }

        public void PlayCatterySendAnimation(Transform transform, Vector3 start, Vector3 target, Action action)
        {
            Vector3 from = start;
            Vector3 to = target;
            Vector3 vec1 = new Vector3(0, 2.5f, -1.2f);
            Vector3 vec2 = new Vector3(from.x, 2.5f, -1.2f);

            Vector3[] vecs = new Vector3[] {
                from,
                vec1,
                vec2,
                to,
            };
            BezierManager.PutLocalPath(transform, vecs, 0.05f, 0.8f, DG.Tweening.PathType.Linear, delegate ()
            {
                if (action != null)
                    action();
            });
        }

        public void PlayJumpAnimation(Transform transform, Vector3 start, Vector3 target, float time, Action action)
        {
            Vector3 from = start;
            Vector3 to = target + new Vector3(0, 0, -1.2f);
            Vector3 vec1 = new Vector3(0, 2.5f, -1.2f);
            Vector3 vec2 = new Vector3(from.x, 2.5f, -1.2f);

            Vector3[] vecs = new Vector3[] {
                from,
                vec1,
                vec2,
                to,
            };
            BezierManager.PutLocalPath(transform, vecs, 0.05f, time, DG.Tweening.PathType.Linear, delegate ()
            {
                if (action != null)
                    action();
            });
        }

        public void PlayRollAnimation(Transform transform, Vector3 []target, float time, Action action)
        {
            KTweenUtils.DOPath(transform, target, time, DG.Tweening.PathType.Linear, delegate ()
            {
                action?.Invoke();
            });
        }

        public void PlaySuperTimeEffect(string effectFlag, bool isActive)
        {
            if (isActive)
            {
                if (!effectCacheDic.ContainsKey(effectFlag))
                {
                    GameObject obj = PlayM3CommonEffect((int)MatchEffectType.TiQingJiaEffect, M3GameManager.Instance.gameScreen, new Vector3(0, 0, -5));
                    effectCacheDic.Add(effectFlag, obj);
                }
            }
            else
            {
                if (effectCacheDic.ContainsKey(effectFlag))
                {
                    GameObject.Destroy(effectCacheDic[effectFlag]);
                    effectCacheDic.Remove(effectFlag);
                }
            }
        }

        public GameObject PlayMoveHint(M3DirectionType type, Int2 pos1, Int2 pos2)
        {
            var obj = PlayM3CommonEffect((int)MatchEffectType.MoveHint, crashFxParent);
            switch (type)
            {
                case M3DirectionType.None:
                    break;
                case M3DirectionType.Row:
                    obj.transform.localPosition = new Vector3((pos1.y + pos2.y) / 2.0f, -pos1.x, 0);
                    break;
                case M3DirectionType.Col:
                    obj.transform.localEulerAngles = new Vector3(0, 0, 90);
                    obj.transform.localPosition = new Vector3(pos1.y, -(pos1.x + pos2.x) / 2.0f, 0);
                    break;
                default:
                    break;
            }
            return obj;
        }

        public GameObject LoadM3Prefab(string effectName, GameObject parent)
        {
            GameObject effectObj = null;
            KAssetManager.Instance.TryGetMatchPrefab(effectName, out effectObj);
            if (effectObj == null)
            {
                Debug.Log("特效加载失败，Name: " + effectName);
                return null;
            }
            var tmpObj = GameObject.Instantiate(effectObj);
            tmpObj.transform.SetParent(parent.transform, false);
            return tmpObj;
        }

        public GameObject LoadM3Prefab(string effectName)
        {
            GameObject effectObj = null;
            KAssetManager.Instance.TryGetMatchPrefab(effectName, out effectObj);
            if (effectObj == null)
            {
                Debug.Log("特效加载失败，Name: " + effectName);
                return null;
            }
            return effectObj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="column"></param>
        public void FireArrow(Vector3 pos, bool column)
        {
            var tmpObj = LoadM3Prefab(M3EffectConfig.ArrowEffectName);
            tmpObj = GameObject.Instantiate(tmpObj);
            tmpObj.transform.SetParent(crashFxParent.transform, false);
            tmpObj.transform.localPosition = new Vector3(pos.x, pos.y, -2.2f);
            if (!column)
            {
                tmpObj.transform.localEulerAngles = new Vector3(0, 0, 90);
            }
            GameObject.Destroy(tmpObj, 1);
        }

        public void PlayBonusStepEffect(Vector2 start, Vector2 target, float time, Transform parent, Action action)
        {
            var effectObj = PlayM3CommonEffect((int)MatchEffectType.Bonus_Arrow, parent.gameObject);
            RectTransform rect = effectObj.GetComponent<RectTransform>();
            rect.anchoredPosition = Vector2.zero;
            Vector2 from = target - Vector2.zero;
            rect.right = from.normalized;
            KTweenUtils.RectTransformLocalMoveTo(rect, target, time, delegate ()
           {
               GameObject.Destroy(effectObj);
               action();
           });
        }

        public void PlayerSkillInfectArrow(Vector3 start, Vector3 target, float time, Transform parent, Action action)
        {
            var effectObj = PlayM3CommonEffect((int)MatchEffectType.CatSkill_Infect_1, parent.gameObject);
            Vector3 from = target - start;
            effectObj.transform.right = from.normalized;
            KTweenUtils.MoveTo(effectObj.transform, target, time, delegate ()
            {
                GameObject.Destroy(effectObj);
                action();
            });
        }

        public void PlayerSkillCrossArrow(Vector3 start, Vector3 target, Int2 grid, float time, Transform parent, Action action)
        {
            var effectObj = PlayM3CommonEffect((int)MatchEffectType.CatSkill_Cross_Trail, parent.gameObject);

            effectObj.transform.localScale = Vector3.one;
            Vector3 from = target - start;
            effectObj.transform.right = from.normalized;
            KTweenUtils.MoveTo(effectObj.transform, target, time, delegate ()
            {
                GameObject.Destroy(effectObj);
                var effect = PlayM3CommonEffect((int)MatchEffectType.CatSkill_Cross_Boom, crashFxParent, M3Supporter.Instance.GetItemPositionByGrid(grid.x, grid.y));
                FrameScheduler.instance.Add(20, action);
            });
        }

        public void PlayerSkillShockWaveArrow(Vector3 start, Vector3 target, float time, Transform parent, Action action)
        {
            var effectObj = PlayM3CommonEffect((int)MatchEffectType.CatSkill_Infect_1, parent.gameObject);
            Vector3 from = target - start;
            effectObj.transform.right = from.normalized;
            KTweenUtils.MoveTo(effectObj.transform, target, time, delegate ()
            {
                GameObject.Destroy(effectObj);
                action();
            });
        }

        public void PlayerSkillCrushArrow(Vector3 start, Vector3 target, float time, Transform parent, Action action)
        {
            var effectObj = PlayM3CommonEffect((int)MatchEffectType.CatSkill_Infect_1, parent.gameObject);
            Vector3 from = target - start;
            effectObj.transform.right = from.normalized;
            KTweenUtils.MoveTo(effectObj.transform, target, time, delegate ()
            {
                GameObject.Destroy(effectObj);
                action();
            });
        }

        public void ShowScoreText(int score, int type, Vector3 pos)
        {
            var effect = M3Effect.GetEffectConfig((int)MatchEffectType.Score_Text);
            GameObject effectObj = LoadM3Prefab(effect.modelName, scoreFxParent);
            TextMesh textMesh = effectObj.GetComponentInChildren<TextMesh>();
            textMesh.text = score.ToString();
            effectObj.transform.localPosition = new Vector3(pos.x, pos.y, 0);
            effectObj.transform.localScale = new Vector3(0f, 0f, 1f);
            KTweenUtils.ScaleTo(effectObj.transform, new Vector3(1f, 1f, 1f), 0.5f);
            FrameScheduler.instance.Add(20, delegate ()
            {
                KTweenUtils.LocalMoveTo(effectObj.transform, effectObj.transform.localPosition + new Vector3(0, 1, 0), 0.6f);
                KTweenUtils.DoFade(textMesh.gameObject, 0f, 0.6f, delegate ()
                {
                    KTweenUtils.DoFade(textMesh.gameObject, 1, 0); GameObject.Destroy(effectObj);
                });
            });
        }

        public void PlayMagicCatShake(Transform trans, Action action)
        {
            trans.localRotation = Quaternion.Euler(Vector3.zero);
            Vector3 vec1 = new Vector3(0, 0, -20);
            Vector3 vec2 = new Vector3(0, 0, 20);

            for (int i = 0; i < 4; i++)
            {
                FrameScheduler.instance.Add(20 * i, delegate ()
                {
                    KTweenUtils.RotateTo(trans, vec1, 0.2f, delegate ()
                    {

                        KTweenUtils.RotateTo(trans, vec2, 0.2f, delegate ()
                        {
                            trans.localEulerAngles = Vector3.zero;
                        });
                    });
                });
            }
        }

        public GameObject LoadCharacterEffect(string effectName)
        {
            GameObject effectObj = null;
            KAssetManager.Instance.TryGetMatchPrefab(effectName, out effectObj);
            if (effectObj == null)
            {
                Debuger.LogError("特效加载失败，Name: " + effectName);
                return null;
            }
            return effectObj;
        }

        public KFxBehaviour PlayCatSkill(string effectName)
        {
            GameObject effectObj = M3FxManager.Instance.LoadCharacterEffect(effectName);
            effectObj = GameObject.Instantiate(effectObj);
            effectObj.transform.SetParent(M3GameManager.Instance.catManager.skillRoot.transform, false);
            return effectObj.AddComponent<KFxBehaviour>();
        }

        public KFxBehaviour PlayCatSkillBoom(string effectName, Vector3 localPos)
        {
            if (string.IsNullOrEmpty(effectName))
                return null;
            GameObject effectObj = M3FxManager.Instance.LoadCharacterEffect(effectName);
            effectObj = GameObject.Instantiate(effectObj);
            effectObj.transform.SetParent(crashFxParent.transform, false);
            effectObj.transform.localPosition = localPos;
            return effectObj.AddComponent<KFxBehaviour>();
        }

        public float VectorAngle(Vector2 v1, Vector2 v2)
        {
            float angle;
            Vector2 to = new Vector2(1, 0);
            Vector2 from = v2 - v1;
            Vector3 cross = Vector3.Cross(from, to);
            angle = Vector2.Angle(from, to);
            return cross.z > 0 ? -angle : angle;
            //return angle;
        }
        #endregion

    }
}