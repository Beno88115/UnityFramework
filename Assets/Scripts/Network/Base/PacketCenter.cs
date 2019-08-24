using System;
using System.Collections.Generic;

public partial class PacketCenter
{
    private readonly Dictionary<int, Type> m_Types;

    public PacketCenter()
    { 
        m_Types = new Dictionary<int, Type>();
        RegisterPackets();
    }

    public Type GetType(int id)
    {
        Type type = null;
        if (!m_Types.TryGetValue(id, out type)) {
            UnityEngine.Debug.LogWarningFormat("[packetcenter] unregistered packet with id = {0}", id);
            return null;
        }
        return type;
    }

    private void R(int id, Type type)
    {
        if (m_Types.ContainsKey(id)) {
            UnityEngine.Debug.LogWarningFormat("[packetcenter] register multiple packets with id = {0}", id);
            return;
        }
        m_Types.Add(id, type);
    }
}
