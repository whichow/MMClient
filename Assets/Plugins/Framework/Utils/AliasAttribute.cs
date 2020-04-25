// ***********************************************************************
// Company          : Kunpo
// Author           : KimCh
// Created          : 2016-10-12
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using System;
using UnityEngine;

/// <summary>
/// 别名
/// </summary>
[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
public class AliasAttribute : PropertyAttribute
{
    public readonly string alias;

    public AliasAttribute(string alias)
    {
        this.alias = alias;
    }
}
