/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/02/02 16:46:20
 * Description:   
 * Modify Record: 
 *************************************************************/

using System;
using System.IO;

namespace Runtime
{
    /// <summary>
    /// 对BinaryReader与BinaryWriter的扩展方法
    /// </summary>
    public static class BinaryExtension
    {
        /// <summary>
        /// 从二进制流读取编码后的32位有符号整数
        /// </summary>
        /// <param name="binaryReader">二进制流</param>
        /// <returns>32位有符号整数</returns>
        /// <exception cref="Exception"></exception>
        public static int Read7BitEncodedInt32(this BinaryReader binaryReader)
        {
            var value = 0;
            var shift = 0;
            byte b;
            do
            {
                if (shift >= 35)
                {
                    throw new Exception("7 bit encoded int is invalid.");
                }

                b = binaryReader.ReadByte();
                value |= (b & 0x7f) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);

            return value;
        }
    }
}