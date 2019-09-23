using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;

public class UICryptWindow : UIWindow
{
    [SerializeField]
    Button btnCrypt;
    [SerializeField]
    Button btnBack;

    void Start()
    {
        this.btnCrypt.onClick.AddListener(this.OnCryptButtonClicked);
        this.btnBack.onClick.AddListener(this.OnBackButtonClicked);
    }

    private void OnCryptButtonClicked()
    {
        string serverkey = "ncd2QzaH2DI=";
        string clientkey = "sXyvydU6vYI=";

        Debug.Log(Utility.Crypt.Base64Encode(Utility.Crypt.DHExchange(Utility.Crypt.Base64Decode(serverkey))));
        Debug.Log(Utility.Crypt.Base64Encode(Utility.Crypt.DHExchange(Utility.Crypt.Base64Decode(clientkey))));

        var secret = Utility.Crypt.DHSecret(Utility.Crypt.Base64Decode(serverkey), Utility.Crypt.Base64Decode(clientkey));
        Debug.Log(Utility.Crypt.Base64Encode(Utility.Crypt.HMac64(Utility.Crypt.Base64Decode(clientkey), secret)));
        Debug.Log(Utility.Crypt.HexEncode(secret));

        var handshake = string.Format("{0}@{1}#{2}:{3}", Utility.Crypt.Base64Encode("xbb"), Utility.Crypt.Base64Encode("1000"), Utility.Crypt.Base64Encode("1001") , 1);
        Debug.Log(handshake);
        Debug.Log(Utility.Crypt.Base64Encode(Utility.Crypt.HashKey(handshake)));
	    var hmac = Utility.Crypt.HMac64(Utility.Crypt.HashKey(handshake), secret);
        Debug.Log(Utility.Crypt.Base64Encode(hmac));

        string token = string.Format("{0}@{1}:{2}", Utility.Crypt.Base64Encode("xbb"), Utility.Crypt.Base64Encode("sample"), Utility.Crypt.Base64Encode("123456"));
        var etoken = Utility.Crypt.DesEncode(secret, token);
        Debug.Log(etoken.Length);
        Debug.Log(Utility.Crypt.Base64Encode(etoken));
    }

    private void OnBackButtonClicked()
    {
        UIManager.Instance.PopWindow(this.SerialId);
    }
}
