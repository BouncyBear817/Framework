// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/5 15:36:41
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;

namespace Framework
{
    /// <summary>
    /// 文件信息
    /// </summary>
    public struct FileInfo
    {
        private readonly string mName;
        private readonly long mOffset;
        private readonly int mLength;

        public FileInfo(string name, long offset, int length)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Name is invalid.");
            }

            if (offset < 0L)
            {
                throw new Exception("Offset is invalid.");
            }

            if (length < 0)
            {
                throw new Exception("Length is invalid.");
            }

            mName = name;
            mOffset = offset;
            mLength = length;
        }

        /// <summary>
        /// 文件是否有效
        /// </summary>
        public bool IsValid => !string.IsNullOrEmpty(mName) && mOffset >= 0L && mLength >= 0;

        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name => mName;

        /// <summary>
        /// 文件偏移
        /// </summary>
        public long Offset => mOffset;

        /// <summary>
        /// 文件长度
        /// </summary>
        public int Length => mLength;
    }
}