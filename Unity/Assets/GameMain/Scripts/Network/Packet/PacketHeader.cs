/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/22 16:21:43
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;

/// <summary>
/// 网络消息包头
/// </summary>
public class PacketHeader : IPacketHeader, IReference
{
    /// <summary>
    /// 网络消息包编号
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// 网络消息包头长度
    /// </summary>
    public int PacketLength { get; set; }

    /// <summary>
    /// 网络消息包是否有效
    /// </summary>
    public bool IsValid => Id > 0 && PacketLength >= 0;

    /// <summary>
    /// 清理网络消息包头
    /// </summary>
    public void Clear()
    {
        Id = 0;
        PacketLength = 0;
    }
}