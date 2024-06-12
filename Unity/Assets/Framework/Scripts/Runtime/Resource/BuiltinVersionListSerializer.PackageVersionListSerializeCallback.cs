// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/12 13:58:55
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.IO;
using System.Text;
using Framework;

namespace Runtime
{
    public static partial class BuiltinVersionListSerializer
    {
#if UNITY_EDITOR
        /// <summary>
        /// 序列化单机模式版本资源列表（版本0）回调函数
        /// </summary>
        /// <param name="stream">指定流</param>
        /// <param name="versionList">单机模式版本资源列表（版本0）</param>
        /// <returns>是否成功序列化单机模式版本资源列表（版本0）</returns>
        public static bool PackageVersionListSerializeCallback_V0(Stream stream, PackageVersionList versionList)
        {
            if (!versionList.IsValid)
            {
                return false;
            }

            Utility.Random.GetRandomBytes(sCachedHashBytes);
            using (var binaryWriter = new BinaryWriter(stream, Encoding.UTF8))
            {
                binaryWriter.Write(sCachedHashBytes);
                binaryWriter.WriteEncryptedString(versionList.ApplicableVersion, sCachedHashBytes);
                binaryWriter.Write(versionList.InternalResourceVersion);
                var assets = versionList.Assets;
                binaryWriter.Write(assets.Length);
                var resources = versionList.Resources;
                binaryWriter.Write(resources.Length);
                foreach (var resource in resources)
                {
                    binaryWriter.WriteEncryptedString(resource.Name, sCachedHashBytes);
                    binaryWriter.WriteEncryptedString(resource.Variant, sCachedHashBytes);
                    binaryWriter.Write(resource.LoadType);
                    binaryWriter.Write(resource.Length);
                    binaryWriter.Write(resource.HashCode);
                    var assetIndexes = resource.AssetIndexes;
                    binaryWriter.Write(assetIndexes.Length);
                    var hashBytes = new byte[CachedHashBytesLength];
                    foreach (var assetIndex in assetIndexes)
                    {
                        Utility.Converter.GetBytes(resource.HashCode, hashBytes);
                        var asset = assets[assetIndex];
                        binaryWriter.WriteEncryptedString(asset.Name, hashBytes);
                        var dependencyAssetIndexes = asset.DependencyAssetIndexes;
                        binaryWriter.Write(dependencyAssetIndexes.Length);
                        foreach (var dependencyAssetIndex in dependencyAssetIndexes)
                        {
                            binaryWriter.WriteEncryptedString(assets[dependencyAssetIndex].Name, hashBytes);
                        }
                    }
                }

                var resourceGroups = versionList.ResourceGroups;
                binaryWriter.Write(resourceGroups.Length);
                foreach (var resourceGroup in resourceGroups)
                {
                    binaryWriter.WriteEncryptedString(resourceGroup.Name, sCachedHashBytes);
                    var resourceIndexes = resourceGroup.ResourceIndexes;
                    binaryWriter.Write(resourceIndexes.Length);
                    foreach (var resourceIndex in resourceIndexes)
                    {
                        binaryWriter.Write(resourceIndex);
                    }
                }
            }

            Array.Clear(sCachedHashBytes, 0, CachedHashBytesLength);
            return true;
        }

        /// <summary>
        /// 序列化单机模式版本资源列表（版本1）回调函数
        /// </summary>
        /// <param name="stream">指定流</param>
        /// <param name="versionList">单机模式版本资源列表（版本1）</param>
        /// <returns>是否成功序列化单机模式版本资源列表（版本1）</returns>
        public static bool PackageVersionListSerializeCallback_V1(Stream stream, PackageVersionList versionList)
        {
            if (!versionList.IsValid)
            {
                return false;
            }

            Utility.Random.GetRandomBytes(sCachedHashBytes);
            using (var binaryWriter = new BinaryWriter(stream, Encoding.UTF8))
            {
                binaryWriter.Write(sCachedHashBytes);
                binaryWriter.WriteEncryptedString(versionList.ApplicableVersion, sCachedHashBytes);
                binaryWriter.Write7BitEncodedInt32(versionList.InternalResourceVersion);

                var assets = versionList.Assets;
                binaryWriter.Write7BitEncodedInt32(assets.Length);
                foreach (var asset in assets)
                {
                    binaryWriter.WriteEncryptedString(asset.Name, sCachedHashBytes);
                    var dependencyAssetIndexes = asset.DependencyAssetIndexes;
                    binaryWriter.Write7BitEncodedInt32(dependencyAssetIndexes.Length);
                    foreach (var dependencyAssetIndex in dependencyAssetIndexes)
                    {
                        binaryWriter.Write7BitEncodedInt32(dependencyAssetIndex);
                    }
                }

                var resources = versionList.Resources;
                binaryWriter.Write7BitEncodedInt32(resources.Length);
                foreach (var resource in resources)
                {
                    binaryWriter.WriteEncryptedString(resource.Name, sCachedHashBytes);
                    binaryWriter.WriteEncryptedString(resource.Variant, sCachedHashBytes);
                    binaryWriter.WriteEncryptedString(resource.Extension != DefaultExtension ? resource.Extension : null, sCachedHashBytes);
                    binaryWriter.Write(resource.LoadType);
                    binaryWriter.Write7BitEncodedInt32(resource.Length);
                    binaryWriter.Write(resource.HashCode);
                    var assetIndexes = resource.AssetIndexes;
                    binaryWriter.Write7BitEncodedInt32(assetIndexes.Length);
                    foreach (var assetIndex in assetIndexes)
                    {
                        binaryWriter.Write7BitEncodedInt32(assetIndex);
                    }
                }

                var resourceGroups = versionList.ResourceGroups;
                binaryWriter.Write7BitEncodedInt32(resourceGroups.Length);
                foreach (var resourceGroup in resourceGroups)
                {
                    binaryWriter.WriteEncryptedString(resourceGroup.Name, sCachedHashBytes);
                    var resourceIndexes = resourceGroup.ResourceIndexes;
                    binaryWriter.Write7BitEncodedInt32(resourceIndexes.Length);
                    foreach (var resourceIndex in resourceIndexes)
                    {
                        binaryWriter.Write7BitEncodedInt32(resourceIndex);
                    }
                }
            }

            Array.Clear(sCachedHashBytes, 0, CachedHashBytesLength);
            return true;
        }

        /// <summary>
        /// 序列化单机模式版本资源列表（版本2）回调函数
        /// </summary>
        /// <param name="stream">指定流</param>
        /// <param name="versionList">单机模式版本资源列表（版本2）</param>
        /// <returns>是否成功序列化单机模式版本资源列表（版本2）</returns>
        public static bool PackageVersionListSerializeCallback_V2(Stream stream, PackageVersionList versionList)
        {
            if (!versionList.IsValid)
            {
                return false;
            }

            Utility.Random.GetRandomBytes(sCachedHashBytes);
            using (var binaryWriter = new BinaryWriter(stream, Encoding.UTF8))
            {
                binaryWriter.Write(sCachedHashBytes);
                binaryWriter.WriteEncryptedString(versionList.ApplicableVersion, sCachedHashBytes);
                binaryWriter.Write7BitEncodedInt32(versionList.InternalResourceVersion);

                var assets = versionList.Assets;
                binaryWriter.Write7BitEncodedInt32(assets.Length);
                foreach (var asset in assets)
                {
                    binaryWriter.WriteEncryptedString(asset.Name, sCachedHashBytes);
                    var dependencyAssetIndexes = asset.DependencyAssetIndexes;
                    binaryWriter.Write7BitEncodedInt32(dependencyAssetIndexes.Length);
                    foreach (var dependencyAssetIndex in dependencyAssetIndexes)
                    {
                        binaryWriter.Write7BitEncodedInt32(dependencyAssetIndex);
                    }
                }

                var resources = versionList.Resources;
                binaryWriter.Write7BitEncodedInt32(resources.Length);
                foreach (var resource in resources)
                {
                    binaryWriter.WriteEncryptedString(resource.Name, sCachedHashBytes);
                    binaryWriter.WriteEncryptedString(resource.Variant, sCachedHashBytes);
                    binaryWriter.WriteEncryptedString(resource.Extension != DefaultExtension ? resource.Extension : null, sCachedHashBytes);
                    binaryWriter.Write(resource.LoadType);
                    binaryWriter.Write7BitEncodedInt32(resource.Length);
                    binaryWriter.Write(resource.HashCode);
                    var assetIndexes = resource.AssetIndexes;
                    binaryWriter.Write7BitEncodedInt32(assetIndexes.Length);
                    foreach (var assetIndex in assetIndexes)
                    {
                        binaryWriter.Write7BitEncodedInt32(assetIndex);
                    }
                }

                var fileSystems = versionList.FileSystems;
                binaryWriter.Write7BitEncodedInt32(fileSystems.Length);
                foreach (var fileSystem in fileSystems)
                {
                    binaryWriter.WriteEncryptedString(fileSystem.Name, sCachedHashBytes);
                    var resourceIndexes = fileSystem.ResourceIndexes;
                    binaryWriter.Write7BitEncodedInt32(resourceIndexes.Length);
                    foreach (var resourceIndex in resourceIndexes)
                    {
                        binaryWriter.Write7BitEncodedInt32(resourceIndex);
                    }
                }

                var resourceGroups = versionList.ResourceGroups;
                binaryWriter.Write7BitEncodedInt32(resourceGroups.Length);
                foreach (var resourceGroup in resourceGroups)
                {
                    binaryWriter.WriteEncryptedString(resourceGroup.Name, sCachedHashBytes);
                    var resourceIndexes = resourceGroup.ResourceIndexes;
                    binaryWriter.Write7BitEncodedInt32(resourceIndexes.Length);
                    foreach (var resourceIndex in resourceIndexes)
                    {
                        binaryWriter.Write7BitEncodedInt32(resourceIndex);
                    }
                }
            }

            Array.Clear(sCachedHashBytes, 0, CachedHashBytesLength);
            return true;
        }
#endif
    }
}