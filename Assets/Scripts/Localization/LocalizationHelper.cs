using System.IO;
using UnityEngine;
using GameFramework;
using GameFramework.Localization;
using SimpleJSON;
using System.Collections.Generic;

public class LocalizationHelper : ILocalizationHelper
{
    private GameFramework.Localization.ILocalizationModule m_LocalizationModule;

    public LocalizationHelper()
    {
        m_LocalizationModule = GameFrameworkEntry.GetModule<GameFramework.Localization.ILocalizationModule>();
    }

    /// <summary>
    /// 获取系统语言。
    /// </summary>
    public GameFramework.Localization.Language SystemLanguage
    {
        get
        {
            return GameFramework.Localization.Language.English;
        }
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
        TextAsset textAsset = dictionaryAsset as TextAsset;
        if (textAsset == null)
            return false;

        return m_LocalizationModule.ParseDictionary(textAsset.text, userData);
    }

    /// <summary>
    /// 解析字典。
    /// </summary>
    /// <param name="text">要解析的字典文本。</param>
    /// <param name="userData">用户自定义数据。</param>
    /// <returns>是否解析字典成功。</returns>
    public bool ParseDictionary(string text, object userData)
    {
        JSONNode node = JSONNode.Parse(text);
        var iter = node.GetEnumerator();
        while (iter.MoveNext()) 
        {
            KeyValuePair<string, JSONNode> pair = (KeyValuePair<string, JSONNode>)iter.Current;
            m_LocalizationModule.AddRawString(pair.Key, pair.Value);
        }
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
