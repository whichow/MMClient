///** 
//*FileName:     GiveNameItem.cs 
//*Author:       HeJunJie 
//*Version:      1.0 
//*UnityVersion：5.6.2f1
//*Date:         2018-01-10 
//*Description:    
//*History: 
//*/
//using Game.Match3;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Game.UI
//{
//    public class GiveNameItem : MonoBehaviour
//    {

//        private Image catBlue1;
//        private Image catBlue2;
//        private Image catBlue3;

//        private Image catYellow1;
//        private Image catYellow2;
//        private Image catYellow3;
//        private void Awake()
//        {
//            catBlue1 = transform.Find("cat1").GetComponent<Image>();
//            catBlue2 = transform.Find("cat2").GetComponent<Image>();
//            catBlue3 = transform.Find("cat3").GetComponent<Image>();

//            catYellow1 = transform.Find("cat1 (1)").GetComponent<Image>();
//            catYellow2 = transform.Find("cat2 (1)").GetComponent<Image>();
//            catYellow3 = transform.Find("cat3 (1)").GetComponent<Image>();

//            catBlue1.overrideSprite = KIconManager.Instance.GetMatch3ElementIcon(ElementConfig.Get(1003).icon);
//            catBlue2.overrideSprite = KIconManager.Instance.GetMatch3ElementIcon(ElementConfig.Get(1003).icon);
//            catBlue3.overrideSprite = KIconManager.Instance.GetMatch3ElementIcon(ElementConfig.Get(1003).icon);

//            catYellow1.overrideSprite = KIconManager.Instance.GetMatch3ElementIcon(ElementConfig.Get(1002).icon);
//            catYellow2.overrideSprite = KIconManager.Instance.GetMatch3ElementIcon(ElementConfig.Get(1002).icon);
//            catYellow3.overrideSprite = KIconManager.Instance.GetMatch3ElementIcon(ElementConfig.Get(1002).icon);

//        }
//    }
//}