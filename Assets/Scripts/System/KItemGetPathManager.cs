// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 2017-10-24
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KMissionManager" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    using Callback = System.Action<int, string, object>;

    public class KItemGetPathManager : KGameModule
    {

        private Dictionary<int, KItemGetPath> _getpath = new Dictionary<int, KItemGetPath>();

        public Dictionary<int, KItemGetPath> DictGetPath
        {
            get
            {
                return _getpath;
            }
        }

        public void Load(Hashtable table)
        {
            if (table != null)
            {
                var list = table.GetArrayList("Getpath");
                if (list != null && list.Count > 0)
                {
                    var tmpT = new Hashtable();
                    for (int i = 0; i < list.Count - 1; i++)
                    {
                        var tmpL0 = (ArrayList)list[0];
                        var tmpLi = (ArrayList)list[i + 1];
                        for (int j = 0; j < tmpL0.Count; j++)
                        {
                            tmpT[tmpL0[j]] = tmpLi[j];
                        }
                        var getpath = new KItemGetPath();
                        getpath.Load(tmpT);
                        _getpath.Add(getpath.id, getpath);
                    }
                }
            }
        }




        public string ShowGetPaths(int[] pathArry)
        {
            string pathString = string.Empty;
            List<string> lstString = new List<string>();
            List<int> lstInt = new List<int>();
            foreach (var item in _getpath.Values)
            {
                lstInt.Add(item.description);
            }
            foreach (var item in _getpath.Values)
            {
                lstString.Add(KLocalization.GetLocalString(item.description));
            }
            for (int i = 0; i < lstInt.Count; i++)
            {
                for (int ji = 0; ji < pathArry.Length; ji++)
                {
                    if (lstInt[i] == pathArry[ji])
                    {
                        if (pathString == string.Empty)
                        {
                            pathString = lstString[i];
                        }
                        else
                        {
                            pathString = pathString + "," + lstString[i];
                        }
                    }
                }
            }
            return pathString;
        }

        public string ShowGetPath(int pathid)
        {
            return KLocalization.GetLocalString(_getpath[pathid].description);
        }

        #region Unity
        private void Awake()
        {
            Instance = this;
        }

        public override void Load()
        {
            TextAsset tmpText;
            if (KAssetManager.Instance.TryGetExcelAsset("Getpath", out tmpText))
            {
                if (tmpText)
                {
                    var tmpJson = tmpText.bytes.ToJsonTable();
                    Load(tmpJson);
                }
            }
        }

        #endregion

        #region STATIC

        public static KItemGetPathManager Instance;

        #endregion
    }
}