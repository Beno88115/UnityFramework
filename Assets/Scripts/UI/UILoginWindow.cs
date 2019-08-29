using UnityEngine;
using UnityEngine.UI;

public class UILoginWindow : UIWindow 
{
    [SerializeField]
    Button btnLogIn;
    [SerializeField]
    Button btnConfig;
    [SerializeField]
    Button btnLocalization;
    [SerializeField]
    Button btnEvent;
    [SerializeField]
    Button btnRes;
    [SerializeField]
    Button btnNetwork;
    [SerializeField]
    Button btnCrypt;

    void Awake()
    {
        this.btnLogIn.onClick.AddListener(this.OnLoginButtonClicked);
        this.btnConfig.onClick.AddListener(this.OnConfigButtonClicked);
        this.btnLocalization.onClick.AddListener(this.OnLocalizationButtonClicked);
        this.btnEvent.onClick.AddListener(this.OnEventButtonClicked);
        this.btnRes.onClick.AddListener(this.OnResButtonClicked);
        this.btnNetwork.onClick.AddListener(this.OnNetworkButtonClicked);
        this.btnCrypt.onClick.AddListener(this.OnCryptButtonClicked);
    }

    private void OnLoginButtonClicked()
    {
        UIManager.Instance.PushWindow("UIHome");
    }

    private void OnConfigButtonClicked()
    {
        UIManager.Instance.PushWindow("UIConfig");
    }

    private void OnLocalizationButtonClicked()
    {
        UIManager.Instance.PushWindow("UILocalization");
    }

    private void OnEventButtonClicked()
    {
        UIManager.Instance.PushWindow("UIEventCenter");
    }

    private void OnResButtonClicked()
    {
        UIManager.Instance.PushWindow("UIResource");
    }

    private void OnNetworkButtonClicked()
    {
        UIManager.Instance.PushWindow("UINetwork");
    }

    private void OnCryptButtonClicked()
    {
        UIManager.Instance.PushWindow("UICrypt");
    }
}