using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILocalizationWindow : UIWindow
{
    [SerializeField]
    Button btnLoad;
    [SerializeField]
    Button btnEnglish;
    [SerializeField]
    Button btnChinese;

    void Start()
    {
        this.btnLoad.onClick.AddListener(OnLocalizationButtonClicked);
        this.btnEnglish.onClick.AddListener(OnEnglishButtonClicked);
        this.btnChinese.onClick.AddListener(OnChineseButtonClicked);
    }

    private void OnLocalizationButtonClicked()
    {
        LocalizationManager.Instance.Load(OnLoadLocalizationSuccessCallback, OnLoadLocalizationFailureCallback);
        btnLoad.interactable = false;
    }

    private void OnEnglishButtonClicked()
    {
        LocalizationManager.Instance.CurrentLanguage = GameFramework.Localization.Language.English;
    }

    private void OnChineseButtonClicked()
    {
        LocalizationManager.Instance.CurrentLanguage = GameFramework.Localization.Language.ChineseSimplified;
    }
    
    private void OnLoadLocalizationFailureCallback(string localizedAssetName, string errMessage)
    {
        Debug.LogError("================err:" + errMessage);
    }

    private void OnLoadLocalizationSuccessCallback()
    {
        Debug.Log("==============load localization success");

        Debug.Log(LocalizationManager.Instance.GetString(Localized.Text.AD_ISNOT_READY));
    }
}
