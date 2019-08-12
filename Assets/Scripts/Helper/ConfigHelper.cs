using System.IO;
using GameFramework;
using GameFramework.Config;

public class ConfigHelper : IConfigHelper 
{
    /// <summary>
    /// 加载配置。
    /// </summary>
    /// <param name="configAsset">配置资源。</param>
    /// <param name="loadType">配置加载方式。</param>
    /// <param name="userData">用户自定义数据。</param>
    /// <returns>是否加载成功。</returns>
    public bool LoadConfig(object configAsset, LoadType loadType, object userData)
    {
        return true;
    }

    /// <summary>
    /// 解析配置。
    /// </summary>
    /// <param name="text">要解析的配置文本。</param>
    /// <param name="userData">用户自定义数据。</param>
    /// <returns>是否解析配置成功。</returns>
    public bool ParseConfig(string text, object userData)
    {
        return true;
    }

    /// <summary>
    /// 解析配置。
    /// </summary>
    /// <param name="bytes">要解析的配置二进制流。</param>
    /// <param name="userData">用户自定义数据。</param>
    /// <returns>是否解析配置成功。</returns>
    public bool ParseConfig(byte[] bytes, object userData)
    {
        return true;
    }

    /// <summary>
    /// 解析配置。
    /// </summary>
    /// <param name="stream">要解析的配置二进制流。</param>
    /// <param name="userData">用户自定义数据。</param>
    /// <returns>是否解析配置成功。</returns>
    public bool ParseConfig(Stream stream, object userData)
    {
        return true;
    }

    /// <summary>
    /// 释放配置资源。
    /// </summary>
    /// <param name="configAsset">要释放的配置资源。</param>
    public void ReleaseConfigAsset(object configAsset)
    {
        
    }
}
