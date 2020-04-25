// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using Game.UI;
using UnityEngine;

namespace Game.Build
{
    public class GameManager : MonoBehaviour
    {
        private void OnOpenWindow()
        {

        }

        #region Unity
        private void Awake()
        {
            KLoading.LoadCompleted();
        }

        // Use this for initialization
        private void OnEnable()
        {
            KUIWindow.OpenWindow<MainWindow>();
            if (GuideManager.Instance.status != GuideManager.EGuideStatus.Completed)
            {
                KUIWindow.OpenWindow<GuideWindow>();
            }
            AudioClip clip;
            if (KAssetManager.Instance.TryGetSoundAsset("Music/music_worldscene", out clip))
            {
                KSoundManager.PlayMusic(clip, false);
            }
        }

        // Update is called once per frame
        private void Update()
        {

        }

        #endregion
    }
}
