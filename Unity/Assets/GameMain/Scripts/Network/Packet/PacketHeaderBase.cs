/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/22 16:21:43
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;

namespace Runtime
{
    public abstract class PacketHeaderBase : IPacketHeader, IReference
    {
        public abstract PacketType PacketType { get; }

        public int Id { get; set; }

        public int PacketLength { get; set; }

        public bool IsValid => PacketType != PacketType.Undefined && Id > 0 && PacketLength >= 0;

        public void Clear()
        {
            Id = 0;
            PacketLength = 0;
        }
    }
}