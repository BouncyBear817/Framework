/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/23 10:37:15
 * Description:
 * Modify Record:
 *************************************************************/

/// <summary>
/// 服务器至客户端网络消息包基类
/// </summary>
public class SCPacketBase : PacketBase
{
    /// <summary>
    /// 网络消息包类型
    /// </summary>
    public override PacketType PacketType => PacketType.ServerToClient;

    /// <summary>
    /// 事件编号
    /// </summary>
    public override int Id => MessageId;

    /// <summary>
    /// 清理引用
    /// </summary>
    public override void Clear()
    {
    }
}