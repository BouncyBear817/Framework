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
    /// <summary>
    /// 数据表管理器接口
    /// </summary>
    public interface IDataTableManager
    {
        /// <summary>
        /// 数据表数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 缓冲二进制流大小
        /// </summary>
        int CachedBytesSize { get; }

        /// <summary>
        /// 设置资源管理器
        /// </summary>
        /// <param name="resourceManager">资源管理器</param>
        void SetResourceManager(IResourceManager resourceManager);

        /// <summary>
        /// 设置数据表数据提供者辅助器
        /// </summary>
        /// <param name="dataProviderHelper">数据表数据提供者辅助器</param>
        void SetDataProviderHelper(IDataProviderHelper<DataTableBase> dataProviderHelper);

        /// <summary>
        /// 设置数据表辅助器
        /// </summary>
        /// <param name="dataTableHelper">数据表辅助器</param>
        void SetDataTableHelper(IDataTableHelper dataTableHelper);

        /// <summary>
        /// 确保二进制流缓存分配足够的内存并缓存
        /// </summary>
        /// <param name="ensureSize">足够的内存大小</param>
        void EnsureCachedBytesSize(int ensureSize);

        /// <summary>
        /// 释放缓存的二进制流
        /// </summary>
        void FreeCachedBytes();

        /// <summary>
        /// 检查是否存在数据表
        /// </summary>
        /// <param name="name">数据表名称</param>
        /// <typeparam name="T">数据表类型</typeparam>
        /// <returns>是否存在数据表</returns>
        bool HasDataTable<T>(string name = null) where T : IDataRow;

        /// <summary>
        /// 检查是否存在数据表
        /// </summary>
        /// <param name="dataRowType">数据表类型</param>
        /// <param name="name">数据表名称</param>
        /// <returns>是否存在数据表</returns>
        bool HasDataTable(Type dataRowType, string name = null);

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="name">数据表名称</param>
        /// <typeparam name="T">数据表类型</typeparam>
        /// <returns>数据表</returns>
        IDataTable<T> GetDataTable<T>(string name = null) where T : IDataRow;

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="dataRowType">数据表类型</param>
        /// <param name="name">数据表名称</param>
        /// <returns>数据表</returns>
        DataTableBase GetDataTable(Type dataRowType, string name = null);

        /// <summary>
        /// 获取所有数据表
        /// </summary>
        /// <returns>所有数据表</returns>
        DataTableBase[] GetAllDataTables();

        /// <summary>
        /// 获取所有数据表
        /// </summary>
        /// <param name="results">所有数据表</param>
        void GetAllDataTables(List<DataTableBase> results);

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="name">数据表名称</param>
        /// <typeparam name="T">数据表类型</typeparam>
        /// <returns>创建的数据表</returns>
        IDataTable<T> CreateDataTable<T>(string name = null) where T : class, IDataRow, new();

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="dataRowType">数据表类型</param>
        /// <param name="name">数据表名称</param>
        /// <returns>创建的数据表</returns>
        DataTableBase CreateDataTable(Type dataRowType, string name = null);

        /// <summary>
        /// 销毁数据表
        /// </summary>
        /// <param name="name">数据表名称</param>
        /// <typeparam name="T">数据表类型</typeparam>
        /// <returns>是否销毁成功</returns>
        bool DestroyDataTable<T>(string name = null) where T : IDataRow;

        /// <summary>
        /// 销毁数据表
        /// </summary>
        /// <param name="dataRowType">数据表类型</param>
        /// <param name="name">数据表名称</param>
        /// <returns>是否销毁成功</returns>
        bool DestroyDataTable(Type dataRowType, string name = null);

        /// <summary>
        /// 销毁数据表
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <typeparam name="T">数据表类型</typeparam>
        /// <returns>是否销毁成功</returns>
        bool DestroyDataTable<T>(IDataTable<T> dataTable) where T : IDataRow;

        /// <summary>
        /// 销毁数据表
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <returns>是否销毁成功</returns>
        bool DestroyDataTable(DataTableBase dataTable);
    }
}