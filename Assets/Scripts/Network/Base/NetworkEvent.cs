using System;

/// <summary>
/// 网络消息编号[1, 100]。
/// </summary>
public static class NetworkEvent 
{
    public static readonly int ON_CONNECTED = 20;
    public static readonly int ON_ERROR = 21;
    public static readonly int ON_CLOSE = 22;
    public static readonly int ON_MISS_HEART_BEAT = 23;
    public static readonly int ON_CUSTOM_ERROR = 24;
}