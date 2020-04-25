using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game
{
    public class TabItem : MonoBehaviour
    {
        private Animation _animation;
        private int CatNo;
        public Text CountOfNew;
        private bool exActive = true;
        public Image icon;
        private Button myButton;
        private Action<int> onClick;
        public Image tabImage;

        private void Awake()
        {
            this._animation = base.GetComponent<Animation>();
        }

        public void Init(Sprite pIcon, string pName, Sprite tabIcon, bool pActivate, int pCatNo, Action<int> pOnClick)
        {
            this.onClick = pOnClick;
            this.CatNo = pCatNo;
            this.icon.sprite = pIcon;
            this.SetMaterial(pActivate);
            this.tabImage.sprite = tabIcon;
            if ((this.exActive != pActivate) && (this._animation != null))
            {
                if (!pActivate)
                {
                    this._animation.Play("Normal");
                }
                else
                {
                    this._animation.Play("Pressed");
                }
            }
            else if (pActivate)
            {
                this._animation.Play("Pressed");
            }
            this.exActive = pActivate;
        }

        public void Init(Sprite pIcon, string pName, int pCountNew, Sprite tabIcon, bool pActivate, int pCatNo, Action<int> pOnClick)
        {
            this.onClick = pOnClick;
            this.CatNo = pCatNo;
            if (pCountNew < 1)
            {
                this.CountOfNew.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                this.CountOfNew.transform.parent.gameObject.SetActive(true);
                this.CountOfNew.text = pCountNew.ToString();
            }
            this.icon.sprite = pIcon;
            this.tabImage.sprite = tabIcon;
            this.SetMaterial(pActivate);
            if ((this.exActive != pActivate) && (this._animation != null))
            {
                if (!pActivate)
                {
                    this._animation.Play("Normal");
                }
                else
                {
                    this._animation.Play("Pressed");
                }
            }
            else if (pActivate)
            {
                this._animation.Play("Pressed");
            }
            this.exActive = pActivate;
        }

        public void OnClick()
        {
            if (this.onClick != null)
            {
                this.onClick(this.CatNo);
            }
        }

        private void OnDisable()
        {
            if (this.myButton != null)
            {
                base.GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }

        private void OnEnable()
        {
            if (base.GetComponent<Button>() != null)
            {
                this.myButton = base.GetComponent<Button>();
                this.myButton.onClick.AddListener(new UnityAction(this.OnClick));
            }
        }

        public void Reset(bool pActivate, int pCountNew = -1)
        {
            this.SetMaterial(pActivate);
            if ((this.exActive != pActivate) && (this._animation != null))
            {
                if (!pActivate)
                {
                    this._animation.Play("Normal");
                }
                else
                {
                    this._animation.Play("Pressed");
                }
            }
            else if (pActivate)
            {
                this._animation.Play("Pressed");
            }
            this.exActive = pActivate;
            if (pCountNew != -1)
            {
                if (pCountNew < 1)
                {
                    this.CountOfNew.transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    this.CountOfNew.transform.parent.gameObject.SetActive(true);
                    this.CountOfNew.text = pCountNew.ToString();
                }
            }
        }

        public void SetActive(bool act)
        {
            if (!act)
            {
                this._animation.Play("Normal");
            }
            else
            {
                this._animation.Play("Pressed");
            }
            this.SetMaterial(act);
        }

        private void SetMaterial(bool act)
        {
            //this.icon.material = !act ? null : Materials.Get(MaterialType.UIPost);
            //this.tabImage.material = !act ? null : Materials.Get(MaterialType.UIPost);
        }
    }
}