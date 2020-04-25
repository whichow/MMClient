// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "ConsoleCommand" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;

namespace K.Console
{
    /// <summary>
    /// Use Console.Log() anywhere in your code. The Console prefab will display the output.
    /// </summary>
    public class Console : MonoBehaviour
    {


        public static void Log(string line)
        {
            Debug.Log(line);
            if (_ConsoleWindow)
            {
                _ConsoleWindow.AppendLine(line);
            }
        }

        public static void Clear()
        {
            _ConsoleWindow.ClearOutput();
        }

        public static void Hide()
        {
            _ConsoleWindow.ClearOutput();
            _CanvasObject.SetActive(false);
        }

        public static string ExecuteCommand(string command, string[] args)
        {
            return ConsoleCommand.ExecuteCommand(command, args);
        }

        #region Field

        private static ConsoleWindow _ConsoleWindow;
        private static GameObject _CanvasObject;

        private int _touchFrame;
        private int _touchCount;

        #endregion

        #region Unity

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _CanvasObject = transform.Find("Canvas").gameObject;
            _ConsoleWindow = transform.Find("Canvas/Console").GetComponent<ConsoleWindow>();
        }

        private void Start()
        {
            ConsoleCommand.RegisterCommand(HelpCommand.Name, HelpCommand.Description, HelpCommand.Specification, HelpCommand.Execute);
            ConsoleCommand.RegisterCommand(ClearCommand.Name, ClearCommand.Description, ClearCommand.Specification, ClearCommand.Execute);
            ConsoleCommand.RegisterCommand(CloseCommand.Name, CloseCommand.Description, CloseCommand.Specification, CloseCommand.Execute);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                _CanvasObject.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                _CanvasObject.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_CanvasObject.activeSelf)
                {
                    _ConsoleWindow.SelectInputHistory(true);
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (_CanvasObject.activeSelf)
                {
                    _ConsoleWindow.SelectInputHistory(false);
                }
            }
            //match3

#if !UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                var mp = Input.mousePosition;
                if (mp.x > 150 || mp.y > 150)
                {
                    return;
                }

                if (Time.frameCount - _touchFrame > 60)
                {
                    _touchCount = 0;
                }

                if (_touchCount == 0)
                {
                    _touchFrame = Time.frameCount;
                }

                if (_touchCount++ > 3)
                {
                    _touchFrame = 0;
                    _touchCount = 0;
                    _CanvasObject.SetActive(!_CanvasObject.activeSelf);
                }
            }
#endif
        }

        #endregion
    }
}