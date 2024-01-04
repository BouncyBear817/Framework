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
            mReadDataSuccessEventHandler = null;
            mReadDataFailureEventHandler = null;
            mReadDataUpdateEventHandler = null;
            mReadDataDependencyAssetEventHandler = null;
        }

        public static int CachedBytesSize => sCachedBytes?.Length ?? 0;

        public event EventHandler<ReadDataSuccessEventArgs> ReadDataSuccess
        {
            add => mReadDataSuccessEventHandler += value;
            remove => mReadDataSuccessEventHandler -= value;
        }

        public event EventHandler<ReadDataFailureEventArgs> ReadDataFailure
        {
            add => mReadDataFailureEventHandler += value;
            remove => mReadDataFailureEventHandler -= value;
        }

        public event EventHandler<ReadDataUpdateEventArgs> ReadDataUpdate
        {
            add => mReadDataUpdateEventHandler += value;
            remove => mReadDataUpdateEventHandler -= value;
        }

        public event EventHandler<ReadDataDependencyAssetEventArgs> ReadDataDependencyAsset
        {
            add => mReadDataDependencyAssetEventHandler += value;
            remove => mReadDataDependencyAssetEventHandler -= value;
        }

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

        private static void FreeCachedBytes()
        {
            sCachedBytes = null;
        }

        public void ReadData(string dataAssetName, int priority, object userData)
        {
            if (mDataProviderHelper == null)
            {
                throw new Exception("You must set data provider helper first.");
            }
        }

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

        public bool ParseData(byte[] dataBytes, object userData)
        {
            if (dataBytes == null)
            {
                throw new Exception("Data bytes is invalid.");
            }

            return ParseData(dataBytes, 0, dataBytes.Length, userData);
        }

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

        public void SetDataProviderHelper(IDataProviderHelper<T> dataProviderHelper)
        {
            mDataProviderHelper = dataProviderHelper ?? throw new Exception("Data Provider Helper is invalid.");
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