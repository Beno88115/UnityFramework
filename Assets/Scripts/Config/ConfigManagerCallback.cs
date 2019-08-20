using System;

public partial class ConfigManager : SingletonMono<ConfigManager>
{
    public delegate void LoadConfigsCompleteCallback();
    public delegate void LoadConfigsFailureCallback(string configTableAssetName, string errorMessage);
    public delegate void LoadConfigsProgressCallback(string configTableAssetName);
}
