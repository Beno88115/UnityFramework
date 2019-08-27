using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    }

    public override void OnOpen(object userData)
    {
        txtTip.text = "New Text";
    }

    private void OnConnectButtonClicked()
    {
        ModelManager.Instance.Get<LoginModel>().Login();
    }

    private void OnBackButtonClicked()
    {
        UIManager.Instance.PopWindow(this.SerialId);
    }
}
