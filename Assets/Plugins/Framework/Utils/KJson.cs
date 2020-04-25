// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

//using ArrayList = System.Collections.Generic.List<object>;
//using Hashtable = System.Collections.Generic.Dictionary<string, object>;

public static class KJson
{
    #region Helper

    private static int _bufferIndex = 0;
    private static char[] _jsonBuffer = new char[1024 * 1024];

    private static int buffLength
    {
        get { return _bufferIndex; }
    }

    private static void ClearBuffer()
    {
        _bufferIndex = 0;
    }

    private static void Append(char value)
    {
        _jsonBuffer[_bufferIndex++] = value;
    }

    private static void Append(string value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            _jsonBuffer[_bufferIndex++] = value[i];
        }
    }

    private static char GetChar(int index)
    {
        return _jsonBuffer[index];
    }

    private static string GetJosnString()
    {
        return new string(_jsonBuffer, 0, _bufferIndex);
    }

    private static byte[] GetJsonBytes()
    {
        return Encoding.UTF8.GetBytes(_jsonBuffer, 0, _bufferIndex);
    }

    private static void SetJosnString(string value)
    {
        int i = 0;
        for (; i < value.Length; i++)
        {
            var tChar = value[i];
            if (tChar == '{' || tChar == '[')
                break;
        }

        _bufferIndex = 0;
        for (; i < value.Length; i++)
        {
            _jsonBuffer[_bufferIndex++] = value[i];
        }
    }

    private static void SetJsonBytes(byte[] value)
    {
        _bufferIndex = 0;
        _bufferIndex = Encoding.UTF8.GetChars(value, 0, value.Length, _jsonBuffer, 0);
        for (int i = 0; i < _jsonBuffer.Length; i++)
        {
            var tChar = _jsonBuffer[i];
            if (tChar != '{' && tChar != '[')
            {
                _jsonBuffer[i] = ' ';
            }
            else
            {
                break;
            }
        }
    }

    #endregion

    #region Serialize

    private static void WriteObj(object obj)
    {
        if (obj is string)
        {
            WriteString((string)obj);
        }
        else if (obj is ValueType)
        {
            WriteNumber(obj);
        }
        else if (obj is IDictionary)
        {
            WriteTable((IDictionary)obj);
        }
        else if (obj is IList)
        {
            WriteList((IList)obj);
        }
        else
        {
            WriteOther(obj);
        }
    }

    private static void WriteNumber(object obj)
    {
        Append(obj is bool ? obj.ToString().ToLower() : obj.ToString());
    }

    private static void WriteString(string obj)
    {
        Append('\"');
        foreach (var ch in obj)
        {
            switch (ch)
            {
                case '"':
                    Append("\\\"");
                    break;
                case '\\':
                    Append("\\\\");
                    break;
                case '\n':
                    Append("\\n");
                    break;
                case '\r':
                    Append("\\r");
                    break;
                case '\t':
                    Append("\\t");
                    break;
                default:
                    Append(ch);
                    break;
            }
        }
        Append('\"');
    }

    private static void WriteList(IList list)
    {
        if (list.Count == 8)
        {
            Append("\n"); // temp test
        }
        Append('[');
        int i = 0;
        foreach (var obj in list)
        {
            if (i++ > 0)
            {
                Append(',');
            }
            WriteObj(obj);
        }
        Append(']');
    }

    private static void WriteTable(IDictionary table)
    {
        Append('{');
        int i = 0;
        foreach (DictionaryEntry entry in table)
        {
            if (i++ > 0)
            {
                Append(',');
            }
            WriteString(entry.Key.ToString());
            Append(':');
            WriteObj(entry.Value);
        }
        Append('}');
    }

    private static void WriteOther(object obj)
    {
        Append("null");
    }

    #endregion

    #region Deserialize

    private enum TOKEN
    {
        None,
        Null,
        True,
        False,
        Colon,
        Comma,
        Number,
        String,
        CurlyLeft,
        CurlyRight,
        SquareLeft,
        SquareRight,
    };

    private static int _strIndex = 0;
    private static StringBuilder _strBuffer = new StringBuilder(32);
    private static StringBuilder _wordBuffer = new StringBuilder(32);

    /// <summary>Passes the character.</summary>
    private static void PassChar()
    {
        if (_strIndex < buffLength)
        {
            _strIndex++;
        }
    }
    /// <summary>Passes the white.</summary>
    private static void PassWhite()
    {
        while (_strIndex < buffLength)
        {
            if (" \n\r\t".IndexOf(GetChar(_strIndex)) == -1)
            {
                break;
            }
            _strIndex++;
        }
    }
    /// <summary>Reads the word.</summary>
    /// <returns></returns>
    private static string ReadWord()
    {
        _wordBuffer.Length = 0;
        while ((_strIndex < buffLength) && ("{}[],: \"\n\r\t".IndexOf(GetChar(_strIndex)) == -1))
        {
            _wordBuffer.Append(GetChar(_strIndex++));
        }
        return _wordBuffer.ToString();
    }
    /// <summary>Reads the token.</summary>
    /// <returns></returns>
    private static TOKEN ReadToken()
    {
        PassWhite();
        if (_strIndex < buffLength)
        {
            var peekChar = GetChar(_strIndex);

            switch (peekChar)
            {
                case '"':
                    return TOKEN.String;
                case '{':
                    return TOKEN.CurlyLeft;
                case '}':
                    return TOKEN.CurlyRight;
                case '[':
                    return TOKEN.SquareLeft;
                case ']':
                    return TOKEN.SquareRight;
                case ',':
                    _strIndex++;
                    return TOKEN.Comma;
                case ':':
                    _strIndex++;
                    return TOKEN.Colon;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '-':
                case '.':
                    return TOKEN.Number;
            }

            var word = ReadWord();
            switch (word)
            {
                case "true":
                    return TOKEN.True;
                case "false":
                    return TOKEN.False;
                case "null":
                    return TOKEN.Null;
            }
        }
        return TOKEN.None;
    }
    /// <summary>Reads the number.</summary>
    /// <returns></returns>
    private static object ReadNumber()
    {
        var number = ReadWord();
        if (number.IndexOf('.') == -1)
        {
            int parsedInt;
            int.TryParse(number, out parsedInt);
            return parsedInt;
        }
        else
        {
            float parsedFloat;
            float.TryParse(number, out parsedFloat);
            return parsedFloat;
        }
    }
    /// <summary>Reads the string.</summary>
    /// <returns></returns>
    private static string ReadString()
    {
        _strIndex++;//"
        _strBuffer.Length = 0;
        while (_strIndex < buffLength)
        {
            var tmpChar = GetChar(_strIndex++);
            switch (tmpChar)
            {
                case '"':
                    return _strBuffer.ToString();
                case '\\':
                    if (_strIndex < buffLength)
                    {
                        var escape = GetChar(_strIndex++);
                        switch (escape)
                        {
                            case '"':
                                _strBuffer.Append('"');
                                break;
                            case '\\':
                                _strBuffer.Append('\\');
                                break;
                            case 'n':
                                _strBuffer.Append('\n');
                                break;
                            case 'r':
                                _strBuffer.Append('\r');
                                break;
                            case 't':
                                _strBuffer.Append('\t');
                                break;
                            default:
                                _strBuffer.Append('\\');
                                _strBuffer.Append(escape);
                                break;
                        }
                    }
                    break;
                default:
                    _strBuffer.Append(tmpChar);
                    break;
            }
        }
        return _strBuffer.ToString();
    }
    private static ArrayList ReadList()/// <summary>Reads the array.</summary>
    {
        _strIndex++;//[
        var list = new ArrayList();
        while (_strIndex < buffLength)
        {
            TOKEN token = ReadToken();
            switch (token)
            {
                case TOKEN.Comma:
                    continue;
                case TOKEN.SquareRight:
                    _strIndex++;//]
                    return list;
                case TOKEN.None:
                    return null;
                default:
                    list.Add(ReadObject());
                    break;
            }
        }
        return list;
    }
    private static Hashtable ReadTable()/// <summary>Reads the table.</summary>
    {
        _strIndex++;//{
        var table = new Hashtable(StringComparer.OrdinalIgnoreCase);
        while (_strIndex < buffLength)
        {
            TOKEN token = ReadToken();
            switch (token)
            {
                case TOKEN.Comma:
                    continue;
                case TOKEN.CurlyRight:
                    _strIndex++;//}
                    return table;
                case TOKEN.None:
                    return null;
                default:
                    var key = ReadString();
                    var colon = ReadToken();
                    if (!string.IsNullOrEmpty(key) && colon == TOKEN.Colon)
                    {
                        var value = ReadObject();
                        table[key] = value;
                    }
                    break;
            }
        }
        return table;
    }
    private static object ReadObject()
    {
        var token = ReadToken();
        switch (token)
        {
            case TOKEN.String:
                return ReadString();
            case TOKEN.Number:
                return ReadNumber();
            case TOKEN.CurlyLeft:
                return ReadTable();
            case TOKEN.SquareLeft:
                return ReadList();
            case TOKEN.True:
                return true;
            case TOKEN.False:
                return false;
            case TOKEN.Null:
                return null;
            default:
                return null;
        }
    }

    #endregion

    #region API

    /// <summary>
    /// Converts a IDictionary / IList object or a simple type (string, int, etc.) into a JSON string
    /// </summary>
    /// <param name="json">A Dictionary;string, object; / List;object;</param>
    /// <returns>A JSON encoded string, or null if object 'json' is not serializable</returns>
    public static string Serialize(object obj)
    {
        ClearBuffer();
        WriteObj(obj);
        return GetJosnString();
    }
    /// <summary>Serializes to bytes.</summary>
    /// <param name="obj">The object.</param>
    /// <returns></returns>
    public static byte[] SerializeToBytes(object obj)
    {
        ClearBuffer();
        WriteObj(obj);
        return GetJsonBytes();
    }
    /// <summary>
    /// Parses the string json into a value
    /// </summary>
    /// <param name="json">A JSON string.</param>
    /// <returns>An List;object&gt;, a Dictionary;string, object;, a double, an integer,a string, null, true, or false</returns>
    public static object Deserialize(string json)
    {
        if (!string.IsNullOrEmpty(json))
        {
            _strIndex = 0;
            SetJosnString(json);
            return ReadObject();
        }
        return null;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="jsonBytes"></param>
    /// <returns></returns>
    public static object Deserialize(byte[] jsonBytes)
    {
        if (jsonBytes != null)
        {
            _strIndex = 0;
            SetJsonBytes(jsonBytes);
            return ReadObject();
        }
        return null;
    }

    #endregion

    #region objectConvert

    public static int GetInt(object obj, int defaultValue = 0)
    {
        if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
        {
            return Convert.ToInt32(obj);
        }
        return defaultValue;
    }

    public static float GetFloat(object obj, float defaultValue = 0f)
    {
        if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
        {
            return Convert.ToSingle(obj);
        }
        return defaultValue;
    }

    public static bool GetBool(object obj, bool defaultValue = false)
    {
        if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
        {
            return Convert.ToSingle(obj) > 0;
        }
        return defaultValue;
    }

    #endregion

    #region Extensions 

    /// <summary>
    /// string转ArrayList
    /// </summary>
    /// <param name="jsonText"></param>
    /// <returns></returns>
    public static ArrayList ToJsonList(this string jsonText)
    {
        return KJson.Deserialize(jsonText) as ArrayList;
    }

    /// <summary>
    /// string转HashTable
    /// </summary>
    /// <param name="jsonText"></param>
    /// <returns></returns>
    public static Hashtable ToJsonTable(this string jsonText)
    {
        return KJson.Deserialize(jsonText) as Hashtable;
    }

    /// <summary>
    /// json bytes转HashTable
    /// </summary>
    /// <param name="jsonBytes"></param>
    /// <returns></returns>
    public static Hashtable ToJsonTable(this byte[] jsonBytes)
    {
        return KJson.Deserialize(jsonBytes) as Hashtable;
    }

    /// <summary>
    /// ArrayList转string
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static string ToJsonText(this ArrayList list)
    {
        return KJson.Serialize(list);
    }

    /// <summary>
    /// Hashtable转string
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    public static string ToJsonText(this Hashtable table)
    {
        return KJson.Serialize(table);
    }

    /// <summary>Hashtable转json bytes.</summary>
    /// <param name="table">The table.</param>
    /// <returns></returns>
    public static byte[] ToJsonBytes(this Hashtable table)
    {
        return KJson.SerializeToBytes(table);
    }

    /// <summary>
    /// 特定解析器()
    /// </summary>
    /// <param name="list"></param>
    /// <param name="resolver"></param>
    public static void Resolve(this ArrayList list, Action<Hashtable> resolver)
    {
        if (list != null && list.Count > 1)
        {
            var tmpT = new Hashtable();

            var tmpL0 = list[0] as ArrayList;
            for (int i = 1; i < list.Count; i++)
            {
                var tmpLi = list[i] as ArrayList;
                for (int j = 0; j < tmpL0.Count; j++)
                {
                    tmpT[tmpL0[j]] = tmpLi[j];
                }

                if (resolver != null)
                {
                    resolver(tmpT);
                }
            }
        }
    }

    #region Get常规类型

    /// <summary>
    /// 从Hashtable中获取指定key的int
    /// </summary>
    /// <param name="table"></param>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static int GetInt(this Hashtable table, string key, int defaultValue = 0)
    {
        if (table != null && table.Contains(key))
        {
            var obj = table[key];
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            {
                return Convert.ToInt32(obj);
            }
        }
        return defaultValue;
        //return GetValue<int>(table, key, defaultValue);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static int GetInt(this ArrayList list, int index, int defaultValue = 0)
    {
        if (list != null && list.Count > index)
        {
            var obj = list[index];
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            {
                return Convert.ToInt32(obj);
            }
        }
        return defaultValue;
        //return GetValue<int>(list, index, defaultValue);
    }

    /// <summary>
    /// 从Hashtable中获取指定key的float
    /// </summary>
    /// <param name="table"></param>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static float GetFloat(this Hashtable table, string key, float defaultValue = 0f)
    {
        if (table != null && table.Contains(key))
        {
            var obj = table[key];
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            {
                return Convert.ToSingle(obj);
            }
        }
        return defaultValue;
        //return GetValue<float>(table, key, defaultValue);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static float GetFloat(this ArrayList list, int index, float defaultValue = 0f)
    {
        if (list != null && list.Count > index)
        {
            var obj = list[index];
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            {
                return Convert.ToSingle(obj);
            }
        }
        return defaultValue;
        //return GetValue<float>(list, index, defaultValue);
    }

    /// <summary>
    /// 从Hashtable中获取指定key的string
    /// </summary>
    /// <param name="table"></param>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static string GetString(this Hashtable table, string key, string defaultValue = null)
    {
        if (table != null && table.Contains(key))
        {
            var obj = table[key];
            if (obj != null)
            {
                return obj.ToString();
            }
        }
        return defaultValue;
        //return GetValue<string>(table, key, defaultValue);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static string GetString(this ArrayList list, int index, string defaultValue = null)
    {
        if (list != null && list.Count > index)
        {
            var obj = list[index];
            if (obj != null)
            {
                return obj.ToString();
            }
        }
        return defaultValue;
        //return GetValue<string>(list, index, defaultValue);
    }

    /// <summary>
    /// 从Hashtable中获取指定key的ArrayList
    /// </summary>
    /// <param name="table"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static ArrayList GetArrayList(this Hashtable table, string key)
    {
        return GetValue<ArrayList>(table, key, null);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static ArrayList GetList(this ArrayList list, int index)
    {
        return GetValue<ArrayList>(list, index, null);
    }

    /// <summary>
    /// 从Hashtable中获取指定key的Hashtable
    /// </summary>
    /// <param name="table"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Hashtable GetTable(this Hashtable table, string key)
    {
        return GetValue<Hashtable>(table, key, null);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static Hashtable GetTable(this ArrayList list, int index)
    {
        return GetValue<Hashtable>(list, index, null);
    }

    #endregion

    #region Hashtable通过Key获取列表

    public static List<int> GetIntList(this Hashtable table, string key)
    {
        if (table.Contains(key))
        {
            ArrayList array = table[key] as ArrayList;
            if (array != null)
            {
                List<int> list = new List<int>(array.Count);
                foreach (var item in array)
                {

                    list.Add(KJson.GetInt(item));
                }
                return list;
            }
        }
        return null;
    }

    public static List<float> GetFloatList(this Hashtable table, string key)
    {
        if (table.Contains(key))
        {
            ArrayList array = table[key] as ArrayList;
            if (array != null)
            {
                List<float> list = new List<float>();
                foreach (var item in array)
                {
                    list.Add(KJson.GetFloat(item));
                }
                return list;
            }
        }
        return null;
    }

    public static List<bool> GetBoolList(this Hashtable table, string key)
    {
        if (table.Contains(key))
        {
            ArrayList array = table[key] as ArrayList;
            if (array != null)
            {
                List<bool> list = new List<bool>();
                foreach (var item in array)
                {
                    list.Add(KJson.GetBool(item));
                }
                return list;
            }
        }
        return null;
    }

    public static List<string> GetStringList(this Hashtable table, string key)
    {
        if (table.Contains(key))
        {
            ArrayList array = table[key] as ArrayList;
            if (array != null)
            {
                List<string> list = new List<string>();
                foreach (var item in array)
                {
                    list.Add(item.ToString());
                }
                return list;
            }
        }
        return null;
    }

    public static List<Hashtable> GetHashtableList(this Hashtable table, string key)
    {
        if (table.Contains(key))
        {
            ArrayList array = table[key] as ArrayList;
            if (array != null)
            {
                List<Hashtable> list = new List<Hashtable>();
                foreach (var item in array)
                {
                    list.Add(item as Hashtable);
                }
                return list;
            }
        }
        return null;
    }

    #endregion

    #region Get 泛型

    /// <summary>
    /// 从Hashtable中获取指定key和类型的ArrayList
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T[] GetArray<T>(this Hashtable table, string key)
    {
        if (table.Contains(key))
        {
            var list = table[key];
            if (list is ArrayList)
            {
                return (T[])((ArrayList)list).ToArray(typeof(T));
            }
        }
#if DEBUG_MY
        ////Debug.LogWarning("not exist [" + typeof(T) + "] Key:" + key);
#endif
        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static T[] GetArray<T>(this ArrayList list, int index)
    {
        if (list != null && list.Count > index)
        {
            var arrayList = list[index];
            if (arrayList is ArrayList)
            {
                return (T[])((ArrayList)arrayList).ToArray(typeof(T));
            }
        }
#if DEBUG_MY
        ////Debug.LogWarning("not exist [" + typeof(T) + "] Key:" + key);
#endif
        return null;
    }

    /// <summary>
    /// 从Hashtable中获取指定key的value值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table"></param>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    private static T GetValue<T>(Hashtable table, string key, T defaultValue = default(T))
    {
        if (table != null && table.Contains(key))
        {
            var obj = table[key];
            if (obj is T)
            {
                return (T)obj;
            }
        }
#if DEBUG_MY
        ////Debuger.LogWarning("not exist [" + typeof(T) + "] Key:" + key);
#endif
        return defaultValue;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    private static T GetValue<T>(ArrayList list, int index, T defaultValue = default(T))
    {
        if (list != null && list.Count > index)
        {
            var obj = list[index];
            if (obj is T)
            {
                return (T)obj;
            }
        }
#if DEBUG_MY
        ////Debug.LogWarning("not exist [" + typeof(T) + "] Key:" + key);
#endif
        return defaultValue;
    }

    #endregion

    #endregion

}
