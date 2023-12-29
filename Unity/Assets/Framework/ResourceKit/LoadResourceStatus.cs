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
    /// 加载资源状态
    /// </summary>
    public enum LoadResourceStatus : byte
    {
        /// <summary>
        /// 加载资源完成
        /// </summary>
        Success = 0,

        /// <summary>
        /// 加载资源不存在
        /// </summary>
        NotExist,

        /// <summary>
        /// 加载资源未准备
        /// </summary>
        NotReady,

        /// <summary>
        /// 加载资源依赖错误
        /// </summary>
        DependencyError,

        /// <summary>
        /// 加载资源类型错误
        /// </summary>
        TypeError,

        /// <summary>
        /// 加载资源错误
        /// </summary>
        AssetError
    }
}