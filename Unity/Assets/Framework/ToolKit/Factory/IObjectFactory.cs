namespace Framework
{
    public interface IObjectFactory<T>
    {
        T Create();
    }
}