// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/12 13:58:55
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.IO;
using System.Text;

namespace Framework.Runtime
{
    public static partial class BuiltinVersionListSerializer
    {
        /// <summary>
        /// 尝试从可更新模式版本资源列表（版本0）获取指定键的值回调函数
        /// </summary>
        /// <param name="stream">指定流</param>
        /// <param name="key">指定键</param>
        /// <param name="value">指定键的值</param>
        /// <returns>是否成功从可更新模式版本资源列表（版本0）获取指定键的值</returns>
        public static bool UpdatableVersionListTryGetValueCallback_V0(Stream stream, string key, out object value)
        {
            value = null;
            if (key != "InternalResourceVersion")
            {
                return false;
            }

            using (var binaryReader = new BinaryReader(stream, Encoding.UTF8))
            {
                binaryReader.BaseStream.Position += CachedHashBytesLength;
                var stringLength = binaryReader.ReadByte();
                binaryReader.BaseStream.Position += stringLength;
                value = binaryReader.ReadInt32();
            }

            return true;
        }

        /// <summary>
        /// 尝试从可更新模式版本资源列表（版本1或版本2）获取指定键的值回调函数
        /// </summary>
        /// <param name="stream">指定流</param>
        /// <param name="key">指定键</param>
        /// <param name="value">指定键的值</param>
        /// <returns>是否成功从可更新模式版本资源列表（版本1或版本2）获取指定键的值</returns>
        public static bool UpdatableVersionListTryGetValueCallback_V1_V2(Stream stream, string key, out object value)
        {
            value = null;
            if (key != "InternalResourceVersion")
            {
                return false;
            }

            using (var binaryReader = new BinaryReader(stream, Encoding.UTF8))
            {
                binaryReader.BaseStream.Position += CachedHashBytesLength;
                var stringLength = binaryReader.ReadByte();
                binaryReader.BaseStream.Position += stringLength;
                value = binaryReader.Read7BitEncodedInt32();
            }

            return true;
        }
    }
}