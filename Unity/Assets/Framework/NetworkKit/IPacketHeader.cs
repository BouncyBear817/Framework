/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/19 17:3:41
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// 网络消息包头接口
    /// </summary>
    public interface IPacketHeader
    {
        /// <summary>
        /// 网络消息包头长度
        /// </summary>
        int PacketLength { get; }
    }
}