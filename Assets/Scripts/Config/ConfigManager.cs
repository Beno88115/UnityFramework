using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public partial class ConfigManager : SingletonMono<ConfigManager> 
{
    private GameFramework.Config.IConfigModule m_ConfigModule;

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

    public void LoadConfigs(LoadConfigsCompleteCallback completeCallback, LoadConfigsFailureCallback failureCallback)
    {
        if (completeCallback == null || failureCallback == null)
            return;

        m_LoadConfigsCompleteCallback = completeCallback;
        m_LoadConfigsFailureCallback = failureCallback;

        for (int i = 0; i < this.m_ConfigNameList.Length; ++i)
        {
            string configName = this.m_ConfigNameList[i];
            this.m_ConfigModule.LoadConfigTable(configName, LoadType.Text, configName);
        }
    }

    private void OnLoadConfigSuccess(object sender, GameFramework.Config.LoadConfigSuccessEventArgs e)
    {
        
    }

    private void OnLoadConfigFailure(object sender, GameFramework.Config.LoadConfigFailureEventArgs e)
    {
        if (m_LoadConfigsFailureCallback != null)
        {
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
