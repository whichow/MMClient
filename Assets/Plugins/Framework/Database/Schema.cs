// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "Schema" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;

public class Schema
{
    private List<string> _columns = new List<string>();

    public string name
    {
        get;
        private set;
    }

    public List<string> columns
    {
        get { return _columns; }
    }

    public Schema(string name, string[] columns)
    {
        if (!string.IsNullOrEmpty(name))
        {
            this.name = name;
        }

        if (columns != null && columns.Length > 0)
        {
            _columns.AddRange(columns);
        }
    }

    public void AddColumn(string name)
    {
        _columns.Add(name);
    }

    public void AddColumns(string[] names)
    {
        _columns.AddRange(names);
    }
}
