using System;
using UnityEngine;
using GameFramework;
using GameFramework.Resource;
using GameFramework.Download;
using GameFramework.ObjectPool;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public partial class ResourceManager : SingletonMono<ResourceManager> 
{
#if UNITY_EDITOR
    private const string kSimulateAssetBundle = "SIMULATE_ASSETBUNDLE";
	private static int s_SimulateAssetBundleInEditor = -1;
#endif

    private IResourceModule m_ResModule;
    private Dictionary<string, List<LoadResCallbacks>> m_AssetBeingLoaded;
    private LoadAssetCallbacks m_LoadAssetCallbacks;

    protected override void Awake()
    {
        base.Awake();

        m_AssetBeingLoaded = new Dictionary<string, List<LoadResCallbacks>>();
        m_LoadAssetCallbacks = new LoadAssetCallbacks(OnLoadAssetSuccessCallback, OnLoadAssetFailureCallback, OnLoadAssetUpdateCallback, OnLoadAssetDependencyAssetCallback);
    }

    public void Initialize()
    {
        this.m_ResModule = GameFrameworkEntry.GetModule<IResourceModule>();
        this.m_ResModule.SetResourceHelper(gameObject.AddComponent<ResourceHelper>());
        this.m_ResModule.SetResourceSimulationHelper(new ResourceSimulationHelper());

        this.m_ResModule.SetDownloadModule(GameFrameworkEntry.GetModule<IDownloadModule>());
        this.m_ResModule.SetObjectPoolModule(GameFrameworkEntry.GetModule<IObjectPoolModule>());

        this.m_ResModule.SetReadOnlyPath(Application.streamingAssetsPath);
        this.m_ResModule.SetReadWritePath(Application.persistentDataPath);

#if UNITY_EDITOR
        this.m_ResModule.SetResourceMode(SimulateAssetBundleInEditor ? ResourceMode.Simulation : ResourceMode.Package);
#else
        this.m_ResModule.SetResourceMode(ResourceMode.Package);
#endif

        this.m_ResModule.ResourceUpdateStart += OnResourceUpdateStart;
        this.m_ResModule.ResourceUpdateChanged += OnResourceUpdateChanged;
        this.m_ResModule.ResourceUpdateSuccess += OnResourceUpdateSuccess;
        this.m_ResModule.ResourceUpdateFailure += OnResourceUpdateFailure;

        for (int i = 0; i < AppConst.kResourceAgentCount; ++i) {
            this.m_ResModule.AddLoadResourceAgentHelper(gameObject.AddComponent<LoadResourceAgentHelper>());
        }

        // set variant for specific platform or device
        // this.m_ResModule.SetCurrentVariant("");
    }

    public void InitResources(InitResourcesCompleteCallback initResourcesCompleteCallback)
    {
#if UNITY_EDITOR
        if (SimulateAssetBundleInEditor) {
            m_ResModule.InitSimulationResources();
            if (initResourcesCompleteCallback != null) {
                initResourcesCompleteCallback.Invoke();
            }
            return;
        }
#endif
        m_ResModule.InitResources(()=>{
            if (initResourcesCompleteCallback != null) {
                initResourcesCompleteCallback.Invoke();
            }
        });
    }

    public void LoadAsset(string assetName, LoadAssetCompleteCallback callback)
    {
        this.LoadAsset(assetName, typeof(UnityEngine.Object), callback, null, null);
    }

    public void LoadAsset(string assetName, Type assetType, LoadAssetCompleteCallback callback)
    {
        this.LoadAsset(assetName, assetType, callback, null, null);
    }

    public void LoadAsset(string assetName, LoadAssetCompleteCallback callback, object userData)
    {
        this.LoadAsset(assetName, typeof(UnityEngine.Object), callback, null, userData);
    }

    public void LoadAsset(string assetName, Type assetType, LoadAssetCompleteCallback completeCallback, LoadAssetFailureCallback failureCallback, object userData)
    {
        if (string.IsNullOrEmpty(assetName)) {
            throw new GameFrameworkException("asset name is invalid.");
        }

        if (assetType == null) {
            throw new GameFrameworkException("asset type is invalid.");
        }

        if (completeCallback == null) {
            throw new GameFrameworkException("callback is invalid.");
        }

        bool isLoading = false;
        List<LoadResCallbacks> callbacks = null;
        if (!m_AssetBeingLoaded.TryGetValue(assetName, out callbacks)) {
            callbacks = new List<LoadResCallbacks>();
        }
        else {
            isLoading = true;
        }

        for (int i = 0; i < callbacks.Count; ++i) {
            var cb = callbacks[i];
            if (cb.LoadAssetCompleteCallback == completeCallback) {
                return;
            }
            if (failureCallback != null && (cb.LoadAssetFailureCallback == failureCallback)) {
                return;
            }
        }

        callbacks.Add(new LoadResCallbacks(completeCallback, failureCallback));
        m_AssetBeingLoaded[assetName] = callbacks;

        if (!isLoading) {
            m_ResModule.LoadAsset(assetName, assetType, m_LoadAssetCallbacks, userData);
        }
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

    private void OnLoadAssetSuccessCallback(string assetName, object assetObject, float duration, object userData)
    {
        List<LoadResCallbacks> callbacks = null;
        if (m_AssetBeingLoaded.TryGetValue(assetName, out callbacks)) {
            if (callbacks != null) {
                for (int i = 0; i < callbacks.Count; ++i) {
                    LoadResCallbacks callback = callbacks[i];
                    if (callback != null) {
                        callback.LoadAssetCompleteCallback(assetName, assetObject);
                    }
                }
                callbacks.Clear();
            }
            m_AssetBeingLoaded.Remove(assetName);
        }
    }

    private void OnLoadAssetFailureCallback(string assetName, LoadResourceStatus status, string errorMessage, object userData)
    {
        List<LoadResCallbacks> callbacks = null;
        if (m_AssetBeingLoaded.TryGetValue(assetName, out callbacks)) {
            if (callbacks != null) {
                for (int i = 0; i < callbacks.Count; ++i) {
                    LoadResCallbacks callback = callbacks[i];
                    if (callback != null && callback.LoadAssetFailureCallback != null) {
                        callback.LoadAssetFailureCallback(assetName, errorMessage);
                    }
                }
                callbacks.Clear();
            }
            m_AssetBeingLoaded.Remove(assetName);
        } 
    }

    private void OnLoadAssetUpdateCallback(string assetName, float progress, object userData)
    {
    }

    private void OnLoadAssetDependencyAssetCallback(string assetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
    {
    }

    protected override bool IsGlobalScope 
    { 
        get { return true; } 
    }

#if UNITY_EDITOR
	public static bool SimulateAssetBundleInEditor
    {
        get
        {
            if (s_SimulateAssetBundleInEditor == -1) {
                s_SimulateAssetBundleInEditor = EditorPrefs.GetBool(kSimulateAssetBundle, true) ? 1 : 0;
            }
            return s_SimulateAssetBundleInEditor != 0;
        }
        set
        {
            int newValue = value ? 1 : 0;
            if (newValue != s_SimulateAssetBundleInEditor) {
                s_SimulateAssetBundleInEditor = newValue;
                EditorPrefs.SetBool(kSimulateAssetBundle, value);
            }
        }
    }
#endif
}
