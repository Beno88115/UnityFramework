using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using GameFramework;
using GameFramework.Data;

public class DataHelper : IDataHelper
{
    private IDataModule m_DataModule;

    public DataHelper()
    {
        m_DataModule = GameFrameworkEntry.GetModule<IDataModule>();
    }

    /// <summary>
    /// 解析数据。
    /// </summary>
    /// <param name="text">要解析的数据文本。</param>
    /// <returns>成功解析的数据对象名称，错误解析返回null。</returns>
    public string[] ParseData(string text)
    {
        return null;
    }

    /// <summary>
    /// 解析数据。
    /// </summary>
    /// <param name="text">要解析的二进制数据流。</param>
    /// <returns>成功解析的数据对象名称，错误解析返回null。</returns>
    public string[] ParseData(byte[] bytes)
    {
        return null;
    }

    /// <summary>
    /// 解析数据。
    /// </summary>
    /// <param name="text">要解析的文件流。</param>
    /// <returns>成功解析的数据对象名称，错误解析返回null。</returns>
    public string[] ParseData(Stream stream)
    {
        return null;
    }
}
