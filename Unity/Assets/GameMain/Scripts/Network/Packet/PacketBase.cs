/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/22 10:27:32
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;

/// <summary>
/// 网络消息包基类
/// </summary>
public abstract class PacketBase : Packet
{
    /// <summary>
    /// 网络消息包类型
    /// </summary>
    public abstract PacketType PacketType { get; }

    /// <summary>
    /// 网络消息编号
    /// </summary>
    public int MessageId { get; set; }

    /// <summary>
    /// 网络消息内容
    /// </summary>
    public byte[] MessageBody { get; set; }
}