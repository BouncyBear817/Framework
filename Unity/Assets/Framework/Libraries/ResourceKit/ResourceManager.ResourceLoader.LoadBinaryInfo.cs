// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/21 16:4:39
//  * Description:
//  * Modify Record:
//  *************************************************************/

namespace Framework
{
    public sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {
        private sealed partial class ResourceLoader
        {
            private sealed class LoadBinaryInfo : IReference
            {
                private string mBinaryAssetName;
                private ResourceInfo mResourceInfo;
                private LoadBinaryCallbacks mLoadBinaryCallbacks;
                private object mUserData;

                public LoadBinaryInfo()
                {
                    mBinaryAssetName = null;
                    mResourceInfo = null;
                    mLoadBinaryCallbacks = null;
                    mUserData = null;
                }

                public string BinaryAssetName => mBinaryAssetName;

                public ResourceInfo ResourceInfo => mResourceInfo;

                public LoadBinaryCallbacks LoadBinaryCallbacks => mLoadBinaryCallbacks;

                public object UserData => mUserData;

                public static LoadBinaryInfo Create(string binaryAssetName, ResourceInfo resourceInfo,
                    LoadBinaryCallbacks loadBinaryCallbacks, object userData)
                {
                    var info = ReferencePool.Acquire<LoadBinaryInfo>();
                    info.mBinaryAssetName = binaryAssetName;
                    info.mResourceInfo = resourceInfo;
                    info.mLoadBinaryCallbacks = loadBinaryCallbacks;
                    info.mUserData = userData;
                    return info;
                }

                public void Clear()
                {
                    mBinaryAssetName = null;
                    mResourceInfo = null;
                    mLoadBinaryCallbacks = null;
                    mUserData = null;
                }
            }
        }
    }
}