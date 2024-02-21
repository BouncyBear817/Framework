/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/02/19 15:31:07
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// 网络地址类型
    /// </summary>
    public enum AddressFamily : byte
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// IP版本4
        /// </summary>
        IPv4,

        /// <summary>
        /// IP版本6
        /// </summary>
        IPv6
    }
}