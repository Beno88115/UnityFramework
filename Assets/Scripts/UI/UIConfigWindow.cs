using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIConfigWindow : UIWindow 
{
    [SerializeField]
    Button btnLoad;
    [SerializeField]
    Button btnBack;

    void Start()
    {
        this.btnLoad.onClick.AddListener(this.OnLoadConfigButtonClicked);
        this.btnBack.onClick.AddListener(this.OnBackButtonClicked);
    }

    void OnLoadConfigButtonClicked()
    {
        ConfigManager.Instance.LoadConfigs(OnLoadConfigsProgressCallback, OnLoadConfigsSuccessCallback, OnLoadConfigsFailureCallback);
        btnLoad.interactable = false;
    }

    void OnBackButtonClicked()
    {
        UIManager.Instance.PopWindow(this.SerialId);
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
