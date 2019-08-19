using System.IO;
using GameFramework;
using GameFramework.Config;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class ConfigHelper : IConfigHelper 
{
    private GameFramework.Config.IConfigModule m_ConfigModule;

    public ConfigHelper()
    {
        m_ConfigModule = GameFrameworkEntry.GetModule<GameFramework.Config.IConfigModule>();
    }

    /// <summary>
    /// 加载数据表。
    /// </summary>
    /// <param name="configTableAsset">数据表资源。</param>
    /// <param name="loadType">数据表加载方式。</param>
    /// <param name="userData">用户自定义数据。</param>
    /// <returns>是否加载成功。</returns>
    public bool LoadConfigTable(object configTableAsset, LoadType loadType, object userData)
    {
        return true;
    }

    /// <summary>
    /// 获取数据表行片段。
    /// </summary>
    /// <param name="text">要解析的数据表文本。</param>
    /// <returns>数据表行片段。</returns>
    public IEnumerable<object> GetDataRowSegments(string text)
    {
        // JSONNode node = JSON.Parse(text);
        // return node.GetEnumerator();
        return null;
    }

    /// <summary>
    /// 获取数据表行片段。
    /// </summary>
    /// <param name="bytes">要解析的数据表二进制流。</param>
    /// <returns>数据表行片段。</returns>
    public IEnumerable<object> GetDataRowSegments(byte[] bytes)
    {
        return null;
    }

    /// <summary>
    /// 获取数据表行片段。
    /// </summary>
    /// <param name="stream">要解析的数据表二进制流。</param>
    /// <returns>数据表行片段。</returns>
    public IEnumerable<object> GetDataRowSegments(Stream stream)
    {
        return null;
    }

    /// <summary>
    /// 释放数据表资源。
    /// </summary>
    /// <param name="configTableAsset">要释放的数据表资源。</param>
    public void ReleaseConfigTableAsset(object configTableAsset)
    {
    }
}
