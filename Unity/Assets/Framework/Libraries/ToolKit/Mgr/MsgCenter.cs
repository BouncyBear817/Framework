using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 消息中转站
    /// </summary>
    public class MsgCenter : MonoSingleton<MsgCenter>
    {
        private Dictionary<int, Func<MgrBehaviour>> mManagers = new Dictionary<int, Func<MgrBehaviour>>();
        
        /// <summary>
        /// 注册管理器工厂
        /// </summary>
        /// <param name="managerId"></param>
        /// <param name="managerFactory"></param>
        public void RegisterManagerFactory(int managerId, Func<MgrBehaviour> managerFactory)
        {
            if (!mManagers.ContainsKey(managerId))
            {
                mManagers.Add(managerId, managerFactory);
            }
        }
        
        /// <summary>
        /// 向各个模块发送消息
        /// </summary>
        /// <param name="msg"></param>
        public void SendMsg(IMsg msg)
        {
            foreach (var manager in mManagers)
            {
                if (manager.Key == msg.ManagerId)
                {
                    manager.Value().SendMsg(msg);
                }
            }
        }
    }
}