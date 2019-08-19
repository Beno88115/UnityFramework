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
    /// 数据表管理器接口。
    /// </summary>
    public interface IConfigModule
    {
        /// <summary>
        /// 获取数据表数量。
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// 加载数据表成功事件。
        /// </summary>
        event EventHandler<LoadConfigSuccessEventArgs> LoadConfigSuccess;

        /// <summary>
        /// 加载数据表失败事件。
        /// </summary>
        event EventHandler<LoadConfigFailureEventArgs> LoadConfigFailure;

        /// <summary>
        /// 加载数据表更新事件。
        /// </summary>
        event EventHandler<LoadConfigUpdateEventArgs> LoadConfigUpdate;

        /// <summary>
        /// 加载数据表时加载依赖资源事件。
        /// </summary>
        event EventHandler<LoadConfigDependencyAssetEventArgs> LoadConfigDependencyAsset;

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceModule">资源管理器。</param>
        void SetResourceModule(IResourceModule resourceModule);

        /// <summary>
        /// 设置数据表辅助器。
        /// </summary>
        /// <param name="configTableHelper">数据表辅助器。</param>
        void SetConfigHelper(IConfigHelper configTableHelper);

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="configTableAssetName">数据表资源名称。</param>
        /// <param name="loadType">数据表加载方式。</param>
        void LoadConfigTable(string configTableAssetName, LoadType loadType);

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="configTableAssetName">数据表资源名称。</param>
        /// <param name="loadType">数据表加载方式。</param>
        /// <param name="priority">加载数据表资源的优先级。</param>
        void LoadConfigTable(string configTableAssetName, LoadType loadType, int priority);

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="configTableAssetName">数据表资源名称。</param>
        /// <param name="loadType">数据表加载方式。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadConfigTable(string configTableAssetName, LoadType loadType, object userData);

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="configTableAssetName">数据表资源名称。</param>
        /// <param name="loadType">数据表加载方式。</param>
        /// <param name="priority">加载数据表资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadConfigTable(string configTableAssetName, LoadType loadType, int priority, object userData);

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <returns>是否存在数据表。</returns>
        bool HasConfigTable<T>() where T : IConfigRow;

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <param name="configRowType">数据表行的类型。</param>
        /// <returns>是否存在数据表。</returns>
        bool HasConfigTable(Type configRowType);

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <returns>是否存在数据表。</returns>
        bool HasConfigTable<T>(string name) where T : IConfigRow;

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <param name="configRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <returns>是否存在数据表。</returns>
        bool HasConfigTable(Type configRowType, string name);

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <returns>要获取的数据表。</returns>
        IConfigTable<T> GetConfigTable<T>() where T : IConfigRow;

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <param name="configRowType">数据表行的类型。</param>
        /// <returns>要获取的数据表。</returns>
        ConfigTableBase GetConfigTable(Type configRowType);

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <returns>要获取的数据表。</returns>
        IConfigTable<T> GetConfigTable<T>(string name) where T : IConfigRow;

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <param name="configRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <returns>要获取的数据表。</returns>
        ConfigTableBase GetConfigTable(Type configRowType, string name);

        /// <summary>
        /// 获取所有数据表。
        /// </summary>
        /// <returns>所有数据表。</returns>
        ConfigTableBase[] GetAllConfigTables();

        /// <summary>
        /// 获取所有数据表。
        /// </summary>
        /// <param name="results">所有数据表。</param>
        void GetAllConfigTables(List<ConfigTableBase> results);

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>要创建的数据表。</returns>
        IConfigTable<T> CreateConfigTable<T>(string text) where T : class, IConfigRow, new();

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="configRowType">数据表行的类型。</param>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>要创建的数据表。</returns>
        ConfigTableBase CreateConfigTable(Type configRowType, string text);

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>要创建的数据表。</returns>
        IConfigTable<T> CreateConfigTable<T>(string name, string text) where T : class, IConfigRow, new();

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="configRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>要创建的数据表。</returns>
        ConfigTableBase CreateConfigTable(Type configRowType, string name, string text);

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="bytes">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        IConfigTable<T> CreateConfigTable<T>(byte[] bytes) where T : class, IConfigRow, new();

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="configRowType">数据表行的类型。</param>
        /// <param name="bytes">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        ConfigTableBase CreateConfigTable(Type configRowType, byte[] bytes);

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <param name="bytes">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        IConfigTable<T> CreateConfigTable<T>(string name, byte[] bytes) where T : class, IConfigRow, new();

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="configRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <param name="bytes">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        ConfigTableBase CreateConfigTable(Type configRowType, string name, byte[] bytes);

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="stream">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        IConfigTable<T> CreateConfigTable<T>(Stream stream) where T : class, IConfigRow, new();

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="configRowType">数据表行的类型。</param>
        /// <param name="stream">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        ConfigTableBase CreateConfigTable(Type configRowType, Stream stream);

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <param name="stream">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        IConfigTable<T> CreateConfigTable<T>(string name, Stream stream) where T : class, IConfigRow, new();

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="configRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <param name="stream">要解析的数据表二进制流。</param>
        /// <returns>要创建的数据表。</returns>
        ConfigTableBase CreateConfigTable(Type configRowType, string name, Stream stream);

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <returns>是否销毁数据表成功。</returns>
        bool DestroyConfigTable<T>() where T : IConfigRow;

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <param name="configRowType">数据表行的类型。</param>
        /// <returns>是否销毁数据表成功。</returns>
        bool DestroyConfigTable(Type configRowType);

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <returns>是否销毁数据表成功。</returns>
        bool DestroyConfigTable<T>(string name) where T : IConfigRow;

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <param name="configRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <returns>是否销毁数据表成功。</returns>
        bool DestroyConfigTable(Type configRowType, string name);
    }
}
