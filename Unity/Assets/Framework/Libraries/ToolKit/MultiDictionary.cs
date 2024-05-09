/************************************************************
 * Unity Version: 2022.3.0f1c1
 * Author:        bear
 * CreateTime:    2023/12/20 10:40:38
 * Description:   多值字典类
 * Modify Record:
 *************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 多值字典类
    /// </summary>
    /// <typeparam name="TKey">多值字典的主键类型</typeparam>
    /// <typeparam name="TValue">多值字典的值类型</typeparam>
    public sealed class MultiDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, LinkedList<TValue>>>, IEnumerable
    {
        private Dictionary<TKey, LinkedList<TValue>> mDictionary;

        public MultiDictionary()
        {
            mDictionary = new Dictionary<TKey, LinkedList<TValue>>();
        }

        /// <summary>
        /// 字典中主键数量
        /// </summary>
        public int Count => mDictionary.Count;

        /// <summary>
        /// 多值字典指定主键的范围
        /// </summary>
        /// <param name="key"></param>
        public LinkedList<TValue> this[TKey key]
        {
            get
            {
                mDictionary.TryGetValue(key, out var range);
                return range;
            }
        }

        /// <summary>
        /// 清理多值字典
        /// </summary>
        public void Clear()
        {
            mDictionary.Clear();
        }

        /// <summary>
        /// 多值字典中是否包含指定主键
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns>多值字典中是否包含指定主键</returns>
        public bool Contains(TKey key)
        {
            return mDictionary.ContainsKey(key);
        }

        /// <summary>
        /// 多值字典中是否包含指定主键与指定值
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="value">值</param>
        /// <returns>多值字典中是否包含指定主键与指定值</returns>
        public bool Contains(TKey key, TValue value)
        {
            if (mDictionary.TryGetValue(key, out var linkedList))
            {
                return linkedList.Contains(value);
            }

            return false;
        }

        /// <summary>
        /// 尝试获取多值字典中指定主键的值
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="linkedList">主键的范围</param>
        /// <returns>是否存在多值字典中指定主键的值</returns>
        public bool TryGetValue(TKey key, out LinkedList<TValue> linkedList)
        {
            return mDictionary.TryGetValue(key, out linkedList);
        }

        /// <summary>
        /// 增加多值字典中指定主键的值
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="value">值</param>
        public void Add(TKey key, TValue value)
        {
            if (mDictionary.TryGetValue(key, out var linkedList))
            {
                linkedList.AddLast(value);
            }
            else
            {
                var list = new LinkedList<TValue>();
                list.AddFirst(value);
                mDictionary.Add(key, list);
            }
        }

        /// <summary>
        /// 移除多值字典中指定主键的值
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="value">值</param>
        /// <returns>是否成功移除多值字典中指定主键的值</returns>
        public bool Remove(TKey key, TValue value)
        {
            if (mDictionary.TryGetValue(key, out var linkedList))
            {
                for (var current = linkedList.First;
                     current != null && current != linkedList.Last;
                     current = current.Next)
                {
                    if (current.Value.Equals(value))
                    {
                        linkedList.Remove(current);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 移除多值字典中指定主键的所有值
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否成功移除多值字典中指定主键的所有值</returns>
        public bool RemoveAll(TKey key)
        {
            if (mDictionary.TryGetValue(key, out var linkedList))
            {
                linkedList.Clear();
                mDictionary.Remove(key);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 返回循环访问集合的枚举数
        /// </summary>
        /// <returns>循环访问集合的枚举数</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(mDictionary);
        }

        /// <summary>
        /// 返回循环访问集合的枚举数
        /// </summary>
        /// <returns>循环访问集合的枚举数</returns>
        /// <exception cref="NotImplementedException"></exception>
        IEnumerator<KeyValuePair<TKey, LinkedList<TValue>>> IEnumerable<KeyValuePair<TKey, LinkedList<TValue>>>.
            GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 返回循环访问集合的枚举数
        /// </summary>
        /// <returns>循环访问集合的枚举数</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 循环访问集合的枚举数
        /// </summary>
        public struct Enumerator : IEnumerator<KeyValuePair<TKey, LinkedList<TValue>>>
        {
            private Dictionary<TKey, LinkedList<TValue>>.Enumerator mEnumerator;

            public Enumerator(Dictionary<TKey, LinkedList<TValue>> dictionary) : this()
            {
                if (dictionary == null)
                {
                    throw new Exception("Dictionary is invalid.");
                }

                mEnumerator = dictionary.GetEnumerator();
            }

            /// <summary>
            /// 获取当前节点
            /// </summary>
            public KeyValuePair<TKey, LinkedList<TValue>> Current => mEnumerator.Current;

            /// <summary>
            /// 获取当前的枚举数
            /// </summary>
            object IEnumerator.Current => mEnumerator.Current;

            /// <summary>
            /// 获取下一个节点
            /// </summary>
            /// <returns>是否成功获取下一个节点</returns>
            public bool MoveNext()
            {
                return mEnumerator.MoveNext();
            }

            /// <summary>
            /// 重置枚举数
            /// </summary>
            public void Reset()
            {
                ((IEnumerator<KeyValuePair<TKey, LinkedList<TValue>>>)mEnumerator).Reset();
            }

            /// <summary>
            /// 清理枚举数
            /// </summary>
            public void Dispose()
            {
                mEnumerator.Dispose();
            }
        }
    }
}