/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/1 11:16:41
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;
using UnityEngine;

namespace Framework.Runtime
{
    /// <summary>
    /// 全局配置辅助器基类
    /// </summary>
    public abstract class ConfigHelperBase : MonoBehaviour, IDataProviderHelper<IConfigManager>, IConfigHelper
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="dataProviderOwner">数据提供者的持有者</param>
        /// <param name="dataAssetName">数据资源名称</param>
        /// <param name="dataAsset">数据资源</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否读取数据成功</returns>
        public abstract bool ReadData(IConfigManager dataProviderOwner, string dataAssetName, object dataAsset,
            object userData);

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
        public abstract bool ReadData(IConfigManager dataProviderOwner, string dataAssetName, byte[] dataBytes,
            int startIndex,
            int length,
            object userData);

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="dataProviderOwner">数据提供者的持有者</param>
        /// <param name="dataString">数据字符串</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析数据成功</returns>
        public abstract bool ParseData(IConfigManager dataProviderOwner, string dataString, object userData);

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="dataProviderOwner">数据提供者的持有者</param>
        /// <param name="dataBytes">数据二进制流</param>
        /// <param name="startIndex">二进制流起始位置</param>
        /// <param name="length">二进制流长度</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析数据成功</returns>
        public abstract bool ParseData(IConfigManager dataProviderOwner, byte[] dataBytes, int startIndex, int length,
            object userData);

        /// <summary>
        /// 释放内容资源
        /// </summary>
        /// <param name="dataProviderOwner">数据提供者的持有者</param>
        /// <param name="dataAsset">内容资源</param>
        public abstract void ReleaseDataAsset(IConfigManager dataProviderOwner, object dataAsset);
    }
}