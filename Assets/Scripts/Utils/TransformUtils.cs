// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "TransformUtils" company=""></copyright>
// <summary></summary>
// ***********************************************************************
namespace Game
{
    using UnityEngine;

    public static class TransformUtils
    {
        public static T Instantiate<T>(T prefab, Transform parent) where T : Component
        {
            T component = Object.Instantiate(prefab);
            component.name = prefab.name;
            component.transform.SetParent(parent, false);
            component.transform.localPosition = prefab.transform.localPosition;
            component.transform.localRotation = prefab.transform.localRotation;
            component.transform.localScale = prefab.transform.localScale;
            return component;
        }

        public static T Instantiate<T>(T prefab, Transform parent, bool linked) where T : Component
        {
            if (prefab)
            {
                Vector3 position = parent.TransformPoint(prefab.transform.localPosition);
                Quaternion rotation = parent.rotation * prefab.transform.localRotation;
                T component = Object.Instantiate(prefab, position, rotation) as T;
                component.transform.SetParent(linked ? parent : null, false);
                return component;
            }
            else
            {
                return null;
            }
        }

        public static T Instantiate<T>(T prefab, Transform parent, bool linked, bool pooled) where T : Component
        {
            if (prefab)
            {
                Vector3 position = parent.TransformPoint(prefab.transform.localPosition);
                Quaternion rotation = parent.rotation * prefab.transform.localRotation;
                T component = pooled ? KPool.Spawn(prefab, position, rotation) : Object.Instantiate(prefab, position, rotation) as T;
                component.transform.SetParent(linked ? parent : null, false);
                return component;
            }
            else
            {
                return null;
            }
        }

        public static GameObject Instantiate(GameObject prefab, Transform parent)
        {
            if (prefab)
            {
                GameObject gameObject = Object.Instantiate(prefab) as GameObject;
                gameObject.name = prefab.name;
                gameObject.transform.SetParent(parent, false);
                gameObject.transform.localPosition = prefab.transform.localPosition;
                gameObject.transform.localRotation = prefab.transform.localRotation;
                gameObject.transform.localScale = prefab.transform.localScale;
                return gameObject;
            }
            return null;
        }

        public static GameObject Instantiate(GameObject prefab, Transform parent, bool linked)
        {
            if (prefab)
            {
                Vector3 position = parent.transform.TransformPoint(prefab.transform.localPosition);
                Quaternion rotation = parent.transform.rotation * prefab.transform.localRotation;
                GameObject gameObject = Object.Instantiate(prefab, position, rotation) as GameObject;
                gameObject.transform.parent = linked ? parent : null;
                return gameObject;
            }
            return null;
        }

        public static GameObject Instantiate(GameObject prefab, Transform parent, bool linked, bool pooled)
        {
            if (prefab)
            {
                Vector3 position = parent.transform.TransformPoint(prefab.transform.localPosition);
                Quaternion rotation = parent.transform.rotation * prefab.transform.localRotation;
                GameObject gameObject = pooled ? KPool.Spawn(prefab, position, rotation) : Object.Instantiate(prefab, position, rotation) as GameObject;
                gameObject.transform.parent = linked ? parent : null;
                return gameObject;
            }
            return null;
        }


        public static void Instantiate(GameObject prefab, Transform parent, bool linked, bool pooled, ref GameObject GORef, Vector3 postion, bool collected)
        {
            if (prefab)
            {
                if (collected && GORef)
                {
                    KPool.Despawn(GORef);
                }
                GORef = ((!pooled) ? GameObject.Instantiate(prefab) : KPool.Spawn(prefab)) as GameObject;
                if (linked)
                {
                    GORef.transform.parent = linked ? parent : null;
                    GORef.transform.localScale = prefab.transform.localScale;
                    GORef.transform.localPosition = postion;
                }
            }
        }

        public static string GetPath(Transform transform)
        {
            return (transform) ? (TransformUtils.GetPath(transform.parent) + "/" + transform.name) : string.Empty;
        }

        /// <summary>
        /// change transform by params
        /// </summary>
        /// <param name="t"></param>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        /// <param name="sca"></param>
        /// <param name="parent"></param>
        public static void ChangeTransform(ref Transform t, Vector3 pos, Vector3 rot
            , Vector3 sca, Transform parent = null)
        {
            if (t == null)
            {
                return;
            }
            t.parent = parent;
            t.localPosition = pos;
            t.localRotation = Quaternion.Euler(rot);
            t.localScale = sca;
            SetLayerSameWithParent(t.gameObject);
        }
        /// <summary>
        /// 立即销毁某物体
        /// </summary>
        /// <param name="obj"></param>
        public static void Destroy(Transform target)
        {
            target.parent = null;
            Object.Destroy(target.gameObject);
        }
        /// <summary>
        /// 销毁所有子
        /// </summary>
        /// <param name="target"></param>
        public static void DestroyChildren(this Transform target)
        {
            while (target.childCount > 0)
            {
                var child = target.GetChild(0);
                Destroy(child);
            }
        }
        /// <summary>
        /// 从自己或父级获取指定的Component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static T FindComponentInThisOrParents<T>(Transform transform) where T : Component
        {
            var tmp = transform;
            while (tmp != null)
            {
                T component = transform.GetComponent<T>();
                if (component)
                {
                    return component;
                }
                tmp = tmp.parent;
            }
            return (T)((object)null);
        }
        /// <summary>
        /// 获取指定名字的子物体
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childName"></param>
        /// <param name="similar"></param>
        /// <returns></returns>
        public static Transform GetChildByName(Transform parent, string childName, bool similar = false)
        {
            if (parent == null) return null;
            foreach (Transform child in parent)
            {
                if (child.name == childName || (similar && child.name.IndexOf(childName) != -1))
                {
                    return child;
                }
                else
                {
                    Transform child2 = GetChildByName(child, childName, similar);
                    if (child2 != null)
                    {
                        return child2;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 获取transform层级关系
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string GetHierarchy(Transform trans, string separator = "/")
        {
            if (trans == null)
            {
                return null;
            }
            string path = trans.name;
            while (trans.parent != null)
            {
                trans = trans.parent;
                path = trans.name + separator + path;
            }
            return path;
        }
        /// <summary>
        /// 生成prefab并返回对应类型实例，使用prefab的Transform信息，如果有父级保证层级一致
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <param name="linked"></param>
        /// <returns></returns>
        public static T InstantiateComByPrefabTrans<T>(T prefab, Transform parent = null
            , bool linked = true) where T : Component
        {
            if (prefab != null)
            {
                Vector3 position = parent.TransformPoint(prefab.transform.localPosition);
                Quaternion rotation = parent.rotation * prefab.transform.localRotation;
                T component = Object.Instantiate(prefab, position, rotation) as T;
                component.transform.parent = linked ? parent : null;
                if (parent != null && linked)
                {
                    SetLayerSameWithParent(component.gameObject);
                }
                return component;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 生成prefab并返回实例，使用prefab的Transform信息，如果有父级保证层级一致
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <param name="linked"></param>
        /// <returns></returns>
        public static GameObject InstantiateGOByPrefabTrans(GameObject prefab, Transform parent = null
            , bool linked = true)
        {
            if (prefab != null)
            {
                Vector3 position = parent.TransformPoint(prefab.transform.localPosition);
                Quaternion rotation = parent.rotation * prefab.transform.localRotation;
                GameObject gameObject = Object.Instantiate(prefab, position, rotation) as GameObject;
                gameObject.transform.parent = linked ? parent : null;
                if (parent != null && linked)
                {
                    SetLayerSameWithParent(gameObject);
                }
                return gameObject;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 生成prefab并返回对应类型实例，使用传来的Transform信息，如果有父级保证层级一致
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefab"></param>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        /// <param name="parent"></param>
        /// <param name="linked"></param>
        /// <returns></returns>
        public static T InstantiateComByGivenTrans<T>(T prefab, Vector3 pos, Vector3 rot
            , Vector3 scale, Transform parent = null, bool linked = true)
            where T : Component
        {
            if (prefab != null)
            {
                T component = Object.Instantiate(prefab) as T;
                component.transform.parent = linked ? parent : null;
                component.transform.localPosition = pos;
                component.transform.localRotation = Quaternion.Euler(rot);
                component.transform.localScale = scale;
                if (parent != null && linked)
                {
                    SetLayerSameWithParent(component.gameObject);
                }
                return component;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 生成prefab并返回实例，使用传来的Transform信息，如果有父级保证层级一致
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        /// <param name="parent"></param>
        /// <param name="linked"></param>
        /// <returns></returns>
        public static GameObject InstantiateGoByGivenTrans(GameObject prefab, Vector3 pos
            , Vector3 rot, Vector3 scale, Transform parent = null, bool linked = true
            )
        {
            if (prefab != null)
            {
                GameObject gameObject = Object.Instantiate(prefab) as GameObject;
                gameObject.transform.parent = linked ? parent : null;
                gameObject.transform.localPosition = pos;
                gameObject.transform.localRotation = Quaternion.Euler(rot);
                gameObject.transform.localScale = scale;
                if (parent != null && linked)
                {
                    SetLayerSameWithParent(gameObject);
                }
                return gameObject;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 生成prefab并返回对应类型实例，Transform信息归零，如果有父级保证层级一致
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <param name="linked"></param>
        /// <returns></returns>
        public static T InstantiateComByDefaultTrans<T>(T prefab, Transform parent = null
            , bool linked = true) where T : Component
        {
            if (prefab != null)
            {
                T component = Object.Instantiate(prefab) as T;
                component.transform.parent = linked ? parent : null;
                component.transform.localPosition = Vector3.zero;
                component.transform.localRotation = Quaternion.identity;
                component.transform.localScale = Vector3.one;
                if (parent != null && linked)
                {
                    SetLayerSameWithParent(component.gameObject);
                }
                return component;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 生成prefab并返回实例，Transform信息归零，如果有父级保证层级一致
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <param name="linked"></param>
        /// <returns></returns>
        public static GameObject InstantiateGoByDefaultTrans(GameObject prefab, Transform parent = null
            , bool linked = true)
        {
            if (prefab != null)
            {
                GameObject gameObject = Object.Instantiate(prefab) as GameObject;
                gameObject.transform.parent = linked ? parent : null;
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localRotation = Quaternion.identity;
                gameObject.transform.localScale = Vector3.one;
                if (parent != null && linked)
                {
                    SetLayerSameWithParent(gameObject);
                }
                return gameObject;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 判断父子关系
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        public static bool IsChild(Transform parent, Transform child)
        {
            if (parent == null || child == null) return false;

            while (child != null)
            {
                if (child == parent) return true;
                child = child.parent;
            }
            return false;
        }
        /// <summary>
        /// 设置层级
        /// </summary>
        /// <param name="go"></param>
        /// <param name="layerName"></param>
        public static void SetLayer(GameObject go, string layerName)
        {
            if (go != null && !layerName.IsNullOrEmpty())
            {
                int layer = LayerMask.NameToLayer(layerName);
                SetLayer(go, layer);
            }
        }
        /// <summary>
        /// 设置层级
        /// </summary>
        /// <param name="go"></param>
        /// <param name="layer"></param>
        public static void SetLayer(GameObject go, int layer)
        {
            if (go != null)
            {
                go.layer = layer;
                Transform t = go.transform;
                for (int i = 0, imax = t.childCount; i < imax; ++i)
                {
                    Transform child = t.GetChild(i);
                    SetLayer(child.gameObject, layer);
                }
            }
        }
        /// <summary>
        /// 如果有父级保证层级一致
        /// </summary>
        /// <param name="trans"></param>
        public static void SetLayerSameWithParent(GameObject go)
        {
            if (go != null)
            {
                Transform parent = go.transform.parent;
                if (parent != null)
                {
                    SetLayer(go, parent.gameObject.layer);
                }
            }
        }
    }
}
