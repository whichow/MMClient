
[System.Serializable]
public struct Int2
{
    public int x, y;

    public Int2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Int2(int[] array)
    {
        if (array != null && array.Length >= 2)
        {
            x = array[0];
            y = array[1];
        }
        else
        {
            x = 0;
            y = 0;
        }
    }

    public void Set(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void SetUnit(int value)
    {
        x = value >> 16;
        y = (short)value;
    }

    public int GetUnit()
    {
        return ((x << 16) | ((ushort)y));
    }

    public int[] ToArray()
    {
        return new int[] { x, y };
    }
    public static Int2 operator +(Int2 a, Int2 b)
    {
        return new Int2(a.x + b.x, a.y + b.y);
    }

    public static Int2 operator -(Int2 a, Int2 b)
    {
        return new Int2(a.x - b.x, a.y - b.y);
    }

    public override string ToString()
    {
        return string.Format("{0},{1}", x, y);
    }

}

