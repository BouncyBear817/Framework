// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/2/20 14:36:15
//  * Description:
//  * Modify Record:
//  *************************************************************/

namespace Framework
{
    /// <summary>
    /// 网络管理器
    /// </summary>
    public sealed partial class NetworkManager : FrameworkModule, INetworkManager
    {
        private sealed class HeartBeatState
        {
            private float mHeartBeatElapseSeconds;
            private int mMissHeartBeatCount;

            public HeartBeatState()
            {
                mHeartBeatElapseSeconds = 0f;
                mMissHeartBeatCount = 0;
            }

            public float HeartBeatElapseSeconds
            {
                get => mHeartBeatElapseSeconds;
                set => mHeartBeatElapseSeconds = value;
            }

            public int MissHeartBeatCount
            {
                get => mMissHeartBeatCount;
                set => mMissHeartBeatCount = value;
            }

            public void Reset(bool resetHeartBeatElapseSeconds)
            {
                if (resetHeartBeatElapseSeconds)
                {
                    mHeartBeatElapseSeconds = 0f;
                }

                mMissHeartBeatCount = 0;
            }
        }
    }
}