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

namespace Framework.Runtime
{
    public static partial class BuiltinVersionListSerializer
    {
        /// <summary>
        /// 序列化本地版本资源列表（版本0）回调函数
        /// </summary>
        /// <param name="stream">指定流</param>
        /// <param name="versionList">本地版本资源列表（版本0）</param>
        /// <returns>是否成功序列化本地版本资源列表（版本0）</returns>
        public static bool LocalVersionListSerializeCallback_V0(Stream stream, LocalVersionList versionList)
        {
            if (!versionList.IsValid)
            {
                return false;
            }

            Utility.Random.GetRandomBytes(sCachedHashBytes);
            using (var binaryWriter = new BinaryWriter(stream, Encoding.UTF8))
            {
                binaryWriter.Write(sCachedHashBytes);
                
                var resources = versionList.Resources;
                binaryWriter.Write(resources.Length);
                foreach (var resource in resources)
                {
                    binaryWriter.WriteEncryptedString(resource.Name, sCachedHashBytes);
                    binaryWriter.WriteEncryptedString(resource.Variant, sCachedHashBytes);
                    binaryWriter.Write(resource.LoadType);
                    binaryWriter.Write(resource.Length);
                    binaryWriter.Write(resource.HashCode);
                }
            }

            Array.Clear(sCachedHashBytes, 0, CachedHashBytesLength);
            return true;
        }

        /// <summary>
        /// 序列化本地版本资源列表（版本1）回调函数
        /// </summary>
        /// <param name="stream">指定流</param>
        /// <param name="versionList">本地版本资源列表（版本1）</param>
        /// <returns>是否成功序列化本地版本资源列表（版本1）</returns>
        public static bool LocalVersionListSerializeCallback_V1(Stream stream, LocalVersionList versionList)
        {
            if (!versionList.IsValid)
            {
                return false;
            }

            Utility.Random.GetRandomBytes(sCachedHashBytes);
            using (var binaryWriter = new BinaryWriter(stream, Encoding.UTF8))
            {
                binaryWriter.Write(sCachedHashBytes);
                
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
                }
            }

            Array.Clear(sCachedHashBytes, 0, CachedHashBytesLength);
            return true;
        }

        /// <summary>
        /// 序列化本地版本资源列表（版本2）回调函数
        /// </summary>
        /// <param name="stream">指定流</param>
        /// <param name="versionList">本地版本资源列表（版本2）</param>
        /// <returns>是否成功序列化本地版本资源列表（版本2）</returns>
        public static bool LocalVersionListSerializeCallback_V2(Stream stream, LocalVersionList versionList)
        {
            if (!versionList.IsValid)
            {
                return false;
            }

            Utility.Random.GetRandomBytes(sCachedHashBytes);
            using (var binaryWriter = new BinaryWriter(stream, Encoding.UTF8))
            {
                binaryWriter.Write(sCachedHashBytes);
                
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
            }

            Array.Clear(sCachedHashBytes, 0, CachedHashBytesLength);
            return true;
        }
    }
}