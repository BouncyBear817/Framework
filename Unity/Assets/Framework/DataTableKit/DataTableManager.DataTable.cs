/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/02/02 16:46:07
 * Description:   
 * Modify Record: 
 *************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
    public sealed partial class DataTableManager : FrameworkModule, IDataTableManager
    {
        private sealed class DataTable<T> : DataTableBase, IDataTable<T> where T : class, IDataRow, new()
        {
            private readonly Dictionary<int, T> mDataSet;
            private T mMinIdDataRow;
            private T mMaxIdDataRow;

            public DataTable(string name) : base(name)
            {
                mDataSet = new Dictionary<int, T>();
                mMinIdDataRow = null;
                mMaxIdDataRow = null;
            }

            /// <summary>
            /// 数据表行类型
            /// </summary>
            public override Type Type => typeof(T);

            /// <summary>
            /// 数据表行数量
            /// </summary>
            public override int Count => mDataSet.Count;

            /// <summary>
            /// 获取数据表行
            /// </summary>
            /// <param name="id">行的编号</param>
            public T this[int id] => GetDataRow(id);

            /// <summary>
            /// 获取最小编号的数据表行
            /// </summary>
            public T MinIdDataRow => mMinIdDataRow;

            /// <summary>
            /// 获取最大编号的数据表行
            /// </summary>
            public T MaxIdDataRow => mMaxIdDataRow;

            /// <summary>
            /// 是否存在数据表行
            /// </summary>
            /// <param name="id">行的编号</param>
            /// <returns>是否存在数据表行</returns>
            public override bool HasDataRow(int id)
            {
                return mDataSet.ContainsKey(id);
            }

            /// <summary>
            /// 是否存在数据表行
            /// </summary>
            /// <param name="condition">检查条件</param>
            /// <returns>是否存在数据表行</returns>
            public bool HasDataRow(Predicate<T> condition)
            {
                if (condition == null)
                {
                    throw new Exception("Condition is invalid.");
                }

                foreach (var (_, dataRow) in mDataSet)
                {
                    if (condition(dataRow))
                    {
                        return true;
                    }
                }

                return false;
            }

            public T GetDataRow(int id)
            {
                if (mDataSet.TryGetValue(id, out var dataRow))
                {
                    return dataRow;
                }

                return null;
            }

            public T GetDataRow(Predicate<T> condition)
            {
                if (condition == null)
                {
                    throw new Exception("Condition is invalid.");
                }

                foreach (var (_, dataRow) in mDataSet)
                {
                    if (condition(dataRow))
                    {
                        return dataRow;
                    }
                }

                return null;
            }

            public T[] GetDataRows(Predicate<T> condition)
            {
                if (condition == null)
                {
                    throw new Exception("Condition is invalid.");
                }

                var results = new List<T>();
                foreach (var (_, dataRow) in mDataSet)
                {
                    if (condition(dataRow))
                    {
                        results.Add(dataRow);
                    }
                }

                return results.ToArray();
            }

            public void GetDataRows(Predicate<T> condition, List<T> results)
            {
                if (condition == null)
                {
                    throw new Exception("Condition is invalid.");
                }

                if (results == null)
                {
                    throw new Exception("Results is invalid.");
                }

                results.Clear();
                foreach (var (_, dataRow) in mDataSet)
                {
                    if (condition(dataRow))
                    {
                        results.Add(dataRow);
                    }
                }
            }

            public T[] GetDataRows(Comparison<T> comparison)
            {
                if (comparison == null)
                {
                    throw new Exception("Comparison is invalid.");
                }

                var results = new List<T>();
                foreach (var (_, dataRow) in mDataSet)
                {
                    results.Add(dataRow);
                }

                results.Sort(comparison);
                return results.ToArray();
            }

            public void GetDataRows(Comparison<T> comparison, List<T> results)
            {
                if (comparison == null)
                {
                    throw new Exception("Comparison is invalid.");
                }

                if (results == null)
                {
                    throw new Exception("Results is invalid.");
                }

                results.Clear();
                foreach (var (_, dataRow) in mDataSet)
                {
                    results.Add(dataRow);
                }

                results.Sort(comparison);
            }

            public T[] GetDataRows(Predicate<T> condition, Comparison<T> comparison)
            {
                if (condition == null)
                {
                    throw new Exception("Condition is invalid.");
                }

                if (comparison == null)
                {
                    throw new Exception("Comparison is invalid.");
                }

                var results = new List<T>();
                foreach (var (_, dataRow) in mDataSet)
                {
                    if (condition(dataRow))
                    {
                        results.Add(dataRow);
                    }
                }

                results.Sort(comparison);
                return results.ToArray();
            }

            public void GetDataRows(Predicate<T> condition, Comparison<T> comparison, List<T> results)
            {
                if (condition == null)
                {
                    throw new Exception("Condition is invalid.");
                }

                if (comparison == null)
                {
                    throw new Exception("Comparison is invalid.");
                }

                if (results == null)
                {
                    throw new Exception("Results is invalid.");
                }

                results.Clear();
                foreach (var (_, dataRow) in mDataSet)
                {
                    if (condition(dataRow))
                    {
                        results.Add(dataRow);
                    }
                }

                results.Sort(comparison);
            }

            public T[] GetAllDataRows()
            {
                var index = 0;
                var results = new T[mDataSet.Count];
                foreach (var (_, dataRow) in mDataSet)
                {
                    results[index++] = dataRow;
                }

                return results;
            }

            public void GetAllDataRows(List<T> results)
            {
                if (results == null)
                {
                    throw new Exception("Results is invalid.");
                }

                results.Clear();
                foreach (var (_, dataRow) in mDataSet)
                {
                    results.Add(dataRow);
                }
            }

            public override bool AddDataRow(string dataRowString, object userData)
            {
                try
                {
                    var dataRow = new T();
                    if (!dataRow.ParseDataRow(dataRowString, userData))
                    {
                        return false;
                    }

                    InternalAddDataRow(dataRow);
                    return true;
                }
                catch (Exception e)
                {
                    throw new Exception(
                        $"Can not parse data row string for data table ({new TypeNamePair(typeof(T), Name)}) with exception ({e}).");
                }
            }

            public override bool AddDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
            {
                try
                {
                    var dataRow = new T();
                    if (!dataRow.ParseData(dataRowBytes, startIndex, length, userData))
                    {
                        return false;
                    }

                    InternalAddDataRow(dataRow);
                    return true;
                }
                catch (Exception e)
                {
                    throw new Exception(
                        $"Can not parse data row string for data table ({FullName}) with exception ({e}).");
                }
            }

            public override bool RemoveDataRow(int id)
            {
                if (!HasDataRow(id))
                {
                    return false;
                }

                if (!mDataSet.Remove(id))
                {
                    return false;
                }

                if (mMinIdDataRow != null && mMinIdDataRow.Id == id || mMaxIdDataRow != null && mMaxIdDataRow.Id == id)
                {
                    mMinIdDataRow = null;
                    mMaxIdDataRow = null;
                    foreach (KeyValuePair<int, T> dataRow in mDataSet)
                    {
                        if (mMinIdDataRow == null || mMinIdDataRow.Id > dataRow.Key)
                        {
                            mMinIdDataRow = dataRow.Value;
                        }

                        if (mMaxIdDataRow == null || mMaxIdDataRow.Id < dataRow.Key)
                        {
                            mMaxIdDataRow = dataRow.Value;
                        }
                    }
                }

                return true;
            }

            public override void RemoveAllDataRows()
            {
                mDataSet.Clear();
                mMinIdDataRow = null;
                mMaxIdDataRow = null;
            }

            public override void Shutdown()
            {
                mDataSet.Clear();
                mMinIdDataRow = null;
                mMaxIdDataRow = null;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return mDataSet.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private void InternalAddDataRow(T dataRow)
            {
                if (HasDataRow(dataRow.Id))
                {
                    throw new Exception($"Already exist ({dataRow.Id}) in data table ({FullName})");
                }

                mDataSet.Add(dataRow.Id, dataRow);

                if (mMinIdDataRow == null || mMinIdDataRow.Id > dataRow.Id)
                {
                    mMinIdDataRow = dataRow;
                }

                if (mMaxIdDataRow == null || mMaxIdDataRow.Id < dataRow.Id)
                {
                    mMaxIdDataRow = dataRow;
                }
            }
        }
    }
}