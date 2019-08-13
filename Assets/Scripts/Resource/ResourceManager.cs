using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class ResourceManager : SingletonMono<ResourceManager> 
{
    private GameFramework.Resource.IResourceManager m_ResModule;

    protected override void Awake()
    {
        base.Awake();

        this.m_ResModule = GameFrameworkEntry.GetModule<GameFramework.Resource.ResourceManager>();
        this.m_ResModule.SetResourceHelper(new ResourceHelper());
        this.m_ResModule.AddLoadResourceAgentHelper(new LoadResourceAgentHelper());
    }

    public void Initialized()
    {
    }
}
