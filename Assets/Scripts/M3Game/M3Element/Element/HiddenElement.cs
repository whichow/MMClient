/** 
 *FileName:     HiddenElement.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-12-28 
 *Description:    
 *History: 
*/
using Game.DataModel;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    public class HiddenElement : Element
    {
        public Int2 from;

        public Int2 to;

        public GameObject go;

        public bool isBox;

        public void Init(Int2 f, Int2 t)
        {
            from = f;
            to = t;
            CreateHidden();
            SetPosition();
            data = new ElementData();
            data.Init(3055);
        }

        public void CreateHidden()
        {
            this.isBox = Mathf.Abs(from.x - to.x) == Mathf.Abs(from.y - to.y);

            GameObject tmp = null;
            KAssetManager.Instance.TryGetMatchPrefab("TempFish", out tmp);
            go = GameObject.Instantiate(tmp);
            go.transform.SetParent(M3GridManager.Instance.gridParent.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
        }

        public void SetPosition()
        {
            go.transform.localPosition = (M3Supporter.Instance.GetItemPositionByGrid(from.x, from.y) + M3Supporter.Instance.GetItemPositionByGrid(to.x, to.y)) / 2;
            float num = GetScale();
            go.transform.localScale = new Vector3(num, num, 1);
        }

        private float GetScale()
        {
            int numX = Mathf.Abs(from.x - to.x) + 1;
            int numY = Mathf.Abs(from.y - to.y) + 1;

            int num = numX > numY ? numY : numX;

            return num * 0.22f;
        }

        public bool CanCrush()
        {
            bool flag = true;
            for (int i = (this.from.x >= this.to.x) ? this.to.x : this.from.x; i <= ((this.from.x >= this.to.x) ? this.from.x : this.to.x); i++)
            {
                for (int j = (this.from.y >= this.to.y) ? this.to.y : this.from.y; j <= ((this.from.y >= this.to.y) ? this.from.y : this.to.y); j++)
                {
                    if (M3GameManager.Instance.CheckGridValid(i, j) && M3GridManager.Instance.gridCells[i, j].gridInfo.HaveIce)
                    {
                        flag = false;
                        break;
                    }
                }
                if (!flag)
                {
                    break;
                }
            }
            return flag;
        }

        public override void Crush()
        {
            base.Crush();
            OnDisappear();
        }

        public override void OnDisappear()
        {
            base.OnDisappear();
            M3GameManager.Instance.modeManager.GameModeCtrl.AddTarget(new List<int> { data.config.ID }, go.transform.position, this);
            GameObject.Destroy(go);
        }

    }
}