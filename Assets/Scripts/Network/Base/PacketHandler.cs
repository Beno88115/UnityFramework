using GameFramework;
using GameFramework.Network;

public class PacketHandler : IPacketHandler
{
    /// <summary>
    /// 获取网络消息包协议编号。
    /// </summary>
    public int Id
    {
        get;
        set;
    }

    /// <summary>
    /// 网络消息包处理函数。
    /// </summary>
    /// <param name="sender">网络消息包源。</param>
    /// <param name="packet">网络消息包内容。</param>
    public void Handle(object sender, Packet packet)
    {
    }
}