/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/23 10:36:41
 * Description:
 * Modify Record:
 *************************************************************/

namespace Runtime
{
    public sealed class CSPacketHeader : PacketHeaderBase
    {
        public override PacketType PacketType => PacketType.ClientToServer;
    }
}