/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/22 10:27:32
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;

public abstract class PacketBase : Packet
{
    public abstract PacketType PacketType { get; }

    public byte[] MessageBody { get; set; }
    
    public int MessageId { get; set; }
}