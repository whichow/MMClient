/** 
 *FileName:     M3CellData.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-07-10 
 *Description:    单元格数据
 *History: 
*/
using System.Collections;
using System.Collections.Generic;

namespace Game.Match3
{
    /// <summary>
    /// 三消单元格数据 （编辑器导出）
    /// </summary>
    public class M3CellData
    {
        public int index;

        /// <summary>
        /// 格子棋盘X坐标(第几行)  坐标系是x向下，y向右 （坐标体系是反的，要特别注意）
        /// </summary>
        public int gridX;
        /// <summary>
        /// 格子棋盘Y坐标(第几列)  坐标系是x向下，y向右 （坐标体系是反的，要特别注意）
        /// </summary>
        public int gridY;

        /// <summary>
        /// 格子是否显示 false表示该单元格被去除
        /// </summary>
        public bool isActive;

        /// <summary>
        /// 默认生成元素是否随机
        /// </summary>
        public bool isRandomElement;

        /// <summary>
        /// 是否是空格子 空则不会默认生成元素
        /// </summary>
        public bool isEmpty;

        /// <summary>
        /// 元素列表
        /// </summary>
        public List<int> elementsList;

        #region C&D
        public M3CellData()
        {
            elementsList = new List<int>();
        }

        public M3CellData(int x, int y, int mindex, bool active = true)
        {
            gridX = x;
            gridY = y;
            isActive = active;
            index = mindex;
            elementsList = new List<int>();
        }
        #endregion

        public void Parse(Hashtable table)
        {
            isActive = true;
            elementsList = new List<int>();

            index = table.GetInt("index");
            gridX = table.GetInt("row");
            gridY = table.GetInt("col");

            isEmpty = table.GetInt("empty") == 1;
            isRandomElement = table.GetInt("random") == 1;

            string ele = table.GetString("elements");
            if (ele == "-1")
            {
                isActive = false;
                elementsList.Add(-1);
            }
            else if (ele == "0")
            {
            }
            else
            {
                string[] eles = ele.Split('|');
                int tempId = 0;
                for (int i = 0; i < eles.Length; i++)
                {
                    tempId = int.Parse(eles[i]);
                    if (tempId == 0)
                        continue;
                    elementsList.Add(tempId);
                }
            }
        }

    }
}