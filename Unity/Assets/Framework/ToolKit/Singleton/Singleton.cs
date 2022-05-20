
using System;
using UnityEngine;

namespace Framework
{
    internal interface ISingleton
    {
        void OnSingletonInit();
    }

    internal abstract class Singleton<T> : ISingleton where T : Singleton<T>
    {
        protected static T mInstance;

        static object mLock = new object();

        public static T Instance
        {
            get
            {
                lock (mLock)
                {
                    mInstance ??= SingletonCreator.CreateSingleton<T>();
                    return mInstance;
                }
            }
        }

        public void OnSingletonInit()
        {
            
        }
    }

    internal class SingletonCreator
    {
        static T CreateNonPublicConstructObject<T>() where T : class
        {
            var type = typeof(T);

            var constructorInfos = type.GetConstructors(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            var ctor = Array.Find(constructorInfos, c => c.GetParameters().Length == 0);

            if (ctor == null)
            {
                throw new Exception("Non-Public Constructor not found! in " + type);
            }

            return ctor.Invoke(null) as T;
        }

        static T CreateMonoSingleton<T>() where T : class, ISingleton
        {
            T instance = null;

            instance = UnityEngine.Object.FindObjectOfType(typeof(T)) as T;
            if (instance != null)
            {
                instance.OnSingletonInit();
                return instance;
            }

            var obj = new GameObject(typeof(T).Name);
            UnityEngine.Object.DontDestroyOnLoad(obj);
            instance = obj.AddComponent(typeof(T)) as T;
            instance.OnSingletonInit();

            return instance;
        }

        public static T CreateSingleton<T>() where T : class, ISingleton
        {
            var type = typeof(T);
            var monoBehaviour = typeof(MonoBehaviour);

            if (monoBehaviour.IsAssignableFrom(type))
            {
                return CreateMonoSingleton<T>();
            }
            else
            {
                var instance = CreateNonPublicConstructObject<T>();
                instance.OnSingletonInit();
                return instance;
            }
        }
    }
}
