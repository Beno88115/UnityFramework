using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;

public class UINetworkWindow : UIWindow
{
    [SerializeField]
    Button btnConnect;
    [SerializeField] 
    Button btnBack;
    [SerializeField]
    Text txtTip;

    void Start()
    {
        this.btnConnect.onClick.AddListener(this.OnConnectButtonClicked);
        this.btnBack.onClick.AddListener(this.OnBackButtonClicked);

        LoginModel model = ModelManager.Instance.Get<LoginModel>();
        model.NetworkConnected += OnNetworkConnected;
        model.NetworkConnecteFailure += OnNetworkConnecteFailure;
        model.LoginSuccess += OnLoginSuccess;
        model.LoginFailure += OnLoginFailure;
    }

    public override void OnOpen(object userData)
    {
        txtTip.text = "";
    }

    private void OnConnectButtonClicked()
    {
        ModelManager.Instance.Get<LoginModel>().Login("xbb", "123456", "sample");
        // txtTip.text = "start connect";
    }

    private void OnBackButtonClicked()
    {
        UIManager.Instance.PopWindow(this.SerialId);
    }

    private void OnNetworkConnecteFailure(string errMessage)
    {
        txtTip.text = "connect failure";
    }

    private void OnNetworkConnected()
    {
        Debug.Log("connect success");
        txtTip.text = "connect success";
    }

    private void OnLoginFailure(string errMessage)
    {
        // txtTip.text = "login failure: " + errMessage;
    }

    private void OnLoginSuccess(int subid)
    {
        Debug.Log("login success: " + subid);
        // txtTip.text = "login sucess: " + subid;
    }
}
