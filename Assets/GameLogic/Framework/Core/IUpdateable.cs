
namespace Framework.Core
{
    public interface IUpdateable
    {
        bool blEnable { get; }
        void Update();
    }

    public interface IDispose
    {
        void Dispose();
    }
}