// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KGameModuleManager" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class KGameModuleManager : SingletonUnity<KGameModuleManager>
    {
        List<KGameModule> _gameModules = new List<KGameModule>();

        public void Init()
        {
            _gameModules.ForEach(gm => gm.Init());
            _gameModules.ForEach(gm => gm.InitComplete());
        }

        public void Load()
        {
            _gameModules.ForEach(gm => gm.Load());
            _gameModules.ForEach(gm => gm.LoadComplete());
        }

        // Use this for initialization
        void Start()
        {
            OnStart();
        }

        public void OnStart()
        {
            _gameModules.Clear();
            GetComponentsInChildren(true, _gameModules);
            _gameModules.Sort((a, b) => b.priority.CompareTo(a.priority));
        }
    }
}