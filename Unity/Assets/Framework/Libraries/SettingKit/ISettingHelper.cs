// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/18 11:39:24
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 游戏配置辅助器接口
    /// </summary>
    public interface ISettingHelper
    {
        /// <summary>
        /// 游戏配置项数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 加载游戏配置
        /// </summary>
        /// <returns>是否成功加载游戏配置</returns>
        bool Load();

        /// <summary>
        /// 保存游戏配置
        /// </summary>
        /// <returns>是否成功保存游戏配置</returns>
        bool Save();

        /// <summary>
        /// 获取所有游戏配置项的名称
        /// </summary>
        /// <returns>所有游戏配置项的名称</returns>
        string[] GetAllSettingNames();

        /// <summary>
        /// 获取所有游戏配置项的名称
        /// </summary>
        /// <param name="results">所有游戏配置项的名称</param>
        void GetAllSettingNames(List<string> results);

        /// <summary>
        /// 检查是否存在指定的游戏配置项
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <returns>是否存在指定的游戏配置项</returns>
        bool HasSetting(string settingName);

        /// <summary>
        /// 移除指定的游戏配置项
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <returns>是否成功移除指定的游戏配置项</returns>
        bool RemoveSetting(string settingName);

        /// <summary>
        /// 移除所有的游戏配置项
        /// </summary>
        void RemoveAllSettings();

        /// <summary>
        /// 从指定的游戏配置项中读取布尔值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的布尔值</returns>
        bool GetBool(string settingName, bool defaultValue = false);

        /// <summary>
        /// 向指定的游戏配置项写入布尔值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的布尔值</param>
        void SetBool(string settingName, bool value);

        /// <summary>
        /// 从指定的游戏配置项中读取整数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的整数值</returns>
        int GetInt(string settingName, int defaultValue = 0);

        /// <summary>
        /// 向指定的游戏配置项写入整数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的整数值</param>
        void SetInt(string settingName, int value);

        /// <summary>
        /// 从指定的游戏配置项中读取浮点数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的浮点数值</returns>
        float GetFloat(string settingName, float defaultValue = 0f);

        /// <summary>
        /// 向指定的游戏配置项写入浮点数值
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的浮点数值</param>
        void SetFloat(string settingName, float value);

        /// <summary>
        /// 从指定的游戏配置项中读取字符串
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的字符串</returns>
        string GetString(string settingName, string defaultValue = null);

        /// <summary>
        /// 向指定的游戏配置项写入字符串
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的字符串</param>
        void SetString(string settingName, string value);

        /// <summary>
        /// 从指定的游戏配置项中读取对象
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>读取的对象</returns>
        T GetObject<T>(string settingName);

        /// <summary>
        /// 从指定的游戏配置项中读取对象
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>读取的对象</returns>
        T GetObject<T>(string settingName, T defaultValue);

        /// <summary>
        /// 向指定的游戏配置项写入对象
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的对象</param>
        /// <typeparam name="T">对象类型</typeparam>
        void SetObject<T>(string settingName, T value);

        /// <summary>
        /// 从指定的游戏配置项中读取对象
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <returns>读取的对象</returns>
        object GetObject(Type objectType, string settingName);

        /// <summary>
        /// 从指定的游戏配置项中读取对象
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="defaultValue">指定的游戏配置项不存在，使用默认值</param>
        /// <returns>读取的对象</returns>
        object GetObject(Type objectType, string settingName, object defaultValue);

        /// <summary>
        /// 向指定的游戏配置项写入对象
        /// </summary>
        /// <param name="settingName">指定的游戏配置项名称</param>
        /// <param name="value">写入的对象</param>
        void SetObject(string settingName, object value);
    }
}