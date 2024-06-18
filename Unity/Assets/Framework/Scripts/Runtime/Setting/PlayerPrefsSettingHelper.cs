// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/18 16:56:48
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Runtime
{
    public class PlayerPrefsSettingHelper : SettingHelperBase
    {
        /// <summary>
        /// 游戏配置项数量
        /// </summary>
        public override int Count => -1;

        /// <summary>
        /// 加载游戏配置
        /// </summary>
        /// <returns>是否成功加载游戏配置</returns>
        public override bool Load()
        {
            return true;
        }

        /// <summary>
        /// 保存游戏配置
        /// </summary>
        /// <returns>是否成功保存游戏配置</returns>
        public override bool Save()
        {
            PlayerPrefs.Save();
            return true;
        }

        /// <summary>
        /// 获取所有游戏配置项的名称
        /// </summary>
        /// <returns>所有游戏配置项的名称</returns>
        public override string[] GetAllSettingNames()
        {
            throw new NotSupportedException("GetAllSettingNames");
        }

        /// <summary>
        /// 获取所有游戏配置项的名称
        /// </summary>
        /// <param name="results">所有游戏配置项的名称</param>
        public override void GetAllSettingNames(List<string> results)
        {
            throw new NotSupportedException("GetAllSettingNames");
        }

        /// <summary>
        /// 检查是否存在指定的游戏配置项
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <returns>是否存在指定的游戏配置项</returns>
        public override bool HasSetting(string settingName)
        {
            return PlayerPrefs.HasKey(settingName);
        }

        /// <summary>
        /// 移除指定的游戏配置项
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <returns>是否成功移除指定的游戏配置项</returns>
        public override bool RemoveSetting(string settingName)
        {
            if (!HasSetting(settingName))
            {
                return false;
            }

            PlayerPrefs.DeleteKey(settingName);
            return true;
        }

        /// <summary>
        /// 移除所有的游戏配置项
        /// </summary>
        public override void RemoveAllSettings()
        {
            PlayerPrefs.DeleteAll();
        }

        /// <summary>
        /// 从指定的游戏配置项中读取布尔值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的布尔值</returns>
        public override bool GetBool(string settingName, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(settingName, defaultValue ? 1 : 0) != 0;
        }

        /// <summary>
        /// 向指定的游戏配置项写入布尔值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的布尔值</param>
        public override void SetBool(string settingName, bool value)
        {
            PlayerPrefs.SetInt(settingName, value ? 1 : 0);
        }

        /// <summary>
        /// 从指定的游戏配置项中读取整数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的整数值</returns>
        public override int GetInt(string settingName, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定的游戏配置项写入整数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的整数值</param>
        public override void SetInt(string settingName, int value)
        {
            PlayerPrefs.SetInt(settingName, value);
        }

        /// <summary>
        /// 从指定的游戏配置项中读取浮点数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的浮点数值</returns>
        public override float GetFloat(string settingName, float defaultValue = 0f)
        {
            return PlayerPrefs.GetFloat(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定的游戏配置项写入浮点数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的浮点数值</param>
        public override void SetFloat(string settingName, float value)
        {
            PlayerPrefs.SetFloat(settingName, value);
        }

        /// <summary>
        /// 从指定的游戏配置项中读取字符串
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的字符串</returns>
        public override string GetString(string settingName, string defaultValue = null)
        {
            return PlayerPrefs.GetString(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定的游戏配置项写入字符串
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的字符串</param>
        public override void SetString(string settingName, string value)
        {
            PlayerPrefs.SetString(settingName, value);
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
            PlayerPrefs.SetString(settingName, Utility.Json.ToJson(value));
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
            PlayerPrefs.SetString(settingName, Utility.Json.ToJson(value));
        }
    }
}