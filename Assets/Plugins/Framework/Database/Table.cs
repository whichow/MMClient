// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "Table" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;

public class Table
{
    /// <summary>
    /// 行数据
    /// </summary>
    private List<Row> _rowList = new List<Row>();
    private Dictionary<int, Row> _rowDictionary = new Dictionary<int, Row>();
    /// <summary>
    /// 列数据
    /// </summary>
    private List<Column> _columnList = new List<Column>();
    private Dictionary<string, Column> _columnDictionary = new Dictionary<string, Column>();

    internal Table(Database database, Schema schema)
    {
        this.database = database;

        this.name = schema.name;
        foreach (var column in schema.columns)
        {
            AddColumn(new Column(column));
        }
    }

    /// <summary>
    /// 表名
    /// </summary>
    public string name
    {
        get;
        private set;
    }

    /// <summary>
    /// 数据库
    /// </summary>
    public Database database
    {
        get;
        private set;
    }

    /// <summary>
    /// 行数
    /// </summary>
    public int rowCount
    {
        get { return _rowDictionary.Count; }
    }

    /// <summary>
    /// 列数
    /// </summary>
    public int columnCount
    {
        get { return _columnDictionary.Count; }
    }

    /// <summary>
    /// 增加列
    /// </summary>
    /// <param name="column"></param>
    private void AddColumn(Column column)
    {
        column.SetTable(this, _columnDictionary.Count);
        if (!_columnDictionary.ContainsKey(column.name))
        {
            _columnDictionary.Add(column.name, column);
            _columnList.Add(column);
        }
    }

    /// <summary>
    /// 获取列(名字)
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Column GetColumn(string name)
    {
        Column ret;
        _columnDictionary.TryGetValue(name, out ret);
        return ret;
    }

    /// <summary>
    /// 获取列(序号)
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Column GetColumnByIndex(int index)
    {
        return _columnList[index];
    }

    /// <summary>
    /// 获取所有列
    /// </summary>
    /// <returns></returns>
    public Column[] GetColumns()
    {
        return _columnList.ToArray();
    }

    internal int GetColumnIndex(string name)
    {
        var column = this.GetColumn(name);
        return column.index;
    }

    /// <summary>
    /// 增加行数据
    /// </summary>
    /// <param name="id"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public Row SetRow(int id, object[] values)
    {
        Row ret;
        if (!_rowDictionary.TryGetValue(id, out ret))
        {
            ret = new Row(this, id);
            _rowDictionary.Add(id, ret);
            _rowList.Add(ret);
        }

        ret.SetValues(values);
        return ret;
    }

    /// <summary>
    /// 增加行数据
    /// </summary>
    /// <param name="id"></param>
    /// <param name="column"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public Row SetRow(int id, int column, object value)
    {
        Row ret;
        if (!_rowDictionary.TryGetValue(id, out ret))
        {
            ret = new Row(this, id);
            _rowDictionary.Add(id, ret);
            _rowList.Add(ret);
        }

        ret.SetValue(column, value);
        return ret;
    }
    /// <summary>
    /// 增加行数据
    /// </summary>
    /// <param name="id"></param>
    /// <param name="column"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public Row SetRow(int id, string column, object value)
    {
        Row ret;
        if (!_rowDictionary.TryGetValue(id, out ret))
        {
            ret = new Row(this, id);
            _rowDictionary.Add(id, ret);
            _rowList.Add(ret);
        }

        ret.SetValue(column, value);
        return ret;
    }
    /// <summary>
    /// 获取行(id)
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Row GetRow(int id)
    {
        Row ret;
        _rowDictionary.TryGetValue(id, out ret);
        return ret;
    }

    /// <summary>
    /// 获取行(序号)
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Row GetRowByIndex(int index)
    {
        return _rowList[index];
    }

    /// <summary>
    /// 获取所有行
    /// </summary>
    /// <returns></returns>
    public Row[] GetRows()
    {
        return _rowList.ToArray();
    }

    /// <summary>
    /// 获取数据(高效不检查)
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public object GetValue(int id, int column)
    {
        Row row;
        if (_rowDictionary.TryGetValue(id, out row))
        {
            return row[column];
        }
        return null;
    }
    /// <summary>
    /// 获取数据(高效不检查)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public object GetValue(int id, string column)
    {
        Row row;
        if (_rowDictionary.TryGetValue(id, out row))
        {
            return row[column];
        }
        return null;
    }
}
