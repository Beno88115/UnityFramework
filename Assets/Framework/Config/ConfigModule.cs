//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Resource;
using System;
using System.Collections.Generic;
using System.IO;

namespace GameFramework.Config
{
    /// <summary>
    /// 数据表管理器。
    /// </summary>
    internal sealed partial class ConfigModule : GameFrameworkModule, IConfigModule
    {
        private readonly Dictionary<string, ConfigTableBase> m_ConfigTables;
        private readonly LoadAssetCallbacks m_LoadAssetCallbacks;
        private IResourceModule m_ResourceModule;
        private IConfigHelper m_ConfigHelper;
        private EventHandler<LoadConfigSuccessEventArgs> m_ConfigSuccessEventHandler;
        private EventHandler<LoadConfigFailureEventArgs> m_ConfigFailureEventHandler;
        private EventHandler<LoadConfigUpdateEventArgs> m_ConfigUpdateEventHandler;
        private EventHandler<LoadConfigDependencyAssetEventArgs> m_ConfigDependencyAssetEventHandler;

        /// <summary>
        /// 初始化数据表管理器的新实例。
        /// </summary>
        public ConfigModule()
        {
            m_ConfigTables = new Dictionary<string, ConfigTableBase>();
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadConfigTableSuccessCallback, LoadConfigTableFailureCallback, LoadConfigTableUpdateCallback, LoadConfigTableDependencyAssetCallback);
            m_ResourceModule = null;
            m_ConfigHelper = null;
            m_ConfigSuccessEventHandler = null;
            m_ConfigFailureEventHandler = null;
            m_ConfigUpdateEventHandler = null;
            m_ConfigDependencyAssetEventHandler = null;
        }

        /// <summary>
        /// 获取数据表数量。
        /// </summary>
        public int Count
        {
            get
            {
                return m_ConfigTables.Count;
            }
        }

        /// <summary>
        /// 加载数据表成功事件。
        /// </summary>
        public event EventHandler<LoadConfigSuccessEventArgs> LoadConfigSuccess
        {
            add
            {
                m_ConfigSuccessEventHandler += value;
            }
            remove
            {
                m_ConfigSuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载数据表失败事件。
        /// </summary>
        public event EventHandler<LoadConfigFailureEventArgs> LoadConfigFailure
        {
            add
            {
                m_ConfigFailureEventHandler += value;
            }
            remove
            {
                m_ConfigFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载数据表更新事件。
        /// </summary>
        public event EventHandler<LoadConfigUpdateEventArgs> LoadConfigUpdate
        {
            add
            {
                m_ConfigUpdateEventHandler += value;
            }
            remove
            {
                m_ConfigUpdateEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载数据表时加载依赖资源事件。
        /// </summary>
        public event EventHandler<LoadConfigDependencyAssetEventArgs> LoadConfigDependencyAsset
        {
            add
            {
                m_ConfigDependencyAssetEventHandler += value;
            }
            remove
            {
                m_ConfigDependencyAssetEventHandler -= value;
            }
        }

        /// <summary>
        /// 数据表管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 关闭并清理数据表管理器。
        /// </summary>
        internal override void Shutdown()
        {
            foreach (KeyValuePair<string, ConfigTableBase> configTable in m_ConfigTables)
            {
                configTable.Value.Shutdown();
            }

            m_ConfigTables.Clear();
        }

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceModule">资源管理器。</param>
        public void SetResourceModule(IResourceModule resourceModule)
        {
            if (resourceModule == null)
            {
                throw new GameFrameworkException("Resource manager is invalid.");
            }

            m_ResourceModule = resourceModule;
        }

        /// <summary>
        /// 设置数据表辅助器。
        /// </summary>
        /// <param name="configTableHelper">数据表辅助器。</param>
        public void SetConfigHelper(IConfigHelper configTableHelper)
        {
            if (configTableHelper == null)
            {
                throw new GameFrameworkException("Data table helper is invalid.");
            }

            m_ConfigHelper = configTableHelper;
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="configTableAssetName">数据表资源名称。</param>
        /// <param name="loadType">数据表加载方式。</param>
        public void LoadConfigTable(string configTableAssetName, LoadType loadType)
        {
            LoadConfigTable(configTableAssetName, loadType, Constant.DefaultPriority, null);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="configTableAssetName">数据表资源名称。</param>
        /// <param name="loadType">数据表加载方式。</param>
        /// <param name="priority">加载数据表资源的优先级。</param>
        public void LoadConfigTable(string configTableAssetName, LoadType loadType, int priority)
        {
            LoadConfigTable(configTableAssetName, loadType, priority, null);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="configTableAssetName">数据表资源名称。</param>
        /// <param name="loadType">数据表加载方式。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadConfigTable(string configTableAssetName, LoadType loadType, object userData)
        {
            LoadConfigTable(configTableAssetName, loadType, Constant.DefaultPriority, userData);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="configTableAssetName">数据表资源名称。</param>
        /// <param name="loadType">数据表加载方式。</param>
        /// <param name="priority">加载数据表资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadConfigTable(string configTableAssetName, LoadType loadType, int priority, object userData)
        {
            if (m_ResourceModule == null)
            {
                throw new GameFrameworkException("You must set resource manager first.");
            }

            if (m_ConfigHelper == null)
            {
                throw new GameFrameworkException("You must set config table helper first.");
            }

            m_ResourceModule.LoadAsset(configTableAssetName, priority, m_LoadAssetCallbacks, LoadConfigTableInfo.Create(loadType, userData));
        }

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <returns>是否存在数据表。</returns>
        public bool HasConfigTable<T>() where T : IConfigRow
        {
            return InternalHasConfigTable(Utility.Text.GetFullName<T>(string.Empty));
        }

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <returns>是否存在数据表。</returns>
        public bool HasConfigTable(Type dataRowType)
        {
            if (dataRowType == null)
            {
                throw new GameFrameworkException("Data row type is invalid.");
            }

            if (!typeof(IConfigRow).IsAssignableFrom(dataRowType))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data row type '{0}' is invalid.", dataRowType.FullName));
            }

            return InternalHasConfigTable(Utility.Text.GetFullName(dataRowType, string.Empty));
        }

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <returns>是否存在数据表。</returns>
        public bool HasConfigTable<T>(string name) where T : IConfigRow
        {
            return InternalHasConfigTable(Utility.Text.GetFullName<T>(name));
        }

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <returns>是否存在数据表。</returns>
        public bool HasConfigTable(Type dataRowType, string name)
        {
            if (dataRowType == null)
            {
                throw new GameFrameworkException("Data row type is invalid.");
            }

            if (!typeof(IConfigRow).IsAssignableFrom(dataRowType))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data row type '{0}' is invalid.", dataRowType.FullName));
            }

            return InternalHasConfigTable(Utility.Text.GetFullName(dataRowType, name));
        }

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <returns>要获取的数据表。</returns>
        public IConfigTable<T> GetConfigTable<T>() where T : IConfigRow
        {
            return (IConfigTable<T>)InternalGetConfigTable(Utility.Text.GetFullName<T>(string.Empty));
        }

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <returns>要获取的数据表。</returns>
        public ConfigTableBase GetConfigTable(Type dataRowType)
        {
            if (dataRowType == null)
            {
                throw new GameFrameworkException("Data row type is invalid.");
            }

            if (!typeof(IConfigRow).IsAssignableFrom(dataRowType))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data row type '{0}' is invalid.", dataRowType.FullName));
            }

            return InternalGetConfigTable(Utility.Text.GetFullName(dataRowType, string.Empty));
        }

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <returns>要获取的数据表。</returns>
        public IConfigTable<T> GetConfigTable<T>(string name) where T : IConfigRow
        {
            return (IConfigTable<T>)InternalGetConfigTable(Utility.Text.GetFullName<T>(name));
        }

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <returns>要获取的数据表。</returns>
        public ConfigTableBase GetConfigTable(Type dataRowType, string name)
        {
            if (dataRowType == null)
            {
                throw new GameFrameworkException("Data row type is invalid.");
            }

            if (!typeof(IConfigRow).IsAssignableFrom(dataRowType))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data row type '{0}' is invalid.", dataRowType.FullName));
            }

            return InternalGetConfigTable(Utility.Text.GetFullName(dataRowType, name));
        }

        /// <summary>
        /// 获取所有数据表。
        /// </summary>
        /// <returns>所有数据表。</returns>
        public ConfigTableBase[] GetAllConfigTables()
        {
            int index = 0;
            ConfigTableBase[] results = new ConfigTableBase[m_ConfigTables.Count];
            foreach (KeyValuePair<string, ConfigTableBase> configTable in m_ConfigTables)
            {
                results[index++] = configTable.Value;
            }

            return results;
        }

        /// <summary>
        /// 获取所有数据表。
        /// </summary>
        /// <param name="results">所有数据表。</param>
        public void GetAllConfigTables(List<ConfigTableBase> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<string, ConfigTableBase> configTable in m_ConfigTables)
            {
                results.Add(configTable.Value);
            }
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>要创建的数据表。</returns>
        public IConfigTable<T> CreateConfigTable<T>(string text) where T : class, IConfigRow, new()
        {
            return CreateConfigTable<T>(string.Empty, text);
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>要创建的数据表。</returns>
        public ConfigTableBase CreateConfigTable(Type dataRowType, string text)
        {
            return CreateConfigTable(dataRowType, string.Empty, text);
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>要创建的数据表。</returns>
        public IConfigTable<T> CreateConfigTable<T>(string name, string text) where T : class, IConfigRow, new()
        {
            if (HasConfigTable<T>(name))
            {
                throw new GameFrameworkException(Utility.Text.Format("Already exist config table '{0}'.", Utility.Text.GetFullName<T>(name)));
            }

            ConfigTable<T> configTable = new ConfigTable<T>(name);
            InternalCreateConfigTable(configTable, text);
            m_ConfigTables.Add(Utility.Text.GetFullName<T>(name), configTable);
            return configTable;
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="configRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>要创建的数据表。</returns>
        public ConfigTableBase CreateConfigTable(Type configRowType, string name, string text)
        {
            if (configRowType == null)
            {
                throw new GameFrameworkException("Data row type is invalid.");
            }

            if (!typeof(IConfigRow).IsAssignableFrom(configRowType))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data row type '{0}' is invalid.", configRowType.FullName));
            }

            if (HasConfigTable(configRowType, name))
            {
                throw new GameFrameworkException(Utility.Text.Format("Already exist config table '{0}'.", Utility.Text.GetFullName(configRowType, name)));
            }

            Type configTableType = typeof(ConfigTable<>).MakeGenericType(configRowType);
            ConfigTableBase configTable = (ConfigTableBase)Activator.CreateInstance(configTableType, name);
            InternalCreateConfigTable(configTable, text);
            m_ConfigTables.Add(Utility.Text.GetFullName(configRowType, name), configTable);
            return configTable;
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="bytes">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        public IConfigTable<T> CreateConfigTable<T>(byte[] bytes) where T : class, IConfigRow, new()
        {
            return CreateConfigTable<T>(string.Empty, bytes);
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="bytes">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        public ConfigTableBase CreateConfigTable(Type dataRowType, byte[] bytes)
        {
            return CreateConfigTable(dataRowType, string.Empty, bytes);
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <param name="bytes">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        public IConfigTable<T> CreateConfigTable<T>(string name, byte[] bytes) where T : class, IConfigRow, new()
        {
            if (HasConfigTable<T>(name))
            {
                throw new GameFrameworkException(Utility.Text.Format("Already exist config table '{0}'.", Utility.Text.GetFullName<T>(name)));
            }

            ConfigTable<T> configTable = new ConfigTable<T>(name);
            InternalCreateConfigTable(configTable, bytes);
            m_ConfigTables.Add(Utility.Text.GetFullName<T>(name), configTable);
            return configTable;
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <param name="bytes">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        public ConfigTableBase CreateConfigTable(Type dataRowType, string name, byte[] bytes)
        {
            if (dataRowType == null)
            {
                throw new GameFrameworkException("Data row type is invalid.");
            }

            if (!typeof(IConfigRow).IsAssignableFrom(dataRowType))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data row type '{0}' is invalid.", dataRowType.FullName));
            }

            if (HasConfigTable(dataRowType, name))
            {
                throw new GameFrameworkException(Utility.Text.Format("Already exist config table '{0}'.", Utility.Text.GetFullName(dataRowType, name)));
            }

            Type configTableType = typeof(ConfigTable<>).MakeGenericType(dataRowType);
            ConfigTableBase configTable = (ConfigTableBase)Activator.CreateInstance(configTableType, name);
            InternalCreateConfigTable(configTable, bytes);
            m_ConfigTables.Add(Utility.Text.GetFullName(dataRowType, name), configTable);
            return configTable;
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="stream">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        public IConfigTable<T> CreateConfigTable<T>(Stream stream) where T : class, IConfigRow, new()
        {
            return CreateConfigTable<T>(string.Empty, stream);
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="stream">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        public ConfigTableBase CreateConfigTable(Type dataRowType, Stream stream)
        {
            return CreateConfigTable(dataRowType, string.Empty, stream);
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <param name="stream">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        public IConfigTable<T> CreateConfigTable<T>(string name, Stream stream) where T : class, IConfigRow, new()
        {
            if (HasConfigTable<T>(name))
            {
                throw new GameFrameworkException(Utility.Text.Format("Already exist config table '{0}'.", Utility.Text.GetFullName<T>(name)));
            }

            ConfigTable<T> configTable = new ConfigTable<T>(name);
            InternalCreateConfigTable(configTable, stream);
            m_ConfigTables.Add(Utility.Text.GetFullName<T>(name), configTable);
            return configTable;
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <param name="stream">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        public ConfigTableBase CreateConfigTable(Type dataRowType, string name, Stream stream)
        {
            if (dataRowType == null)
            {
                throw new GameFrameworkException("Data row type is invalid.");
            }

            if (!typeof(IConfigRow).IsAssignableFrom(dataRowType))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data row type '{0}' is invalid.", dataRowType.FullName));
            }

            if (HasConfigTable(dataRowType, name))
            {
                throw new GameFrameworkException(Utility.Text.Format("Already exist config table '{0}'.", Utility.Text.GetFullName(dataRowType, name)));
            }

            Type configTableType = typeof(ConfigTable<>).MakeGenericType(dataRowType);
            ConfigTableBase configTable = (ConfigTableBase)Activator.CreateInstance(configTableType, name);
            InternalCreateConfigTable(configTable, stream);
            m_ConfigTables.Add(Utility.Text.GetFullName(dataRowType, name), configTable);
            return configTable;
        }

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        public bool DestroyConfigTable<T>() where T : IConfigRow
        {
            return InternalDestroyConfigTable(Utility.Text.GetFullName<T>(string.Empty));
        }

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <returns>是否销毁数据表成功。</returns>
        public bool DestroyConfigTable(Type dataRowType)
        {
            if (dataRowType == null)
            {
                throw new GameFrameworkException("Data row type is invalid.");
            }

            if (!typeof(IConfigRow).IsAssignableFrom(dataRowType))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data row type '{0}' is invalid.", dataRowType.FullName));
            }

            return InternalDestroyConfigTable(Utility.Text.GetFullName(dataRowType, string.Empty));
        }

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        public bool DestroyConfigTable<T>(string name) where T : IConfigRow
        {
            return InternalDestroyConfigTable(Utility.Text.GetFullName<T>(name));
        }

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <returns>是否销毁数据表成功。</returns>
        public bool DestroyConfigTable(Type dataRowType, string name)
        {
            if (dataRowType == null)
            {
                throw new GameFrameworkException("Data row type is invalid.");
            }

            if (!typeof(IConfigRow).IsAssignableFrom(dataRowType))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data row type '{0}' is invalid.", dataRowType.FullName));
            }

            return InternalDestroyConfigTable(Utility.Text.GetFullName(dataRowType, name));
        }

        private bool InternalHasConfigTable(string fullName)
        {
            return m_ConfigTables.ContainsKey(fullName);
        }

        private ConfigTableBase InternalGetConfigTable(string fullName)
        {
            ConfigTableBase configTable = null;
            if (m_ConfigTables.TryGetValue(fullName, out configTable))
            {
                return configTable;
            }

            return null;
        }

        private void InternalCreateConfigTable(ConfigTableBase configTable, string text)
        {
            IEnumerable<object> dataRowSegments = null;
            try
            {
                dataRowSegments = m_ConfigHelper.GetDataRowSegments(text);
            }
            catch (Exception exception)
            {
                if (exception is GameFrameworkException)
                {
                    throw;
                }

                throw new GameFrameworkException(Utility.Text.Format("Can not get config row segments with exception '{0}'.", exception.ToString()), exception);
            }

            if (dataRowSegments == null)
            {
                throw new GameFrameworkException("Data row segments is invalid.");
            }

            foreach (object dataRowSegment in dataRowSegments)
            {
                if (!configTable.AddConfigRow(dataRowSegment))
                {
                    throw new GameFrameworkException("Add config row failure.");
                }
            }
        }

        private void InternalCreateConfigTable(ConfigTableBase configTable, byte[] bytes)
        {
            IEnumerable<object> dataRowSegments = null;
            try
            {
                dataRowSegments = m_ConfigHelper.GetDataRowSegments(bytes);
            }
            catch (Exception exception)
            {
                if (exception is GameFrameworkException)
                {
                    throw;
                }

                throw new GameFrameworkException(Utility.Text.Format("Can not get config row segments with exception '{0}'.", exception.ToString()), exception);
            }

            if (dataRowSegments == null)
            {
                throw new GameFrameworkException("Data row segments is invalid.");
            }

            foreach (object dataRowSegment in dataRowSegments)
            {
                if (!configTable.AddConfigRow(dataRowSegment))
                {
                    throw new GameFrameworkException("Add config row failure.");
                }
            }
        }

        private void InternalCreateConfigTable(ConfigTableBase configTable, Stream stream)
        {
            IEnumerable<object> dataRowSegments = null;
            try
            {
                dataRowSegments = m_ConfigHelper.GetDataRowSegments(stream);
            }
            catch (Exception exception)
            {
                if (exception is GameFrameworkException)
                {
                    throw;
                }

                throw new GameFrameworkException(Utility.Text.Format("Can not get config row segments with exception '{0}'.", exception.ToString()), exception);
            }

            if (dataRowSegments == null)
            {
                throw new GameFrameworkException("Data row segments is invalid.");
            }

            foreach (object dataRowSegment in dataRowSegments)
            {
                if (!configTable.AddConfigRow(dataRowSegment))
                {
                    throw new GameFrameworkException("Add config row failure.");
                }
            }
        }

        private bool InternalDestroyConfigTable(string fullName)
        {
            ConfigTableBase configTable = null;
            if (m_ConfigTables.TryGetValue(fullName, out configTable))
            {
                configTable.Shutdown();
                return m_ConfigTables.Remove(fullName);
            }

            return false;
        }

        private void LoadConfigTableSuccessCallback(string configTableAssetName, object configTableAsset, float duration, object userData)
        {
            LoadConfigTableInfo loadConfigTableInfo = (LoadConfigTableInfo)userData;
            if (loadConfigTableInfo == null)
            {
                throw new GameFrameworkException("Load config table info is invalid.");
            }

            try
            {
                if (!m_ConfigHelper.LoadConfigTable(configTableAsset, loadConfigTableInfo.LoadType, loadConfigTableInfo.UserData))
                {
                    throw new GameFrameworkException(Utility.Text.Format("Load config table failure in helper, asset name '{0}'.", configTableAssetName));
                }

                if (m_ConfigSuccessEventHandler != null)
                {
                    LoadConfigSuccessEventArgs loadConfigTableSuccessEventArgs = LoadConfigSuccessEventArgs.Create(configTableAssetName, loadConfigTableInfo.LoadType, duration, loadConfigTableInfo.UserData);
                    m_ConfigSuccessEventHandler(this, loadConfigTableSuccessEventArgs);
                    ReferencePool.Release(loadConfigTableSuccessEventArgs);
                }
            }
            catch (Exception exception)
            {
                if (m_ConfigFailureEventHandler != null)
                {
                    LoadConfigFailureEventArgs loadConfigTableFailureEventArgs = LoadConfigFailureEventArgs.Create(configTableAssetName, loadConfigTableInfo.LoadType, exception.ToString(), loadConfigTableInfo.UserData);
                    m_ConfigFailureEventHandler(this, loadConfigTableFailureEventArgs);
                    ReferencePool.Release(loadConfigTableFailureEventArgs);
                    return;
                }

                throw;
            }
            finally
            {
                ReferencePool.Release(loadConfigTableInfo);
                m_ConfigHelper.ReleaseConfigTableAsset(configTableAsset);
            }
        }

        private void LoadConfigTableFailureCallback(string configTableAssetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            LoadConfigTableInfo loadConfigTableInfo = (LoadConfigTableInfo)userData;
            if (loadConfigTableInfo == null)
            {
                throw new GameFrameworkException("Load config table info is invalid.");
            }

            string appendErrorMessage = Utility.Text.Format("Load config table failure, asset name '{0}', status '{1}', error message '{2}'.", configTableAssetName, status.ToString(), errorMessage);
            if (m_ConfigFailureEventHandler != null)
            {
                LoadConfigFailureEventArgs loadConfigTableFailureEventArgs = LoadConfigFailureEventArgs.Create(configTableAssetName, loadConfigTableInfo.LoadType, appendErrorMessage, loadConfigTableInfo.UserData);
                m_ConfigFailureEventHandler(this, loadConfigTableFailureEventArgs);
                ReferencePool.Release(loadConfigTableFailureEventArgs);
                ReferencePool.Release(loadConfigTableInfo);
                return;
            }

            ReferencePool.Release(loadConfigTableInfo);
            throw new GameFrameworkException(appendErrorMessage);
        }

        private void LoadConfigTableUpdateCallback(string configTableAssetName, float progress, object userData)
        {
            LoadConfigTableInfo loadConfigTableInfo = (LoadConfigTableInfo)userData;
            if (loadConfigTableInfo == null)
            {
                throw new GameFrameworkException("Load config table info is invalid.");
            }

            if (m_ConfigUpdateEventHandler != null)
            {
                LoadConfigUpdateEventArgs loadConfigTableUpdateEventArgs = LoadConfigUpdateEventArgs.Create(configTableAssetName, loadConfigTableInfo.LoadType, progress, loadConfigTableInfo.UserData);
                m_ConfigUpdateEventHandler(this, loadConfigTableUpdateEventArgs);
                ReferencePool.Release(loadConfigTableUpdateEventArgs);
            }
        }

        private void LoadConfigTableDependencyAssetCallback(string configTableAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            LoadConfigTableInfo loadConfigTableInfo = (LoadConfigTableInfo)userData;
            if (loadConfigTableInfo == null)
            {
                throw new GameFrameworkException("Load config table info is invalid.");
            }

            if (m_ConfigDependencyAssetEventHandler != null)
            {
                LoadConfigDependencyAssetEventArgs loadConfigTableDependencyAssetEventArgs = LoadConfigDependencyAssetEventArgs.Create(configTableAssetName, dependencyAssetName, loadedCount, totalCount, loadConfigTableInfo.UserData);
                m_ConfigDependencyAssetEventHandler(this, loadConfigTableDependencyAssetEventArgs);
                ReferencePool.Release(loadConfigTableDependencyAssetEventArgs);
            }
        }
    }
}
