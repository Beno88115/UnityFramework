using System.IO;
using GameFramework;
using GameFramework.Config;
using System.Collections.Generic;

public class ConfigHelper : IConfigHelper 
{
    /// <summary>
    /// 加载数据表。
    /// </summary>
    /// <param name="dataTableAsset">数据表资源。</param>
    /// <param name="loadType">数据表加载方式。</param>
    /// <param name="userData">用户自定义数据。</param>
    /// <returns>是否加载成功。</returns>
    public bool LoadConfigTable(object dataTableAsset, LoadType loadType, object userData)
    {
        return true;
    }

    /// <summary>
    /// 获取数据表行片段。
    /// </summary>
    /// <param name="text">要解析的数据表文本。</param>
    /// <returns>数据表行片段。</returns>
    public IEnumerable<GameFrameworkSegment<string>> GetDataRowSegments(string text)
    {
        return null;
    }

    /// <summary>
    /// 获取数据表行片段。
    /// </summary>
    /// <param name="bytes">要解析的数据表二进制流。</param>
    /// <returns>数据表行片段。</returns>
    public IEnumerable<GameFrameworkSegment<byte[]>> GetDataRowSegments(byte[] bytes)
    {
        return null;
    }

    /// <summary>
    /// 获取数据表行片段。
    /// </summary>
    /// <param name="stream">要解析的数据表二进制流。</param>
    /// <returns>数据表行片段。</returns>
    public IEnumerable<GameFrameworkSegment<Stream>> GetDataRowSegments(Stream stream)
    {
        return null;
    }

    /// <summary>
    /// 释放数据表资源。
    /// </summary>
    /// <param name="dataTableAsset">要释放的数据表资源。</param>
    public void ReleaseConfigTableAsset(object dataTableAsset)
    {

    }
}
