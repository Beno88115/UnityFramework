using System;

public partial class LocalizationManager : SingletonMono<LocalizationManager>
{
    public delegate void LoadLocalizedAssetCompleteCallback();
    public delegate void LoadLocalizedAssetFailureCallback(string localizedAssetName, string errorMessage);
}