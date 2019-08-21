using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GameFramework;
using GameFramework.Resource;
using GameFramework.Download;
using GameFramework.ObjectPool;

public partial class ResourceManager : SingletonMono<ResourceManager> 
{
    private IResourceModule m_ResModule;
    private IObjectPoolModule m_ObjectPoolModule;
    private IObjectPool<ResObject> m_ObjectPool;
    private Dictionary<string, LoadAssetCompleteCallback> m_AssetBeingLoaded;
    private LoadAssetCallbacks m_LoadAssetCallbacks;

    protected override void Awake()
    {
        base.Awake();

        m_AssetBeingLoaded = new Dictionary<string, LoadAssetCompleteCallback>();
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
        this.m_ResModule.SetResourceMode(ResourceMode.Simulation);

        this.m_ResModule.ResourceUpdateStart += OnResourceUpdateStart;
        this.m_ResModule.ResourceUpdateChanged += OnResourceUpdateChanged;
        this.m_ResModule.ResourceUpdateSuccess += OnResourceUpdateSuccess;
        this.m_ResModule.ResourceUpdateFailure += OnResourceUpdateFailure;

        for (int i = 0; i < AppConst.kResourceAgentCount; ++i) {
            this.m_ResModule.AddLoadResourceAgentHelper(new LoadResourceAgentHelper());
        }

        this.m_ObjectPoolModule = GameFrameworkEntry.GetModule<IObjectPoolModule>();
        this.m_ObjectPool = this.m_ObjectPoolModule.CreateSingleSpawnObjectPool<ResObject>("Res Object Pool");
    }

    public void InitResources()
    {
        m_ResModule.InitResources();
    }

    /// <summary>
    /// 异步加载资源。
    /// </summary>
    /// <param name="assetName">要加载资源的名称。</param>
    /// <param name="LoadAssetSuccessCallback">加载资源成功回调函数。</param>
    public void LoadAsset(string assetName, LoadAssetCompleteCallback callback)
    {
        this.LoadAsset(assetName, typeof(UnityEngine.Object), callback, null);
    }

    /// <summary>
    /// 异步加载资源。
    /// </summary>
    /// <param name="assetName">要加载资源的名称。</param>
    /// <param name="assetType">要加载资源的类型。</param>
    /// <param name="LoadAssetSuccessCallback">加载资源成功回调函数。</param>
    public void LoadAsset(string assetName, Type assetType, LoadAssetCompleteCallback callback)
    {
        this.LoadAsset(assetName, assetType, callback, null);
    }

    /// <summary>
    /// 异步加载资源。
    /// </summary>
    /// <param name="assetName">要加载资源的名称。</param>
    /// <param name="LoadAssetSuccessCallback">加载资源成功回调函数。</param>
    /// <param name="userData">用户自定义数据。</param>
    public void LoadAsset(string assetName, LoadAssetCompleteCallback callback, object userData)
    {
        this.LoadAsset(assetName, typeof(UnityEngine.Object), callback, userData);
    }

    /// <summary>
    /// 异步加载资源。
    /// </summary>
    /// <param name="assetName">要加载资源的名称。</param>
    /// <param name="assetType">要加载资源的类型。</param>
    /// <param name="LoadAssetSuccessCallback">加载资源成功回调函数。</param>
    /// <param name="userData">用户自定义数据。</param>
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

        m_AssetBeingLoaded.Add(assetName, callback);
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
        LoadAssetCompleteCallback callback = null;
        if (m_AssetBeingLoaded.TryGetValue(assetName, out callback)) {
            if (callback != null) {
                callback(assetObject);
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
