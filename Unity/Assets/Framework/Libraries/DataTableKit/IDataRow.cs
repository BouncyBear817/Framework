/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/02/02 16:46:07
 * Description:   
 * Modify Record: 
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// 数据表行接口
    /// </summary>
    public interface IDataRow
    {
        /// <summary>
        /// 数据表行编号
        /// </summary>
        int Id { get; }

        /// <summary>
        /// 解析数据表行
        /// </summary>
        /// <param name="dataRowString">数据表行字符串</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析成功</returns>
        bool ParseDataRow(string dataRowString, object userData);

        /// <summary>
        /// 解析数据表行
        /// </summary>
        /// <param name="dataRowBytes">数据表行二进制流</param>
        /// <param name="startIndex">二进制流起始位置</param>
        /// <param name="length">二进制流长度</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析成功</returns>
        public bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData);
    }
}