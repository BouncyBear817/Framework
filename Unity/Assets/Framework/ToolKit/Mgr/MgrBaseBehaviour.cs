using System;
using UnityEngine;

namespace Framework
{
    public abstract class MgrBaseBehaviour : MonoBehaviour
    {
        protected bool mReceiveMsgInActive = true;

        public void Process(int eventId, params object[] param)
        {
            if (mReceiveMsgInActive && gameObject.activeInHierarchy || !mReceiveMsgInActive)
            {
                var msg = param[0] as IMsg;
                if (msg != null)
                {
                    ProcessMsg(eventId, msg as Msg);
                    msg.Processed = true;
                }
                
            }
        }

        protected virtual void ProcessMsg(int eventId, Msg msg)
        {
        }
    }
}