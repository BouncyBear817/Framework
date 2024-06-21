// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/12 13:58:55
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.IO;
using System.Text;
using Framework;

namespace Framework.Runtime
{
    public static partial class BuiltinVersionListSerializer
    {
        /// <summary>
        /// 反序列化资源包版本资源列表（版本0）回调函数
        /// </summary>
        /// <param name="stream">指定流</param>
        /// <returns>反序列化资源包版本资源列表（版本0）</returns>
        public static ResourcePackVersionList ResourcePackVersionListDeserializeCallback_V0(Stream stream)
        {
            using (var binaryReader = new BinaryReader(stream, Encoding.UTF8))
            {
                var encryptBytes = binaryReader.ReadBytes(CachedHashBytesLength);
                var dataOffset = binaryReader.ReadInt32();
                var dataLength = binaryReader.ReadInt64();
                var dataHashCode = binaryReader.ReadInt32();

                var resourceCount = binaryReader.Read7BitEncodedInt32();
                var resources = resourceCount > 0 ? new ResourcePackVersionList.Resource[resourceCount] : null;
                for (int i = 0; i < resourceCount; i++)
                {
                    var name = binaryReader.ReadEncryptedString(encryptBytes);
                    var variant = binaryReader.ReadEncryptedString(encryptBytes);
                    var extension = binaryReader.ReadEncryptedString(encryptBytes) ?? DefaultExtension;
                    var loadType = binaryReader.ReadByte();
                    var offset = binaryReader.Read7BitEncodedInt64();
                    var length = binaryReader.Read7BitEncodedInt32();
                    var hashCode = binaryReader.ReadInt32();
                    var compressedLength = binaryReader.Read7BitEncodedInt32();
                    var compressedHashCode = binaryReader.ReadInt32();
                    resources[i] = new ResourcePackVersionList.Resource(name, variant, extension, loadType, offset, length, hashCode, compressedLength, compressedHashCode);
                }

                return new ResourcePackVersionList(dataOffset, dataLength, dataHashCode, resources);
            }
        }
    }
}