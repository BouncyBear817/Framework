/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/02/02 16:46:20
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;
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

        /// <summary>
        /// 数据表数量
        /// </summary>
        public int Count => mDataTableManager.Count;

        /// <summary>
        /// 缓冲二进制流大小
        /// </summary>
        public int CachedBytesSize => mDataTableManager.CachedBytesSize;

        protected override void Awake()
        {
            base.Awake();
            mDataTableManager = FrameworkEntry.GetModule<IDataTableManager>();
            if (mDataTableManager == null)
            {
                Log.Fatal("Data table manager is invalid.");
            }
        }

        private void Start()
        {
            mEventComponent = MainEntryHelper.GetComponent<EventComponent>();
            if (mEventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
            }
        }

        /// <summary>
        /// 设置资源管理器
        /// </summary>
        /// <param name="resourceManager">资源管理器</param>
        public void SetResourceManager(IResourceManager resourceManager)
        {
            mDataTableManager.SetResourceManager(resourceManager);
        }

        /// <summary>
        /// 设置数据表数据提供者辅助器
        /// </summary>
        /// <param name="dataProviderHelper">数据表数据提供者辅助器</param>
        public void SetDataProviderHelper(IDataProviderHelper<DataTableBase> dataProviderHelper)
        {
            mDataTableManager.SetDataProviderHelper(dataProviderHelper);
        }

        /// <summary>
        /// 设置数据表辅助器
        /// </summary>
        /// <param name="dataTableHelper">数据表辅助器</param>
        public void SetDataTableHelper(IDataTableHelper dataTableHelper)
        {
            mDataTableManager.SetDataTableHelper(dataTableHelper);
        }

        /// <summary>
        /// 确保二进制流缓存分配足够的内存并缓存
        /// </summary>
        /// <param name="ensureSize">足够的内存大小</param>
        public void EnsureCachedBytesSize(int ensureSize)
        {
            mDataTableManager.EnsureCachedBytesSize(ensureSize);
        }

        /// <summary>
        /// 释放缓存的二进制流
        /// </summary>
        public void FreeCachedBytes()
        {
            mDataTableManager.FreeCachedBytes();
        }

        /// <summary>
        /// 检查是否存在数据表
        /// </summary>
        /// <param name="name">数据表名称</param>
        /// <typeparam name="T">数据表类型</typeparam>
        /// <returns>是否存在数据表</returns>
        public bool HasDataTable<T>(string name = null) where T : IDataRow
        {
            return mDataTableManager.HasDataTable<T>(name);
        }

        /// <summary>
        /// 检查是否存在数据表
        /// </summary>
        /// <param name="dataRowType">数据表类型</param>
        /// <param name="name">数据表名称</param>
        /// <returns>是否存在数据表</returns>
        public bool HasDataTable(Type dataRowType, string name = null)
        {
            return mDataTableManager.HasDataTable(dataRowType, name);
        }

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="name">数据表名称</param>
        /// <typeparam name="T">数据表类型</typeparam>
        /// <returns>数据表</returns>
        public IDataTable<T> GetDataTable<T>(string name = null) where T : IDataRow
        {
            return mDataTableManager.GetDataTable<T>(name);
        }

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="dataRowType">数据表类型</param>
        /// <param name="name">数据表名称</param>
        /// <returns>数据表</returns>
        public DataTableBase GetDataTable(Type dataRowType, string name = null)
        {
            return mDataTableManager.GetDataTable(dataRowType, name);
        }

        /// <summary>
        /// 获取所有数据表
        /// </summary>
        /// <returns>所有数据表</returns>
        public DataTableBase[] GetAllDataTables()
        {
            return mDataTableManager.GetAllDataTables();
        }

        /// <summary>
        /// 获取所有数据表
        /// </summary>
        /// <param name="results">所有数据表</param>
        public void GetAllDataTables(List<DataTableBase> results)
        {
            mDataTableManager.GetAllDataTables(results);
        }

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="name">数据表名称</param>
        /// <typeparam name="T">数据表类型</typeparam>
        /// <returns>创建的数据表</returns>
        public IDataTable<T> CreateDataTable<T>(string name = null) where T : class, IDataRow, new()
        {
            var dataTable = mDataTableManager.CreateDataTable<T>(name);
            var dataTableBase = dataTable as DataTableBase;
            dataTableBase.ReadDataSuccess += OnReadDataSuccess;
            dataTableBase.ReadDataFailure += OnReadDataFailure;

            if (mEnableLoadDataTableUpdateEvent)
            {
                dataTableBase.ReadDataUpdate += OnReadDataUpdate;
            }

            if (mEnableLoadDataTableDependencyAssetEvent)
            {
                dataTableBase.ReadDataDependencyAsset += OnReadDataDependencyAsset;
            }

            return dataTable;
        }

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="dataRowType">数据表类型</param>
        /// <param name="name">数据表名称</param>
        /// <returns>创建的数据表</returns>
        public DataTableBase CreateDataTable(Type dataRowType, string name = null)
        {
            var dataTable = mDataTableManager.CreateDataTable(dataRowType, name);
            dataTable.ReadDataSuccess += OnReadDataSuccess;
            dataTable.ReadDataFailure += OnReadDataFailure;

            if (mEnableLoadDataTableUpdateEvent)
            {
                dataTable.ReadDataUpdate += OnReadDataUpdate;
            }

            if (mEnableLoadDataTableDependencyAssetEvent)
            {
                dataTable.ReadDataDependencyAsset += OnReadDataDependencyAsset;
            }

            return dataTable;
        }

        /// <summary>
        /// 销毁数据表
        /// </summary>
        /// <param name="name">数据表名称</param>
        /// <typeparam name="T">数据表类型</typeparam>
        /// <returns>是否销毁成功</returns>
        public bool DestroyDataTable<T>(string name = null) where T : IDataRow
        {
            return mDataTableManager.DestroyDataTable<T>(name);
        }

        /// <summary>
        /// 销毁数据表
        /// </summary>
        /// <param name="dataRowType">数据表类型</param>
        /// <param name="name">数据表名称</param>
        /// <returns>是否销毁成功</returns>
        public bool DestroyDataTable(Type dataRowType, string name = null)
        {
            return mDataTableManager.DestroyDataTable(dataRowType, name);
        }

        /// <summary>
        /// 销毁数据表
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <typeparam name="T">数据表类型</typeparam>
        /// <returns>是否销毁成功</returns>
        public bool DestroyDataTable<T>(IDataTable<T> dataTable) where T : IDataRow
        {
            return mDataTableManager.DestroyDataTable<T>(dataTable);
        }

        /// <summary>
        /// 销毁数据表
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <returns>是否销毁成功</returns>
        public bool DestroyDataTable(DataTableBase dataTable)
        {
            return mDataTableManager.DestroyDataTable(dataTable);
        }


        private void OnReadDataSuccess(object sender, ReadDataSuccessEventArgs e)
        {
            mEventComponent.Fire(this, LoadDataTableSuccessEventArgs.Create(e));
        }

        private void OnReadDataFailure(object sender, ReadDataFailureEventArgs e)
        {
            Log.Warning($"Load data table failure, asset name ({e.DataAssetName}, error message ({e.ErrorMessage}))");
            mEventComponent.Fire(this, LoadDataTableFailureEventArgs.Create(e));
        }

        private void OnReadDataUpdate(object sender, ReadDataUpdateEventArgs e)
        {
            mEventComponent.Fire(this, LoadDataTableUpdateEventArgs.Create(e));
        }

        private void OnReadDataDependencyAsset(object sender, ReadDataDependencyAssetEventArgs e)
        {
            mEventComponent.Fire(this, LoadDataTableDependencyAssetEventArgs.Create(e));
        }
    }
}