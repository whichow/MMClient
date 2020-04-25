// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "Value" company=""></copyright>
// <summary></summary>
// ***********************************************************************
public abstract class Value
{
    public enum Type : byte
    {
        kObject,
        kStruct,
        kInt16,
        kInt32,
        kInt64,
        kUInt16,
        kUInt32,
        kUInt64,
        kFloat,
        kDouble,
        kDemcimal,
    }

    public abstract Type GetValueType();
}

public class ValueObject : Value
{
    public object data
    {
        get;
        set;
    }

    public ValueObject(object data)
    {
        this.data = data;
    }

    public override Type GetValueType()
    {
        return Type.kObject;
    }
}

public class ValueStruct<T> : Value where T : struct
{
    public T data
    {
        get;
        set;
    }

    public ValueStruct(T data)
    {
        this.data = data;
    }

    public override Type GetValueType()
    {
        return Type.kStruct;
    }
}
