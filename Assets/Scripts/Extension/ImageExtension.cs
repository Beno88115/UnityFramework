using UnityEngine;
using UnityEngine.UI;

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

    public static void SetTexture(this RawImage image, string assetName)
    {
        ResourceManager.Instance.LoadAsset(assetName, typeof(Texture2D), (object asset)=>{
            if (asset != null) {
                image.texture = (Texture2D)asset;
            }
        });
    }
}
