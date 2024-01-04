/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2023/12/28 15:08:22
* Description:   
* Modify Record: 
*************************************************************/

namespace Framework
{
    public interface IDataProviderHelper<T>
    {
        bool ReadData(T dataProviderOwner, string dataAssetName, object dataAsset, object userData);

        bool ReadData(T dataProviderOwner, string dataAssetName, byte[] dataBytes, int startIndex, int length,
            object userData);

        bool ParseData(T dataProviderOwner, string dataString, object userData);

        bool ParseData(T dataProviderOwner, byte[] dataBytes, int startIndex, int length, object userData);

        bool ParseData(T dataProviderOwner, object dataAsset);

        void ReleaseDataAsset(T dataProviderOwner, object dataAsset);
    }
}


