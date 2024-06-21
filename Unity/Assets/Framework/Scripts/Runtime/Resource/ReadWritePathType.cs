// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/11 14:29:11
//  * Description:
//  * Modify Record:
//  *************************************************************/

namespace Framework.Runtime
{
    /// <summary>
    /// 读写区路径类型
    /// </summary>
    public enum ReadWritePathType : byte
    {
        /// <summary>
        /// 未指定
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// 临时缓存
        /// </summary>
        TemporaryCache,

        /// <summary>
        /// 持久化数据
        /// </summary>
        PersistentData
    }
}