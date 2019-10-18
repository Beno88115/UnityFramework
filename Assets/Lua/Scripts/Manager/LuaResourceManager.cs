using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using System;

public class LuaResourceManager : Singleton<LuaResourceManager>
{
    private Dictionary<string, List<LuaFunction>> m_AssetBeingLoaded = new Dictionary<string, List<LuaFunction>>();

    public void Initialize()
    {
        ResourceManager.Instance.Initialize();
    }

    public void LoadAsset(string assetName, LuaFunction callback)
    {
        this.LoadAsset(assetName, LuaType.Object, callback);
    }

    public void LoadAsset(string assetName, Type assetType, LuaFunction callback)
    {
        if (string.IsNullOrEmpty(assetName) || assetType == null || callback == null) {
            return;
        }

        if (m_AssetBeingLoaded.ContainsKey(assetName)) {
            return;
        }

        List<LuaFunction> callbacks = null;
        if (!m_AssetBeingLoaded.TryGetValue(assetName, out callbacks)) {
            callbacks = new List<LuaFunction>();
            m_AssetBeingLoaded[assetName] = callbacks;
        }
        callbacks.Add(callback);

        ResourceManager.Instance.LoadAsset(assetName, assetType, OnLoadAssetSuccessCallback, OnLoadAssetFailureCallback, null);
    }

    private void OnLoadAssetSuccessCallback(string assetName, object assetObject)
    {
        List<LuaFunction> callbacks = null;
        if (m_AssetBeingLoaded.TryGetValue(assetName, out callbacks)) {
            if (callbacks != null) {
                for (int i = 0; i < callbacks.Count; ++i) {
                    LuaFunction callback = callbacks[i];
                    if (callback != null) {
                        callback.Call(assetName, assetObject);
                    }
                }
                callbacks.Clear();
            }
            m_AssetBeingLoaded.Remove(assetName);
        }
    }

    private void OnLoadAssetFailureCallback(string assetName, string errMessage)
    {
        List<LuaFunction> callbacks = null;
        if (m_AssetBeingLoaded.TryGetValue(assetName, out callbacks)) {
            if (callbacks != null) {
                callbacks.Clear();
            }
            m_AssetBeingLoaded.Remove(assetName);
        }
    }
}
