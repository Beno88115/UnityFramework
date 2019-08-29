using System;
using System.Net;
using UnityEngine;
using GameFramework;
using GameFramework.Network;

public class NetworkManager : SingletonMono<NetworkManager> 
{
    private INetworkModule m_NetworkModule;
    private INetworkChannel m_NetworkChannel;
    private EventPool<CustomEventArgs> m_EventPool;

    protected override void Awake()
    {
        base.Awake();
        this.m_EventPool = new EventPool<CustomEventArgs>(EventPoolMode.AllowNoHandler | EventPoolMode.AllowMultiHandler);
    }

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

    public void Subscribe(int id, EventHandler<CustomEventArgs> handler)
    {
        this.m_EventPool.Subscribe(id, handler);
    }

    public void Unsubscribe(int id, EventHandler<CustomEventArgs> handler)
    {
        if (this.m_EventPool.EventCount > 0) {
            this.m_EventPool.Unsubscribe(id, handler);
        }
    }

    public void Connect(string ip, int port)
    {
        if (!this.m_NetworkChannel.Connected) {
            this.m_NetworkChannel.Connect(IPAddress.Parse(ip), port);
        }
        else {
            this.m_EventPool.FireNow(this, CustomEventArgs.Create(NetworkEvent.ON_CONNECTED));
        }
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
        this.m_EventPool.FireNow(this, CustomEventArgs.Create(packet.Id, packet));
    }

    private void OnNetworkConnected(object sender, NetworkConnectedEventArgs e)
    {
        UnityEngine.Debug.Log("connect success");
        this.m_EventPool.FireNow(this, CustomEventArgs.Create(NetworkEvent.ON_CONNECTED));
    }

    private void OnNetworkClosed(object sender, NetworkClosedEventArgs e)
    {
        UnityEngine.Debug.Log("network closed");
        this.m_EventPool.FireNow(this, CustomEventArgs.Create(NetworkEvent.ON_CLOSE));
    }

    private void OnNetworkMissHeartBeat(object sender, NetworkMissHeartBeatEventArgs e)
    {
        UnityEngine.Debug.Log("network miss heart beat");
        this.m_EventPool.FireNow(this, CustomEventArgs.Create(NetworkEvent.ON_MISS_HEART_BEAT));
    }

    private void OnNetworkError(object sender, NetworkErrorEventArgs e)
    {
        UnityEngine.Debug.LogError(e.ErrorMessage);
        this.m_EventPool.FireNow(this, CustomEventArgs.Create(NetworkEvent.ON_ERROR));
    }

    private void OnNetworkCustomError(object sender, NetworkCustomErrorEventArgs e)
    {
        UnityEngine.Debug.LogError("network custom error");
        this.m_EventPool.FireNow(this, CustomEventArgs.Create(NetworkEvent.ON_CUSTOM_ERROR));
    }

    protected override bool IsGlobalScope
    {
        get { return true; }
    }
}
