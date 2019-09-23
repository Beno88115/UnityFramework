using System;
using UnityEngine;
using UnityEngine.Networking;
using GameFramework.Resource;

public class LoadResourceAgentHelper : MonoBehaviour, ILoadResourceAgentHelper
{
    private string m_FileFullPath = null;
    private string m_BytesFullPath = null;
    private int m_LoadType = 0;
    private string m_AssetName = null;
    private float m_LastProgress = 0f;
    private bool m_Disposed = false;

    private UnityWebRequest m_UnityWebRequest = null;
    private AssetBundleCreateRequest m_FileAssetBundleCreateRequest = null;
    private AssetBundleCreateRequest m_BytesAssetBundleCreateRequest = null;
    private AssetBundleRequest m_AssetBundleRequest = null;
    private AsyncOperation m_AsyncOperation = null;

    private EventHandler<LoadResourceAgentHelperUpdateEventArgs> m_LoadResourceAgentHelperUpdateEventHandler = null;
    private EventHandler<LoadResourceAgentHelperReadFileCompleteEventArgs> m_LoadResourceAgentHelperReadFileCompleteEventHandler = null;
    private EventHandler<LoadResourceAgentHelperReadBytesCompleteEventArgs> m_LoadResourceAgentHelperReadBytesCompleteEventHandler = null;
    private EventHandler<LoadResourceAgentHelperParseBytesCompleteEventArgs> m_LoadResourceAgentHelperParseBytesCompleteEventHandler = null;
    private EventHandler<LoadResourceAgentHelperLoadCompleteEventArgs> m_LoadResourceAgentHelperLoadCompleteEventHandler = null;
    private EventHandler<LoadResourceAgentHelperErrorEventArgs> m_LoadResourceAgentHelperErrorEventHandler = null;

    /// <summary>
    /// 加载资源代理辅助器异步加载资源更新事件。
    /// </summary>
    public event EventHandler<LoadResourceAgentHelperUpdateEventArgs> LoadResourceAgentHelperUpdate
    {
        add { m_LoadResourceAgentHelperUpdateEventHandler += value; }
        remove { m_LoadResourceAgentHelperUpdateEventHandler -= value; }
    }

    /// <summary>
    /// 加载资源代理辅助器异步读取资源文件完成事件。
    /// </summary>
    public event EventHandler<LoadResourceAgentHelperReadFileCompleteEventArgs> LoadResourceAgentHelperReadFileComplete
    {
        add { m_LoadResourceAgentHelperReadFileCompleteEventHandler += value; }
        remove { m_LoadResourceAgentHelperReadFileCompleteEventHandler -= value; }
    }

    /// <summary>
    /// 加载资源代理辅助器异步读取资源二进制流完成事件。
    /// </summary>
    public event EventHandler<LoadResourceAgentHelperReadBytesCompleteEventArgs> LoadResourceAgentHelperReadBytesComplete
    {
        add { m_LoadResourceAgentHelperReadBytesCompleteEventHandler += value; }
        remove { m_LoadResourceAgentHelperReadBytesCompleteEventHandler -= value; }
    }

    /// <summary>
    /// 加载资源代理辅助器异步将资源二进制流转换为加载对象完成事件。
    /// </summary>
    public event EventHandler<LoadResourceAgentHelperParseBytesCompleteEventArgs> LoadResourceAgentHelperParseBytesComplete
    {
        add { m_LoadResourceAgentHelperParseBytesCompleteEventHandler += value; }
        remove { m_LoadResourceAgentHelperParseBytesCompleteEventHandler -= value; }
    }

    /// <summary>
    /// 加载资源代理辅助器异步加载资源完成事件。
    /// </summary>
    public event EventHandler<LoadResourceAgentHelperLoadCompleteEventArgs> LoadResourceAgentHelperLoadComplete
    {
        add { m_LoadResourceAgentHelperLoadCompleteEventHandler += value; }
        remove { m_LoadResourceAgentHelperLoadCompleteEventHandler -= value; }
    }

    /// <summary>
    /// 加载资源代理辅助器错误事件。
    /// </summary>
    public event EventHandler<LoadResourceAgentHelperErrorEventArgs> LoadResourceAgentHelperError
    {
        add { m_LoadResourceAgentHelperErrorEventHandler += value; }
        remove { m_LoadResourceAgentHelperErrorEventHandler -= value; }
    }

    /// <summary>
    /// 通过加载资源代理辅助器开始异步读取资源文件。
    /// </summary>
    /// <param name="fullPath">要加载资源的完整路径名。</param>
    public void ReadFile(string fullPath)
    {
        Debug.Log("========read file:" + fullPath);
        if (m_LoadResourceAgentHelperReadFileCompleteEventHandler == null 
            || m_LoadResourceAgentHelperUpdateEventHandler == null 
            || m_LoadResourceAgentHelperErrorEventHandler == null) {
            Debug.LogError("Load resource agent helper handler is invalid.");
            return;
        }

        m_FileFullPath = fullPath;
        m_FileAssetBundleCreateRequest = AssetBundle.LoadFromFileAsync(m_FileFullPath);
    }

    /// <summary>
    /// 通过加载资源代理辅助器开始异步读取资源二进制流。
    /// </summary>
    /// <param name="fullPath">要加载资源的完整路径名。</param>
    /// <param name="loadType">资源加载方式。</param>
    public void ReadBytes(string fullPath, int loadType)
    {
        Debug.Log("========ReadBytes:" + fullPath);
    }

    /// <summary>
    /// 通过加载资源代理辅助器开始异步将资源二进制流转换为加载对象。
    /// </summary>
    /// <param name="bytes">要加载资源的二进制流。</param>
    public void ParseBytes(byte[] bytes)
    {

    }

    /// <summary>
    /// 通过加载资源代理辅助器开始异步加载资源。
    /// </summary>
    /// <param name="resource">资源。</param>
    /// <param name="assetName">要加载的资源名称。</param>
    /// <param name="assetType">要加载资源的类型。</param>
    /// <param name="isScene">要加载的资源是否是场景。</param>
    public void LoadAsset(object resource, string assetName, Type assetType, bool isScene)
    {
        Debug.Log("========LoadAsset:" + assetName);
    }

    /// <summary>
    /// 重置加载资源代理辅助器。
    /// </summary>
    public void Reset()
    {
        m_FileFullPath = null;
        m_BytesFullPath = null;
        m_LoadType = 0;
        m_AssetName = null;
        m_LastProgress = 0f;

        if (m_UnityWebRequest != null)
        {
            m_UnityWebRequest.Dispose();
            m_UnityWebRequest = null;
        }

        m_FileAssetBundleCreateRequest = null;
        m_BytesAssetBundleCreateRequest = null;
        m_AssetBundleRequest = null;
        m_AsyncOperation = null;
    }
}
