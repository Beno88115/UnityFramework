using System;
using UnityEngine;
using GameFramework.Resource;

public class LoadResourceAgentHelper : ILoadResourceAgentHelper 
{
    /// <summary>
    /// 加载资源代理辅助器异步加载资源更新事件。
    /// </summary>
    public event EventHandler<LoadResourceAgentHelperUpdateEventArgs> LoadResourceAgentHelperUpdate
    {
        add {}
        remove {}
    }

    /// <summary>
    /// 加载资源代理辅助器异步读取资源文件完成事件。
    /// </summary>
    public event EventHandler<LoadResourceAgentHelperReadFileCompleteEventArgs> LoadResourceAgentHelperReadFileComplete
    {
        add {}
        remove {}
    }

    /// <summary>
    /// 加载资源代理辅助器异步读取资源二进制流完成事件。
    /// </summary>
    public event EventHandler<LoadResourceAgentHelperReadBytesCompleteEventArgs> LoadResourceAgentHelperReadBytesComplete
    {
        add {}
        remove {}
    }

    /// <summary>
    /// 加载资源代理辅助器异步将资源二进制流转换为加载对象完成事件。
    /// </summary>
    public event EventHandler<LoadResourceAgentHelperParseBytesCompleteEventArgs> LoadResourceAgentHelperParseBytesComplete
    {
        add {}
        remove {}
    }

    /// <summary>
    /// 加载资源代理辅助器异步加载资源完成事件。
    /// </summary>
    public event EventHandler<LoadResourceAgentHelperLoadCompleteEventArgs> LoadResourceAgentHelperLoadComplete
    {
        add {}
        remove {}
    }

    /// <summary>
    /// 加载资源代理辅助器错误事件。
    /// </summary>
    public event EventHandler<LoadResourceAgentHelperErrorEventArgs> LoadResourceAgentHelperError
    {
        add {}
        remove {}
    }

    /// <summary>
    /// 通过加载资源代理辅助器开始异步读取资源文件。
    /// </summary>
    /// <param name="fullPath">要加载资源的完整路径名。</param>
    public void ReadFile(string fullPath)
    {

    }

    /// <summary>
    /// 通过加载资源代理辅助器开始异步读取资源二进制流。
    /// </summary>
    /// <param name="fullPath">要加载资源的完整路径名。</param>
    /// <param name="loadType">资源加载方式。</param>
    public void ReadBytes(string fullPath, int loadType)
    {

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

    }

    /// <summary>
    /// 重置加载资源代理辅助器。
    /// </summary>
    public void Reset()
    {

    }
}
