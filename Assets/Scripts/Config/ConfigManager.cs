using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class ConfigManager : SingletonMono<ConfigManager> 
{
    private GameFramework.Config.IConfigModule m_ConfigModule;

    protected override void Awake()
    {
        base.Awake();

        this.m_ConfigModule = GameFrameworkEntry.GetModule<GameFramework.Config.IConfigModule>();
        this.m_ConfigModule.SetConfigHelper(new ConfigHelper());
        this.m_ConfigModule.SetResourceModule(GameFrameworkEntry.GetModule<GameFramework.Resource.IResourceModule>());
    }
}
