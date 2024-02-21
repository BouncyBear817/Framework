// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/2/20 16:39:15
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.Net.Sockets;

namespace Framework
{
    /// <summary>
    /// 网络管理器
    /// </summary>
    public sealed partial class NetworkManager : FrameworkModule, INetworkManager
    {
        private sealed class ConnectState
        {
            private readonly Socket mSocket;
            private readonly object mUserData;

            public ConnectState(Socket socket, object userData)
            {
                mSocket = socket;
                mUserData = userData;
            }

            public Socket Socket => mSocket;

            public object UserData => mUserData;
        }
    }

    
}