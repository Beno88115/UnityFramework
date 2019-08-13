using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class ConfigManager : SingletonMono<ConfigManager> 
{
    private GameFramework.Config.IConfigManager m_ConfigModule;

    protected override void Awake()
    {
        base.Awake();

        this.m_ConfigModule = GameFrameworkEntry.GetModule<GameFramework.Config.IConfigManager>();
        this.m_ConfigModule.SetConfigHelper(new ConfigHelper());
        this.m_ConfigModule.SetResourceManager(GameFrameworkEntry.GetModule<GameFramework.Resource.IResourceManager>());
    }
}
