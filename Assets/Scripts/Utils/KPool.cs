// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using UnityEngine;
using System.Collections.Generic;

namespace Game
{

    /// <summary>
    /// Class KSpawnPool 生成池 只在当前scene下有效
    /// </summary>
    public sealed class KPool : MonoBehaviour
    {
        #region MODEL

        /// <summary>
        /// Class InstanceInfo 生成的实例
        /// </summary>
        private sealed class InstanceInfo
        {
            private bool _inUse;
            private GameObject _gameObject;
            private InstanceTable _instanceTable;

            public bool inUse
            {
                get { return _inUse; }
            }

            public GameObject gameObject
            {
                get { return _gameObject; }
            }

            public InstanceTable instanceTable
            {
                get { return _instanceTable; }
            }

            /// <summary>Despawns this instance.</summary>
            public void Despawn()
            {
                if (_inUse)
                {
                    _inUse = false;
                    _instanceTable.Despawn(_gameObject);
                }
            }

            /// <summary>Initializes the specified in use.</summary>
            /// <param name="inUse">if set to <c>true</c> [in use].</param>
            /// <param name="instance">The instance.</param>
            /// <param name="spawnTable">The spawn table.</param>
            public void Init(bool inUse, GameObject instance, InstanceTable spawnTable)
            {
                this._inUse = inUse;
                this._gameObject = instance;
                this._instanceTable = spawnTable;
            }

            /// <summary>Initializes a new instance of the <see cref="InstanceInfo"/> class.</summary>
            /// <param name="inUse">if set to <c>true</c> [in use].</param>
            /// <param name="instance">The instance.</param>
            /// <param name="spawnTable">The spawn table.</param>
            public InstanceInfo(bool inUse, GameObject instance, InstanceTable spawnTable)
            {
                this._inUse = inUse;
                this._gameObject = instance;
                this._instanceTable = spawnTable;
            }
        }

        /// <summary>
        /// Class InstanceTable 生成表
        /// </summary>
        private sealed class InstanceTable
        {
            private const int MAX_COUNT = 16;

            private GameObject _prefabObj;
            private List<GameObject> _activeObjs = new List<GameObject>(MAX_COUNT);
            private Stack<GameObject> _despawnObjs = new Stack<GameObject>(MAX_COUNT);
            private Stack<GameObject> _inactiveObjs = new Stack<GameObject>(MAX_COUNT);

            /// <summary>
            /// Gets the prefab.
            /// </summary>
            /// <value>
            /// The prefab.
            /// </value>
            public GameObject prefab
            {
                get { return _prefabObj; }
            }
            /// <summary>
            /// Gets the active count.
            /// </summary>
            /// <value>
            /// The active count.
            /// </value>
            public int activeCount
            {
                get { return _activeObjs.Count; }
            }
            /// <summary>
            /// Gets the inactive count.
            /// </summary>
            /// <value>
            /// The inactive count.
            /// </value>
            public int inactiveCount
            {
                get { return _inactiveObjs.Count; }
            }
            /// <summary>
            /// Gets the despawn count.
            /// </summary>
            /// <value>
            /// The despawn count.
            /// </value>
            public int despawnCount
            {
                get { return _despawnObjs.Count; }
            }
            /// <summary>
            /// Gets the active objects.
            /// </summary>
            /// <value>
            /// The active objects.
            /// </value>
            public List<GameObject> activeObjects
            {
                get { return this._activeObjs; }
            }
            /// <summary>
            /// Initializes a new instance of the <see cref="InstanceTable"/> class.
            /// </summary>
            /// <param name="prefab">The prefab.</param>
            public InstanceTable(GameObject prefab)
            {
                _prefabObj = prefab;
            }
            /// <summary>
            /// Creates the new object.
            /// </summary>
            /// <returns></returns>
            private GameObject CreateNewObj()
            {
                return GameObject.Instantiate(_prefabObj) as GameObject;
            }
            /// <summary>
            /// Creates the new object.
            /// </summary>
            /// <param name="position">The position.</param>
            /// <param name="rotation">The rotation.</param>
            /// <returns></returns>
            private GameObject CreateNewObj(Vector3 position, Quaternion rotation)
            {
                return GameObject.Instantiate(_prefabObj, position, rotation) as GameObject;
            }
            /// <summary>
            /// Creates the new object.
            /// </summary>
            /// <param name="localPosition">The position.</param>
            /// <param name="localRotation">The rotation.</param>
            /// <returns></returns>
            private GameObject CreateNewObj(Vector3 localPosition, Vector3 localRotation, Vector3 localScale, Transform linkParent)
            {
                GameObject gameObj = GameObject.Instantiate(_prefabObj) as GameObject;
                //gameObj.SetActive(false);
                gameObj.transform.SetParent(linkParent, false);
                gameObj.transform.localPosition = localPosition;
                gameObj.transform.localEulerAngles = localRotation;
                gameObj.transform.localScale = localScale;
                gameObj.SetActive(true);
                return gameObj;
            }
            /// <summary>
            /// Anies the inactive object.
            /// </summary>
            /// <returns></returns>
            private GameObject AnyInactiveObj()
            {
                return _despawnObjs.Count > 0 ? _despawnObjs.Pop() : (_inactiveObjs.Count > 0 ? _inactiveObjs.Pop() : null);
            }
            /// <summary>
            /// Pres the spawn.
            /// </summary>
            /// <param name="count">The count.</param>
            public void PreSpawn(int count)
            {
                int tmpCount = count - _activeObjs.Count - _inactiveObjs.Count;
                for (; tmpCount > 0; tmpCount--)
                {
                    GameObject gameObject = CreateNewObj();
                    //                    gameObject.hideFlags = HideFlags.HideInHierarchy;//不可见
                    gameObject.SetActive(false);
                    _inactiveObjs.Push(gameObject);
                }
            }
            /// <summary>
            /// Spawns the specified position.
            /// </summary>
            /// <param name="position">The position.</param>
            /// <param name="rotation">The rotation.</param>
            /// <returns></returns>
            public GameObject Spawn(Vector3 position, Quaternion rotation)
            {
                GameObject gameObject = this.AnyInactiveObj();

                if (gameObject == null)
                {
                    gameObject = this.CreateNewObj(position, rotation);
                }
                else
                {
                    gameObject.transform.position = position;
                    gameObject.transform.rotation = rotation;
                    gameObject.hideFlags = 0;
                    gameObject.SetActive(true);
                }

                _activeObjs.Add(gameObject);
                return gameObject;
            }

            /// <summary>
            /// Spawns the specified position.
            /// </summary>
            /// <param name="localPosition">The position.</param>
            /// <param name="localRotation">The rotation.</param>
            /// <returns></returns>
            public GameObject Spawn(Vector3 localPosition, Vector3 localRotation, Vector3 localScale, Transform linkParent)
            {
                GameObject gameObject = this.AnyInactiveObj();

                if (gameObject == null)
                {
                    gameObject = this.CreateNewObj(localPosition, localRotation, localScale, linkParent);
                }
                else
                {
                    gameObject.transform.SetParent(linkParent, false);
                    gameObject.transform.localPosition = localPosition;
                    gameObject.transform.localEulerAngles = localRotation;
                    gameObject.transform.localScale = localScale;
                    gameObject.hideFlags = 0;
                    gameObject.SetActive(true);
                }

                this._activeObjs.Add(gameObject);
                return gameObject;
            }
            /// <summary>
            /// Despawns the specified game object.
            /// </summary>
            /// <param name="gameObj">The game object.</param>
            public void Despawn(GameObject gameObj)//防止卡
            {
                if (gameObj == null || this._despawnObjs.Contains(gameObj) || this._inactiveObjs.Contains(gameObj))
                {
                    return;
                }
                else
                {
                    gameObj.SetActive(false);
                    gameObj.transform.SetParent(null, false);
                    gameObj.hideFlags = HideFlags.HideInHierarchy;
                    this._despawnObjs.Push(gameObj);
                }
            }
            /// <summary>
            /// Updates this instance.
            /// </summary>
            public void Update()
            {
                while (this._despawnObjs.Count > 0)
                {
                    GameObject gameObj = this._despawnObjs.Pop();
                    this._activeObjs.Remove(gameObj);
                    this._inactiveObjs.Push(gameObj);
                }
            }

            public override string ToString()
            {
                if (_prefabObj)
                {
                    return _prefabObj.name + " " + (activeCount + despawnCount + inactiveCount);
                }
                return base.ToString();
            }
        }

        #endregion

        #region FIELD

        /// <summary>The _need update</summary>
        private bool _needUpdate;
        /// <summary>The _spawn instances</summary>
        private Dictionary<GameObject, InstanceInfo> _allInstanceInfos = new Dictionary<GameObject, InstanceInfo>();
        /// <summary>The _spawn tables used</summary>
        private Dictionary<GameObject, InstanceTable> _allInstanceTables = new Dictionary<GameObject, InstanceTable>();

        #endregion

        #region METHOD

        /// <summary>Gets the or create table.</summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns></returns>
        private InstanceTable GetOrCreateTable(GameObject prefab)
        {
            InstanceTable instanceTable = null;
            if (prefab == null)
            {
                Debug.LogError("[KPool.GetOrCreateTable] prefab is null!");
            }
            else
            {
                if (!_allInstanceTables.TryGetValue(prefab, out instanceTable))
                {
                    instanceTable = new InstanceTable(prefab);
                    _allInstanceTables[prefab] = instanceTable;
                }
            }
            return instanceTable;
        }

        /// <summary>Does the pre spawn.</summary>
        /// <param name="prefab">The prefab.</param>
        /// <param name="count">The count.</param>
        private void DoPreSpawn(GameObject prefab, int count)
        {
            this.GetOrCreateTable(prefab).PreSpawn(count);
            _needUpdate = true;
        }

        /// <summary>Does the spawn.</summary>
        /// <param name="prefab">The prefab.</param>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <returns></returns>
        private GameObject DoSpawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (prefab == null)
            {
                Debug.LogError("[KPool.DoSpawn] prefab is null!");
                return null;
            }

            var createTable = this.GetOrCreateTable(prefab);
            GameObject gameObject = createTable.Spawn(position, rotation);
            InstanceInfo instanceInfo;
            if (_allInstanceInfos.TryGetValue(gameObject, out instanceInfo))
            {
                instanceInfo.Init(true, gameObject, createTable);
            }
            else
            {
                instanceInfo = new InstanceInfo(true, gameObject, createTable);
                _allInstanceInfos.Add(gameObject, instanceInfo);
            }
            _needUpdate = true;
            return gameObject;
        }

        /// <summary>Does the spawn.</summary>
        /// <param name="prefab">The prefab.</param>
        /// <param name="localPosition">The local position.</param>
        /// <param name="localRotation">The local rotation.</param>
        /// <param name="linkParent">The parent.</param>
        /// <returns></returns>
        private GameObject DoSpawn(GameObject prefab, Vector3 localPosition, Vector3 localRotation, Vector3 localScale, Transform linkParent)
        {
            if (prefab == null)
            {
                Debug.LogError("[KPool.DoSpawn] prefab is null!");
                return null;
            }

            var createTable = this.GetOrCreateTable(prefab);
            var gameObject = createTable.Spawn(localPosition, localRotation, localScale, linkParent);
            InstanceInfo instanceInfo;
            if (_allInstanceInfos.TryGetValue(gameObject, out instanceInfo))
            {
                instanceInfo.Init(true, gameObject, createTable);
            }
            else
            {
                instanceInfo = new InstanceInfo(true, gameObject, createTable);
                _allInstanceInfos.Add(gameObject, instanceInfo);
            }
            _needUpdate = true;
            return gameObject;
        }

        /// <summary>Does the despawn.</summary>
        /// <param name="gameObj">The game object.</param>
        private void DoDespawn(GameObject gameObj)
        {
            InstanceInfo instanceInfo;
            if (_allInstanceInfos.TryGetValue(gameObj, out instanceInfo))
            {
                instanceInfo.Despawn();
                _needUpdate = true;
                return;
            }

            UnityEngine.Object.Destroy(gameObj);
        }

        /// <summary>Does the despawn table.</summary>
        /// <param name="spawnTable">The spawn table.</param>
        private void DoDespawnTable(InstanceTable spawnTable)
        {
            foreach (var kvPair in _allInstanceInfos)
            {
                var instanceInfo = kvPair.Value;
                if (spawnTable == instanceInfo.instanceTable)
                {
                    instanceInfo.Despawn();
                }
            }
            _needUpdate = true;
        }

        /// <summary>Does the despawn all.</summary>
        private void DoDespawnAll()
        {
            foreach (var kvPair in _allInstanceInfos)
            {
                kvPair.Value.Despawn();
            }
            this._needUpdate = true;
        }

        #endregion

        #region UNITY

        /// <summary>Lates the update.</summary>
        private void LateUpdate()
        {
            if (_needUpdate)
            {
                foreach (var kvPair in _allInstanceTables)
                {
                    kvPair.Value.Update();
                }
                _needUpdate = false;
            }
        }

#if UNITY_EDITOR
        private void OnGUI()
        {
            int count = 0;
            foreach (var kvPair in _allInstanceTables)
            {
                count += kvPair.Value.activeCount;
            }
            GUI.Label(new Rect(10, 0, 90, 25), "pc:" + count);
        }
#endif

        #endregion

        #region STATIC

        private static KPool _Instance;

        /// <summary>Gets the instance.</summary>
        public static KPool Instance
        {
            get
            {
                if (!_Instance)
                {
                    _Instance = new GameObject("Pool").AddComponent<KPool>();
                }
                return _Instance;
            }
        }

        /// <summary>Spawns the specified prefab.</summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns></returns>
        public static GameObject Spawn(GameObject prefab)
        {
            return (KPool.Instance) ? KPool.Instance.DoSpawn(prefab, Vector3.zero, Quaternion.identity) : null;
        }

        /// <summary>Spawns the specified prefab.</summary>
        /// <param name="prefab">The prefab.</param>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <returns></returns>
        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return (KPool.Instance) ? KPool.Instance.DoSpawn(prefab, position, rotation) : null;
        }

        /// <summary>Spawns the specified prefab.</summary>
        /// <param name="prefab">The prefab.</param>
        /// <param name="localPosition">The local position.</param>
        /// <param name="localRotation">The local rotation.</param>
        /// <param name="linkParent">The link parent.</param>
        /// <returns></returns>
        public static GameObject Spawn(GameObject prefab, Vector3 localPosition, Vector3 localRotation, Transform linkParent)
        {
            return (KPool.Instance) ? KPool.Instance.DoSpawn(prefab, localPosition, localRotation, Vector3.one, linkParent) : null;
        }

        /// <summary>Spawns the specified prefab.</summary>
        /// <param name="prefab">The prefab.</param>
        /// <param name="localPosition">The local position.</param>
        /// <param name="localRotation">The local rotation.</param>
        /// <param name="linkParent">The link parent.</param>
        /// <returns></returns>
        public static GameObject Spawn(GameObject prefab, Vector3 localPosition, Vector3 localRotation, Vector3 localScale, Transform linkParent)
        {
            return (KPool.Instance) ? KPool.Instance.DoSpawn(prefab, localPosition, localRotation, localScale, linkParent) : null;
        }

        /// <summary>Despawns the specified object.</summary>
        /// <param name="obj">The object.</param>
        public static void Despawn(GameObject obj)
        {
            if (KPool.Instance && obj)
            {
                KPool.Instance.DoDespawn(obj);
            }
        }

        /// <summary>Spawns the specified prefab.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefab">The prefab.</param>
        /// <returns></returns>
        public static T Spawn<T>(T prefab) where T : Component
        {
            if (KPool.Instance && prefab)
            {
                GameObject gameObject = KPool.Instance.DoSpawn(prefab.gameObject, Vector3.zero, Quaternion.identity);
                if (gameObject)
                {
                    return gameObject.GetComponent<T>();
                }
            }
            return (T)((object)null);
        }

        /// <summary>Spawns the specified prefab.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefab">The prefab.</param>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <returns></returns>
        public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            if (KPool.Instance && prefab)
            {
                GameObject gameObject = KPool.Instance.DoSpawn(prefab.gameObject, position, rotation);
                if (gameObject)
                {
                    return gameObject.GetComponent<T>();
                }
            }
            return null;
        }

        /// <summary>Despawns the specified object.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        public static void Despawn<T>(T obj) where T : Component
        {
            if (KPool.Instance && obj)
            {
                KPool.Instance.DoDespawn(obj.gameObject);
            }
        }

        #endregion
    }
}