using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using GameFramework;
using System.Collections.Generic;

/// <summary>
/// 登录服务器登录验证。
/// </summary>
public class LoginModel : ModelBase 
{
    private class ThreadEvent
    {
        public GameFrameworkAction<object> Handle { get; private set; }
        public object UserData { get; private set; }

        public ThreadEvent(GameFrameworkAction<object> handle, object userData)
        {
            this.Handle = handle;
            this.UserData = userData;
        }
    }

    private Socket m_Socket = null;
    private string m_UserName = null;
    private string m_UserPwd = null;
    private string m_ServerName = null;

    Queue<ThreadEvent> m_ThreadEvents = null;

    public GameFrameworkAction<string> NetworkConnecteFailure = null;
    public GameFrameworkAction NetworkConnected = null;
    public GameFrameworkAction<string> LoginFailure = null;
    public GameFrameworkAction<int> LoginSuccess = null;

    private void Awake()
    {
        m_ThreadEvents = new Queue<ThreadEvent>();
    }

    protected override void OnSubscribe()
    {
    }

    protected override void OnUnSubscribe()
    {
    }

    public void Login(string userName, string userPwd, string serverName)
    {
        if (m_Socket != null) {
            Close();
        }

        AppConst.kSecret = 0;
        m_UserName = userName;
        m_UserPwd = userPwd;
        m_ServerName = serverName;

        try {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 8001;

            m_Socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            m_Socket.BeginConnect(ipAddress, port, OnConnectCallback, null);
        }
        catch (Exception ex) {
            Debug.LogError(ex.Message);
            Close();

            if (NetworkConnecteFailure != null) {
                NetworkConnecteFailure(ex.Message);
            }
        }
    }

    private void OnConnectCallback(IAsyncResult ar)
    {
        try {
            m_Socket.EndConnect(ar);
        }
        catch (Exception ex) {
            Debug.LogError(ex.Message);
            Close();

            RunOnMainThread((ud)=>{
                if (NetworkConnecteFailure != null) {
                    NetworkConnecteFailure((string)ud);
                }
            }, ex.Message);
            return;
        }

        Debug.Log(m_Socket.RemoteEndPoint.Serialize().Family);

        RunOnMainThread((ud)=>{
            if (NetworkConnected != null) {
                NetworkConnected();
            }
            Authenticate();
        });
    }

    private void Authenticate()
    {
        try {
            byte[] challenge = Utility.Crypt.Base64Decode(ReceiveData());

            byte[] clientKey = Utility.Crypt.RandomKey();
            SendData(Utility.Crypt.Base64Encode(Utility.Crypt.DHExchange(clientKey)));

            byte[] serverKey = Utility.Crypt.Base64Decode(ReceiveData());
            ulong secret = Utility.Crypt.DHSecret(serverKey, clientKey);

            ulong hmac = Utility.Crypt.HMac64(challenge, secret);
            SendData(Utility.Crypt.Base64Encode(hmac));

            string token = string.Format("{0}@{1}:{2}", Utility.Crypt.Base64Encode(m_UserName), Utility.Crypt.Base64Encode(m_ServerName), Utility.Crypt.Base64Encode(m_UserPwd));            
            SendData(Utility.Crypt.Base64Encode(Utility.Crypt.DesEncode(secret, token)));

            string result = string.Empty;
            if (CheckResult(ReceiveData(), ref result)) {
                if (LoginSuccess != null) {
                    LoginSuccess(Convert.ToInt32(result));
                }
                AppConst.kSecret = secret;
            }
            else {
                if (LoginFailure != null) {
                    LoginFailure(result);
                }
            }
            Close();
        }
        catch (Exception ex) {
            Debug.LogError(ex.Message);
            Close();

            if (LoginFailure != null) {
                LoginFailure(ex.Message);
            }
        }
    }

    private bool CheckResult(string result, ref string msg)
    {
        if (!string.IsNullOrEmpty(result)) {
            string[] strs = result.Split(' ');
            if (strs.Length > 1) {
                int code = Convert.ToInt32(strs[0]);
                if (code == 200) {
                    msg = System.Text.Encoding.UTF8.GetString(Utility.Crypt.Base64Decode(strs[1]));
                }
                else {
                    msg = result;
                }
                return code == 200;
            }
        }
        return false;
    }

    private string ReceiveData()
    {
        byte[] buffer = new byte[1024];
        int len = m_Socket.Receive(buffer);
        return System.Text.Encoding.UTF8.GetString(buffer, 0, len).Replace("\n", "");
    }

    private void SendData(string data)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data + "\n");
        m_Socket.Send(bytes);
    }

    private void Close()
    {
        if (m_Socket == null) {
            return;
        }
        m_Socket.Close();
        m_Socket = null;
    }
    
    private void RunOnMainThread(GameFrameworkAction<object> handle, object userData = null)
    {
        if (handle != null) {
            lock (m_ThreadEvents) {
                m_ThreadEvents.Enqueue(new ThreadEvent(handle, userData));
            }
        }
    }

    private void Update()
    {
        if (m_ThreadEvents.Count > 0) {
            lock (m_ThreadEvents) {
                ThreadEvent te = m_ThreadEvents.Dequeue();
                if (te != null) {
                    te.Handle(te.UserData);
                }
            }
        }
    }
}
