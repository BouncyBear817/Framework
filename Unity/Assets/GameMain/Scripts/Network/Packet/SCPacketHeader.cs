/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/23 10:38:4
 * Description:
 * Modify Record:
 *************************************************************/

namespace Runtime
{
    public sealed class SCPacketHeader : PacketHeaderBase
    {
        public override PacketType PacketType => PacketType.ServerToClient;
    }
}