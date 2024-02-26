/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/02/02 16:46:08
 * Description:   
 * Modify Record: 
 *************************************************************/

using System;

namespace Framework
{
    /// <summary>
    /// 数据表基类
    /// </summary>
    public abstract class DataTableBase : IDataProvider<DataTableBase>
    {
        private readonly string mName;
        private readonly DataProvider<DataTableBase> mDataProvider;

        protected DataTableBase(string name)
        {
            mName = name ?? string.Empty;
            mDataProvider = new DataProvider<DataTableBase>(this);
        }

        /// <summary>
        /// 数据表名称
        /// </summary>
        public string Name => mName;

        /// <summary>
        /// 数据表行类型
        /// </summary>
        public abstract Type Type { get; }

        /// <summary>
        /// 数据表行数量
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// 数据表完整名称
        /// </summary>
        public string FullName => new TypeNamePair(Type, mName).ToString();

        /// <summary>
        /// 读取数据成功事件
        /// </summary>
        public event EventHandler<ReadDataSuccessEventArgs> ReadDataSuccess
        {
            add => mDataProvider.ReadDataSuccess += value;
            remove => mDataProvider.ReadDataSuccess -= value;
        }

        /// <summary>
        /// 读取数据失败事件
        /// </summary>
        public event EventHandler<ReadDataFailureEventArgs> ReadDataFailure
        {
            add => mDataProvider.ReadDataFailure += value;
            remove => mDataProvider.ReadDataFailure -= value;
        }

        /// <summary>
        /// 读取数据更新事件
        /// </summary>
        public event EventHandler<ReadDataUpdateEventArgs> ReadDataUpdate
        {
            add => mDataProvider.ReadDataUpdate += value;
            remove => mDataProvider.ReadDataUpdate -= value;
        }

        /// <summary>
        /// 读取数据加载依赖资源事件
        /// </summary>
        public event EventHandler<ReadDataDependencyAssetEventArgs> ReadDataDependencyAsset
        {
            add => mDataProvider.ReadDataDependencyAsset += value;
            remove => mDataProvider.ReadDataDependencyAsset -= value;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="dataAssetName">数据资源名称</param>
        /// <param name="priority">数据资源加载优先级</param>
        /// <param name="userData">自定义数据</param>
        public void ReadData(string dataAssetName, int priority, object userData)
        {
            mDataProvider.ReadData(dataAssetName, priority, userData);
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="dataString">数据字符串</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析成功</returns>
        public bool ParseData(string dataString, object userData)
        {
            return mDataProvider.ParseData(dataString, userData);
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="dataBytes">数据二进制流</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析成功</returns>
        public bool ParseData(byte[] dataBytes, object userData)
        {
            return mDataProvider.ParseData(dataBytes, userData);
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="dataBytes">数据二进制流</param>
        /// <param name="startIndex">二进制流起始位置</param>
        /// <param name="length">二进制流长度</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析成功</returns>
        public bool ParseData(byte[] dataBytes, int startIndex, int length, object userData)
        {
            return mDataProvider.ParseData(dataBytes, startIndex, length, userData);
        }

        /// <summary>
        /// 检查是否存在数据表行
        /// </summary>
        /// <param name="id">行的编号</param>
        /// <returns>是否存在数据表行</returns>
        public abstract bool HasDataRow(int id);

        /// <summary>
        /// 增加数据表行
        /// </summary>
        /// <param name="dataRowString">要解析的数据表行字符串</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否增加成功</returns>
        public abstract bool AddDataRow(string dataRowString, object userData);

        /// <summary>
        /// 增加数据表行
        /// </summary>
        /// <param name="dataRowBytes">要解析的数据表行二进制流</param>
        /// <param name="startIndex">二进制流起始位置</param>
        /// <param name="length">二进制流长度</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否增加成功</returns>
        public abstract bool AddDataRow(byte[] dataRowBytes, int startIndex, int length, object userData);

        /// <summary>
        /// 移除数据表行
        /// </summary>
        /// <param name="id">行的编号</param>
        /// <returns>是否移除成功</returns>
        public abstract bool RemoveDataRow(int id);

        /// <summary>
        /// 移除所有数据表行
        /// </summary>
        public abstract void RemoveAllDataRows();

        /// <summary>
        /// 设置资源管理器
        /// </summary>
        /// <param name="resourceManager">资源管理器</param>
        public void SetResourceManager(IResourceManager resourceManager)
        {
            mDataProvider.SetResourceManager(resourceManager);
        }

        /// <summary>
        /// 设置数据提供者辅助器
        /// </summary>
        /// <param name="dataProviderHelper">数据提供者辅助器</param>
        public void SetDataProviderHelper(IDataProviderHelper<DataTableBase> dataProviderHelper)
        {
            mDataProvider.SetDataProviderHelper(dataProviderHelper);
        }

        /// <summary>
        /// 关闭并清理数据表
        /// </summary>
        public abstract void Shutdown();
    }
}