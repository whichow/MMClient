/** 
 *FileName:     #SCRIPTFULLNAME# 
 *Author:       #AUTHOR# 
 *Version:      #VERSION# 
 *UnityVersion：#UNITYVERSION#
 *Date:         #DATE# 
 *Description:    
 *History: 
*/
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class M3TargetItem : KUIItem
    {

        private Image icon;
        private Text count;
        private Image complete;
        public Sprite iconSprite;
        private void Awake()
        {
            icon = transform.Find("icon").GetComponent<Image>();
            count = transform.Find("num").GetComponent<Text>();
            complete = transform.Find("complete").GetComponent<Image>();
            complete.gameObject.SetActive(false);
        }

        public void UpdateData(string iconName, int num)
        {

            icon.sprite = Game.KIconManager.Instance.GetMatch3ElementIcon(iconName);
            iconSprite = icon.sprite;
            count.text = num.ToString();
        }

        public void ReduceNum(int num)
        {
            count.text = num.ToString();
            if (num <= 0)
            {
                count.gameObject.SetActive(false);
                complete.gameObject.SetActive(true);
            }
            KTweenUtils.ScaleTo(transform, new Vector3(1.2f, 1.2f, 1.2f), 0.12f, delegate ()
            {

                KTweenUtils.ScaleTo(transform, new Vector3(0.9f, 0.9f, 0.9f), 0.12f, delegate ()
                {
                    KTweenUtils.ScaleTo(transform, Vector3.one, 0);

                });
            });
        }
    }
}