/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/1 10:45:44
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using Framework;
using UnityEngine;

namespace Runtime
{
    /// <summary>
    /// 全局配置组件
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Framework/Config")]
    public class ConfigComponent : FrameworkComponent
    {
        private IConfigManager mConfigManager = null;
        private EventComponent mEventComponent = null;

        [SerializeField] private bool mEnableLoadConfigUpdateEvent = false;

        [SerializeField] private bool mEnableLoadConfigDependencyAssetEvent = false;

        [SerializeField] private string mConfigHelperTypeName = "Runtime.DefaultConfigHelper";

        [SerializeField] private ConfigHelperBase mCustomConfigHelper = null;

        [SerializeField] private int mCachedBytesSize = 0;

        /// <summary>
        /// 全局配置项数量
        /// </summary>
        public int Count => mConfigManager.Count;

        /// <summary>
        /// 缓冲二进制流大小
        /// </summary>
        public int CachedBytesSize => mConfigManager.CachedBytesSize;

        protected override void Awake()
        {
            base.Awake();

            mConfigManager = FrameworkEntry.GetModule<IConfigManager>();
            if (mConfigManager == null)
            {
                Log.Error("Config manager is invalid.");
                return;
            }

            mConfigManager.ReadDataSuccess += OnReadDataSuccess;
            mConfigManager.ReadDataFailure += OnReadDataFailure;

            if (mEnableLoadConfigUpdateEvent)
            {
                mConfigManager.ReadDataUpdate += OnReadDataUpdate;
            }

            if (mEnableLoadConfigDependencyAssetEvent)
            {
                mConfigManager.ReadDataDependencyAsset += OnReadDataDependencyAsset;
            }
        }

        private void Start()
        {
            var baseComponent = MainEntryHelper.GetComponent<BaseComponent>();
            if (baseComponent == null)
            {
                Log.Error("Base component is invalid.");
                return;
            }

            mEventComponent = MainEntryHelper.GetComponent<EventComponent>();
            if (mEventComponent == null)
            {
                Log.Error("Event component  is invalid.");
                return;
            }

            var configHelper = Helper.CreateHelper(mConfigHelperTypeName, mCustomConfigHelper);
            if (configHelper == null)
            {
                Log.Error(
                    $"Can not create config helper, please check config helper type name ({mConfigHelperTypeName}).");
                return;
            }

            configHelper.name = "Config Helper";
            var configHelperTransform = configHelper.transform;
            configHelperTransform.SetParent(this.transform);
            configHelperTransform.localScale = Vector3.one;

            mConfigManager.SetDataProviderHelper(configHelper);
            mConfigManager.SetConfigHelper(configHelper);

            if (mCachedBytesSize > 0)
            {
                EnsureCachedBytesSize(mCachedBytesSize);
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="dataAssetName">数据资源名称</param>
        /// <param name="priority">数据资源加载优先级</param>
        /// <param name="userData">自定义数据</param>
        public void ReadData(string dataAssetName, int priority, object userData)
        {
            mConfigManager.ReadData(dataAssetName, priority, userData);
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="dataString">数据字符串</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析成功</returns>
        public bool ParseData(string dataString, object userData)
        {
            return mConfigManager.ParseData(dataString, userData);
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="dataBytes">数据二进制流</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>是否解析成功</returns>
        public bool ParseData(byte[] dataBytes, object userData)
        {
            return mConfigManager.ParseData(dataBytes, userData);
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
            return mConfigManager.ParseData(dataBytes, startIndex, length, userData);
        }

        /// <summary>
        /// 设置资源管理器
        /// </summary>
        /// <param name="resourceManager">资源管理器</param>
        public void SetResourceManager(IResourceManager resourceManager)
        {
            mConfigManager.SetResourceManager(resourceManager);
        }

        /// <summary>
        /// 设置全局配置数据提供者辅助器
        /// </summary>
        /// <param name="dataProviderHelper">全局配置数据提供者辅助器</param>
        public void SetDataProviderHelper(IDataProviderHelper<IConfigManager> dataProviderHelper)
        {
            mConfigManager.SetDataProviderHelper(dataProviderHelper);
        }

        /// <summary>
        /// 设置全局配置辅助器
        /// </summary>
        /// <param name="configHelper">全局配置辅助器</param>
        public void SetConfigHelper(IConfigHelper configHelper)
        {
            mConfigManager.SetConfigHelper(configHelper);
        }

        /// <summary>
        /// 确保二进制流缓存分配足够的内存并缓存
        /// </summary>
        /// <param name="ensureSize"></param>
        public void EnsureCachedBytesSize(int ensureSize)
        {
            mConfigManager.EnsureCachedBytesSize(ensureSize);
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
            return mConfigManager.HasConfig(configName);
        }

        /// <summary>
        /// 从指定全局配置项中读取布尔值
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <returns>读取的布尔值</returns>
        public bool GetBool(string configName)
        {
            return mConfigManager.GetBool(configName);
        }

        /// <summary>
        /// 从指定全局配置项中读取布尔值
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <param name="defaultValue">读取失败时，使用默认值</param>
        /// <returns>读取的布尔值</returns>
        public bool GetBool(string configName, bool defaultValue)
        {
            return mConfigManager.GetBool(configName, defaultValue);
        }

        /// <summary>
        /// 从指定全局配置项中读取整数值
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <returns>读取的整数值</returns>
        public int GetInt(string configName)
        {
            return mConfigManager.GetInt(configName);
        }

        /// <summary>
        /// 从指定全局配置项中读取整数值
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <param name="defaultValue">读取失败时，使用默认值</param>
        /// <returns>读取的整数值</returns>
        public int GetInt(string configName, int defaultValue)
        {
            return mConfigManager.GetInt(configName, defaultValue);
        }

        /// <summary>
        /// 从指定全局配置项中读取浮点数值
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <returns>读取的浮点数值</returns>
        public float GetFloat(string configName)
        {
            return mConfigManager.GetFloat(configName);
        }

        /// <summary>
        /// 从指定全局配置项中读取浮点数值
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <param name="defaultValue">读取失败时，使用默认值</param>
        /// <returns>读取的浮点数值</returns>
        public float GetFloat(string configName, float defaultValue)
        {
            return mConfigManager.GetFloat(configName, defaultValue);
        }

        /// <summary>
        /// 从指定全局配置项中读取字符串
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <returns>读取的字符串</returns>
        public string GetString(string configName)
        {
            return mConfigManager.GetString(configName);
        }

        /// <summary>
        /// 从指定全局配置项中读取字符串
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <param name="defaultValue">读取失败时，使用默认值</param>
        /// <returns>读取的字符串</returns>
        public string GetString(string configName, string defaultValue)
        {
            return mConfigManager.GetString(configName, defaultValue);
        }

        /// <summary>
        /// 增加指定全局配置项
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <param name="configValue">全局配置项的值</param>
        /// <returns>是否增加全局配置项成功</returns>
        public bool AddConfig(string configName, string configValue)
        {
            return mConfigManager.AddConfig(configName, configValue);
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
            return mConfigManager.AddConfig(configName, boolValue, intValue, floatValue, stringValue);
        }

        /// <summary>
        /// 移除指定全局配置项
        /// </summary>
        /// <param name="configName">全局配置项的名称</param>
        /// <returns>是否移除指定全局配置项成功</returns>
        public bool RemoveConfig(string configName)
        {
            return mConfigManager.RemoveConfig(configName);
        }

        /// <summary>
        /// 清空所有全局配置项
        /// </summary>
        public void RemoveAllConfigs()
        {
            mConfigManager.RemoveAllConfigs();
        }

        private void OnReadDataSuccess(object sender, ReadDataSuccessEventArgs e)
        {
            mEventComponent.Fire(this, LoadConfigSuccessEventArgs.Create(e));
        }

        private void OnReadDataFailure(object sender, ReadDataFailureEventArgs e)
        {
            Log.Warning($"Load config failure, asset name ({e.DataAssetName}), error message ({e.ErrorMessage})");
            mEventComponent.Fire(this, LoadConfigFailureEventArgs.Create(e));
        }

        private void OnReadDataUpdate(object sender, ReadDataUpdateEventArgs e)
        {
            mEventComponent.Fire(this, LoadConfigUpdateEventArgs.Create(e));
        }

        private void OnReadDataDependencyAsset(object sender, ReadDataDependencyAssetEventArgs e)
        {
            mEventComponent.Fire(this, LoadConfigDependencyAssetEventArgs.Create(e));
        }
    }
}