// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/18 15:24:17
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using Framework;
using UnityEngine;

namespace Framework.Runtime
{
    /// <summary>
    /// 默认游戏配置辅助器
    /// </summary>
    public class DefaultSettingHelper : SettingHelperBase
    {
        private const string SettingFileName = "FrameworkSetting.dat";

        private string mFilePath = null;
        private DefaultSetting mSettings = null;
        private DefaultSettingSerializer mSettingSerializer = null;

        /// <summary>
        /// 游戏配置项数量
        /// </summary>
        public override int Count => mSettings != null ? mSettings.Count : 0;

        /// <summary>
        /// 游戏配置存储文件路径
        /// </summary>
        public string FilePath => mFilePath;

        /// <summary>
        /// 默认游戏配置
        /// </summary>
        public DefaultSetting Settings => mSettings;

        /// <summary>
        /// 默认游戏配置序列化器
        /// </summary>
        public DefaultSettingSerializer SettingSerializer => mSettingSerializer;

        /// <summary>
        /// 加载游戏配置
        /// </summary>
        /// <returns>是否成功加载游戏配置</returns>
        public override bool Load()
        {
            try
            {
                if (!File.Exists(mFilePath))
                {
                    return true;
                }

                using (var fileStream = new FileStream(mFilePath, FileMode.Open, FileAccess.Read))
                {
                    mSettingSerializer.Deserialize(fileStream);
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.Warning($"Load setting failure with exception ({e}).");
                return false;
            }
        }

        /// <summary>
        /// 保存游戏配置
        /// </summary>
        /// <returns>是否成功保存游戏配置</returns>
        public override bool Save()
        {
            try
            {
                using (var fileStream = new FileStream(mFilePath, FileMode.Create, FileAccess.Write))
                {
                    return mSettingSerializer.Serialize(fileStream, mSettings);
                }
            }
            catch (Exception e)
            {
                Log.Warning($"Save setting failure with exception ({e}).");
                return false;
            }
        }

        /// <summary>
        /// 获取所有游戏配置项的名称
        /// </summary>
        /// <returns>所有游戏配置项的名称</returns>
        public override string[] GetAllSettingNames()
        {
            return mSettings.GetAllSettingNames();
        }

        /// <summary>
        /// 获取所有游戏配置项的名称
        /// </summary>
        /// <param name="results">所有游戏配置项的名称</param>
        public override void GetAllSettingNames(List<string> results)
        {
            mSettings.GetAllSettingNames(results);
        }

        /// <summary>
        /// 检查是否存在指定的游戏配置项
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <returns>是否存在指定的游戏配置项</returns>
        public override bool HasSetting(string settingName)
        {
            return mSettings.HasSetting(settingName);
        }

        /// <summary>
        /// 移除指定的游戏配置项
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <returns>是否成功移除指定的游戏配置项</returns>
        public override bool RemoveSetting(string settingName)
        {
            return mSettings.RemoveSetting(settingName);
        }

        /// <summary>
        /// 移除所有的游戏配置项
        /// </summary>
        public override void RemoveAllSettings()
        {
            mSettings.RemoveAllSettings();
        }

        /// <summary>
        /// 从指定的游戏配置项中读取布尔值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的布尔值</returns>
        public override bool GetBool(string settingName, bool defaultValue = false)
        {
            return mSettings.GetBool(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定的游戏配置项写入布尔值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的布尔值</param>
        public override void SetBool(string settingName, bool value)
        {
            mSettings.SetBool(settingName, value);
        }

        /// <summary>
        /// 从指定的游戏配置项中读取整数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的整数值</returns>
        public override int GetInt(string settingName, int defaultValue = 0)
        {
            return mSettings.GetInt(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定的游戏配置项写入整数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的整数值</param>
        public override void SetInt(string settingName, int value)
        {
            mSettings.SetInt(settingName, value);
        }

        /// <summary>
        /// 从指定的游戏配置项中读取浮点数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的浮点数值</returns>
        public override float GetFloat(string settingName, float defaultValue = 0f)
        {
            return mSettings.GetFloat(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定的游戏配置项写入浮点数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的浮点数值</param>
        public override void SetFloat(string settingName, float value)
        {
            mSettings.SetFloat(settingName, value);
        }

        /// <summary>
        /// 从指定的游戏配置项中读取字符串
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的字符串</returns>
        public override string GetString(string settingName, string defaultValue = null)
        {
            return mSettings.GetString(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定的游戏配置项写入字符串
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的字符串</param>
        public override void SetString(string settingName, string value)
        {
            mSettings.SetString(settingName, value);
        }

        /// <summary>
        /// 从指定的游戏配置项中读取对象
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>读取的对象</returns>
        public override T GetObject<T>(string settingName)
        {
            return Utility.Json.ToObject<T>(GetString(settingName));
        }

        /// <summary>
        /// 从指定的游戏配置项中读取对象
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>读取的对象</returns>
        public override T GetObject<T>(string settingName, T defaultValue)
        {
            var json = GetString(settingName, null);
            return json == null ? defaultValue : Utility.Json.ToObject<T>(json);
        }

        /// <summary>
        /// 向指定的游戏配置项写入对象
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的对象</param>
        /// <typeparam name="T">对象类型</typeparam>
        public override void SetObject<T>(string settingName, T value)
        {
            SetString(settingName, Utility.Json.ToJson(value));
        }

        /// <summary>
        /// 从指定的游戏配置项中读取对象
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <returns>读取的对象</returns>
        public override object GetObject(Type objectType, string settingName)
        {
            return Utility.Json.ToObject(objectType, GetString(settingName));
        }

        /// <summary>
        /// 从指定的游戏配置项中读取对象
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的对象</returns>
        public override object GetObject(Type objectType, string settingName, object defaultValue)
        {
            var json = GetString(settingName, null);
            return json == null ? defaultValue : Utility.Json.ToObject(objectType, json);
        }

        /// <summary>
        /// 向指定的游戏配置项写入对象
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的对象</param>
        public override void SetObject(string settingName, object value)
        {
            SetString(settingName, Utility.Json.ToJson(value));
        }

        private void Awake()
        {
            mFilePath = Utility.Path.GetRegularPath(Path.Combine(Application.persistentDataPath, SettingFileName));
            mSettings = new DefaultSetting();
            mSettingSerializer = new DefaultSettingSerializer();
            mSettingSerializer.RegisterSerializeCallback(0, SerializeDefaultSettingCallback);
            mSettingSerializer.RegisterDeserializeCallback(0, DeserializeDefaultSettingCallback);
        }

        private bool SerializeDefaultSettingCallback(Stream stream, DefaultSetting data)
        {
            mSettings.Serialize(stream);
            return true;
        }

        private DefaultSetting DeserializeDefaultSettingCallback(Stream stream)
        {
            mSettings.Deserialize(stream);
            return mSettings;
        }
    }
}