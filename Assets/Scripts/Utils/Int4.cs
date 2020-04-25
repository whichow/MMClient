
[System.Serializable]
public struct Int4
{
    public int x, y, z, w;

    public Int4(int x, int y, int z, int w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public Int2 point
    {
        get { return new Int2(x, y); }
    }

    public Int2 size
    {

        get { return new Int2(z, w); }
    }
}
