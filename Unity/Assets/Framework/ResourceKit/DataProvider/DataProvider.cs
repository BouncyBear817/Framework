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

        private IDataProviderHelper<T> mDataProviderHelper;

        private EventHandler<ReadDataSuccessEventArgs> mReadDataSuccessEventHandler;
        private EventHandler<ReadDataFailureEventArgs> mReadDataFailureEventHandler;
        private EventHandler<ReadDataUpdateEventArgs> mReadDataUpdateEventHandler;
        private EventHandler<ReadDataDependencyAssetEventArgs> mReadDataDependencyAssetEventHandler;

        public DataProvider(T owner)
        {
            mOwner = owner;
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
            if (dataBytes == null)
            {
                throw new Exception("Data bytes is invalid.");
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
    }
}