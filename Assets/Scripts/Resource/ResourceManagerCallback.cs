using System;

public partial class ResourceManager : SingletonMono<ResourceManager>
{
    public delegate void InitResourcesCompleteCallback();
    public delegate void LoadAssetCompleteCallback(object assetObject);
}