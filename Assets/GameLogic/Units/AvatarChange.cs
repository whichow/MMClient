/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/9 15:35:23
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum EAvatarPart
    {
        none,
        hair,
        coat,
        pant,
        foot,
    }

    public class AvatarChange
    {
        private GameObject sourceAvatar;
        private GameObject rootBone;
        //目标骨骼位置信息
        private Transform[] _sourceBones;
        private Dictionary<string, SkinnedMeshRenderer> _defaultPartSMRDic;

        public AvatarChange(GameObject avatar)
        {
            sourceAvatar = avatar;
            rootBone = avatar.transform.Find("Bip01").gameObject;
        }

        public void SetDefaultAll()
        {
            SetDefaultPart(EAvatarPart.hair);
            SetDefaultPart(EAvatarPart.coat);
            SetDefaultPart(EAvatarPart.pant);
            SetDefaultPart(EAvatarPart.foot);
        }

        /// <summary>
        /// 设置默认装 
        /// </summary>
        /// <param name="part"></param>
        public void SetDefaultPart(EAvatarPart part)
        {
            string partName = part.ToString();
            SkinnedMeshRenderer sSMR = DefaultPartSMRDic[partName];
            Transform tf = sourceAvatar.transform.Find(partName);
            if (tf != null)
            {
                GameObject.Destroy(tf.gameObject);
                sSMR.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 换装
        /// </summary>
        /// <param name="resName"> 部位资源名</param>
        public void ChangePart(string resName)
        {
            KAssetManager.Instance.TryGetCharacterPrefab("Part/" + resName, OnLoaded);
        }

        public void ChangePart(GameObject obj)
        {
            OnLoaded(obj);
        }

        private void OnLoaded(GameObject obj)
        {
            if (obj == null) return;

            string resName = obj.name;
            EAvatarPart partType = EAvatarPart.none;
            if (resName.Contains("h_"))
            {
                partType = EAvatarPart.hair;
            }
            else if (resName.Contains("j_"))
            {
                partType = EAvatarPart.coat;
            }
            else if (resName.Contains("p_"))
            {
                partType = EAvatarPart.pant;
            }
            else if (resName.Contains("s_"))
            {
                partType = EAvatarPart.foot;
            }
            StartChangeAvatar(partType, obj);
        }

        private void StartChangeAvatar(EAvatarPart partType, GameObject partObj)
        {
            GameObject part = GameObject.Instantiate(partObj as GameObject);
            part.SetActive(false);

            SkinnedMeshRenderer[] smrs = part.GetComponentsInChildren<SkinnedMeshRenderer>();
            ChangeAvatar(partType.ToString(), smrs[0]);

            GameObject.Destroy(part);
        }

        private void ChangeAvatar(string partName, SkinnedMeshRenderer targetSMR)
        {
            //骨骼要对应，绑定骨骼
            List<Transform> bones = new List<Transform>();
            foreach (Transform smr in targetSMR.bones)
            {
                foreach (Transform targetBone in SourceBones)
                {
                    if (targetBone.name == smr.name)
                    {
                        bones.Add(targetBone);
                        break;
                    }
                }
            }

            //SkinnedMeshRenderer sSMR = sourceAvatar.GetComponent<SkinnedMeshRenderer>(partName);
            SkinnedMeshRenderer sSMR = DefaultPartSMRDic[partName];
            Transform tf = sourceAvatar.transform.Find(partName);
            if (tf != null)
            {
                GameObject.Destroy(tf.gameObject);
            }
            else
            {
                sSMR.gameObject.SetActive(false);
            }

            GameObject targetObj = new GameObject();
            targetObj.name = partName;
            targetObj.transform.parent = sourceAvatar.transform;
            targetObj.transform.position = targetSMR.transform.position;

            SkinnedMeshRenderer newSMR = targetObj.AddComponent<SkinnedMeshRenderer>();
            newSMR.sharedMesh = targetSMR.sharedMesh;
            newSMR.materials = targetSMR.materials;
            newSMR.bones = bones.ToArray();
            newSMR.rootBone = sSMR.rootBone;
        }

        private Transform[] SourceBones
        {
            get
            {
                if (_sourceBones == null)
                {
                    _sourceBones = rootBone.GetComponentsInChildren<Transform>(true);
                }
                return _sourceBones;
            }
        }

        private Dictionary<string, SkinnedMeshRenderer> DefaultPartSMRDic
        {
            get
            {
                if (_defaultPartSMRDic == null)
                {
                    _defaultPartSMRDic = new Dictionary<string, SkinnedMeshRenderer>();
                    string[] list = Enum.GetNames(typeof(EAvatarPart));
                    foreach (var name in list)
                    {
                        if (name != "none")
                        {
                            SkinnedMeshRenderer smr = sourceAvatar.GetComponent<SkinnedMeshRenderer>(name);
                            smr.gameObject.name = "Default" + name;
                            _defaultPartSMRDic.Add(name, smr);
                        }
                    }
                }
                return _defaultPartSMRDic;
            }
        }
        
    }
}
