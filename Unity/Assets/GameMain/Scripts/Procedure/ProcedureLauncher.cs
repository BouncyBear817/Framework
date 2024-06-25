// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/24 13:58:53
//  * Description:
//  * Modify Record:
//  *************************************************************/

using Framework;
using ProcedureOwner = Framework.IFsm<Framework.IProcedureManager>;

namespace GameMain
{
    public class ProcedureLauncher : ProcedureBase
    {
        public override bool UseNativeDialog => true;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Info("Procedure Launcher...");
            ChangeState<ProcedureSplash>(procedureOwner);
        }
    }
}