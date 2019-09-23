using System;
using UnityEngine;
using GameFramework;
using GameFramework.Resource;
using GameFramework.Download;
using GameFramework.ObjectPool;
using System.Collections.Generic;

public partial class ResourceManager : SingletonMono<ResourceManager> 
{
    private IResourceModule m_ResModule;
    private Dictionary<string, List<LoadAssetCompleteCallback>> m_AssetBeingLoaded;
    private LoadAssetCallbacks m_LoadAssetCallbacks;

    protected override void Awake()
    {
        base.Awake();

        m_AssetBeingLoaded = new Dictionary<string, List<LoadAssetCompleteCallback>>();
        m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadAssetSuccessCallback, LoadAssetFailureCallback, LoadAssetUpdateCallback, LoadAssetDependencyAssetCallback);
    }

    public void Initialize()
    {
        this.m_ResModule = GameFrameworkEntry.GetModule<IResourceModule>();
        this.m_ResModule.SetResourceHelper(new ResourceHelper());
        this.m_ResModule.SetResourceSimulationHelper(new ResourceSimulationHelper());

        this.m_ResModule.SetDownloadModule(GameFrameworkEntry.GetModule<IDownloadModule>());
        this.m_ResModule.SetObjectPoolModule(GameFrameworkEntry.GetModule<IObjectPoolModule>());

        this.m_ResModule.SetReadOnlyPath(Application.streamingAssetsPath);
        this.m_ResModule.SetReadWritePath(Application.persistentDataPath);
        this.m_ResModule.SetResourceMode(ResourceMode.Package);

        this.m_ResModule.ResourceUpdateStart += OnResourceUpdateStart;
        this.m_ResModule.ResourceUpdateChanged += OnResourceUpdateChanged;
        this.m_ResModule.ResourceUpdateSuccess += OnResourceUpdateSuccess;
        this.m_ResModule.ResourceUpdateFailure += OnResourceUpdateFailure;

        for (int i = 0; i < AppConst.kResourceAgentCount; ++i) {
            this.m_ResModule.AddLoadResourceAgentHelper(gameObject.AddComponent<LoadResourceAgentHelper>());
        }
    }

    public void InitResources()
    {
        m_ResModule.InitResources(()=>{
            
        });
    }

    public void LoadAsset(string assetName, LoadAssetCompleteCallback callback)
    {
        this.LoadAsset(assetName, typeof(UnityEngine.Object), callback, null);
    }

    public void LoadAsset(string assetName, Type assetType, LoadAssetCompleteCallback callback)
    {
        this.LoadAsset(assetName, assetType, callback, null);
    }

    public void LoadAsset(string assetName, LoadAssetCompleteCallback callback, object userData)
    {
        this.LoadAsset(assetName, typeof(UnityEngine.Object), callback, userData);
    }

    public void LoadAsset(string assetName, Type assetType, LoadAssetCompleteCallback callback, object userData)
    {
        if (string.IsNullOrEmpty(assetName)) {
            throw new GameFrameworkException("asset name is invalid.");
        }

        if (assetType == null) {
            throw new GameFrameworkException("asset type is invalid.");
        }

        if (callback == null) {
            throw new GameFrameworkException("callback is invalid.");
        }

        if (m_AssetBeingLoaded.ContainsKey(assetName)) {
            throw new GameFrameworkException("asset is loading.");
        }

        // ResObject resObject = m_ObjectPool.Spawn(assetName);
        // if (resObject == null)
        // {
        //     m_AssetBeingLoaded.Add(assetName, callback);
        //     m_ResModule.LoadAsset(assetName, GameFramework.Resource.Constant.DefaultPriority, m_LoadAssetCallbacks, userData);
        // }
        // else
        // {
        //     callback(resObject.Target);
        // }

        List<LoadAssetCompleteCallback> callbacks = null;
        if (!m_AssetBeingLoaded.TryGetValue(assetName, out callbacks)) {
            callbacks = new List<LoadAssetCompleteCallback>();
            m_AssetBeingLoaded[assetName] = callbacks;
        }
        callbacks.Add(callback);

        m_ResModule.LoadAsset(assetName, assetType, m_LoadAssetCallbacks, userData);
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

    private void LoadAssetSuccessCallback(string assetName, object assetObject, float duration, object userData)
    {
        List<LoadAssetCompleteCallback> callbacks = null;
        if (m_AssetBeingLoaded.TryGetValue(assetName, out callbacks)) {
            if (callbacks != null) {
                for (int i = 0; i < callbacks.Count; ++i) {
                    LoadAssetCompleteCallback callback = callbacks[i];
                    if (callback != null) {
                        callback(assetObject);
                    }
                }
            }
            m_AssetBeingLoaded.Remove(assetName);
        }
    }

    private void LoadAssetFailureCallback(string assetName, LoadResourceStatus status, string errorMessage, object userData)
    {
        Debug.LogError(errorMessage);
    }

    private void LoadAssetUpdateCallback(string assetName, float progress, object userData)
    {
    }

    private void LoadAssetDependencyAssetCallback(string assetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
    {
    }

    protected override bool IsGlobalScope 
    { 
        get { return true; } 
    }
}
