using Game.UI;
using System;

namespace Game.Match3
{
    public class M3GameUIManager
    {

        public M3GameUIManager()
        {
        }

        public void OpenWindow<T>(object windowData = null, Action callback = null) where T : KUIWindow, new()
        {
            KUIWindow.OpenWindow<T>(windowData, callback);
        }
        public void CloseWindow<T>() where T : KUIWindow, new()
        {
            KUIWindow.CloseWindow<T>();
        }
        public void EnterGame()
        {
            KUIWindow.OpenWindow<M3GameUIWindow>();
        }
    }
}