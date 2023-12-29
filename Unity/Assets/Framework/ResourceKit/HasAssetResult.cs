/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2023/12/29 16:18:55
* Description:   
* Modify Record: 
*************************************************************/

namespace Framework
{
    /// <summary>
    /// 检查资源是否存在的结果
    /// </summary>
    public enum HasAssetResult : byte
    {
        /// <summary>
        /// 资源不存在
        /// </summary>
        NotExist = 0,

        /// <summary>
        /// 资源尚未准备完毕
        /// </summary>
        NotReady,

        /// <summary>
        /// 存在资源且存储在磁盘上
        /// </summary>
        AssetOnDisk,

        /// <summary>
        /// 存在资源且存储在文件系统上
        /// </summary>
        AssetOnFileSystem,

        /// <summary>
        /// 存在资源且存储在网络上
        /// </summary>
        AssetOnNetwork,

        /// <summary>
        /// 存在二进制资源且存储在磁盘上
        /// </summary>
        BinaryOnDisk,

        /// <summary>
        /// 存在二进制资源且存储在文件系统上
        /// </summary>
        BinaryOnFileSystem,

        /// <summary>
        /// 存在二进制资源且存储在网络上
        /// </summary>
        BinaryOnNetwork
    }
}