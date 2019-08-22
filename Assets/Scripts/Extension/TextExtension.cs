using UnityEngine;
using UnityEngine.UI;

public static class TextExtension 
{
    public static void SetFont(this Text text, string assetName)
    {
        ResourceManager.Instance.LoadAsset(assetName, typeof(Font), (object asset)=>{
            if (asset != null) {
                text.font = (Font)asset;
            }
        });
    }
}
