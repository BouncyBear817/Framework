/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/23 10:35:59
 * Description:
 * Modify Record:
 *************************************************************/

namespace Runtime
{
    public class CSPacketBase : PacketBase
    {
        public override PacketType PacketType => PacketType.ClientToServer;
        
        public int MessageId { get; set; }

        public override int Id => MessageId;

        public override void Clear()
        {
        }
    }
}