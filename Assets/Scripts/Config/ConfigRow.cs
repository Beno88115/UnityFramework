using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using System.IO;

public class ConfigRow : GameFramework.Config.IConfigRow 
{
    /// <summary>
    /// 获取数据表行的编号。
    /// </summary>
    public int Id
    {
        get;
        private set;
    }

    /// <summary>
    /// 数据表行文本解析器。
    /// </summary>
    /// <param name="configRowSegment">要解析的数据表行片段。</param>
    /// <returns>是否解析数据表行成功。</returns>
    public virtual bool ParseConfigRow(object configRowSegment)
    {
        return true;
    }
}
