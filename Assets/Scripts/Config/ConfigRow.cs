using SimpleJSON;

public abstract class ConfigRow : GameFramework.Config.IConfigRow 
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
    public bool ParseConfigRow(object configRowSegment)
    {
        JSONNode node = configRowSegment as JSONNode;
        if (node == null)
            return false;

        Id = node[0].AsInt;
        this.Parse(node);

        return true;
    }

    protected virtual void Parse(JSONNode node)
    {
    }
}