/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/28 13:18:26
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using UnityEngine;

namespace Game
{
    public class RoleAvatar
    {
        public const string ANIM_IDLE = "idle";
        public const string ANIM_WALK = "walk";
        public const string ANIM_WIN = "win";
        public const string ANIM_SKILL = "skill";


        #region Member
        public GameObject GO { get; private set; }
        public Transform Tf { get; private set; }

        public int ModID { get; protected set; }
        public string ResName { get; protected set; }

        protected GameObject m_roleGO;
        protected Animator m_animator;
        protected string m_curAnimName;

        #endregion

        #region C&D
        public RoleAvatar(string resName)
        {
            Init(resName);
        }
        public RoleAvatar(int modID)
        {
            ModID = modID;
        }
        #endregion

        #region Public

        public void PlayAnimation(string trigger, float speed = 1f)
        {
            if (m_animator != null)
            {
                m_animator.speed = speed;

                // 确保正确转换过去
                if (!string.IsNullOrEmpty(m_curAnimName))
                    m_animator.ResetTrigger(m_curAnimName);

                m_animator.SetTrigger(trigger);
            }

            m_curAnimName = trigger;
        }

        public virtual void Dispose()
        {
            GameObject.Destroy(GO);
            GO = null;
            Tf = null;

            ModID = 0;
            ResName = null;
            m_roleGO = null;
            m_animator = null;
            m_curAnimName = null;
        }

        #endregion

        #region Private

        protected virtual void Init(string resName)
        {
            ResName = resName;
            GO = new GameObject(resName);
            Tf = GO.transform;
            Load(resName);
        }

        protected virtual void Load(string resName)
        {
            KAssetManager.Instance.TryGetCharacterPrefab(resName, OnLoaded);
        }

        protected virtual void OnLoaded(GameObject prefab)
        {
            if (prefab == null)
            {
                Debuger.LogError("Prefab is null! " + ResName);
            }
            else if (Tf != null)
            {
                m_roleGO = UnityEngine.Object.Instantiate(prefab);
                m_roleGO.transform.SetParent(Tf, false);
                m_animator = m_roleGO.GetComponent<Animator>();
                if (string.IsNullOrEmpty(m_curAnimName))
                {
                    PlayAnimation(m_curAnimName);
                }
            }
        }

        protected virtual void PlaySound(string name)
        {
            //int a = (catShopId / 100) % 10;
            //int b = catShopId % 10;
            int id = ModID;// a * 1000 + b;
            AudioClip clip;
            if (KAssetManager.Instance.TryGetSoundAsset(string.Format("Audio/sounds_{0}_{1}", id, name), out clip))
            {
                KSoundManager.PlayAudio(clip);
            }
        }

        #endregion

    }
}
