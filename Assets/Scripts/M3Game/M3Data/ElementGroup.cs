///** 
// *FileName:     ElementGroup.cs 
// *Author:       HeJunJie 
// *Version:      1.0 
// *UnityVersion：5.6.2f1
// *Date:         2018-01-15 
// *Description:    
// *History: 
//*/
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Game.Match3
//{
//    public class ElementGroup
//    {
//        public int id;
//        public string name;
//        public List<List<int>> elementID;



//        public static ElementGroup[] elementGroup;
//        public static ElementGroup GetElementGroup(int id)
//        {
//            var group = System.Array.Find(elementGroup, g => g.id == id);
//            return group;
//        }
//        public static void Load(Hashtable table)
//        {
//            var elementGroup = table.GetArrayList("ElementGroup");
//            if (elementGroup != null && elementGroup.Count > 0)
//            {
//                var tmpCT = new Hashtable();

//                ElementGroup.elementGroup = new ElementGroup[elementGroup.Count - 1];
//                for (int i = 0; i < ElementGroup.elementGroup.Length; i++)
//                {
//                    var tmpL0 = (ArrayList)elementGroup[0];
//                    var tmpLi = (ArrayList)elementGroup[i + 1];
//                    for (int j = 0; j < tmpL0.Count; j++)
//                    {
//                        tmpCT[tmpL0[j]] = tmpLi[j];
//                    }

//                    ElementGroup.elementGroup[i] = new ElementGroup();
//                    ElementGroup.elementGroup[i].Parse(tmpCT);
//                }
//            }
//        }
//        public void Parse(Hashtable table)
//        {
//            this.id = table.GetInt("ID");
//            this.name = table.GetString("Name");
//            //var list = table.GetList("ElementId");
//            //for (int i = 0; i < list.Count; i++)
//            //{
//            //    var arrlist = list.GetArray<int>(i);
//            //    List<int> tmpList = new List<int>();
//            //    for (int j = 0; j < arrlist.Length; j++)
//            //    {
//            //        tmpList.Add(arrlist[j]);
//            //    }
//            //    this.elementID.Add(tmpList);
//            //}
//        }
//    }
//}