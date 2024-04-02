/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/3/29 14:39:2
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    public sealed partial class ConfigManager : FrameworkModule, IConfigManager
    {
        private readonly Dictionary<string, ConfigData> mConfigDatas;
        private readonly DataProvider<IConfigManager> mDataProvider;
        private IConfigHelper mConfigHelper;

        public ConfigManager()
        {
            mConfigDatas = new Dictionary<string, ConfigData>();
            mDataProvider = new DataProvider<IConfigManager>(this);
            mConfigHelper = null;
        }

        /// <summary>
        /// 全局配置项数量
        /// </summary>
        public int Count => mConfigDatas.Count;

        /// <summary>
        /// 缓冲二进制流大小
        /// </summary>
        public int CachedBytesSize => DataProvider<IConfigManager>.CachedBytesSize;

        /// <summary>
        /// 读取全局配置成功事件
        /// </summary>
        public event EventHandler<ReadDataSuccessEventArgs> ReadDataSuccess
        {
            add => mDataProvider.ReadDataSuccess += value;
            remove => mDataProvider.ReadDataSuccess -= value;
        }

        /// <summary>
        /// 读取全局配置失败事件
        /// </summary>
        public event EventHandler<ReadDataFailureEventArgs> ReadDataFailure
        {
            add => mDataProvider.ReadDataFailure += value;
            remove => mDataProvider.ReadDataFailure -= value;
        }

        /// <summary>
        /// 读取全局配置更新事件
        /// </summary>
        public event EventHandler<ReadDataUpdateEventArgs> ReadDataUpdate
        {
            add => mDataProvider.ReadDataUpdate += value;
            remove => mDataProvider.ReadDataUpdate -= value;
        }

        /// <summary>
        /// 读取全局配置加载依赖资源事件
        /// </summary>
        public event EventHandler<ReadDataDependencyAssetEventArgs> ReadDataDependencyAsset
        {
            add => mDataProvider.ReadDataDependencyAsset += value;
            remove => mDataProvider.ReadDataDependencyAsset -= value;
        }

        /// <summary>
        /// 全局配置模块轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 关闭并清理全局配置模块
        /// </summary>
        public override void Shutdown()
        {
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="dataAssetName">数据资源名称</param>
        /// <param name="priority">数据资源加载优先级</param>
        /// <param name="userData">自定义数据</param>
        public void ReadData(string dataAssetName, int priority, object userData)
        {
            mDataProvider.ReadData(dataAssetName, priority, userData);
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="dataString">数据字符串</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析成功</returns>
        public bool ParseData(string dataString, object userData)
        {
            return mDataProvider.ParseData(dataString, userData);
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="dataBytes">数据二进制流</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析成功</returns>
        public bool ParseData(byte[] dataBytes, object userData)
        {
            return mDataProvider.ParseData(dataBytes, userData);
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="dataBytes">数据二进制流</param>
        /// <param name="startIndex">二进制流起始位置</param>
        /// <param name="length">二进制流长度</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析成功</returns>
        public bool ParseData(byte[] dataBytes, int startIndex, int length, object userData)
        {
            return mDataProvider.ParseData(dataBytes, startIndex, length, userData);
        }

        /// <summary>
        /// 设置资源管理器
        /// </summary>
        /// <param name="resourceManager">资源管理器</param>
        public void SetResourceManager(IResourceManager resourceManager)
        {
            mDataProvider.SetResourceManager(resourceManager);
        }

        /// <summary>
        /// 设置全局配置数据提供者辅助器
        /// </summary>
        /// <param name="dataProviderHelper">全局配置数据提供者辅助器</param>
        public void SetDataProviderHelper(IDataProviderHelper<IConfigManager> dataProviderHelper)
        {
            mDataProvider.SetDataProviderHelper(dataProviderHelper);
        }

        /// <summary>
        /// 设置全局配置辅助器
        /// </summary>
        /// <param name="configHelper">全局配置辅助器</param>
        public void SetConfigHelper(IConfigHelper configHelper)
        {
            if (configHelper == null)
            {
                throw new Exception("Config helper is invalid.");
            }

            mConfigHelper = configHelper;
        }

        /// <summary>
        /// 确保二进制流缓存分配足够的内存并缓存
        /// </summary>
        /// <param name="ensureSize"></param>
        public void EnsureCachedBytesSize(int ensureSize)
        {
            DataProvider<IConfigManager>.EnsureCachedBytesSize(ensureSize);
        }

        /// <summary>
        /// 释放缓存的二进制流
        /// </summary>
        public void FreeCachedBytes()
        {
            DataProvider<IConfigManager>.FreeCachedBytes();
        }

        /// <summary>
        /// 检查是否存在全局配置项
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <returns>是否存在指定的全局配置项</returns>
        public bool HasConfig(string configName)
        {
            return GetConfigData(configName).HasValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取布尔值
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <returns>读取的布尔值</returns>
        public bool GetBool(string configName)
        {
            var configData = GetConfigData(configName);
            if (!configData.HasValue)
            {
                throw new Exception($"Config name ({configName}) is not exist.");
            }

            return configData.Value.BoolValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取布尔值
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <param name="defaultValue">读取失败时，使用默认值</param>
        /// <returns>读取的布尔值</returns>
        public bool GetBool(string configName, bool defaultValue)
        {
            var configData = GetConfigData(configName);
            return configData.HasValue ? configData.Value.BoolValue : defaultValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取整数值
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <returns>读取的整数值</returns>
        public int GetInt(string configName)
        {
            var configData = GetConfigData(configName);
            if (!configData.HasValue)
            {
                throw new Exception($"Config name ({configName}) is not exist.");
            }

            return configData.Value.IntValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取整数值
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <param name="defaultValue">读取失败时，使用默认值</param>
        /// <returns>读取的整数值</returns>
        public int GetInt(string configName, int defaultValue)
        {
            var configData = GetConfigData(configName);
            return configData.HasValue ? configData.Value.IntValue : defaultValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取浮点数值
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <returns>读取的浮点数值</returns>
        public float GetFloat(string configName)
        {
            var configData = GetConfigData(configName);
            if (!configData.HasValue)
            {
                throw new Exception($"Config name ({configName}) is not exist.");
            }

            return configData.Value.FloatValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取浮点数值
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <param name="defaultValue">读取失败时，使用默认值</param>
        /// <returns>读取的浮点数值</returns>
        public float GetFloat(string configName, float defaultValue)
        {
            var configData = GetConfigData(configName);
            return configData.HasValue ? configData.Value.FloatValue : defaultValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取字符串
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <returns>读取的字符串</returns>
        public string GetString(string configName)
        {
            var configData = GetConfigData(configName);
            if (!configData.HasValue)
            {
                throw new Exception($"Config name ({configName}) is not exist.");
            }

            return configData.Value.StringValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取字符串
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <param name="defaultValue">读取失败时，使用默认值</param>
        /// <returns>读取的字符串</returns>
        public string GetString(string configName, string defaultValue)
        {
            var configData = GetConfigData(configName);
            return configData.HasValue ? configData.Value.StringValue : defaultValue;
        }

        /// <summary>
        /// 增加指定全局配置项
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <param name="configValue">全局配置项的值</param>
        /// <returns>是否增加全局配置项成功</returns>
        public bool AddConfig(string configName, string configValue)
        {
            var boolValue = false;
            bool.TryParse(configValue, out boolValue);

            var intValue = 0;
            int.TryParse(configValue, out intValue);

            var floatValue = 0f;
            float.TryParse(configValue, out floatValue);

            return AddConfig(configName, boolValue, intValue, floatValue, configValue);
        }

        /// <summary>
        /// 增加指定全局配置项
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <param name="boolValue">全局配置项布尔值</param>
        /// <param name="intValue">全局配置项整数值</param>
        /// <param name="floatValue">全局配置项浮点数值</param>
        /// <param name="stringValue">全局配置项字符串</param>
        /// <returns>是否增加全局配置项成功</returns>
        public bool AddConfig(string configName, bool boolValue, int intValue, float floatValue, string stringValue)
        {
            if (HasConfig(configName))
            {
                return false;
            }

            mConfigDatas.Add(configName, new ConfigData(boolValue, intValue, floatValue, stringValue));
            return true;
        }

        /// <summary>
        /// 移除指定全局配置项
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <returns>是否移除指定全局配置项成功</returns>
        public bool RemoveConfig(string configName)
        {
            if (!HasConfig(configName))
            {
                return false;
            }

            return mConfigDatas.Remove(configName);
        }

        /// <summary>
        /// 清空所有全局配置项
        /// </summary>
        public void RemoveAllConfigs()
        {
            mConfigDatas.Clear();
        }

        private ConfigData? GetConfigData(string configName)
        {
            if (string.IsNullOrEmpty(configName))
            {
                throw new Exception("Config name is invalid.");
            }

            return mConfigDatas.GetValueOrDefault(configName);
        }
    }
}