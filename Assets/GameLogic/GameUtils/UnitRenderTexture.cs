/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/28 12:25:22
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UnitRenderTexture
    {
        #region static

        //private static Dictionary<string, UnitRenderTexture> m_urtDic;
        private static int URTPosArg = -100;

        public static UnitRenderTexture Get(RawImage rawImage)
        {
            UnitRenderTexture urt = new UnitRenderTexture();
            if (rawImage != null)
            {
                rawImage.texture = urt.RenderTexture;
            }
            return urt;
        }

        public static void SetActive(UnitRenderTexture urt, bool visibe)
        {
            if (urt != null)
            {
                urt.SetActive(visibe);
            }
        }

        public static void Destory(UnitRenderTexture urt)
        {
            if (urt != null)
            {
                urt.Dispose();
            }
        }

        #endregion



        #region Member

        private GameObject m_cameraGO;
        private Transform m_unitTf;
        private RoleAvatar m_role;

        public RenderTexture RenderTexture { get; private set; }

        #endregion

        #region C&D
        public UnitRenderTexture()
        {
            GameObject prefab;
            if (KAssetManager.Instance.TryGetGlobalPrefab("UnitToUICamera", out prefab))
            {
                m_cameraGO = UnityEngine.Object.Instantiate(prefab);
                m_cameraGO.transform.position = Vector3.one * URTPosArg;
                URTPosArg -= 5;
            }

            RenderTexture = UnityEngine.Object.Instantiate(m_cameraGO.GetComponent<Camera>().targetTexture);
            m_cameraGO.GetComponent<Camera>().targetTexture = RenderTexture;
            m_unitTf = m_cameraGO.transform.Find("Unit");
        }
        #endregion

        #region Public

        public void SetRole(string resName)
        {
            if (m_role != null)
            {
                if (m_role.ResName == resName)
                {
                    return;
                }
                else
                {
                    m_role.Dispose();
                }
            }

            if (!string.IsNullOrEmpty(resName))
            {
                m_role = new RoleAvatar(resName);
                m_role.Tf.SetParent(m_unitTf, false);
                m_role.Tf.localPosition = new Vector3(0, -0.4f, 0);
            }
        }

        public void SetPlayer(int playerId)
        {
            if (m_role != null)
            {
                if (m_role.ModID == playerId)
                {
                    return;
                }
                else
                {
                    m_role.Dispose();
                }
            }

            m_role = new PlayerAvatar(playerId);
            m_role.Tf.SetParent(m_unitTf, false);
            m_role.Tf.localPosition = new Vector3(0, -0.9f, 0);
        }

        public void SetPet(int modID)
        {
            if (m_role != null)
            {
                if (m_role.ModID == modID)
                {
                    return;
                }
                else
                {
                    m_role.Dispose();
                }
            }

            m_role = new PetAvatar(modID);
            m_role.Tf.SetParent(m_unitTf, false);
            m_role.Tf.localPosition = new Vector3(0, -0.4f, 0);
        }

        public PlayerAvatar GetPlayerAvatar()
        {
            return m_role as PlayerAvatar;
        }

        public void PlayAnimation(string trigger, float speed = 1f)
        {
            m_role.PlayAnimation(trigger, speed);
        }

        public void SetActive(bool b)
        {
            if (m_cameraGO != null)
            {
                m_cameraGO.SetActive(b);
            }
        }

        public void Dispose()
        {
            if (m_role != null)
            {
                m_role.Dispose();
                m_role = null;
            }
            GameObject.Destroy(m_cameraGO);
            m_cameraGO = null;
            m_unitTf = null;
            RenderTexture = null;
        }

        #endregion

        #region Private


        #endregion

    }
}
