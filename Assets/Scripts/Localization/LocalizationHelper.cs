using System.IO;
using UnityEngine;
using GameFramework;
using GameFramework.Localization;

public class LocalizationHelper : ILocalizationHelper
{
    /// <summary>
    /// 获取系统语言。
    /// </summary>
    public Language SystemLanguage
    {
        get;
    }

    /// <summary>
    /// 加载字典。
    /// </summary>
    /// <param name="dictionaryAsset">字典资源。</param>
    /// <param name="loadType">字典加载方式。</param>
    /// <param name="userData">用户自定义数据。</param>
    /// <returns>是否加载成功。</returns>
    public bool LoadDictionary(object dictionaryAsset, LoadType loadType, object userData)
    {
        return true;
    }

    /// <summary>
    /// 解析字典。
    /// </summary>
    /// <param name="text">要解析的字典文本。</param>
    /// <param name="userData">用户自定义数据。</param>
    /// <returns>是否解析字典成功。</returns>
    public bool ParseDictionary(string text, object userData)
    {
        return true;
    }

    /// <summary>
    /// 解析字典。
    /// </summary>
    /// <param name="bytes">要解析的字典二进制流。</param>
    /// <param name="userData">用户自定义数据。</param>
    /// <returns>是否解析字典成功。</returns>
    public bool ParseDictionary(byte[] bytes, object userData)
    {
        return true;
    }

    /// <summary>
    /// 解析字典。
    /// </summary>
    /// <param name="stream">要解析的字典二进制流。</param>
    /// <param name="userData">用户自定义数据。</param>
    /// <returns>是否解析字典成功。</returns>
    public bool ParseDictionary(Stream stream, object userData)
    {
        return true;
    }

    /// <summary>
    /// 释放字典资源。
    /// </summary>
    /// <param name="dictionaryAsset">要释放的字典资源。</param>
    public void ReleaseDictionaryAsset(object dictionaryAsset)
    {

    }
}
