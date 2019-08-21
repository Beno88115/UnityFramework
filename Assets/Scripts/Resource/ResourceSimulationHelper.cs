#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using GameFramework.Resource;

public class ResourceSimulationHelper : IResourceSimulationHelper 
{
    /// <summary>
    /// 直接从指定文件路径读取资源对象。
    /// </summary>
    /// <param name="assetName">资源名称。</param>
    /// <param name="resourceName">资源包名称。</param>
    public object LoadObject(string assetName, string resourceName, Type type)
    {
#if UNITY_EDITOR
        string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(resourceName, assetName);
        if (assetPaths.Length != 0) {
            for (int i = 0; i < assetPaths.Length; ++i) {
                Object target = AssetDatabase.LoadAssetAtPath(assetPaths[i], type ?? typeof(Object));
                if (target != null) {
                    return target;
                }
            }
        }
#endif
        return null;
    }

    /// <summary>
    /// 获取所有的资源包名称。
    /// </summary>
    public string[] GetAllResourceNames()
    {
        string[] retValue = null;
#if UNITY_EDITOR
        retValue = UnityEditor.AssetDatabase.GetAllAssetBundleNames();
#endif
        return retValue;
    }

    /// <summary>
    /// 通过资源包名称获取所有的资源路径。
    /// </summary>
    /// <param name="resourceName">资源包名称。</param>
    public string[] GetAssetPaths(string resourceName)
    {
        string[] retValue = null;
#if UNITY_EDITOR
        retValue = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle(resourceName);
#endif
        return retValue;
    }

    /// <summary>
    /// 获取资源的变体。
    /// </summary>
    /// <param name="assetName">资源名称。</param>
    public string GetVariantFromAssetName(string assetName)
    {
        string retValue = string.Empty;
#if UNITY_EDITOR
        retValue = UnityEditor.AssetDatabase.GetImplicitAssetBundleVariantName(assetName);
#endif
        return retValue;
    }
}
