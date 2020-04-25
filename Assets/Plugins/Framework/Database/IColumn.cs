// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "IColumn" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System;

public interface IColumn
{
    string name { get; }
    int index { get; }
    Type type { get; }
    Table table { get; }
}
