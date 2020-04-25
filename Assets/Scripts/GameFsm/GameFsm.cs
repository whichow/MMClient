// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "GameFsm" company=""></copyright>
// <summary></summary>
// ***********************************************************************
namespace Game
{
    using UnityEngine;
    public class GameFsm : MonoBehaviour
    {
        public static GameFsm Instance;

        #region Unity  

        private void Awake()
        {
            Instance = this;
        }
        // Use this for initialization
        private void Start()
        {
            KFramework.FsmManager.CreateFsm(this,
                new InitState(),
                new CheckState(),
                new LoginState(),
                new InGameState(),
                new InLevelState(),
                new M3EditorState()).Start<InitState>();
        }

        #endregion
    }
}

