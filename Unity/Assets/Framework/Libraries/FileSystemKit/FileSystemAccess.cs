// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/5 14:38:53
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;

namespace Framework
{
    /// <summary>
    /// 文件系统访问方式
    /// </summary>
    [Flags]
    public enum FileSystemAccess : byte
    {
        /// <summary>
        /// 未指定
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// 只可读
        /// </summary>
        Read = 1,

        /// <summary>
        /// 只可写
        /// </summary>
        Write = 2,

        /// <summary>
        /// 可读写
        /// </summary>
        ReadWrite = 4
    }
}