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
        /// <summary>
        /// 序列化资源包版本资源列表（版本0）回调函数
        /// </summary>
        /// <param name="stream">指定流</param>
        /// <param name="versionList">资源包版本资源列表（版本0）</param>
        /// <returns>是否成功序列化资源包版本资源列表（版本0）</returns>
        public static bool ResourcePackVersionListSerializeCallback_V0(Stream stream, ResourcePackVersionList versionList)
        {
            if (!versionList.IsValid)
            {
                return false;
            }

            Utility.Random.GetRandomBytes(sCachedHashBytes);
            using (var binaryWriter = new BinaryWriter(stream, Encoding.UTF8))
            {
                binaryWriter.Write(sCachedHashBytes);
                binaryWriter.Write(versionList.Offset);
                binaryWriter.Write(versionList.Length);
                binaryWriter.Write(versionList.HashCode);
                var resources = versionList.Resources;
                binaryWriter.Write7BitEncodedInt32(resources.Length);
                foreach (var resource in resources)
                {
                    binaryWriter.WriteEncryptedString(resource.Name, sCachedHashBytes);
                    binaryWriter.WriteEncryptedString(resource.Variant, sCachedHashBytes);
                    binaryWriter.WriteEncryptedString(resource.Extension != DefaultExtension ? resource.Extension : null, sCachedHashBytes);
                    binaryWriter.Write(resource.LoadType);
                    binaryWriter.Write7BitEncodedInt64(resource.Offset);
                    binaryWriter.Write7BitEncodedInt32(resource.Length);
                    binaryWriter.Write(resource.HashCode);
                    binaryWriter.Write7BitEncodedInt32(resource.CompressedLength);
                    binaryWriter.Write(resource.CompressedHashCode);
                }
            }

            Array.Clear(sCachedHashBytes, 0, CachedHashBytesLength);
            return true;
        }
    }
}