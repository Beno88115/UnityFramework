using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class ResourceManager : SingletonMono<ResourceManager> 
{
    private readonly static int RESOURCE_AGENT_COUNT = 3;

    private GameFramework.Resource.IResourceModule m_ResModule;

    public void Initialize()
    {
        this.m_ResModule = GameFrameworkEntry.GetModule<GameFramework.Resource.IResourceModule>();
        this.m_ResModule.SetResourceHelper(new ResourceHelper());
        this.m_ResModule.SetResourceSimulationHelper(new ResourceSimulationHelper());

        this.m_ResModule.SetDownloadModule(GameFrameworkEntry.GetModule<GameFramework.Download.IDownloadModule>());
        this.m_ResModule.SetObjectPoolModule(GameFrameworkEntry.GetModule<GameFramework.ObjectPool.IObjectPoolModule>());

        this.m_ResModule.SetReadOnlyPath(Application.streamingAssetsPath);
        this.m_ResModule.SetReadWritePath(Application.persistentDataPath);
        this.m_ResModule.SetResourceMode(GameFramework.Resource.ResourceMode.Simulation);

        this.m_ResModule.ResourceUpdateStart += OnResourceUpdateStart;
        this.m_ResModule.ResourceUpdateChanged += OnResourceUpdateChanged;
        this.m_ResModule.ResourceUpdateSuccess += OnResourceUpdateSuccess;
        this.m_ResModule.ResourceUpdateFailure += OnResourceUpdateFailure;

        for (int i = 0; i < RESOURCE_AGENT_COUNT; ++i)
        {
            this.m_ResModule.AddLoadResourceAgentHelper(new LoadResourceAgentHelper());
        }
    }

    public void InitResources()
    {
        m_ResModule.InitResources();
    }

    private void OnResourceUpdateStart(object sender, GameFramework.Resource.ResourceUpdateStartEventArgs e)
    {
    }

    private void OnResourceUpdateChanged(object sender, GameFramework.Resource.ResourceUpdateChangedEventArgs e)
    {
    }

    private void OnResourceUpdateSuccess(object sender, GameFramework.Resource.ResourceUpdateSuccessEventArgs e)
    {
    }

    private void OnResourceUpdateFailure(object sender, GameFramework.Resource.ResourceUpdateFailureEventArgs e)
    {
    }

    protected override bool IsGlobalScope 
    { 
        get { return true; } 
    }
}
