/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/23 15:29:35
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataModel
{
    public partial class XTable<TRow> where TRow : new()
    {
        protected bool m_bReady;
        protected Dictionary<int, TRow> m_TableDic;

        public virtual string ResourceName
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReady { get { return m_bReady; } }                                        // 是否准备就绪

        /// <summary>
        /// 文件名和表名统一了后，可用此方法直接加载，一个表对应一个文件
        /// </summary>
        public virtual void Load()
        {
            Load(null);
        }

        public virtual void Load(Action callback)
        {
            if (m_TableDic != null)
            {
                callback?.Invoke();
            }
            else
            {
                TextAsset tmpText;
                if (KAssetManager.Instance.TryGetExcelAsset(ResourceName, out tmpText))
                {
                    if (tmpText)
                    {
                        Hashtable tmpTable = tmpText.bytes.ToJsonTable();
                        LoadFromHashtable(tmpTable);
                        callback?.Invoke();
                    }
                }
            }
        }

        public virtual void LoadFromHashtable(Hashtable table)
        {
            m_TableDic = new Dictionary<int, TRow>();
            var list = table.GetArrayList(ResourceName);
            if (list == null)
            {
                Debuger.LogErrorFormat("[XTable-{0}.LoadFromHashtable] no data!", ResourceName);
            }
            list.Resolve((t) =>
            {
                TRow row = new TRow();
                (row as IXDM).Parse(t);
                m_TableDic.Add((row as IXDM).ID, row);
                Parseed((row as IXDM));
            });
            m_bReady = true;
        }

        protected virtual void Parseed(IXDM xdm)
        {

        }

        public bool ContainsID(int id)
        {
            return m_TableDic.ContainsKey(id);
        }

        public TRow GetByID(int id)
        {
            TRow value;
            if (!m_TableDic.TryGetValue(id, out value))
            {
                Debuger.LogErrorFormat("[XTable-{0}.GetByID] not found id! {1}", ResourceName , id);
                return default(TRow);
            }
            return value;
        }

        public List<TRow> GetAllList()
        {
            List<TRow> list = new List<TRow>();
            foreach (var item in m_TableDic)
            {
                list.Add((TRow)item.Value);
            }
            return list;
        }

    }
}
