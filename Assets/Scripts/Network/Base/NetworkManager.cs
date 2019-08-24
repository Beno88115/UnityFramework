using System.Net;
using GameFramework;
using GameFramework.Network;

public class NetworkManager : SingletonMono<NetworkManager> 
{
    private INetworkModule m_NetworkModule;
    private INetworkChannel m_NetworkChannel;

    public void Initialize()
    {
        this.m_NetworkModule = GameFrameworkEntry.GetModule<INetworkModule>();
        this.m_NetworkChannel = this.m_NetworkModule.CreateNetworkChannel("N", new NetworkChannelHelper());
        this.m_NetworkChannel.RegisterHandler(new PacketHandler());
        this.m_NetworkChannel.SetDefaultHandler(this.OnNetworkMessage);

        this.m_NetworkModule.NetworkConnected += this.OnNetworkConnected;
        this.m_NetworkModule.NetworkClosed += this.OnNetworkClosed;
        this.m_NetworkModule.NetworkMissHeartBeat += this.OnNetworkMissHeartBeat;
        this.m_NetworkModule.NetworkError += this.OnNetworkError;
        this.m_NetworkModule.NetworkCustomError += this.OnNetworkCustomError;
    }

    public void Connect(string ip, int port)
    {
        this.m_NetworkChannel.Connect(IPAddress.Parse(ip), port);
    }

    public void Close()
    {
        this.m_NetworkChannel.Close();
    }

    public void Send<T>(T packet) where T : Packet
    {
        this.m_NetworkChannel.Send<T>(packet);
    }

    private void OnNetworkMessage(object sender, Packet packet)
    {
        UnityEngine.Debug.Log("network message");
    }

    private void OnNetworkConnected(object sender, NetworkConnectedEventArgs e)
    {
        UnityEngine.Debug.Log("connect success");

        UserPacket up = new UserPacket();
        up.UserName = "xbb";
        up.Password = "123456";
        Send(up);
    }

    private void OnNetworkClosed(object sender, NetworkClosedEventArgs e)
    {
        UnityEngine.Debug.Log("network closed");
    }

    private void OnNetworkMissHeartBeat(object sender, NetworkMissHeartBeatEventArgs e)
    {
        UnityEngine.Debug.Log("network miss heart beat");
    }

    private void OnNetworkError(object sender, NetworkErrorEventArgs e)
    {
        UnityEngine.Debug.LogError(e.ErrorMessage);
    }

    private void OnNetworkCustomError(object sender, NetworkCustomErrorEventArgs e)
    {
        UnityEngine.Debug.LogError("network custom error");
    }

    protected override bool IsGlobalScope
    {
        get { return true; }
    }
}
