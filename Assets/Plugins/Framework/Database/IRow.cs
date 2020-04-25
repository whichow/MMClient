// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "IRow" company=""></copyright>
// <summary></summary>
// ***********************************************************************

public interface IRow
{
    int columnCount { get; }
    Column GetColumn(string name);
    Column GetColumn(int index);
}
