/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/22 10:27:32
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;

namespace Runtime
{
    public abstract class PacketBase : Packet
    {
        public abstract PacketType PacketType { get; }
        
        public byte[] ProtoBody { get; set; }
    }
}