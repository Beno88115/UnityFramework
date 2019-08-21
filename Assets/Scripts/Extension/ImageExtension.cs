using UnityEngine;
using UnityEngine.UI;
using GameFramework;
using GameFramework.Resource;

public static class ImageExtension 
{
    public static void SetSprite(this Image image, string assetName)
    {
        ResourceManager.Instance.LoadAsset(assetName, typeof(Sprite), (object asset)=>{
            if (asset != null) {
                image.sprite = (Sprite)asset;
            }
        });
    }
}
