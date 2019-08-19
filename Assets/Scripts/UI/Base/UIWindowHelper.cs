using GameFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIWindowHelper : IUIWindowHelper 
{
    private Dictionary<string, UIGroup> m_Groups = null;

    public UIWindowHelper(Dictionary<string, UIGroup> groups)
    {
        this.m_Groups = groups;
    }

    /// <summary>
    /// 实例化界面。
    /// </summary>
    /// <param name="uiWindowAsset">要实例化的界面资源。</param>
    /// <returns>实例化后的界面。</returns>
    public object InstantiateUIWindow(object uiWindowAsset)
    {
        return UnityEngine.GameObject.Instantiate(uiWindowAsset as UnityEngine.Object);
    }

    /// <summary>
    /// 创建界面。
    /// </summary>
    /// <param name="uiWindowInstance">界面实例。</param>
    /// <param name="uiGroup">界面所属的界面组。</param>
    /// <param name="userData">用户自定义数据。</param>
    /// <returns>界面。</returns>
    public IUIWindow CreateUIWindow(object uiWindowInstance, IUIGroup uiGroup, object userData)
    {
        var ui = uiWindowInstance as GameObject;
        var window = ui.GetComponent<UIWindow>(); 
        window.transform.SetParent(m_Groups[uiGroup.Name].transform, false);
        return window;
    }

    /// <summary>
    /// 释放界面。
    /// </summary>
    /// <param name="uiWindowAsset">要释放的界面资源。</param>
    /// <param name="uiWindowInstance">要释放的界面实例。</param>
    public void ReleaseUIWindow(object uiWindowAsset, object uiWindowInstance)
    {
    }
}
