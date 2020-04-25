using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TEnum
{
    kNone,
}

public enum TEnumF
{
    fNone,
    fOne = 1,
    fTwo = 2,
    fThr = 4,
}

delegate void Tdelegate();

public interface IInterface
{
    void Test();
}

public class Example : MonoBehaviour, IInterface
{
    public const int CONST_KEY = 0;

    private static int _StaticInt = 0;

    public static int StaticInt
    {
        get;
        set;
    }

    public int publicField;

    private int _privateField;

    public int property
    {
        get;
        set;
    }

    public void Method()
    {
        var tmp = 0;
    }

    public void Test()
    {
        throw new NotImplementedException();
    }
}
