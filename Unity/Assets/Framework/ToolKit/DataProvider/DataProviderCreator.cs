using System;

namespace Framework
{
    /// <summary>
    /// 数据提供者创建器
    /// </summary>
    public static class DataProviderCreator
    {
        public static int GetCachedBytesSize<T>()
        {
            return DataProvider<T>.CachedBytesSize;
        }

        public static void EnsureCachedBytesSize<T>(int ensureSize)
        {
            DataProvider<T>.EnsureCachedBytesSize(ensureSize);
        }

        public static void FreeCachedSize<T>()
        {
            DataProvider<T>.FreeCachedBytes();
        }

        public static IDataProvider<T> Create<T>(T owner, IResourceManager resourceManager,
            IDataProviderHelper<T> dataProviderHelper)
        {
            if (owner == null)
            {
                throw new Exception("Owner is invalid.");
            }
            
            if (resourceManager == null)
            {
                throw new Exception("Resource manager is invalid.");
            }
            
            if (dataProviderHelper == null)
            {
                throw new Exception("Data provider helper is invalid.");
            }

            var dataProvider = new DataProvider<T>(owner);
            dataProvider.SetDataProviderHelper(dataProviderHelper);
            return dataProvider;
        }
    }
}