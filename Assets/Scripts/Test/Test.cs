using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

public class Test : MonoBehaviour
{
    void Start()
    {
        AppFacade.Instance.Initialize();
        UIManager.Instance.PushWindow("UILogin");

        // IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        // int port = 8888;

        // var m_Socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        // m_Socket.Connect(ipAddress, port);

        // string msg = "this is a test";
        // byte[] bytes = Encoding.UTF8.GetBytes(msg);

        // List<byte> buffers = new List<byte>();
        // byte[] bb = BitConverter.GetBytes((ushort)bytes.Length);
        // Array.Reverse(bb);
        // buffers.AddRange(bb);
        // buffers.AddRange(bytes);

        // m_Socket.Send(buffers.ToArray());
        // m_Socket.Close();
    }
}
