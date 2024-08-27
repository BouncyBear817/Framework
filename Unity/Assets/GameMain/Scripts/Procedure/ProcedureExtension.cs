// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/8/26 11:46:23
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using Framework;
using Framework.Runtime;
using ProcedureOwner = Framework.IFsm<Framework.IProcedureManager>;

namespace GameMain
{
    public static class ProcedureExtension
    {
        private static Type sLastProcedure;
        private static ProcedureOwner mProcedureOwner;

        public static void SetLastProcedure(this ProcedureComponent procedureComponent, ProcedureOwner procedureOwner, Type lastProcedure)
        {
            if (!lastProcedure.IsAssignableFrom(typeof(ProcedureBase)))
            {
                Log.Warning($"{lastProcedure.FullName} is not assignable from ProcedureBase.");
                return;
            }

            mProcedureOwner = procedureOwner;
            sLastProcedure = lastProcedure;
        }

        public static Type GetLastProcedure(this ProcedureComponent procedureComponent) => sLastProcedure;

        public static ProcedureOwner GetProcedureOwner(this ProcedureComponent procedureComponent) => mProcedureOwner;
    }
}