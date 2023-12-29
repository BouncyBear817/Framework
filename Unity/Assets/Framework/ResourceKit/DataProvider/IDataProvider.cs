/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2023/12/28 15:00:32
* Description:   
* Modify Record: 
*************************************************************/

using System;

namespace Framework
{
    public interface IDataProvider<T>
    {
        event EventHandler<ReadDataSuccessEventArgs> ReadDataSuccess;

        event EventHandler<ReadDataFailureEventArgs> ReadDataFailure;

        event EventHandler<ReadDataUpdateEventArgs> ReadDataUpdate;

        event EventHandler<ReadDataDependencyAssetEventArgs> ReadDataDependencyAsset;

        void ReadData(string dataAssetName, int priority, object userData);

        bool ParseData(string dataString, object userData);

        bool ParseData(byte[] dataBytes, object userData);

        bool ParseData(byte[] dataBytes, int startIndex, int length, object userData);
    }
}