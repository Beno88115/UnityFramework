using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.Config;

public class UILoginWindow : UIWindow 
{
    [SerializeField]
    Button btnLogIn;
    [SerializeField]
    Button btnLogUp;

    void Awake()
    {
        this.btnLogIn.onClick.AddListener(this.OnLoginButtonClicked);
        this.btnLogUp.onClick.AddListener(this.OnLogupButtonClicked);
    }

    private void OnLoginButtonClicked()
    {
        UIManager.Instance.PushWindow("UIHome");
    }

    private void OnLogupButtonClicked()
    {
        ConfigManager.Instance.LoadConfigs(OnLoadConfigsProgressCallback, OnLoadConfigsSuccessCallback, OnLoadConfigsFailureCallback);
        btnLogUp.interactable = false;
    }

    private void OnLoadConfigsProgressCallback(string configTableName)
    {
        Debug.Log("===========config:" + configTableName);
    }

    private void OnLoadConfigsFailureCallback(string configTableName, string errMessage)
    {
        Debug.LogError("================err:" + errMessage);
    }

    private void OnLoadConfigsSuccessCallback()
    {
        Debug.Log("==============load config success");
        
        var cfgRow = ConfigManager.Instance.GetConfigRow<ConfigPropRow>(101);
        if (cfgRow != null)
        {
            Debug.LogFormat("id:{0}, limit:{1}, tex: {2}", cfgRow.Id, cfgRow.Limit, cfgRow.Tex);
        }
    }
}
