// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "Database" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using Game;

public class Database
{
    private string _name;
    private KArchive _archive;

    public string name
    {
        get { return _name; }
    }

    public KArchive archive
    {
        get { return _archive; }
    }

    private Database(string name)
    {
        _name = name;
        _archive = KArchive.Load(name);
    }

    public static Database Create(string name)
    {
        return new Database(name);
    }

    #region 关系型

    private Dictionary<string, Table> _tables = new Dictionary<string, Table>();
    public Table CreateTable(Schema schema)
    {
        if (_tables.ContainsKey(schema.name))
        {
            throw new InvalidOperationException(string.Format("Table '{0}' is exists", schema.name));
        }

        var table = new Table(this, schema);
        _tables[table.name] = table;
        return table;
    }

    public Table GetTable(string name)
    {
        return _tables[name];
    }

    #endregion

    #region 键值型

    #endregion
}
