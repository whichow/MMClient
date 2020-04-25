/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/9 18:33:38
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{

    public class AvatarChangeTest : MonoBehaviour
    {
        public GameObject m_sourceAvatar;
        public GameObject f_sourceAvatar;
        public InputField avatarID;

        private Animator m_animator;
        private Animator f_animator;

        private AvatarChange m_avatarChange;
        private AvatarChange f_avatarChange;

        public void Start()
        {
            KAssetManager.Instance.Initialize();

            m_animator = m_sourceAvatar.GetComponent<Animator>();
            f_animator = f_sourceAvatar.GetComponent<Animator>();
            m_avatarChange = new AvatarChange(m_sourceAvatar);
            f_avatarChange = new AvatarChange(f_sourceAvatar);
        }

        public void PlayAnim(string anim = "idle")
        {
            m_animator.SetTrigger(anim);
            f_animator.SetTrigger(anim);
        }

        public void SetDefault()
        {
            m_avatarChange.SetDefaultAll();
        }

        public void SetDefault(string part)
        {
            m_avatarChange.SetDefaultPart((EAvatarPart)int.Parse(part));
        }

        public void Change1()
        {
            mChange("mh_" + avatarID.text);
            fChange("fh_"+ avatarID.text);
        }

        public void Change2()
        {
            mChange("mj_" + avatarID.text);
            fChange("fj_" + avatarID.text);
        }

        public void Change3()
        {
            mChange("mp_" + avatarID.text);
            fChange("fp_" + avatarID.text);
        }

        public void Change4()
        {
            mChange("ms_" + avatarID.text);
            fChange("fs_" + avatarID.text);
        }

        private void mChange(string resName)
        {
            LoadAsset(resName, (obj) =>
            {
                m_avatarChange.ChangePart(obj);
            });
        }

        private void fChange(string resName)
        {
            LoadAsset(resName, (obj) =>
            {
                f_avatarChange.ChangePart(obj);
            });
        }

        private void LoadAsset(string name, Action<GameObject> callback)
        {
            KAssetManager.Instance.TryGetCharacterPrefab("Part/" + name, callback);
            //var assetPath = "Assets/Res/Character/Part/" + name + ".prefab";
            //var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            //callback?.Invoke(asset);
            //return asset;
        }

    }
}
