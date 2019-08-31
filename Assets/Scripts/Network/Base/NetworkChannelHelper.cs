using System;
using System.IO;
using System.Text;
using GameFramework;
using GameFramework.Network;
using System.Collections.Generic;
using SimpleJSON;

public class NetworkChannelHelper : INetworkChannelHelper 
{
    private INetworkChannel m_NetworkChannel;
    private PacketCenter m_PacketCenter;

    public NetworkChannelHelper()
    {
        m_NetworkChannel = null;
        m_PacketCenter = new PacketCenter();
    }

    /// <summary>
    /// 获取消息包头长度。
    /// </summary>
    public int PacketHeaderLength
    {
        get { return 2; }
    }

    /// <summary>
    /// 初始化网络频道辅助器。
    /// </summary>
    /// <param name="networkChannel">网络频道。</param>
    public void Initialize(INetworkChannel networkChannel)
    {
        m_NetworkChannel = networkChannel;
    }

    /// <summary>
    /// 关闭并清理网络频道辅助器。
    /// </summary>
    public void Shutdown()
    {
    }

    /// <summary>
    /// 发送心跳消息包。
    /// </summary>
    /// <returns>是否发送心跳消息包成功。</returns>
    public bool SendHeartBeat()
    {
        return true;
    }

    /// <summary>
    /// 序列化消息包。
    /// </summary>
    /// <typeparam name="T">消息包类型。</typeparam>
    /// <param name="packet">要序列化的消息包。</param>
    /// <param name="destination">要序列化的目标流。</param>
    /// <returns>是否序列化成功。</returns>
    public bool Serialize<T>(T packet, Stream destination) where T : Packet
    {
        if (packet.Id == 0) {
            return false;
        }

        JSONNode body = (JSONNode)packet.Serialize();
        if (body == null) {
            return false;
        }
    
        JSONObject json = new JSONObject();
        json.Add("id", packet.Id);
        json.Add("msg", body);

        byte[] content = Encoding.UTF8.GetBytes(json.ToString());
        byte[] header = BitConverter.GetBytes((ushort)content.Length);
        Array.Reverse(header);

        List<byte> buffers = new List<byte>();
        buffers.AddRange(header);
        buffers.AddRange(content);

        destination.Write(buffers.ToArray(), 0, buffers.Count);
        destination.Flush();

        return true;
    }

    /// <summary>
    /// 反序列消息包头。
    /// </summary>
    /// <param name="source">要反序列化的来源流。</param>
    /// <param name="customErrorData">用户自定义错误数据。</param>
    /// <returns></returns>
    public IPacketHeader DeserializePacketHeader(Stream source, out object customErrorData)
    {
        customErrorData = null;
        if (source.Length < PacketHeaderLength) {
            return null;
        }

        byte[] bytes = new byte[PacketHeaderLength];
        source.Read(bytes, 0, PacketHeaderLength);
        Array.Reverse(bytes);

        return new PacketHeader(BitConverter.ToUInt16(bytes, 0));
    }

    /// <summary>
    /// 反序列化消息包。
    /// </summary>
    /// <param name="packetHeader">消息包头。</param>
    /// <param name="source">要反序列化的来源流。</param>
    /// <param name="customErrorData">用户自定义错误数据。</param>
    /// <returns>反序列化后的消息包。</returns>
    public Packet DeserializePacket(IPacketHeader packetHeader, Stream source, out object customErrorData)
    {
        customErrorData = null;
        if (source.Length < packetHeader.PacketLength) {
            return null;
        }

        byte[] bytes = new byte[packetHeader.PacketLength];
        source.Read(bytes, 0, bytes.Length);

        string msg = Encoding.UTF8.GetString(bytes);
        if (string.IsNullOrEmpty(msg)) {
            return null;
        }

        JSONNode node = JSON.Parse(msg);
        if (node == null || !node.HasKey("id") || !node.HasKey("msg")) {
            return null;
        }

        int id = node["id"].AsInt;
        if (id == 0) {
            return null;
        }

        Type type = m_PacketCenter.GetType(id);
        if (type == null) {
            return null;
        }

        Packet packet = (Packet)ReferencePool.Acquire(type);
        packet.Deserialize(node["msg"]);
        return packet;
    }
}