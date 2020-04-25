// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
using System.Collections.Generic;
using UnityEngine;

namespace K.AB
{
    [System.Serializable]
    public class ABFilter
    {
        public bool valid = true;
        public string path = string.Empty;
        public string filter = "*.prefab";
        public BuildType buildType;
    }

    public class ABBuildConfig : ScriptableObject
    {
        public List<ABFilter> filters = new List<ABFilter>();
    }
}