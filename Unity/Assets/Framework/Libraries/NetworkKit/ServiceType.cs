/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/19 16:7:51
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// 网络服务类型
    /// </summary>
    public enum ServiceType : byte
    {
        /// <summary>
        /// TCP网络服务
        /// </summary>
        Tcp = 0,

        /// <summary>
        /// 同步接受的TCP网络服务
        /// </summary>
        TcpWithSyncReceive
    }
}