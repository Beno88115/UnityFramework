using UnityEngine;
using LuaInterface;

public class LuaLocalizationManager : Singleton<LuaLocalizationManager> 
{
    private LuaFunction m_LoadLocalizedAssetCompleteCallback;
    private LuaFunction m_LoadLocalizedAssetFailureCallback;

    public void Initialize()
    {
        LocalizationManager.Instance.Initialize();
    }

    public void Load(LuaFunction completeCallback, LuaFunction failureCallback)
    {
        if (completeCallback == null || failureCallback == null) {
            return;
        }

        m_LoadLocalizedAssetCompleteCallback = completeCallback;
        m_LoadLocalizedAssetFailureCallback = failureCallback;

        LocalizationManager.Instance.Load(OnLoadLocalizationSuccessCallback, OnLoadLocalizationFailureCallback);
    }

    public string GetString(int key)
    {
        return LocalizationManager.Instance.GetString(key);
    }

    public string GetString(int key, params object[] args)
    {
        return LocalizationManager.Instance.GetString(key, args);
    }

    public GameFramework.Localization.Language CurrentLanguage
    {
        get { return LocalizationManager.Instance.CurrentLanguage; }
        set { LocalizationManager.Instance.CurrentLanguage = value; }
    }

    private void OnLoadLocalizationSuccessCallback()
    {
        if (m_LoadLocalizedAssetCompleteCallback != null) {
            m_LoadLocalizedAssetCompleteCallback.Call();
        }
    }

    private void OnLoadLocalizationFailureCallback(string localizedAssetName, string errMessage)
    {
        if (m_LoadLocalizedAssetFailureCallback != null) {
            m_LoadLocalizedAssetFailureCallback.Call(localizedAssetName, errMessage);
        }
    }
}
