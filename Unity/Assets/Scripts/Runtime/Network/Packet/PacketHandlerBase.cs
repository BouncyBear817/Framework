/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/22 16:20:43
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;

namespace Runtime
{
    public abstract class PacketHandlerBase : IPacketHandler
    {
        public abstract int Id { get; }
        public abstract void Handle(object sender, Packet packet);
    }
}