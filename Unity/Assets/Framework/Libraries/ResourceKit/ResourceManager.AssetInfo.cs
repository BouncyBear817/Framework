// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/20 16:18:43
//  * Description:
//  * Modify Record:
//  *************************************************************/

namespace Framework
{
    public sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {
        /// <summary>
        /// 资源信息
        /// </summary>
        private sealed class AssetInfo
        {
            private readonly string mAssetName;
            private readonly ResourceName mResourceName;
            private readonly string[] mDependencyAssetNames;

            public AssetInfo(string assetName, ResourceName resourceName, string[] dependencyAssetNames)
            {
                mAssetName = assetName;
                mResourceName = resourceName;
                mDependencyAssetNames = dependencyAssetNames;
            }

            /// <summary>
            /// 资源名称
            /// </summary>
            public string AssetName => mAssetName;

            /// <summary>
            /// 所在资源名称
            /// </summary>
            public ResourceName ResourceName => mResourceName;

            /// <summary>
            /// 依赖资源名称
            /// </summary>
            public string[] DependencyAssetNames => mDependencyAssetNames;
        }
    }
}