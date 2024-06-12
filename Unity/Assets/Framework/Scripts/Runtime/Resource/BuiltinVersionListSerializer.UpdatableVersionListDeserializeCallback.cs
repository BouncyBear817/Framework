// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/12 13:58:55
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Framework;

namespace Runtime
{
    public static partial class BuiltinVersionListSerializer
    {
        /// <summary>
        /// 反序列化可更新模式版本资源列表（版本0）回调函数
        /// </summary>
        /// <param name="stream">指定流</param>
        /// <returns>反序列化可更新模式版本资源列表（版本0）</returns>
        public static UpdatableVersionList UpdatableVersionListDeserializeCallback_V0(Stream stream)
        {
            using (var binaryReader = new BinaryReader(stream, Encoding.UTF8))
            {
                var encryptBytes = binaryReader.ReadBytes(CachedHashBytesLength);
                var applicableVersion = binaryReader.ReadEncryptedString(encryptBytes);
                var internalResourceVersion = binaryReader.ReadInt32();

                var assetCount = binaryReader.ReadInt32();
                var assets = assetCount > 0 ? new UpdatableVersionList.Asset[assetCount] : null;
                var resourceCount = binaryReader.ReadInt32();
                var resources = resourceCount > 0 ? new UpdatableVersionList.Resource[resourceCount] : null;
                var resourceToAssetNames = new string[resourceCount][];
                var assetNameToDependencyAssetNames = new List<KeyValuePair<string, string[]>>(assetCount);
                for (var i = 0; i < resourceCount; i++)
                {
                    var name = binaryReader.ReadEncryptedString(encryptBytes);
                    var variant = binaryReader.ReadEncryptedString(encryptBytes);
                    var loadType = binaryReader.ReadByte();
                    var length = binaryReader.ReadInt32();
                    var hashCode = binaryReader.ReadInt32();
                    var compressedLength = binaryReader.ReadInt32();
                    var compressedHashCode = binaryReader.ReadInt32();
                    Utility.Converter.GetBytes(hashCode, sCachedHashBytes);

                    var assetNameCount = binaryReader.ReadInt32();
                    var assetNames = new string[assetNameCount];
                    for (var j = 0; j < assetNameCount; j++)
                    {
                        assetNames[j] = binaryReader.ReadEncryptedString(sCachedHashBytes);
                        var dependencyAssetNameCount = binaryReader.ReadInt32();
                        var dependencyAssetNames = dependencyAssetNameCount > 0 ? new string[dependencyAssetNameCount] : null;
                        for (int k = 0; k < dependencyAssetNameCount; k++)
                        {
                            dependencyAssetNames[k] = binaryReader.ReadEncryptedString(sCachedHashBytes);
                        }

                        assetNameToDependencyAssetNames.Add(new KeyValuePair<string, string[]>(assetNames[j], dependencyAssetNames));
                    }

                    resourceToAssetNames[i] = assetNames;
                    resources[i] = new UpdatableVersionList.Resource(name, variant, null, loadType, length, hashCode, compressedLength, compressedHashCode,
                        assetNameCount > 0 ? new int[assetNameCount] : null);
                }

                assetNameToDependencyAssetNames.Sort(AssetNameToDependencyAssetNamesComparer);
                Array.Clear(sCachedHashBytes, 0, CachedHashBytesLength);
                var index = 0;
                foreach (var (key, value) in assetNameToDependencyAssetNames)
                {
                    if (value != null)
                    {
                        var dependencyAssetIndexes = new int[value.Length];
                        for (var i = 0; i < value.Length; i++)
                        {
                            dependencyAssetIndexes[i] = GetAssetNameIndex(assetNameToDependencyAssetNames, value[i]);
                        }

                        assets[index++] = new UpdatableVersionList.Asset(key, dependencyAssetIndexes);
                    }
                    else
                    {
                        assets[index++] = new UpdatableVersionList.Asset(key, null);
                    }
                }

                var resourceGroupCount = binaryReader.ReadInt32();
                var resourceGroups = resourceGroupCount > 0 ? new UpdatableVersionList.ResourceGroup[resourceGroupCount] : null;
                for (var i = 0; i < resourceGroupCount; i++)
                {
                    var name = binaryReader.ReadEncryptedString(encryptBytes);
                    var resourceIndexCount = binaryReader.ReadInt32();
                    var resourceIndexes = resourceIndexCount > 0 ? new int[resourceIndexCount] : null;
                    for (var j = 0; j < resourceIndexCount; j++)
                    {
                        resourceIndexes[j] = binaryReader.ReadUInt16();
                    }

                    resourceGroups[i] = new UpdatableVersionList.ResourceGroup(name, resourceIndexes);
                }

                return new UpdatableVersionList(applicableVersion, internalResourceVersion, assets, resources, null, resourceGroups);
            }
        }

        /// <summary>
        /// 反序列化可更新模式版本资源列表（版本1）回调函数
        /// </summary>
        /// <param name="stream">指定流</param>
        /// <returns>反序列化可更新模式版本资源列表（版本1）</returns>
        public static UpdatableVersionList UpdatableVersionListDeserializeCallback_V1(Stream stream)
        {
            using (var binaryReader = new BinaryReader(stream, Encoding.UTF8))
            {
                var encryptBytes = binaryReader.ReadBytes(CachedHashBytesLength);
                var applicableVersion = binaryReader.ReadEncryptedString(encryptBytes);
                var internalResourceVersion = binaryReader.Read7BitEncodedInt32();

                var assetCount = binaryReader.Read7BitEncodedInt32();
                var assets = assetCount > 0 ? new UpdatableVersionList.Asset[assetCount] : null;
                for (var i = 0; i < assetCount; i++)
                {
                    var name = binaryReader.ReadEncryptedString(encryptBytes);
                    var dependencyAssetCount = binaryReader.Read7BitEncodedInt32();
                    var dependencyAssetIndexes = dependencyAssetCount > 0 ? new int[dependencyAssetCount] : null;
                    for (int j = 0; j < dependencyAssetCount; j++)
                    {
                        dependencyAssetIndexes[j] = binaryReader.Read7BitEncodedInt32();
                    }

                    assets[i] = new UpdatableVersionList.Asset(name, dependencyAssetIndexes);
                }

                var resourceCount = binaryReader.Read7BitEncodedInt32();
                var resources = resourceCount > 0 ? new UpdatableVersionList.Resource[resourceCount] : null;
                for (var i = 0; i < resourceCount; i++)
                {
                    var name = binaryReader.ReadEncryptedString(encryptBytes);
                    var variant = binaryReader.ReadEncryptedString(encryptBytes);
                    var extension = binaryReader.ReadEncryptedString(encryptBytes) ?? DefaultExtension;
                    var loadType = binaryReader.ReadByte();
                    var length = binaryReader.Read7BitEncodedInt32();
                    var hashCode = binaryReader.ReadInt32();
                    var compressedLength = binaryReader.Read7BitEncodedInt32();
                    var compressedHashCode = binaryReader.ReadInt32();
                    var assetIndexCount = binaryReader.Read7BitEncodedInt32();
                    var assetIndexes = assetIndexCount > 0 ? new int[assetIndexCount] : null;
                    for (int j = 0; j < assetIndexCount; j++)
                    {
                        assetIndexes[j] = binaryReader.Read7BitEncodedInt32();
                    }

                    resources[i] = new UpdatableVersionList.Resource(name, variant, extension, loadType, length, hashCode, compressedLength, compressedHashCode, assetIndexes);
                }

                var resourceGroupCount = binaryReader.Read7BitEncodedInt32();
                var resourceGroups = resourceGroupCount > 0 ? new UpdatableVersionList.ResourceGroup[resourceGroupCount] : null;
                for (var i = 0; i < resourceGroupCount; i++)
                {
                    var name = binaryReader.ReadEncryptedString(encryptBytes);
                    var resourceIndexCount = binaryReader.Read7BitEncodedInt32();
                    var resourceIndexes = resourceIndexCount > 0 ? new int[resourceIndexCount] : null;
                    for (var j = 0; j < resourceIndexCount; j++)
                    {
                        resourceIndexes[j] = binaryReader.Read7BitEncodedInt32();
                    }

                    resourceGroups[i] = new UpdatableVersionList.ResourceGroup(name, resourceIndexes);
                }

                return new UpdatableVersionList(applicableVersion, internalResourceVersion, assets, resources, null, resourceGroups);
            }
        }

        /// <summary>
        /// 反序列化可更新模式版本资源列表（版本2）回调函数
        /// </summary>
        /// <param name="stream">指定流</param>
        /// <returns>反序列化可更新模式版本资源列表（版本2）</returns>
        public static UpdatableVersionList UpdatableVersionListDeserializeCallback_V2(Stream stream)
        {
            using (var binaryReader = new BinaryReader(stream, Encoding.UTF8))
            {
                var encryptBytes = binaryReader.ReadBytes(CachedHashBytesLength);
                var applicableVersion = binaryReader.ReadEncryptedString(encryptBytes);
                var internalResourceVersion = binaryReader.Read7BitEncodedInt32();

                var assetCount = binaryReader.Read7BitEncodedInt32();
                var assets = assetCount > 0 ? new UpdatableVersionList.Asset[assetCount] : null;
                for (var i = 0; i < assetCount; i++)
                {
                    var name = binaryReader.ReadEncryptedString(encryptBytes);
                    var dependencyAssetCount = binaryReader.Read7BitEncodedInt32();
                    var dependencyAssetIndexes = dependencyAssetCount > 0 ? new int[dependencyAssetCount] : null;
                    for (int j = 0; j < dependencyAssetCount; j++)
                    {
                        dependencyAssetIndexes[j] = binaryReader.Read7BitEncodedInt32();
                    }

                    assets[i] = new UpdatableVersionList.Asset(name, dependencyAssetIndexes);
                }

                var resourceCount = binaryReader.Read7BitEncodedInt32();
                var resources = resourceCount > 0 ? new UpdatableVersionList.Resource[resourceCount] : null;
                for (var i = 0; i < resourceCount; i++)
                {
                    var name = binaryReader.ReadEncryptedString(encryptBytes);
                    var variant = binaryReader.ReadEncryptedString(encryptBytes);
                    var extension = binaryReader.ReadEncryptedString(encryptBytes) ?? DefaultExtension;
                    var loadType = binaryReader.ReadByte();
                    var length = binaryReader.Read7BitEncodedInt32();
                    var hashCode = binaryReader.ReadInt32();
                    var compressedLength = binaryReader.Read7BitEncodedInt32();
                    var compressedHashCode = binaryReader.ReadInt32();
                    var assetIndexCount = binaryReader.Read7BitEncodedInt32();
                    var assetIndexes = assetIndexCount > 0 ? new int[assetIndexCount] : null;
                    for (int j = 0; j < assetIndexCount; j++)
                    {
                        assetIndexes[j] = binaryReader.Read7BitEncodedInt32();
                    }

                    resources[i] = new UpdatableVersionList.Resource(name, variant, extension, loadType, length, hashCode, compressedLength, compressedHashCode, assetIndexes);
                }

                var fileSystemCount = binaryReader.Read7BitEncodedInt32();
                var fileSystems = fileSystemCount > 0 ? new UpdatableVersionList.FileSystem[fileSystemCount] : null;
                for (int i = 0; i < fileSystemCount; i++)
                {
                    var name = binaryReader.ReadEncryptedString(encryptBytes);
                    var resourceIndexCount = binaryReader.Read7BitEncodedInt32();
                    var resourceIndexes = resourceIndexCount > 0 ? new int[resourceIndexCount] : null;
                    for (int j = 0; j < resourceIndexCount; j++)
                    {
                        resourceIndexes[j] = binaryReader.Read7BitEncodedInt32();
                    }

                    fileSystems[i] = new UpdatableVersionList.FileSystem(name, resourceIndexes);
                }

                var resourceGroupCount = binaryReader.Read7BitEncodedInt32();
                var resourceGroups = resourceGroupCount > 0 ? new UpdatableVersionList.ResourceGroup[resourceGroupCount] : null;
                for (var i = 0; i < resourceGroupCount; i++)
                {
                    var name = binaryReader.ReadEncryptedString(encryptBytes);
                    var resourceIndexCount = binaryReader.Read7BitEncodedInt32();
                    var resourceIndexes = resourceIndexCount > 0 ? new int[resourceIndexCount] : null;
                    for (var j = 0; j < resourceIndexCount; j++)
                    {
                        resourceIndexes[j] = binaryReader.Read7BitEncodedInt32();
                    }

                    resourceGroups[i] = new UpdatableVersionList.ResourceGroup(name, resourceIndexes);
                }

                return new UpdatableVersionList(applicableVersion, internalResourceVersion, assets, resources, fileSystems, resourceGroups);
            }
        }
    }
}