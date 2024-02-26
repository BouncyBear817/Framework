/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/02/02 16:46:20
 * Description:   
 * Modify Record: 
 *************************************************************/

using Framework;

namespace Runtime
{
    /// <summary>
    /// 数据表行基类
    /// </summary>
    public abstract class DataRowBase : IDataRow
    {
        /// <summary>
        /// 数据表行基类
        /// </summary>
        public abstract int Id { get; }

        /// <summary>
        /// 解析数据表行
        /// </summary>
        /// <param name="dataRowString">数据表行字符串</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析成功</returns>
        public virtual bool ParseDataRow(string dataRowString, object userData)
        {
            Log.Warning("Not implemented ParseDataRow(string dataRowString, object userData).");
            return false;
        }

        /// <summary>
        /// 解析数据表行
        /// </summary>
        /// <param name="dataBytes">数据表行二进制流</param>
        /// <param name="startIndex">二进制流起始位置</param>
        /// <param name="length">二进制流长度</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析成功</returns>
        public virtual bool ParseData(byte[] dataBytes, int startIndex, int length, object userData)
        {
            Log.Warning("Not implemented ParseData(byte[] dataBytes, int startIndex, int length, object userData).");
            return false;
        }
    }
}