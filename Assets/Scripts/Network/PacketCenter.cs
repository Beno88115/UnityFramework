using System;

public partial class PacketCenter
{
    void RegisterPackets()
    {
        R(10000, typeof(UserPacket));
    }
}