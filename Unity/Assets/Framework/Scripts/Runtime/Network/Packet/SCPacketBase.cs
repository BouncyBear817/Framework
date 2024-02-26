/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/23 10:37:15
 * Description:
 * Modify Record:
 *************************************************************/

namespace Runtime
{
    public abstract class SCPacketBase : PacketBase
    {
        public override PacketType PacketType => PacketType.ServerToClient;
    }
}