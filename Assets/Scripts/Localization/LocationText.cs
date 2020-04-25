
using UnityEngine.UI;

namespace Game
{
    public class LocationText : Text
    {
        public int IDLanguage;

        protected override void Awake()
        {
            base.Awake();
            LocationMgr.Instance.RegisteText(this);
        }

        public bool IDInValid()
        {
            return IDLanguage == 0 || IDLanguage == -100;
        }

        public override string text
        {
            get
            {
                return base.text;
            }

            set
            {
                base.text = value;
            }
        }

        protected new void OnDestroy()
        {
            LocationMgr.Instance.UnRegistText(this);
        }
    }
}