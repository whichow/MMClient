using System.Collections.Generic;

namespace Framework.Core
{
    public class GameComponent : Singleton<GameComponent>
    {
        private List<IUpdateable> _lstUpdateComponents;

        public void Init()
        {
            _lstUpdateComponents = new List<IUpdateable>();
        }

        public void AddUpdateComponent(IUpdateable component)
        {
            if (_lstUpdateComponents.Contains(component))
            {
                Debuger.LogWarning("[GameComponent.AddUpdateComponent() => add update component repeated!!!]");
                return;
            }
            _lstUpdateComponents.Insert(0, component);
        }

        public void RemoveUpdateComponent(IUpdateable component)
        {
            if (!_lstUpdateComponents.Contains(component))
            {
                Debuger.LogWarning("[GameComponent.RemoveUpdateComponent() => remove update component, but not found!!!]");
                return;
            }
            _lstUpdateComponents.Remove(component);
        }

        int i = 0;
        public void Update()
        {
            if (_lstUpdateComponents == null || _lstUpdateComponents.Count == 0)
                return;
            for (i = _lstUpdateComponents.Count - 1; i >= 0; i--)
            {
                if (_lstUpdateComponents[i].blEnable)
                    _lstUpdateComponents[i].Update();
            }
        }
    }
}