using System;

namespace Framework.Core
{
    public class UpdateBase : IUpdateable, IDispose
    {
        public bool blEnable { get; set; }

        public UpdateBase()
        {

        }

        protected virtual void Initialize()
        {
            blEnable = true;
            GameComponent.Instance.AddUpdateComponent(this);
        }

        protected virtual void Remove()
        {
            blEnable = false;
            GameComponent.Instance.RemoveUpdateComponent(this);
        }

        public virtual void Update()
        {

        }

        public virtual void Dispose()
        {
            Remove();
        }
    }

}