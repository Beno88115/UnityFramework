using System;
using GameFramework;
using GameFramework.Config;
using System.Collections.Generic;

public partial class ConfigManager : SingletonMono<ConfigManager> 
{
    private GameFramework.Config.IConfigModule m_ConfigModule;

    private LoadConfigsProgressCallback m_LoadConfigsProgressCallback;
    private LoadConfigsCompleteCallback m_LoadConfigsCompleteCallback;
    private LoadConfigsFailureCallback m_LoadConfigsFailureCallback;

    private Dictionary<string, bool> m_LoadCompleteConfigList = new Dictionary<string, bool>();

    public void Initialize()
    {
        this.m_ConfigModule = GameFrameworkEntry.GetModule<GameFramework.Config.IConfigModule>();
        this.m_ConfigModule.SetConfigHelper(new ConfigHelper());
        this.m_ConfigModule.SetResourceModule(GameFrameworkEntry.GetModule<GameFramework.Resource.IResourceModule>());

        this.m_ConfigModule.LoadConfigSuccess += OnLoadConfigSuccess;
        this.m_ConfigModule.LoadConfigFailure += OnLoadConfigFailure;
        this.m_ConfigModule.LoadConfigUpdate += OnLoadConfigUpdate;
        this.m_ConfigModule.LoadConfigDependencyAsset += OnLoadConfigDependencyAsset;
    }

    /// <summary>
    /// 加载配置文件。
    /// </summary>
    /// <param name="completeCallback">加载配置文件完成回调。</param>
    /// <param name="failureCallback">加载配置文件失败回调。</param>
    public void LoadConfigs(LoadConfigsCompleteCallback completeCallback, LoadConfigsFailureCallback failureCallback)
    {
        LoadConfigs(null, completeCallback, failureCallback);
    }

    /// <summary>
    /// 加载配置文件。
    /// </summary>
    /// <param name="progressCallback">每加载一个配置文件回调。</param>
    /// <param name="completeCallback">加载配置文件完成回调。</param>
    /// <param name="failureCallback">加载配置文件失败回调。</param>
    public void LoadConfigs(LoadConfigsProgressCallback progressCallback, 
        LoadConfigsCompleteCallback completeCallback, LoadConfigsFailureCallback failureCallback)
    {
        if (completeCallback == null || failureCallback == null)
            return;

        m_LoadConfigsProgressCallback = progressCallback;
        m_LoadConfigsCompleteCallback = completeCallback;
        m_LoadConfigsFailureCallback = failureCallback;

        m_LoadCompleteConfigList.Clear();
        for (int i = 0; i < this.m_ConfigNameList.Length; ++i) {
            m_LoadCompleteConfigList.Add(m_ConfigNameList[i], false);
        }

        for (int i = 0; i < this.m_ConfigNameList.Length; ++i) {
            string configName = this.m_ConfigNameList[i];
            this.m_ConfigModule.LoadConfigTable(configName, LoadType.Text, configName);
        }
    }

    /// <summary>
    /// 检查是否存在数据表行。
    /// </summary>
    /// <param name="id">数据表行的编号。</param>
    /// <returns>是否存在数据表行。</returns>
    public bool HasConfigRow<T>(int id) where T : IConfigRow
    {
        return this.m_ConfigModule.GetConfigTable<T>().HasConfigRow(id);
    }

    /// <summary>
    /// 检查是否存在数据表行。
    /// </summary>
    /// <param name="condition">要检查的条件。</param>
    /// <returns>是否存在数据表行。</returns>
    public bool HasConfigRow<T>(Predicate<T> condition) where T : IConfigRow
    {
        return this.m_ConfigModule.GetConfigTable<T>().HasConfigRow(condition);
    }

    /// <summary>
    /// 获取数据表行。
    /// </summary>
    /// <param name="id">数据表行的编号。</param>
    /// <returns>数据表行。</returns>
    public T GetConfigRow<T>(int id) where T : IConfigRow
    {
        return this.m_ConfigModule.GetConfigTable<T>().GetConfigRow(id);
    }

    /// <summary>
    /// 获取符合条件的数据表行。
    /// </summary>
    /// <param name="condition">要检查的条件。</param>
    /// <returns>符合条件的数据表行。</returns>
    /// <remarks>当存在多个符合条件的数据表行时，仅返回第一个符合条件的数据表行。</remarks>
    public T GetConfigRow<T>(Predicate<T> condition) where T : IConfigRow
    {
        return this.m_ConfigModule.GetConfigTable<T>().GetConfigRow(condition);
    }

    /// <summary>
    /// 获取符合条件的数据表行。
    /// </summary>
    /// <param name="condition">要检查的条件。</param>
    /// <returns>符合条件的数据表行。</returns>
    public T[] GetConfigRows<T>(Predicate<T> condition) where T : IConfigRow
    {
        return this.m_ConfigModule.GetConfigTable<T>().GetConfigRows(condition);
    }

    /// <summary>
    /// 获取符合条件的数据表行。
    /// </summary>
    /// <param name="condition">要检查的条件。</param>
    /// <param name="results">符合条件的数据表行。</param>
    public void GetConfigRows<T>(Predicate<T> condition, List<T> results) where T : IConfigRow
    {
        this.m_ConfigModule.GetConfigTable<T>().GetConfigRows(condition, results);
    }

    /// <summary>
    /// 获取排序后的数据表行。
    /// </summary>
    /// <param name="comparison">要排序的条件。</param>
    /// <returns>排序后的数据表行。</returns>
    public T[] GetConfigRows<T>(Comparison<T> comparison) where T : IConfigRow
    {
        return this.m_ConfigModule.GetConfigTable<T>().GetConfigRows(comparison);
    }

    /// <summary>
    /// 获取排序后的数据表行。
    /// </summary>
    /// <param name="comparison">要排序的条件。</param>
    /// <param name="results">排序后的数据表行。</param>
    public void GetConfigRows<T>(Comparison<T> comparison, List<T> results) where T : IConfigRow
    {
        this.m_ConfigModule.GetConfigTable<T>().GetConfigRows(comparison, results);
    }

    /// <summary>
    /// 获取排序后的符合条件的数据表行。
    /// </summary>
    /// <param name="condition">要检查的条件。</param>
    /// <param name="comparison">要排序的条件。</param>
    /// <returns>排序后的符合条件的数据表行。</returns>
    public T[] GetConfigRows<T>(Predicate<T> condition, Comparison<T> comparison) where T : IConfigRow
    {
        return this.m_ConfigModule.GetConfigTable<T>().GetConfigRows(condition, comparison);
    }

    /// <summary>
    /// 获取排序后的符合条件的数据表行。
    /// </summary>
    /// <param name="condition">要检查的条件。</param>
    /// <param name="comparison">要排序的条件。</param>
    /// <param name="results">排序后的符合条件的数据表行。</param>
    public void GetConfigRows<T>(Predicate<T> condition, Comparison<T> comparison, List<T> results) where T : IConfigRow
    {
        this.m_ConfigModule.GetConfigTable<T>().GetConfigRows(condition, comparison, results);
    }

    /// <summary>
    /// 获取所有数据表行。
    /// </summary>
    /// <returns>所有数据表行。</returns>
    public T[] GetAllConfigRows<T>() where T : IConfigRow
    {
        return this.m_ConfigModule.GetConfigTable<T>().GetAllConfigRows();
    }

    /// <summary>
    /// 获取所有数据表行。
    /// </summary>
    /// <param name="results">所有数据表行。</param>
    public void GetAllConfigRows<T>(List<T> results) where T : IConfigRow
    {
        this.m_ConfigModule.GetConfigTable<T>().GetAllConfigRows(results);
    }

    private void OnLoadConfigSuccess(object sender, GameFramework.Config.LoadConfigSuccessEventArgs e)
    {
        string configTableName = e.UserData as string;
        m_LoadCompleteConfigList[configTableName] = true;

        if (m_LoadConfigsProgressCallback != null) {
            m_LoadConfigsProgressCallback(configTableName);
        }

        if (HasLoadCompleted()) {
            m_LoadConfigsCompleteCallback();
        }
    }

    private bool HasLoadCompleted()
    {
        bool hasCompleted = true;
        foreach (var keyValue in m_LoadCompleteConfigList) {
            if (!keyValue.Value) {
                hasCompleted = false;
                break;
            }
        }
        return hasCompleted;
    }

    private void OnLoadConfigFailure(object sender, GameFramework.Config.LoadConfigFailureEventArgs e)
    {
        if (m_LoadConfigsFailureCallback != null) {
            m_LoadConfigsFailureCallback(e.ConfigTableAssetName, e.ErrorMessage);
        }
    }

    private void OnLoadConfigUpdate(object sender, GameFramework.Config.LoadConfigUpdateEventArgs e)
    {
    }

    private void OnLoadConfigDependencyAsset(object sender, GameFramework.Config.LoadConfigDependencyAssetEventArgs e)
    {
    }
}
