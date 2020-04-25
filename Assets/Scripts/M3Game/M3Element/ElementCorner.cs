/** 
 *FileName:     ElementCorner.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2018-01-05 
 *Description:    
 *History: 
*/
using UnityEngine;

namespace Game.Match3
{
    /// <summary>
    /// 元素角标
    /// </summary>
    public class ElementCorner : MonoBehaviour
    {
        private TextMesh num1;
        private TextMesh num2;
        private TextMesh num3;

        private void Awake()
        {
            num1 = transform.Find("1").GetComponent<TextMesh>();
            num2 = transform.Find("2").GetComponent<TextMesh>();
            num3 = transform.Find("3").GetComponent<TextMesh>();
            num1.gameObject.SetActive(false);
            num2.gameObject.SetActive(false);
            num3.gameObject.SetActive(false);
        }

        public void ShowNumber(int num)
        {
            if (num == 1)
                return;
            if (num > 999)
            {
                num1.text = "9";
                num2.text = "9";
                num3.text = "9";
            }
            else
            {
                SetNumber(num1, GetUnit(num), 0);
                SetNumber(num2, GetTen(num), 1);
                SetNumber(num3, GetThousand(num), 1);
            }
        }

        private void SetNumber(TextMesh numTextMesh, int number, int type)
        {
            numTextMesh.text = number.ToString();
            if (number <= (type == 1 ? 0 : 1))
            {
                numTextMesh.gameObject.SetActive(false);
            }
            else
            {
                numTextMesh.gameObject.SetActive(true);
            }
        }

        public int GetUnit(int num)
        {
            int a = num % 10;
            return a;
        }

        public int GetTen(int num)
        {
            int a = num % 100 / 10;
            return a;
        }

        public int GetThousand(int num)
        {
            int a = num % 1000 / 100;
            return a;
        }

    }
}