using System;

public partial class PacketCenter
{
    void RegisterPackets()
    {
        R(1000, typeof(UserPacket));
    }
}