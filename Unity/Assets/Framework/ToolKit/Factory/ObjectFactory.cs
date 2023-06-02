
using System;
namespace Framework
{
    public class DefaultObjectFactory<T> : IObjectFactory<T> where T : new()
    {
        public T Create()
        {
            return new T();
        }
    }

    /// <summary>
    /// 定义工厂对象方法，可自定义返回对象方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomObjectFactory<T> : IObjectFactory<T>
    {
        protected readonly Func<T> FactoryMethod;

        public CustomObjectFactory(Func<T> factoryMethod) => this.FactoryMethod = factoryMethod;

        public T Create()
        {
            return FactoryMethod();
        }
    }

    public class ObjectFactory
    {
        public static object Create(Type type, params object[] constructorArgs)
        {
            return Activator.CreateInstance(type, constructorArgs);
        }

        public static object Create<T>(params object[] constructorArgs)
        {
            return Create(typeof(T), constructorArgs);
        }
    }
}