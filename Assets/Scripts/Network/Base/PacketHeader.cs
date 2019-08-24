using GameFramework.Network;

public class PacketHeader : IPacketHeader 
{
    public PacketHeader(int length)
    {
        this.PacketLength = length;
    }

    /// <summary>
    /// 获取网络消息包长度。
    /// </summary>
    public int PacketLength
    {
        get;
        private set;
    }
}
