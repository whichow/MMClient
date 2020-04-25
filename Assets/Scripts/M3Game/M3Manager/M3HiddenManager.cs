/** 
*FileName:     M3HiddenManager.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-12-28 
*Description:    
*History: 
*/
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    public class M3HiddenManager
    {
        private List<HiddenElement> hiddenList;

        public void Init()
        {
            List<M3HiddenModel> modelList = M3GameManager.Instance.level.HiddenList;
            hiddenList = new List<HiddenElement>();
            for (int i = 0; i < modelList.Count; i++)
            {
                if (IsRandomHidden(modelList[i]))
                {
                    CreateRandomHidden(modelList[i]);
                }
                else
                {
                    CreateHidden(modelList[i].list[0], modelList[i].type);
                }
            }
        }

        private void CreateHidden(M3HiddenItem item, int type)
        {
            HiddenElement element = new HiddenElement();
            element.Init(new Int2(item.from.x, item.from.y), new Int2(item.to.x, item.to.y));
            hiddenList.Add(element);
        }

        private void CreateRandomHidden(M3HiddenModel m3HiddenModel)
        {
            Debug.Log(m3HiddenModel.count);
            for (int i = 0; i < m3HiddenModel.count; i++)
            {
                int random = M3Supporter.Instance.GetRandomInt(0, m3HiddenModel.list.Length);
                Debug.Log(random);
                Debug.Log(m3HiddenModel.list.Length);
                CreateHidden(m3HiddenModel.list[random], m3HiddenModel.type);
                M3HiddenItem[] tmp = new M3HiddenItem[m3HiddenModel.list.Length - 1];
                bool flag = false;
                for (int j = 0; j < m3HiddenModel.list.Length; j++)
                {
                    if (j != random)
                    {
                        if (!flag)
                            tmp[j] = m3HiddenModel.list[j];
                        else
                        {
                            tmp[j - 1] = m3HiddenModel.list[j];
                        }
                    }
                    else
                    {
                        flag = !flag;
                    }
                }
                m3HiddenModel.list = tmp;
                //List<M3HiddenItem> tempList = new List<M3HiddenItem>();
                //for (int j = 0; j < m3HiddenModel.list.Length; j++)
                //{
                //    tempList.Add(m3HiddenModel.list[j]);
                //}
                //tempList.RemoveAt(random);
                //m3HiddenModel.list = tempList.ToArray();
            }
        }

        public bool IsRandomHidden(M3HiddenModel model)
        {
            return model.count != 1;
        }

        public void CrushHiddens()
        {
            if (hiddenList.Count == 0)
                return;
            List<HiddenElement> list = GetCanCrushHidden();
            for (int i = 0; i < list.Count; i++)
            {
                this.hiddenList.Remove(list[i]);
                list[i].Crush();
                Debug.Log("Clear");
            }
        }

        public List<HiddenElement> GetCanCrushHidden()
        {
            List<HiddenElement> list = new List<HiddenElement>();
            for (int i = 0; i < this.hiddenList.Count; i++)
            {
                if (this.hiddenList[i].CanCrush())
                {
                    list.Add(this.hiddenList[i]);
                }
            }
            return list;
        }

        public List<HiddenElement> GetCurrentHiddens()
        {
            return this.hiddenList;
        }

    }
}