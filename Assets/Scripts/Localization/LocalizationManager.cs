using UnityEngine;
using GameFramework;

public class LocalizationManager : SingletonMono<LocalizationManager>
{
    private GameFramework.Localization.ILocalizationManager m_LocalizationModule;

    protected override void Awake()
    {
        base.Awake();

        this.m_LocalizationModule = GameFrameworkEntry.GetModule<GameFramework.Localization.LocalizationManager>();
        this.m_LocalizationModule.SetLocalizationHelper(new LocalizationHelper());
        this.m_LocalizationModule.SetResourceManager(GameFrameworkEntry.GetModule<GameFramework.Resource.IResourceManager>());
    }
}
