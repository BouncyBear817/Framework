/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/02/02 16:46:07
 * Description:   
 * Modify Record: 
 *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    public sealed partial class DataTableManager : FrameworkModule, IDataTableManager
    {
        private readonly Dictionary<TypeNamePair, DataTableBase> mDataTables;
        private IResourceManager mResourceManager;
        private IDataProviderHelper<DataTableBase> mDataProviderHelper;
        private IDataTableHelper mDataTableHelper;

        public DataTableManager()
        {
            mDataTables = new Dictionary<TypeNamePair, DataTableBase>();
            mResourceManager = null;
            mDataProviderHelper = null;
            mDataTableHelper = null;
        }

        /// <summary>
        /// 数据表数量
        /// </summary>
        public int Count => mDataTables.Count;

        /// <summary>
        /// 缓冲二进制流大小
        /// </summary>
        public int CachedBytesSize => DataProvider<DataTableBase>.CachedBytesSize;

        /// <summary>
        /// 框架模块轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 关闭并清理数据表管理器
        /// </summary>
        public override void Shutdown()
        {
            foreach (var (_, dataTable) in mDataTables)
            {
                dataTable.Shutdown();
            }

            mDataTables.Clear();
        }

        /// <summary>
        /// 设置资源管理器
        /// </summary>
        /// <param name="resourceManager">资源管理器</param>
        public void SetResourceManager(IResourceManager resourceManager)
        {
            mResourceManager = resourceManager ?? throw new Exception("Resource Manager is invalid.");
        }

        /// <summary>
        /// 设置数据表数据提供者辅助器
        /// </summary>
        /// <param name="dataProviderHelper">数据表数据提供者辅助器</param>
        public void SetDataProviderHelper(IDataProviderHelper<DataTableBase> dataProviderHelper)
        {
            mDataProviderHelper = dataProviderHelper ?? throw new Exception("Data provider helper is invalid.");
        }

        /// <summary>
        /// 设置数据表辅助器
        /// </summary>
        /// <param name="dataTableHelper">数据表辅助器</param>
        public void SetDataTableHelper(IDataTableHelper dataTableHelper)
        {
            mDataTableHelper = dataTableHelper ?? throw new Exception("Data table helper is invalid.");
        }

        /// <summary>
        /// 确保二进制流缓存分配足够的内存并缓存
        /// </summary>
        /// <param name="ensureSize">足够的内存大小</param>
        public void EnsureCachedBytesSize(int ensureSize)
        {
            DataProvider<DataTableBase>.EnsureCachedBytesSize(ensureSize);
        }

        /// <summary>
        /// 释放缓存的二进制流
        /// </summary>
        public void FreeCachedBytes()
        {
            DataProvider<DataTableBase>.FreeCachedBytes();
        }

        /// <summary>
        /// 检查是否存在数据表
        /// </summary>
        /// <param name="name">数据表名称</param>
        /// <typeparam name="T">数据表类型</typeparam>
        /// <returns>是否存在数据表</returns>
        public bool HasDataTable<T>(string name = null) where T : IDataRow
        {
            return InternalHasDataTable(new TypeNamePair(typeof(T), name));
        }

        /// <summary>
        /// 检查是否存在数据表
        /// </summary>
        /// <param name="dataRowType">数据表类型</param>
        /// <param name="name">数据表名称</param>
        /// <returns>是否存在数据表</returns>
        public bool HasDataTable(Type dataRowType, string name = null)
        {
            if (dataRowType == null)
            {
                throw new Exception("Data row type is invalid.");
            }

            if (!typeof(IDataRow).IsAssignableFrom(dataRowType))
            {
                throw new Exception($"Data row type ({dataRowType.FullName}) is invalid.");
            }

            return InternalHasDataTable(new TypeNamePair(dataRowType, name));
        }

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="name">数据表名称</param>
        /// <typeparam name="T">数据表类型</typeparam>
        /// <returns>数据表</returns>
        public IDataTable<T> GetDataTable<T>(string name = null) where T : IDataRow
        {
            return InternalGetDataTable(new TypeNamePair(typeof(T), name)) as IDataTable<T>;
        }

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="dataRowType">数据表类型</param>
        /// <param name="name">数据表名称</param>
        /// <returns>数据表</returns>
        public DataTableBase GetDataTable(Type dataRowType, string name = null)
        {
            if (dataRowType == null)
            {
                throw new Exception("Data row type is invalid.");
            }

            if (!typeof(IDataRow).IsAssignableFrom(dataRowType))
            {
                throw new Exception($"Data row type ({dataRowType.FullName}) is invalid.");
            }

            return InternalGetDataTable(new TypeNamePair(dataRowType, name));
        }

        /// <summary>
        /// 获取所有数据表
        /// </summary>
        /// <returns>所有数据表</returns>
        public DataTableBase[] GetAllDataTables()
        {
            var index = 0;
            var results = new DataTableBase[mDataTables.Count];
            foreach (var (_, dataTable) in mDataTables)
            {
                results[index++] = dataTable;
            }

            return results;
        }

        /// <summary>
        /// 获取所有数据表
        /// </summary>
        /// <param name="results">所有数据表</param>
        public void GetAllDataTables(List<DataTableBase> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (var (_, dataTable) in mDataTables)
            {
                results.Add(dataTable);
            }
        }

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="name">数据表名称</param>
        /// <typeparam name="T">数据表类型</typeparam>
        /// <returns>创建的数据表</returns>
        public IDataTable<T> CreateDataTable<T>(string name = null) where T : class, IDataRow, new()
        {
            if (mResourceManager == null)
            {
                throw new Exception("You must set resource manager first.");
            }

            if (mDataProviderHelper == null)
            {
                throw new Exception("You must set data provider helper first.");
            }

            var typeNamePair = new TypeNamePair(typeof(T), name);
            if (HasDataTable<T>(name))
            {
                throw new Exception($"Already exist data table ({typeNamePair}).");
            }

            var dataTable = new DataTable<T>(name);
            dataTable.SetResourceManager(mResourceManager);
            dataTable.SetDataProviderHelper(mDataProviderHelper);
            mDataTables.Add(typeNamePair, dataTable);
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
            if (mResourceManager == null)
            {
                throw new Exception("You must set resource manager first.");
            }

            if (mDataProviderHelper == null)
            {
                throw new Exception("You must set data provider helper first.");
            }

            if (dataRowType == null)
            {
                throw new Exception("Data row type is invalid.");
            }

            if (!typeof(IDataRow).IsAssignableFrom(dataRowType))
            {
                throw new Exception($"Data row type ({dataRowType.FullName}) is invalid.");
            }

            var typeNamePair = new TypeNamePair(dataRowType, name);
            if (HasDataTable(dataRowType, name))
            {
                throw new Exception($"Already exist data table ({typeNamePair}).");
            }

            var dataTableType = typeof(DataTable<>).MakeGenericType(dataRowType);
            var dataTable = Activator.CreateInstance(dataTableType, name) as DataTableBase;
            if (dataTable == null)
            {
                throw new Exception($"Create data table ({typeNamePair}) failed.");
            }

            dataTable.SetResourceManager(mResourceManager);
            dataTable.SetDataProviderHelper(mDataProviderHelper);
            mDataTables.Add(typeNamePair, dataTable);
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
            return InternalDestroyDataTable(new TypeNamePair(typeof(T), name));
        }

        /// <summary>
        /// 销毁数据表
        /// </summary>
        /// <param name="dataRowType">数据表类型</param>
        /// <param name="name">数据表名称</param>
        /// <returns>是否销毁成功</returns>
        public bool DestroyDataTable(Type dataRowType, string name = null)
        {
            if (dataRowType == null)
            {
                throw new Exception("Data row type is invalid.");
            }

            if (!typeof(IDataRow).IsAssignableFrom(dataRowType))
            {
                throw new Exception($"Data row type ({dataRowType.FullName}) is invalid.");
            }

            return InternalDestroyDataTable(new TypeNamePair(dataRowType, name));
        }

        /// <summary>
        /// 销毁数据表
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <typeparam name="T">数据表类型</typeparam>
        /// <returns>是否销毁成功</returns>
        public bool DestroyDataTable<T>(IDataTable<T> dataTable) where T : IDataRow
        {
            if (dataTable == null)
            {
                throw new Exception("Data table is invalid.");
            }

            return InternalDestroyDataTable(new TypeNamePair(dataTable.Type, dataTable.Name));
        }

        /// <summary>
        /// 销毁数据表
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <returns>是否销毁成功</returns>
        public bool DestroyDataTable(DataTableBase dataTable)
        {
            if (dataTable == null)
            {
                throw new Exception("Data table is invalid.");
            }

            return InternalDestroyDataTable(new TypeNamePair(dataTable.Type, dataTable.Name));
        }

        private bool InternalHasDataTable(TypeNamePair typeNamePair)
        {
            return mDataTables.ContainsKey(typeNamePair);
        }

        private DataTableBase InternalGetDataTable(TypeNamePair typeNamePair)
        {
            return mDataTables.GetValueOrDefault(typeNamePair);
        }

        private bool InternalDestroyDataTable(TypeNamePair typeNamePair)
        {
            if (mDataTables.TryGetValue(typeNamePair, out var dataTable))
            {
                dataTable.Shutdown();
                return mDataTables.Remove(typeNamePair);
            }

            return false;
        }
    }
}