/** 
 *FileName:     EditorLaunch.cs 
 *Author:        
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-12-25 
 *Description:    
 *History: 
*/
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace M3Editor
{
    public class EditorLaunch : MonoBehaviour
    {

        private void Awake()
        {
            KGameModuleManager.Instance.OnStart();
            KGameModuleManager.Instance.Load();
            SceneManager.LoadScene("M3EditorScene");
        }
    }
}
