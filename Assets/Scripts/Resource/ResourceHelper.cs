using GameFramework.Resource;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System;
using GameFramework;
#if UNITY_WEBGL && !UNITY_EDITOR
using UnityEngine.Networking;
using System.Collections;
#endif

public class ResourceHelper : MonoBehaviour, IResourceHelper 
{
    /// <summary>
    /// 直接从指定文件路径读取数据流。
    /// </summary>
    /// <param name="fileUri">文件路径。</param>
    /// <param name="loadBytesCallback">读取数据流回调函数。</param>
    public void LoadBytes(string fileUri, LoadBytesCallback loadBytesCallback)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        StartCoroutine(LoadBytesForWebGL(fileUri, loadBytesCallback));
#else
        if (!System.IO.File.Exists(fileUri)) {
            return;
        }
        
        string text = Utility.File.ReadAllText(fileUri);
        if (!string.IsNullOrEmpty(text)) {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
            if (loadBytesCallback != null) {
                loadBytesCallback.Invoke(fileUri, bytes, null);
            }
        }
#endif
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

#if UNITY_WEBGL && !UNITY_EDITOR
    IEnumerator LoadBytesForWebGL(string fileUri, LoadBytesCallback loadBytesCallback)
    {
        var req = UnityWebRequest.Get(fileUri);
        yield return req.SendWebRequest();

        if (req.isNetworkError) {
            if (loadBytesCallback != null) {
                loadBytesCallback.Invoke(fileUri, null, req.error);
            }
            yield break;
        }

        var text = DownloadHandlerBuffer.GetContent(req);
        Debug.Log(text);
        if (!string.IsNullOrEmpty(text)) {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
            if (loadBytesCallback != null) {
                loadBytesCallback.Invoke(text, bytes, null);
            }
        }
    }
#endif
}
