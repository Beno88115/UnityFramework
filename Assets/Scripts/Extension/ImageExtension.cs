using UnityEngine;
using UnityEngine.UI;

public static class ImageExtension 
{
    private static readonly ResourceManager s_ResMgr = ResourceManager.Instance;

    public static void SetSprite(this Image image, string assetName)
    {
        s_ResMgr.LoadAsset(assetName, typeof(Sprite), (string name, object asset)=>{
            if (asset != null) {
                var assetType = asset.GetType();
                if (assetType == typeof(Sprite)) {
                    image.sprite = (Sprite)asset;
                }
                else if (assetType == typeof(Texture2D)) {
                    var texture = (Texture2D)asset;
                    image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                }
            }
        });
    }

    public static void SetTexture(this RawImage image, string assetName)
    {
        s_ResMgr.LoadAsset(assetName, typeof(Texture2D), (string name, object asset)=>{
            if (asset != null) {
                image.texture = (Texture2D)asset;
            }
        });
    }
}
