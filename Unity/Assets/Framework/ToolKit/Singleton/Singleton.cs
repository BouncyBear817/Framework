using System;
using UnityEngine;

namespace Framework
{
    public abstract class Singleton<T> : ISingleton where T : Singleton<T>
    {
        private static T mInstance;

        private static object mLock = new object();

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

        public abstract void OnSingletonInit();
    }

    public static class SingletonProperty<T> where T : class, ISingleton
    {
        private static T mInstance;

        private static object mLock = new object();

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

        public static void OnDispose()
        {
            mInstance = null;
        }
    }
    
    /// <summary>
    /// 静态类：MonoBehaviour类的单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MonoSingleton<T> : MonoBehaviour, ISingleton where T : MonoSingleton<T>
    {
        private static T mInstance;
        
        private static object mLock = new object();
        
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
        
        /// <summary>
        /// 应用程序退出：释放当前对象并销毁相关GameObject
        /// </summary>
        protected virtual void OnApplicationQuit()
        {
            if (mInstance == null) return;
            Destroy(mInstance.gameObject);
            mInstance = null;
        }

        /// <summary>
        /// 释放当前对象
        /// </summary>
        protected virtual void OnDestroy()
        {
            mInstance = null;
        }
    }

    public static class SingletonCreator
    {
        private static T CreateNonPublicConstructObject<T>() where T : class
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

        private static T CreateMonoSingleton<T>() where T : class, ISingleton
        {
            var instance = UnityEngine.Object.FindObjectOfType(typeof(T)) as T;
            if (instance != null)
            {
                instance.OnSingletonInit();
                return instance;
            }

            var obj = new GameObject(typeof(T).Name);
            UnityEngine.Object.DontDestroyOnLoad(obj);
            instance = obj.AddComponent(typeof(T)) as T;
            instance?.OnSingletonInit();

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
                instance?.OnSingletonInit();
                return instance;
            }
        }
    }
}
