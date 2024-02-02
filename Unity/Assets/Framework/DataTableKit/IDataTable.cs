/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/02/02 16:46:07
 * Description:   
 * Modify Record: 
 *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 数据表接口
    /// </summary>
    /// <typeparam name="T">数据表行的类型</typeparam>
    public interface IDataTable<T> : IEnumerable<T> where T : IDataRow
    {
        /// <summary>
        /// 数据表名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 数据表行类型
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// 数据表完整名称
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// 数据表行数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 获取数据表行
        /// </summary>
        /// <param name="id">行的编号</param>
        T this[int id] { get; }

        /// <summary>
        /// 获取最小编号的数据表行
        /// </summary>
        T MinIdDataRow { get; }

        /// <summary>
        /// 获取最大编号的数据表行
        /// </summary>
        T MaxIdDataRow { get; }

        /// <summary>
        /// 是否存在数据表行
        /// </summary>
        /// <param name="id">行的编号</param>
        /// <returns>是否存在数据表行</returns>
        bool HasDataRow(int id);

        /// <summary>
        /// 是否存在数据表行
        /// </summary>
        /// <param name="condition">检查条件</param>
        /// <returns>是否存在数据表行</returns>
        bool HasDataRow(Predicate<T> condition);

        /// <summary>
        /// 获取数据表行
        /// </summary>
        /// <param name="id">行的编号</param>
        /// <returns>数据表行</returns>
        T GetDataRow(int id);

        /// <summary>
        /// 获取数据表行
        /// </summary>
        /// <param name="condition">检查条件</param>
        /// <returns>数据表行</returns>
        T GetDataRow(Predicate<T> condition);

        /// <summary>
        /// 获取数据表行集合
        /// </summary>
        /// <param name="condition">检查条件</param>
        /// <returns>数据表行集合</returns>
        T[] GetDataRows(Predicate<T> condition);

        /// <summary>
        /// 获取数据表行集合
        /// </summary>
        /// <param name="condition">检查条件</param>
        /// <param name="results">数据表行集合</param>
        void GetDataRows(Predicate<T> condition, List<T> results);

        /// <summary>
        /// 获取数据表行集合
        /// </summary>
        /// <param name="comparison">排序条件</param>
        /// <returns>数据表行集合</returns>
        T[] GetDataRows(Comparison<T> comparison);

        /// <summary>
        /// 获取数据表行集合
        /// </summary>
        /// <param name="comparison">排序条件</param>
        /// <param name="results">数据表行集合</param>
        void GetDataRows(Comparison<T> comparison, List<T> results);

        /// <summary>
        /// 获取数据表行集合
        /// </summary>
        /// <param name="condition">检查条件</param>
        /// <param name="comparison">排序条件</param>
        /// <returns>数据表行集合</returns>
        T[] GetDataRows(Predicate<T> condition, Comparison<T> comparison);

        /// <summary>
        /// 获取数据表行集合
        /// </summary>
        /// <param name="condition">检查条件</param>
        /// <param name="comparison">排序条件</param>
        /// <param name="results">数据表行集合</param>
        void GetDataRows(Predicate<T> condition, Comparison<T> comparison, List<T> results);

        /// <summary>
        /// 获取所有数据表行
        /// </summary>
        /// <returns>数据表行集合</returns>
        T[] GetAllDataRows();

        /// <summary>
        /// 获取所有数据表行
        /// </summary>
        /// <param name="results">数据表行集合</param>
        void GetAllDataRows(List<T> results);

        /// <summary>
        /// 增加数据表行
        /// </summary>
        /// <param name="dataRowString">要解析的数据表行字符串</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否增加成功</returns>
        bool AddDataRow(string dataRowString, object userData);

        /// <summary>
        /// 增加数据表行
        /// </summary>
        /// <param name="dataRowBytes">要解析的数据表行二进制流</param>
        /// <param name="startIndex">二进制流起始位置</param>
        /// <param name="length">二进制流长度</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否增加成功</returns>
        bool AddDataRow(byte[] dataRowBytes, int startIndex, int length, object userData);

        /// <summary>
        /// 移除数据表行
        /// </summary>
        /// <param name="id">行的编号</param>
        /// <returns>是否移除成功</returns>
        bool RemoveDataRow(int id);

        /// <summary>
        /// 移除所有数据表行
        /// </summary>
        void RemoveAllDataRows();
    }
}