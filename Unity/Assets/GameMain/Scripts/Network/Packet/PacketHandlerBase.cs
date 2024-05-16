/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/22 16:20:43
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;

/// <summary>
/// 网络消息包处理器基类
/// </summary>
public abstract class PacketHandlerBase : IPacketHandler
{
    /// <summary>
    /// 网络消息包协议编号
    /// </summary>
    public abstract int Id { get; }

    /// <summary>
    /// 网络消息包处理函数
    /// </summary>
    /// <param name="sender">网络消息包源</param>
    /// <param name="packet">网络消息包</param>
    public abstract void Handle(object sender, Packet packet);
}