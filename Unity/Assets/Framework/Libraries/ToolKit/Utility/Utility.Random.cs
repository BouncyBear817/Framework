// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/16 15:3:10
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;

namespace Framework
{
    public static partial class Utility
    {
        /// <summary>
        /// Random相关的实用函数
        /// </summary>
        public static class Random
        {
            private static System.Random sRandom = new System.Random((int)DateTime.UtcNow.Ticks);

            /// <summary>
            /// 设置随机数的种子
            /// </summary>
            /// <param name="seed">随机数的种子</param>
            public static void SetSeed(int seed)
            {
                sRandom = new System.Random(seed);
            }

            /// <summary>
            /// 返回非负随机数
            /// </summary>
            /// <returns>大于等于0且小于System.Int32.MaxValue的32位带符号的整数</returns>
            public static int GetRandom()
            {
                return sRandom.Next();
            }

            /// <summary>
            /// 返回非负随机数
            /// </summary>
            /// <param name="maxValue">随机数的上届值，大于等于0</param>
            /// <returns>大于等于0且小于maxValue的32位带符号的整数</returns>
            public static int GetRandom(int maxValue)
            {
                return sRandom.Next(maxValue);
            }

            /// <summary>
            /// 返回随机数
            /// </summary>
            /// <param name="minValue">随机数的下届值</param>
            /// <param name="maxValue">随机数的上届值</param>
            /// <returns>大于等于minValue且小于maxValue的32位带符号的整数</returns>
            public static int GetRandom(int minValue, int maxValue)
            {
                return sRandom.Next(minValue, maxValue);
            }

            /// <summary>
            /// 返回一个介于0.0到1.0之间的随机双精度浮点数
            /// </summary>
            /// <returns>一个介于0.0到1.0之间的双精度浮点数</returns>
            public static double GetRandomDouble()
            {
                return sRandom.NextDouble();
            }

            /// <summary>
            /// 用随机数填充指定字节数组的元素
            /// </summary>
            /// <param name="buffer">包含随机数的字节数组</param>
            public static void GetRandomBytes(byte[] buffer)
            {
                sRandom.NextBytes(buffer);
            }
        }
    }
}