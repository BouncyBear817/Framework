
using System;
namespace Framework
{
    internal interface IObjectFactory<T>
    {
        T Create();
    }

    internal class DefaultObjectFactory<T> : IObjectFactory<T> where T : new()
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
    internal class CustomObjectFactory<T> : IObjectFactory<T>
    {
        protected Func<T> mFactoryMethod;

        public CustomObjectFactory(Func<T> facoryMethod) => mFactoryMethod = facoryMethod;

        public T Create()
        {
            return mFactoryMethod();
        }
    }

    internal class ObjectFactory
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