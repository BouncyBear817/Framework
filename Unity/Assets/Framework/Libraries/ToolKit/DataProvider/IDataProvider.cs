/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2023/12/28 15:00:32
* Description:   
* Modify Record: 
*************************************************************/

using System;

namespace Framework
{
    /// <summary>
    /// 数据提供者接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataProvider<T>
    {
        /// <summary>
        /// 读取数据成功事件
        /// </summary>
        event EventHandler<ReadDataSuccessEventArgs> ReadDataSuccess;

        /// <summary>
        /// 读取数据失败事件
        /// </summary>
        event EventHandler<ReadDataFailureEventArgs> ReadDataFailure;

        /// <summary>
        /// 读取数据更新事件
        /// </summary>
        event EventHandler<ReadDataUpdateEventArgs> ReadDataUpdate;

        /// <summary>
        /// 读取数据加载依赖资源事件
        /// </summary>
        event EventHandler<ReadDataDependencyAssetEventArgs> ReadDataDependencyAsset;

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="dataAssetName">数据资源名称</param>
        /// <param name="priority">数据资源加载优先级</param>
        /// <param name="userData">自定义数据</param>
        void ReadData(string dataAssetName, int priority, object userData);

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="dataString">数据字符串</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析成功</returns>
        bool ParseData(string dataString, object userData);

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="dataBytes">数据二进制流</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析成功</returns>
        bool ParseData(byte[] dataBytes, object userData);

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="dataBytes">数据二进制流</param>
        /// <param name="startIndex">二进制流起始位置</param>
        /// <param name="length">二进制流长度</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析成功</returns>
        bool ParseData(byte[] dataBytes, int startIndex, int length, object userData);
    }
}