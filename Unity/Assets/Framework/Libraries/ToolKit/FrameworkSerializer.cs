using System;
using System.Collections.Generic;
using System.IO;
namespace Framework
{
    /// <summary>
    /// 框架序列化器基类
    /// </summary>
    /// <typeparam name="T">需要序列化的数据类型</typeparam>
    public abstract class FrameworkSerializer<T>
    {
        private readonly Dictionary<byte, SerializeCallback> mSerializeCallbacks;
        private readonly Dictionary<byte, DeserializeCallback> mDeserializeCallbacks;
        private readonly Dictionary<byte, TryGetValueCallback> mTryGetValueCallbacks;

        private byte mLatestSerializeCallbackVersion;

        public FrameworkSerializer()
        {
            mSerializeCallbacks = new Dictionary<byte, SerializeCallback>();
            mDeserializeCallbacks = new Dictionary<byte, DeserializeCallback>();
            mTryGetValueCallbacks = new Dictionary<byte, TryGetValueCallback>();
            mLatestSerializeCallbackVersion = 0;
        }

        /// <summary>
        /// 序列化回调函数
        /// </summary>
        public delegate bool SerializeCallback(Stream stream, T data);

        /// <summary>
        /// 反序列化回调函数
        /// </summary>
        public delegate T DeserializeCallback(Stream stream);

        /// <summary>
        /// 尝试从指定流获取指定键的值的回调函数
        /// </summary>
        public delegate bool TryGetValueCallback(Stream stream, string key, out object value);

        /// <summary>
        /// 注册序列化回调函数
        /// </summary>
        /// <param name="version">列化回调函数的版本</param>
        /// <param name="callback">列化回调函数</param>
        /// <exception cref="Exception"></exception>
        public void RegisterSerializeCallback(byte version, SerializeCallback callback)
        {
            if (callback == null)
            {
                throw new Exception("Serialize callback is invalid.");
            }

            mSerializeCallbacks[version] = callback;
            mLatestSerializeCallbackVersion = version > mLatestSerializeCallbackVersion ? version : mLatestSerializeCallbackVersion;
        }

        /// <summary>
        /// 注册反序列化回调函数
        /// </summary>
        /// <param name="version">反序列化回调函数版本</param>
        /// <param name="callback">反序列化回调函数</param>
        /// <exception cref="Exception"></exception>
        public void RegisterDeserializeCallback(byte version, DeserializeCallback callback)
        {
            if (callback == null)
            {
                throw new Exception("Deserialize callback is invalid.");
            }

            mDeserializeCallbacks[version] = callback;
        }

        /// <summary>
        /// 注册尝试从指定流获取指定键的值的回调函数
        /// </summary>
        /// <param name="version">尝试从指定流获取指定键的值的回调函数版本</param>
        /// <param name="callback">尝试从指定流获取指定键的值的回调函数</param>
        /// <exception cref="Exception"></exception>
        public void RegisterTryGetValueCallback(byte version, TryGetValueCallback callback)
        {
            if (callback == null)
            {
                throw new Exception("TryGetValue callback is invalid.");
            }

            mTryGetValueCallbacks[version] = callback;
        }

        /// <summary>
        /// 序列化数据到目标流中
        /// </summary>
        /// <param name="stream">目标流</param>
        /// <param name="data">要序列化的数据</param>
        /// <param name="version">序列化回调函数的版本</param>
        /// <returns>是否序列化成功</returns>
        /// <exception cref="Exception"></exception>
        public bool Serialize(Stream stream, T data, byte version)
        {
            var header = GetHeader();
            stream.WriteByte(header[0]);
            stream.WriteByte(header[1]);
            stream.WriteByte(header[2]);
            stream.WriteByte(version);

            if (!mSerializeCallbacks.TryGetValue(version, out var callback))
            {
                throw new Exception($"Serialize callback ({version}) is not exist.");
            }

            return callback(stream, data);
        }

        /// <summary>
        /// 反序列化流到数据中
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns>反序列化的数据</returns>
        /// <exception cref="Exception"></exception>
        public T Deserialize(Stream stream)
        {
            var header = GetHeader();
            var header0 = (byte)stream.ReadByte();
            var header1 = (byte)stream.ReadByte();
            var header2 = (byte)stream.ReadByte();
            if (header0 != header[0] || header1 != header[1] || header2 != header[2])
            {
                throw new Exception($"Header is invalid. need ({header[0]}{header[1]}{header[2]}), current ({header0}{header1}{header2})");
            }

            var version = (byte)stream.ReadByte();

            if (!mDeserializeCallbacks.TryGetValue(version, out var callback))
            {
                throw new Exception($"Deserialize callback ({version}) is not exist.");
            }

            return callback(stream);
        }
        
        /// <summary>
        /// 尝试从指定流获取指定键的值
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>是否成功</returns>
        public bool TryGetValue(Stream stream, string key, out object value)
        {
            value = null;
            var header = GetHeader();
            var header0 = (byte)stream.ReadByte();
            var header1 = (byte)stream.ReadByte();
            var header2 = (byte)stream.ReadByte();
            if (header0 != header[0] || header1 != header[1] || header2 != header[2])
            {
                return false;
            }

            var version = (byte)stream.ReadByte();

            return mTryGetValueCallbacks.TryGetValue(version, out var callback) && callback(stream, key, out value);

        }

        /// <summary>
        /// 获取数据头标识
        /// </summary>
        /// <returns></returns>
        protected abstract byte[] GetHeader();
    }
}