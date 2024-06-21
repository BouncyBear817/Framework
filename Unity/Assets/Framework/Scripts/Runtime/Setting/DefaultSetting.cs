// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/18 15:25:51
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Framework;

namespace Framework.Runtime
{
    public sealed class DefaultSetting
    {
        private readonly SortedDictionary<string, string> mSettings = new SortedDictionary<string, string>(StringComparer.Ordinal);

        public DefaultSetting()
        {
        }

        /// <summary>
        /// 游戏配置项数量
        /// </summary>
        public int Count => mSettings.Count;

        /// <summary>
        /// 获取所有游戏配置项的名称
        /// </summary>
        /// <returns>所有游戏配置项的名称</returns>
        public string[] GetAllSettingNames()
        {
            var index = 0;
            var allSettingNames = new string[mSettings.Count];
            foreach (var (name, _) in mSettings)
            {
                allSettingNames[index++] = name;
            }

            return allSettingNames;
        }

        /// <summary>
        /// 获取所有游戏配置项的名称
        /// </summary>
        /// <param name="results">所有游戏配置项的名称</param>
        public void GetAllSettingNames(List<string> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (var (name, _) in mSettings)
            {
                results.Add(name);
            }
        }

        /// <summary>
        /// 检查是否存在指定的游戏配置项
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <returns>是否存在指定的游戏配置项</returns>
        public bool HasSetting(string settingName)
        {
            return mSettings.ContainsKey(settingName);
        }

        /// <summary>
        /// 移除指定的游戏配置项
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <returns>是否成功移除指定的游戏配置项</returns>
        public bool RemoveSetting(string settingName)
        {
            return mSettings.Remove(settingName);
        }

        /// <summary>
        /// 移除所有的游戏配置项
        /// </summary>
        public void RemoveAllSettings()
        {
            mSettings.Clear();
        }

        /// <summary>
        /// 从指定的游戏配置项中读取布尔值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的布尔值</returns>
        public bool GetBool(string settingName, bool defaultValue = false)
        {
            if (!mSettings.TryGetValue(settingName, out var value))
            {
                Log.Warning($"Setting ({settingName}) is not exist.");
                return defaultValue;
            }

            return int.Parse(value) != 0;
        }

        /// <summary>
        /// 向指定的游戏配置项写入布尔值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的布尔值</param>
        public void SetBool(string settingName, bool value)
        {
            mSettings[settingName] = value ? "1" : "0";
        }

        /// <summary>
        /// 从指定的游戏配置项中读取整数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的整数值</returns>
        public int GetInt(string settingName, int defaultValue = 0)
        {
            if (!mSettings.TryGetValue(settingName, out var value))
            {
                Log.Warning($"Setting ({settingName}) is not exist.");
                return defaultValue;
            }

            return int.Parse(value);
        }

        /// <summary>
        /// 向指定的游戏配置项写入整数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的整数值</param>
        public void SetInt(string settingName, int value)
        {
            mSettings[settingName] = value.ToString();
        }

        /// <summary>
        /// 从指定的游戏配置项中读取浮点数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的浮点数值</returns>
        public float GetFloat(string settingName, float defaultValue = 0f)
        {
            if (!mSettings.TryGetValue(settingName, out var value))
            {
                Log.Warning($"Setting ({settingName}) is not exist.");
                return defaultValue;
            }

            return float.Parse(value);
        }

        /// <summary>
        /// 向指定的游戏配置项写入浮点数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的浮点数值</param>
        public void SetFloat(string settingName, float value)
        {
            mSettings[settingName] = value.ToString();
        }

        /// <summary>
        /// 从指定的游戏配置项中读取字符串
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的字符串</returns>
        public string GetString(string settingName, string defaultValue = null)
        {
            if (!mSettings.TryGetValue(settingName, out var value))
            {
                Log.Warning($"Setting ({settingName}) is not exist.");
                return defaultValue;
            }

            return value;
        }

        /// <summary>
        /// 向指定的游戏配置项写入字符串
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的字符串</param>
        public void SetString(string settingName, string value)
        {
            mSettings[settingName] = value;
        }

        /// <summary>
        /// 序列化数据
        /// </summary>
        /// <param name="stream">二进制数据流</param>
        public void Serialize(Stream stream)
        {
            using (var binaryWriter = new BinaryWriter(stream, Encoding.UTF8))
            {
                binaryWriter.Write7BitEncodedInt32(mSettings.Count);
                foreach (var (key, value) in mSettings)
                {
                    binaryWriter.Write(key);
                    binaryWriter.Write(value);
                }
            }
        }

        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="stream">二进制数据流</param>
        public void Deserialize(Stream stream)
        {
            mSettings.Clear();
            using (var binaryReader = new BinaryReader(stream, Encoding.UTF8))
            {
                var settingCount = binaryReader.Read7BitEncodedInt32();
                for (var i = 0; i < settingCount; i++)
                {
                    mSettings.Add(binaryReader.ReadString(), binaryReader.ReadString());
                }
            }
        }
    }
}