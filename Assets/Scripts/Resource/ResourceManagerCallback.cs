using System;

public partial class ResourceManager : SingletonMono<ResourceManager>
{
    public delegate void LoadAssetCompleteCallback(object assetObject);
}