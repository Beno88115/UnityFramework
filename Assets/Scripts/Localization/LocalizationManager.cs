using UnityEngine;
using GameFramework;

public partial class LocalizationManager : SingletonMono<LocalizationManager>
{
    private static readonly string kLocalizationPrefsKey = "Localization.LocaleCode";

    private GameFramework.Localization.ILocalizationModule m_LocalizationModule;
    private GameFramework.Setting.ISettingModule m_SettingModule;

    private LoadLocalizedAssetCompleteCallback m_LoadLocalizedAssetCompleteCallback;
    private LoadLocalizedAssetFailureCallback m_LoadLocalizedAssetFailureCallback;

    private GameFramework.Localization.Language m_Language;

    public void Initialize()
    {
        this.m_LocalizationModule = GameFrameworkEntry.GetModule<GameFramework.Localization.ILocalizationModule>();
        this.m_LocalizationModule.SetLocalizationHelper(new LocalizationHelper());
        this.m_LocalizationModule.SetResourceModule(GameFrameworkEntry.GetModule<GameFramework.Resource.IResourceModule>());

        this.m_SettingModule = GameFrameworkEntry.GetModule<GameFramework.Setting.ISettingModule>();

        this.m_LocalizationModule.LoadDictionarySuccess += OnLoadDictionarySuccess;
        this.m_LocalizationModule.LoadDictionaryFailure += OnLoadDictionaryFailure;
        this.m_LocalizationModule.LoadDictionaryUpdate += OnLoadDictionaryUpdate;
        this.m_LocalizationModule.LoadDictionaryDependencyAsset += OnLoadDictionaryDependencyAsset;
    }

    public void Load(LoadLocalizedAssetCompleteCallback completeCallback, LoadLocalizedAssetFailureCallback failureCallback)
    {
        if (completeCallback == null || failureCallback == null)
            return;

        m_LoadLocalizedAssetCompleteCallback = completeCallback;
        m_LoadLocalizedAssetFailureCallback = failureCallback;
        
        string localizedAssetName = this.m_SettingModule.GetString(kLocalizationPrefsKey, Utility.Enum.GetString(this.m_LocalizationModule.SystemLanguage));
        Debug.Log(localizedAssetName);
        this.m_Language = Utility.Enum.GetEnum<GameFramework.Localization.Language>(localizedAssetName);
        this.m_LocalizationModule.LoadDictionary(localizedAssetName, LoadType.Text);
    }

    public string GetString(int key, params object[] args)
    {
        return this.m_LocalizationModule.GetString(key.ToString(), args);
    }

    public string GetString(Language key, params object[] args)
    {
        return GetString((int)key, args);
    }

    public string GetString(LocalizedSprite key, params object[] args)
    {
        return GetString((int)key, args);
    }

    public GameFramework.Localization.Language CurrentLanguage
    {
        get
        {
            return m_Language;
        }
        set
        {
            if (this.m_Language != value)
            {
                this.m_Language = value;
                this.m_SettingModule.SetString(kLocalizationPrefsKey, Utility.Enum.GetString(value));
            }
        }
    }

    private void OnLoadDictionarySuccess(object sender, GameFramework.Localization.LoadDictionarySuccessEventArgs e)
    {
        m_LoadLocalizedAssetCompleteCallback();
    }

    private void OnLoadDictionaryFailure(object sender, GameFramework.Localization.LoadDictionaryFailureEventArgs e)
    {
        Debug.LogErrorFormat("Load dictionary failure, asset name '{0}', error message '{1}'.", e.DictionaryAssetName, e.ErrorMessage);
        m_LoadLocalizedAssetFailureCallback(e.DictionaryAssetName, e.ErrorMessage);
    }

    private void OnLoadDictionaryUpdate(object sender, GameFramework.Localization.LoadDictionaryUpdateEventArgs e)
    {
    }

    private void OnLoadDictionaryDependencyAsset(object sender, GameFramework.Localization.LoadDictionaryDependencyAssetEventArgs e)
    {
    }
}
