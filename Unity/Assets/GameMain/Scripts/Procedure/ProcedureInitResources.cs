// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/24 14:22:24
//  * Description:
//  * Modify Record:
//  *************************************************************/

using Framework;
using ProcedureOwner = Framework.IFsm<Framework.IProcedureManager>;

namespace GameMain
{
    public class ProcedureInitResources : ProcedureBase
    {
        private bool mInitResourcesComplete = false;

        public override bool UseNativeDialog => true;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            mInitResourcesComplete = false;

            MainEntry.Resource.InitResources(OnInitResourcesComplete);
        }


        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (!mInitResourcesComplete)
            {
                return;
            }

            ChangeState<ProcedurePreload>(procedureOwner);
        }

        private void OnInitResourcesComplete()
        {
            mInitResourcesComplete = true;
            Log.Info("Init Resources complete...");
        }
    }
}