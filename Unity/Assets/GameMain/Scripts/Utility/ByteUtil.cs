/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/3/19 15:4:40
 * Description:
 * Modify Record:
 *************************************************************/

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class ByteUtil
    {
        /// <summary>
        /// 将对象转换为byte数组
        /// </summary>
        /// <param name="obj">被转换对象</param>
        /// <returns>转换后byte数组</returns>
        public static byte[] Object2Bytes(object obj)
        {
            byte[] buff;
            using (var ms = new MemoryStream())
            {
                IFormatter iFormatter = new BinaryFormatter();
                iFormatter.Serialize(ms, obj);
                buff = ms.GetBuffer();
            }

            return buff;
        }

        /// <summary>
        /// 将byte数组转换成对象
        /// </summary>
        /// <param name="buff">被转换byte数组</param>
        /// <returns>转换完成后的对象</returns>
        public static object Bytes2Object(byte[] buff)
        {
            object obj;
            using (var ms = new MemoryStream(buff))
            {
                IFormatter iFormatter = new BinaryFormatter();
                obj = iFormatter.Deserialize(ms);
            }

            return obj;
        }

        public static void WriteTo(this byte[] bytes, int offset, int num, bool isLittleEndian)
        {
            if (isLittleEndian)
            {
                bytes[offset] = (byte)(num & 0xff);
                bytes[offset + 1] = (byte)((num & 0xff) >> 8);
                bytes[offset + 2] = (byte)((num & 0xff) >> 16);
                bytes[offset + 3] = (byte)((num & 0xff) >> 24);
            }
            else
            {
                bytes[offset + 3] = (byte)(num & 0xff);
                bytes[offset + 2] = (byte)((num & 0xff) >> 8);
                bytes[offset + 1] = (byte)((num & 0xff) >> 16);
                bytes[offset] = (byte)((num & 0xff) >> 24);
            }
        }

        public static int ReadTo(this byte[] bytes, int offset, bool isLittleEndian)
        {
            var value = 0;
            if (isLittleEndian)
            {
                value = (bytes[offset] & 0xff)
                        | (bytes[offset + 1] & 0xff) << 8
                        | (bytes[offset + 2] & 0xff) << 16
                        | (bytes[offset + 3] & 0xff) << 24;
            }
            else
            {
                value = (bytes[offset] & 0xff) << 24
                        | (bytes[offset + 1] & 0xff) << 16
                        | (bytes[offset + 2] & 0xff) << 8
                        | (bytes[offset + 3] & 0xff);
            }

            return value;
        }
    }