/************************************************************
 * Unity Version: 2022.3.0f1c1
 * Author:        bear
 * CreateTime:    2023/12/28 15:08:22
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// 数据提供者辅助器接口
    /// </summary>
    /// <typeparam name="T">数据提供者类型</typeparam>
    public interface IDataProviderHelper<T>
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="dataProviderOwner">数据提供者的持有者</param>
        /// <param name="dataAssetName">数据资源名称</param>
        /// <param name="dataAsset">数据资源</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否读取数据成功</returns>
        bool ReadData(T dataProviderOwner, string dataAssetName, object dataAsset, object userData);

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="dataProviderOwner">数据提供者的持有者</param>
        /// <param name="dataAssetName">数据资源名称</param>
        /// <param name="dataBytes">数据二进制流</param>
        /// <param name="startIndex">二进制流起始位置</param>
        /// <param name="length">二进制流长度</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否读取数据成功</returns>
        bool ReadData(T dataProviderOwner, string dataAssetName, byte[] dataBytes, int startIndex, int length,
            object userData);

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="dataProviderOwner">数据提供者的持有者</param>
        /// <param name="dataString">数据字符串</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析数据成功</returns>
        bool ParseData(T dataProviderOwner, string dataString, object userData);

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="dataProviderOwner">数据提供者的持有者</param>
        /// <param name="dataBytes">数据二进制流</param>
        /// <param name="startIndex">二进制流起始位置</param>
        /// <param name="length">二进制流长度</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析数据成功</returns>
        bool ParseData(T dataProviderOwner, byte[] dataBytes, int startIndex, int length, object userData);

        /// <summary>
        /// 释放内容资源
        /// </summary>
        /// <param name="dataProviderOwner">数据提供者的持有者</param>
        /// <param name="dataAsset">内容资源</param>
        void ReleaseDataAsset(T dataProviderOwner, object dataAsset);
    }
}