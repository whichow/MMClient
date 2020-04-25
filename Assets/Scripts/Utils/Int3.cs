
[System.Serializable]
public struct Int3
{
    public int x, y, z;

    public Int3(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public override string ToString()
    {
        return string.Format("Int3({0},{1},{2})",x,y,z);

    }
}