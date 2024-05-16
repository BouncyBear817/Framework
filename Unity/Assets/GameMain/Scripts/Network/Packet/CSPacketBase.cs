/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/23 10:35:59
 * Description:
 * Modify Record:
 *************************************************************/

/// <summary>
/// 客户端至服务端网络消息包基类
/// </summary>
public class CSPacketBase : PacketBase
{
    /// <summary>
    /// 网络消息包类型
    /// </summary>
    public override PacketType PacketType => PacketType.ClientToServer;

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