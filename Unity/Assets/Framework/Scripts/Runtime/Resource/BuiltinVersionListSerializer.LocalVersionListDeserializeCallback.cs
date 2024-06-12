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

namespace Runtime
{
    public static partial class BuiltinVersionListSerializer
    {
        /// <summary>
        /// 反序列化本地版本资源列表（版本0）回调函数
        /// </summary>
        /// <param name="stream">指定流</param>
        /// <returns>反序列化本地版本资源列表（版本0）</returns>
        public static LocalVersionList LocalVersionListDeserializeCallback_V0(Stream stream)
        {
            using (var binaryReader = new BinaryReader(stream, Encoding.UTF8))
            {
                var encryptBytes = binaryReader.ReadBytes(CachedHashBytesLength);
                
                var resourceCount = binaryReader.ReadInt32();
                var resources = resourceCount > 0 ? new LocalVersionList.Resource[resourceCount] : null;
                if (resources != null)
                {
                    for (var i = 0; i < resourceCount; i++)
                    {
                        var name = binaryReader.ReadEncryptedString(encryptBytes);
                        var variant = binaryReader.ReadEncryptedString(encryptBytes);
                        var loadType = binaryReader.ReadByte();
                        var length = binaryReader.ReadInt32();
                        var hashCode = binaryReader.ReadInt32();
                        resources[i] = new LocalVersionList.Resource(name, variant, null, loadType, length, hashCode);
                    }
                }

                return new LocalVersionList(resources, null);
            }
        }

        /// <summary>
        /// 反序列化本地版本资源列表（版本1）回调函数
        /// </summary>
        /// <param name="stream">指定流</param>
        /// <returns>反序列化本地版本资源列表（版本1）</returns>
        public static LocalVersionList LocalVersionListDeserializeCallback_V1(Stream stream)
        {
            using (var binaryReader = new BinaryReader(stream, Encoding.UTF8))
            {
                var encryptBytes = binaryReader.ReadBytes(CachedHashBytesLength);
                
                var resourceCount = binaryReader.Read7BitEncodedInt32();
                var resources = resourceCount > 0 ? new LocalVersionList.Resource[resourceCount] : null;
                if (resources != null)
                {
                    for (var i = 0; i < resourceCount; i++)
                    {
                        var name = binaryReader.ReadEncryptedString(encryptBytes);
                        var variant = binaryReader.ReadEncryptedString(encryptBytes);
                        var extension = binaryReader.ReadEncryptedString(encryptBytes) ?? DefaultExtension;
                        var loadType = binaryReader.ReadByte();
                        var length = binaryReader.ReadInt32();
                        var hashCode = binaryReader.ReadInt32();
                        resources[i] = new LocalVersionList.Resource(name, variant, extension, loadType, length, hashCode);
                    }
                }

                return new LocalVersionList(resources, null);
            }
        }

        /// <summary>
        /// 反序列化本地版本资源列表（版本2）回调函数
        /// </summary>
        /// <param name="stream">指定流</param>
        /// <returns>反序列化本地版本资源列表（版本2）</returns>
        public static LocalVersionList LocalVersionListDeserializeCallback_V2(Stream stream)
        {
            using (var binaryReader = new BinaryReader(stream, Encoding.UTF8))
            {
                var encryptBytes = binaryReader.ReadBytes(CachedHashBytesLength);
                
                var resourceCount = binaryReader.Read7BitEncodedInt32();
                var resources = resourceCount > 0 ? new LocalVersionList.Resource[resourceCount] : null;
                if (resources != null)
                {
                    for (var i = 0; i < resourceCount; i++)
                    {
                        var name = binaryReader.ReadEncryptedString(encryptBytes);
                        var variant = binaryReader.ReadEncryptedString(encryptBytes);
                        var extension = binaryReader.ReadEncryptedString(encryptBytes) ?? DefaultExtension;
                        var loadType = binaryReader.ReadByte();
                        var length = binaryReader.ReadInt32();
                        var hashCode = binaryReader.ReadInt32();
                        resources[i] = new LocalVersionList.Resource(name, variant, extension, loadType, length, hashCode);
                    }
                }

                var fileSystemCount = binaryReader.Read7BitEncodedInt32();
                var fileSystems = fileSystemCount > 0 ? new LocalVersionList.FileSystem[fileSystemCount] : null;
                if (fileSystems != null)
                {
                    for (var i = 0; i < fileSystemCount; i++)
                    {
                        var name = binaryReader.ReadEncryptedString(encryptBytes);
                        var resourceIndexCount = binaryReader.Read7BitEncodedInt32();
                        var resourceIndexes = resourceIndexCount > 0 ? new int[resourceIndexCount] : null;
                        if (resourceIndexes != null)
                        {
                            for (int j = 0; j < resourceIndexCount; j++)
                            {
                                resourceIndexes[j] = binaryReader.Read7BitEncodedInt32();
                            }
                        }

                        fileSystems[i] = new LocalVersionList.FileSystem(name, resourceIndexes);
                    }
                }

                return new LocalVersionList(resources, fileSystems);
            }
        }
    }
}