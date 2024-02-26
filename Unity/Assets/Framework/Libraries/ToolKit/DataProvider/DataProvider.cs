/************************************************************
 * Unity Version: 2022.3.0f1c1
 * Author:        bear
 * CreateTime:    2023/12/28 15:02:35
 * Description:
 * Modify Record:
 *************************************************************/

using System;

namespace Framework
{
    public sealed class DataProvider<T> : IDataProvider<T>
    {
        private const int BlockSize = 1024 * 4;
        private static byte[] sCachedBytes = null;

        private readonly T mOwner;

        private readonly LoadAssetCallbacks mLoadAssetCallbacks;
        private readonly LoadBinaryCallbacks mLoadBinaryCallbacks;

        private IDataProviderHelper<T> mDataProviderHelper;
        private IResourceManager mResourceManager;

        private EventHandler<ReadDataSuccessEventArgs> mReadDataSuccessEventHandler;
        private EventHandler<ReadDataFailureEventArgs> mReadDataFailureEventHandler;
        private EventHandler<ReadDataUpdateEventArgs> mReadDataUpdateEventHandler;
        private EventHandler<ReadDataDependencyAssetEventArgs> mReadDataDependencyAssetEventHandler;

        public DataProvider(T owner)
        {
            mOwner = owner;
            mLoadAssetCallbacks = new LoadAssetCallbacks(LoadAssetSuccessCallback, LoadAssetOrBinaryFailureCallback,
                LoadAssetUpdateCallback, LoadAssetDependencyCallback);
            mLoadBinaryCallbacks = new LoadBinaryCallbacks(LoadBinarySuccessCallback, LoadAssetOrBinaryFailureCallback);
            mDataProviderHelper = null;
            mResourceManager = null;
            mReadDataSuccessEventHandler = null;
            mReadDataFailureEventHandler = null;
            mReadDataUpdateEventHandler = null;
            mReadDataDependencyAssetEventHandler = null;
        }

        /// <summary>
        /// 获取缓冲二进制流的大小
        /// </summary>
        public static int CachedBytesSize => sCachedBytes?.Length ?? 0;

        /// <summary>
        /// 读取数据成功事件
        /// </summary>
        public event EventHandler<ReadDataSuccessEventArgs> ReadDataSuccess
        {
            add => mReadDataSuccessEventHandler += value;
            remove => mReadDataSuccessEventHandler -= value;
        }

        /// <summary>
        /// 读取数据失败事件
        /// </summary>
        public event EventHandler<ReadDataFailureEventArgs> ReadDataFailure
        {
            add => mReadDataFailureEventHandler += value;
            remove => mReadDataFailureEventHandler -= value;
        }

        /// <summary>
        /// 读取数据更新事件
        /// </summary>
        public event EventHandler<ReadDataUpdateEventArgs> ReadDataUpdate
        {
            add => mReadDataUpdateEventHandler += value;
            remove => mReadDataUpdateEventHandler -= value;
        }

        /// <summary>
        /// 读取数据加载依赖资源事件
        /// </summary>
        public event EventHandler<ReadDataDependencyAssetEventArgs> ReadDataDependencyAsset
        {
            add => mReadDataDependencyAssetEventHandler += value;
            remove => mReadDataDependencyAssetEventHandler -= value;
        }

        /// <summary>
        /// 确保二进制流缓存分配足够的内存并缓存
        /// </summary>
        /// <param name="ensureSize"></param>
        /// <exception cref="Exception"></exception>
        public static void EnsureCachedBytesSize(int ensureSize)
        {
            if (ensureSize < 0)
            {
                throw new Exception("EnsureSize is invalid.");
            }

            if (sCachedBytes == null || sCachedBytes.Length < ensureSize)
            {
                FreeCachedBytes();
                var size = (ensureSize - 1 + BlockSize) / BlockSize * BlockSize;
                sCachedBytes = new byte[size];
            }
        }

        /// <summary>
        /// 释放缓存的二进制流
        /// </summary>
        public static void FreeCachedBytes()
        {
            sCachedBytes = null;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="dataAssetName">数据资源名称</param>
        /// <param name="priority">数据资源加载优先级</param>
        /// <param name="userData">自定义数据</param>
        public void ReadData(string dataAssetName, int priority, object userData)
        {
            if (mDataProviderHelper == null)
            {
                throw new Exception("You must set data provider helper first.");
            }

            if (mResourceManager == null)
            {
                throw new Exception("You must set resource manager first.");
            }

            var result = mResourceManager.HasAsset(dataAssetName);
            switch (result)
            {
                case HasAssetResult.AssetOnDisk:
                case HasAssetResult.AssetOnFileSystem:
                    mResourceManager.LoadAsset(new LoadAssetInfo(dataAssetName, priority, userData),
                        mLoadAssetCallbacks);
                    break;
                case HasAssetResult.BinaryOnDisk:
                    mResourceManager.LoadBinary(dataAssetName, mLoadBinaryCallbacks, userData);
                    break;
                case HasAssetResult.BinaryOnFileSystem:
                    var dataLength = mResourceManager.GetBinaryLength(dataAssetName);
                    EnsureCachedBytesSize(dataLength);
                    if (dataLength != mResourceManager.LoadBinaryFromFileSystem(dataAssetName, sCachedBytes))
                    {
                        throw new Exception($"Load Binary ({dataAssetName}) from file system with internal error.");
                    }

                    try
                    {
                        if (!mDataProviderHelper.ReadData(mOwner, dataAssetName, sCachedBytes, 0, dataLength, userData))
                        {
                            throw new Exception($"Load data ({dataAssetName}) failure in data provider helper.");
                        }

                        if (mReadDataSuccessEventHandler != null)
                        {
                            var eventArgs = ReadDataSuccessEventArgs.Create(dataAssetName, 0f, userData);
                            mReadDataSuccessEventHandler(this, eventArgs);
                            ReferencePool.Release(eventArgs);
                        }
                    }
                    catch (Exception e)
                    {
                        if (mReadDataFailureEventHandler != null)
                        {
                            var eventArgs = ReadDataFailureEventArgs.Create(dataAssetName, e.ToString(), userData);
                            mReadDataFailureEventHandler(this, eventArgs);
                            ReferencePool.Release(eventArgs);
                        }

                        throw;
                    }

                    break;
                default:
                    throw new Exception($"Data asset ({dataAssetName}) result is '{result}'");
            }
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="dataString">数据字符串</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析成功</returns>
        public bool ParseData(string dataString, object userData)
        {
            if (mDataProviderHelper == null)
            {
                throw new Exception("You must set data provider helper first.");
            }

            if (dataString == null)
            {
                throw new Exception("Data string is invalid.");
            }

            try
            {
                return mDataProviderHelper.ParseData(mOwner, dataString, userData);
            }
            catch (Exception e)
            {
                throw new Exception($"Can not parse data string with exception ({e})");
            }
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="dataBytes">数据二进制流</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析成功</returns>
        public bool ParseData(byte[] dataBytes, object userData)
        {
            return ParseData(dataBytes, 0, dataBytes.Length, userData);
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="dataBytes">数据二进制流</param>
        /// <param name="startIndex">二进制流起始位置</param>
        /// <param name="length">二进制流长度</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析成功</returns>
        public bool ParseData(byte[] dataBytes, int startIndex, int length, object userData)
        {
            if (mDataProviderHelper == null)
            {
                throw new Exception("You must set data provider helper first.");
            }

            if (dataBytes == null)
            {
                throw new Exception("Data bytes is invalid.");
            }

            if (startIndex < 0 || length < 0 || startIndex + length > dataBytes.Length)
            {
                throw new Exception("Start index or length is invalid.");
            }

            try
            {
                return mDataProviderHelper.ParseData(mOwner, dataBytes, startIndex, length, userData);
            }
            catch (Exception e)
            {
                throw new Exception($"Can not parse data bytes with exception ({e})");
            }
        }

        /// <summary>
        /// 设置数据提供者辅助器
        /// </summary>
        /// <param name="dataProviderHelper">数据提供者辅助器</param>
        /// <exception cref="Exception"></exception>
        public void SetDataProviderHelper(IDataProviderHelper<T> dataProviderHelper)
        {
            mDataProviderHelper = dataProviderHelper ?? throw new Exception("Data Provider Helper is invalid.");
        }

        /// <summary>
        /// 设置资源管理器
        /// </summary>
        /// <param name="resourceManager">资源管理器</param>
        /// <exception cref="Exception"></exception>
        public void SetResourceManager(IResourceManager resourceManager)
        {
            mResourceManager = resourceManager ?? throw new Exception("Resource manager is invalid.");
        }

        private void LoadAssetSuccessCallback(string assetName, object asset, float duration, object userData)
        {
            try
            {
                if (!mDataProviderHelper.ReadData(mOwner, assetName, asset, userData))
                {
                    throw new Exception($"Read Data failure in data provider helper, data asset name : {assetName}");
                }

                if (mReadDataSuccessEventHandler != null)
                {
                    var eventArgs = ReadDataSuccessEventArgs.Create(assetName, duration, userData);
                    mReadDataSuccessEventHandler(this, eventArgs);
                    ReferencePool.Release(eventArgs);
                }
            }
            catch (Exception e)
            {
                if (mReadDataFailureEventHandler != null)
                {
                    var eventArgs = ReadDataFailureEventArgs.Create(assetName, e.ToString(), userData);
                    mReadDataFailureEventHandler(this, eventArgs);
                    ReferencePool.Release(eventArgs);
                }
            }
            finally
            {
                mDataProviderHelper.ReleaseDataAsset(mOwner, asset);
            }
        }

        private void LoadAssetOrBinaryFailureCallback(string assetName, LoadResourceStatus status, string errorMessage,
            object userData)
        {
            string appendErrorMessage =
                $"Load data failure, data asset Name ({assetName}), status ({status}), error message ({errorMessage})";
            if (mReadDataFailureEventHandler != null)
            {
                var eventArgs = ReadDataFailureEventArgs.Create(assetName, appendErrorMessage, userData);
                mReadDataFailureEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
                return;
            }

            throw new Exception(appendErrorMessage);
        }

        private void LoadAssetUpdateCallback(string assetName, float progress, object userData)
        {
            if (mReadDataUpdateEventHandler != null)
            {
                var eventArgs = ReadDataUpdateEventArgs.Create(assetName, progress, userData);
                mReadDataUpdateEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void LoadAssetDependencyCallback(string assetName, string dependencyAssetName, int loadedCount,
            int totalCount, object userData)
        {
            if (mReadDataDependencyAssetEventHandler != null)
            {
                var eventArgs = ReadDataDependencyAssetEventArgs.Create(assetName, dependencyAssetName, loadedCount,
                    totalCount, userData);
                mReadDataDependencyAssetEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void LoadBinarySuccessCallback(string binaryAssetName, byte[] binaryBytes, float duration,
            object userData)
        {
            try
            {
                if (!mDataProviderHelper.ReadData(mOwner, binaryAssetName, binaryBytes, 0, binaryBytes.Length,
                        userData))
                {
                    throw new Exception(
                        $"Read Data failure in data provider helper, data asset name : {binaryAssetName}");
                }

                if (mReadDataSuccessEventHandler != null)
                {
                    var eventArgs = ReadDataSuccessEventArgs.Create(binaryAssetName, duration, userData);
                    mReadDataSuccessEventHandler(this, eventArgs);
                    ReferencePool.Release(eventArgs);
                }
            }
            catch (Exception e)
            {
                if (mReadDataFailureEventHandler != null)
                {
                    var eventArgs = ReadDataFailureEventArgs.Create(binaryAssetName, e.ToString(), userData);
                    mReadDataFailureEventHandler(this, eventArgs);
                    ReferencePool.Release(eventArgs);
                }
            }
        }
    }
}