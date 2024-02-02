/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/02/02 16:46:20
 * Description:   
 * Modify Record: 
 *************************************************************/

using Framework;
using UnityEngine;

namespace Runtime
{
    /// <summary>
    /// 数据表组件
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Framework/DataTable")]
    public sealed class DataTableComponent : FrameworkComponent
    {
        private IDataTableManager mDataTableManager;
        private EventComponent mEventComponent;

        [SerializeField] private bool mEnableLoadDataTableUpdateEvent = false;
        [SerializeField] private bool mEnableLoadDataTableDependencyAssetEvent = false;

        [SerializeField] private string mDataTableHelperTypeName = "Runtime.DefaultDataTableHelper";
        [SerializeField] private DataTableHelperBase mCustomDataTableHelper = null;
        [SerializeField] private int mCachedBytesSize = 0;

        protected override void Awake()
        {
            base.Awake();
            mDataTableManager = FrameworkEntry.GetModule<IDataTableManager>();
            if (mDataTableManager == null)
            {
                Log.Error("Data table manager is invalid.");
            }
        }
    }
}