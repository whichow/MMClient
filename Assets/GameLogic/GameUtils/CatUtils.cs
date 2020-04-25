/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/6/20 11:07:32
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using Game.DataModel;
using Spine.Unity;
using UnityEngine;

namespace Game
{
    public class CatUtils
    {
        public static string GetCatColorString(int color)
        {
            return GetCatColorString((ECatColor)color);
        }

        public static string GetCatColorString(ECatColor color)
        {
            var sb = new System.Text.StringBuilder();
            if ((color & ECatColor.fRed) != 0)
            {
                sb.Append("<color=#f93535>红色</color> ");
            }
            if ((color & ECatColor.fYellow) != 0)
            {
                sb.Append("<color=#ffc823>黄色</color> ");
            }
            if ((color & ECatColor.fBlue) != 0)
            {
                sb.Append("<color=#4f4ffc>蓝色</color> ");
            }
            if ((color & ECatColor.fGreen) != 0)
            {
                sb.Append("<color=#35dc61>绿色</color> ");
            }
            if ((color & ECatColor.fPurple) != 0)
            {
                sb.Append("<color=#d83eff>紫色</color> ");
            }
            if ((color & ECatColor.fBrown) != 0)
            {
                sb.Append("<color=#A0522D>棕色</color> ");
            }
            return sb.ToString();
        }

        #region 获取模型

        public static GameObject GetModel(int cfgID)
        {
            string modelName = XTable.CatXTable.GetByID(cfgID).Model;
            GameObject prefab;
            if (KAssetManager.Instance.TryGetPet2DPrefab(modelName, out prefab))
            {
                return CreateModel(prefab, cfgID);
            }
            return null;
        }

        public static void GetModel(int cfgID, System.Action<GameObject> callback)
        {
            string modelName = XTable.CatXTable.GetByID(cfgID).Model;
            KAssetManager.Instance.TryGetPet2DPrefab(modelName, (prefab) =>
            {
                var go = CreateModel(prefab, cfgID);
                callback?.Invoke(go);
            });
        }

        private static GameObject CreateModel(GameObject prefab, int cfgID)
        {
            GameObject go = Object.Instantiate(prefab);
            go.AddComponent<KCatBehaviour>().catShopId = cfgID;
            return go;
        }

        public static GameObject GetUIModel(int cfgID)
        {
            string modelName = XTable.CatXTable.GetByID(cfgID).Model;
            GameObject prefab;
            if (KAssetManager.Instance.TryGetPet2DPrefab(modelName, out prefab))
            {
                return GetUIModel(prefab, cfgID);
            }
            return null;
        }

        public static void GetUIModel(int cfgID, System.Action<GameObject> callback)
        {
            string modelName = XTable.CatXTable.GetByID(cfgID).Model;
            KAssetManager.Instance.TryGetPet2DPrefab(modelName, (prefab) =>
            {
                var model = GetUIModel(prefab, cfgID);
                callback?.Invoke(model);
            });
        }

        private static GameObject GetUIModel(GameObject prefab, int cfgID)
        {
            var sAnim = prefab.GetComponent<SkeletonAnimation>();
            //Material material = Resources.Load<Material>("Materials/SkeletonGraphicDefault");
            var graphic = SkeletonGraphic.NewSkeletonGraphicGameObject(sAnim.skeletonDataAsset, null);//, material);
            graphic.rectTransform.pivot = new Vector2(0.5f, 0f);
            graphic.rectTransform.sizeDelta = new Vector2(450f, 600f);

            var graphicObject = graphic.gameObject;
            graphicObject.name = sAnim.name;
            graphicObject.layer = LayerMask.NameToLayer("UI");
            graphicObject.AddComponent<KCatBehaviour>().catShopId = cfgID;
            return graphicObject;
        }

        #endregion

    }
}
