// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/24 14:1:13
//  * Description:
//  * Modify Record:
//  *************************************************************/

using Framework;
using ProcedureOwner = Framework.IFsm<Framework.IProcedureManager>;

namespace GameMain
{
    public class ProcedureSplash : ProcedureBase
    {
        public override bool UseNativeDialog => true;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            OnStartSplash(procedureOwner);
        }

        private void OnStartSplash(ProcedureOwner procedureOwner)
        {
            //TODO: 开启Splash....
            Log.Info("Start Splash...");
            OnChangeState(procedureOwner);
        }

        private void OnChangeState(ProcedureOwner procedureOwner)
        {
            if (MainEntry.Base.EditorResourceMode)
            {
                Log.Info("Editor resource mode detected.");
                ChangeState<ProcedurePreload>(procedureOwner);
            }
            else if (MainEntry.Resource.ResourceMode == ResourceMode.StandalonePackage)
            {
                Log.Info("Package resource mode detected.");
                ChangeState<ProcedureInitResources>(procedureOwner);
            }
            else
            {
                Log.Info("Updatable resource mode detected.");
                ChangeState<ProcedureCheckVersion>(procedureOwner);
            }
        }
    }
}