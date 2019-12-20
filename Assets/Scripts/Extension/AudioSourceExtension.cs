using UnityEngine;

public static class AudioSourceExtension 
{
    public static void SetClip(this AudioSource audioSource, string assetName)
    {
        ResourceManager.Instance.LoadAsset(assetName, typeof(AudioClip), (string name, object asset)=>{
            if (asset != null) {
                audioSource.clip = (AudioClip)asset;
            }
        });
    }
}
