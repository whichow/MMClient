// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "Column" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System;

public class Column : IColumn
{
    public string name { get; private set; }

    public int index { get; private set; }

    public Type type { get; private set; }

    public Table table { get; private set; }

    public Column(string name)
        : this(name, null)
    {
    }

    public Column(string name, Type type)
    {
        this.name = name;
        this.type = type;
    }

    internal void SetTable(Table table, int index)
    {
        if (this.table != null)
        {
            throw new InvalidOperationException(string.Format("Column '{0}' is associated to Table '{1}'", this.name, this.table.name));
        }
        this.table = table;
        this.index = index;
    }
}
