// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/24 14:22:49
//  * Description:
//  * Modify Record:
//  *************************************************************/

using Framework;
using ProcedureOwner = Framework.IFsm<Framework.IProcedureManager>;

namespace GameMain
{
    public class ProcedureCheckVersion : ProcedureBase
    {
        private bool mCheckVersionComplete = false;
        private bool mNeedUpdateVersion = false;
        private VersionInfo mVersionInfo;
        
        public override bool UseNativeDialog => true;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            mCheckVersionComplete = false;
            mNeedUpdateVersion = false;
            mVersionInfo = null;
            
            MainEntry.Event.Subscribe(Framework.Runtime.WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            MainEntry.Event.Subscribe(Framework.Runtime.WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
            
            //向服务器请求版本信息
            // MainEntry.WebRequest.AddWebRequest();
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!mCheckVersionComplete)
            {
                return;
            }

            if (mNeedUpdateVersion)
            {
                
            }
            else
            {
                
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            
            MainEntry.Event.UnSubscribe(Framework.Runtime.WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            MainEntry.Event.UnSubscribe(Framework.Runtime.WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
        }

        private void OnWebRequestSuccess(object sender, BaseEventArgs e)
        {
            
        }

        private void OnWebRequestFailure(object sender, BaseEventArgs e)
        {
            
        }
    }
}