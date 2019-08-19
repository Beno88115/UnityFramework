using UnityEngine;
using GameFramework;

public class LocalizationManager : SingletonMono<LocalizationManager>
{
    private GameFramework.Localization.ILocalizationModule m_LocalizationModule;

    protected override void Awake()
    {
        base.Awake();

        this.m_LocalizationModule = GameFrameworkEntry.GetModule<GameFramework.Localization.LocalizationModule>();
        this.m_LocalizationModule.SetLocalizationHelper(new LocalizationHelper());
        this.m_LocalizationModule.SetResourceModule(GameFrameworkEntry.GetModule<GameFramework.Resource.IResourceModule>());
    }
}
