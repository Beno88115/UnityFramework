using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        AppFacade.Instance.Initialize();
        UIManager.Instance.PushWindow("UILogin");

        //  UnityEditor.AssetDatabase.GetAllAssetPaths();
        // UnityEditor.AssetDatabase.GetAllAssetBundleNames();
        // UnityEditor.AssetDatabase.getm
        // var assetBundleNames = UnityEditor.AssetDatabase.GetAllAssetBundleNames();
        // for (int i = 0; i < assetBundleNames.Length; ++i)
        // {
        //     Debug.Log("assetBundle:" + assetBundleNames[i]);
        //     var assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleNames[i]);
        //     if (assetPaths != null) 
        //     {
        //         for (int j = 0; j < assetPaths.Length; ++j)
        //         {
        //             Debug.Log("========" + assetPaths[j]);
        //             // Debug.Log(", variant:" + UnityEditor.AssetDatabase.GetImplicitAssetBundleVariantName(assetBundleNames[i]));                    
        //         }
        //     }
        // }
    }
}
