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

    void Awake()
    {
        this.btnLogIn.onClick.AddListener(this.OnLoginButtonClicked);
        this.btnConfig.onClick.AddListener(this.OnConfigButtonClicked);
        this.btnLocalization.onClick.AddListener(this.OnLocalizationButtonClicked);
        this.btnEvent.onClick.AddListener(this.OnEventButtonClicked);
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
}
