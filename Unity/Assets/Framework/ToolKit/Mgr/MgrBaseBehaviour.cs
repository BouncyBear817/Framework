using System;
using UnityEngine;

namespace Framework
{
    public abstract class MgrBaseBehaviour : MonoBehaviour
    {
        protected bool ReceiveMsgInActive = true;

        protected void Process(int eventId, params object[] param)
        {
            if (ReceiveMsgInActive && gameObject.activeInHierarchy || !ReceiveMsgInActive)
            {
                if (param[0] is IMsg msg)
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