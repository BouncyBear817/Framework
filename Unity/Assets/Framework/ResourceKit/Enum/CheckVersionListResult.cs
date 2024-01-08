/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2024/01/05 11:21:26
* Description:   
* Modify Record: 
*************************************************************/

namespace Framework
{
    /// <summary>
    /// 检查版本资源列表结果
    /// </summary>
    public enum CheckVersionListResult : byte
    {
        /// <summary>
        /// 已经是最新的
        /// </summary>
        Updated = 0,

        /// <summary>
        /// 需要更新
        /// </summary>
        NeedUpdate
    }
}