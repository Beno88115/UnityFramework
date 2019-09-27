﻿using System;
using UnityEngine;

public class TestModel : ModelBase 
{
    public void Login()
    {
        NetworkManager.Instance.Connect("127.0.0.1", 8888);
    }
    
    protected override void OnSubscribe()
    {
        NetworkManager.Instance.Subscribe(1000, this.OnEchoMessage);
        NetworkManager.Instance.Subscribe(NetworkEvent.ON_CONNECTED, this.OnNetworkConnected);
    }

    protected override void OnUnSubscribe()
    {
        NetworkManager.Instance.Unsubscribe(1000, this.OnEchoMessage);
        NetworkManager.Instance.Unsubscribe(NetworkEvent.ON_CONNECTED, this.OnNetworkConnected);
    }

    private void OnNetworkConnected(object sender, CustomEventArgs e)
    {
        UserPacket up = new UserPacket();
        up.UserName = "xbb";
        up.Password = "123456";
        up.Address = "UNIT 1904 19/F PODIUM PLAZA 5 HANOI ROAD TSIM SHA TSUI KL";
        NetworkManager.Instance.Send(up);
    }

    private void OnEchoMessage(object sender, CustomEventArgs e)
    {
        UserPacket up = (UserPacket)e.UserData;
        UnityEngine.Debug.Log("name:" + up.UserName + ", pwd:" + up.Password + ", address:" + up.Address);
    }
}