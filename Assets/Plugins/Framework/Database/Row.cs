// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "Row" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;

public class Row : IRow
{
    private Table _table;
    private object[] _data;

    public int id
    {
        get;
        private set;
    }

    public int columnCount
    {
        get { return _table.columnCount; }
    }

    public object this[int index]
    {
        get { return _data[index]; }
        set { _data[index] = value; }
    }

    public object this[string name]
    {
        get { return _data[_table.GetColumnIndex(name)]; }
        set { _data[_table.GetColumnIndex(name)] = value; }
    }

    internal Row(Table table, int id)
    {
        _table = table;
        _data = new object[table.columnCount];
    }

    internal void SetValue(int column, object value)
    {
        if (column < _data.Length)
        {
            _data[column] = value;
        }
    }
    internal void SetValue(string column, object value)
    {
        _data[_table.GetColumnIndex(column)] = value;
    }

    internal void SetValues(object[] data)
    {
        if (data != null)
        {
            int dl1 = _data.Length;
            int dl2 = data.Length;
            for (int i = 0; i < dl1 && i < dl2; i++)
            {
                _data[i] = data[i];
            }
        }
    }

    public Column GetColumn(int index)
    {
        return _table.GetColumnByIndex(index);
    }

    public Column GetColumn(string name)
    {
        return _table.GetColumn(name);
    }
}
