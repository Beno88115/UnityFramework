using GameFramework.Resource;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System;

public class ResourceHelper : IResourceHelper 
{
    public string ManifestPath
    {
        get
        {
#if UNITY_EDITOR
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "iOS";
            case BuildTarget.WebGL:
                return "WebGL";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "Windows";
            case BuildTarget.StandaloneOSX:
                return "OSX";
            default:
                return null;
            }
#else
            switch (Application.platform)
            {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "iOS";
            case RuntimePlatform.WebGLPlayer:
                return "WebGL";
            case RuntimePlatform.WindowsPlayer:
                return "Windows";
            case RuntimePlatform.OSXPlayer:
                return "OSX";
            default:
                return null;
            }
#endif
        }
    }

    /// <summary>
    /// 直接从指定文件路径读取数据流。
    /// </summary>
    /// <param name="fileUri">文件路径。</param>
    /// <param name="loadBytesCallback">读取数据流回调函数。</param>
    public void LoadBytes(string fileUri, LoadBytesCallback loadBytesCallback)
    {
        fileUri = System.IO.Path.Combine(Application.streamingAssetsPath, "version.dat");
        UnityEngine.Debug.Log("==========fileuri:" + fileUri);
        if (!System.IO.File.Exists(fileUri)) {

        }
        
        string text = Utility.File.ReadAllText(fileUri);
        if (!string.IsNullOrEmpty(text)) {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
            if (loadBytesCallback != null) {
                loadBytesCallback.Invoke(fileUri, bytes, null);
            }
        }
    }

    /// <summary>
    /// 卸载场景。
    /// </summary>
    /// <param name="sceneAssetName">场景资源名称。</param>
    /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
    /// <param name="userData">用户自定义数据。</param>
    public void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
    {
    }

    /// <summary>
    /// 释放资源。
    /// </summary>
    /// <param name="objectToRelease">要释放的资源。</param>
    public void Release(object objectToRelease)
    {
    }
}
