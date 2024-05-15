/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/22 10:25:52
 * Description:
 * Modify Record:
 *************************************************************/

public enum PacketType : byte
{
    /// <summary>
    /// 未定义
    /// </summary>
    Undefined = 0,

    /// <summary>
    /// 客户端发往服务器的包
    /// </summary>
    ClientToServer,

    /// <summary>
    /// 服务器发往客户端的包
    /// </summary>
    ServerToClient
}