using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;
using GameFramework.UI;
using GameFramework.Resource;
using GameFramework.ObjectPool;

public class UIManager : SingletonMono<UIManager> 
{
    private static readonly string kCanvasName = "UICanvas";

    private IUIModule m_UIModule = null;
    private Dictionary<string, UIGroup> m_Groups = new Dictionary<string, UIGroup>();
    private Canvas m_Canvas = null;

    protected override void Awake()
    {
        base.Awake();
        InitCanvas();
    }

    public void Initialize()
    {
        this.m_UIModule = GameFrameworkEntry.GetModule<IUIModule>();
        this.m_UIModule.SetUIWindowHelper(new UIWindowHelper(m_Groups));
        this.m_UIModule.SetResourceModule(GameFrameworkEntry.GetModule<IResourceModule>());
        this.m_UIModule.SetObjectPoolModule(GameFrameworkEntry.GetModule<IObjectPoolModule>());

        this.m_UIModule.OpenUIWindowSuccess += this.OnOpenWindowSuccess;
        this.m_UIModule.OpenUIWindowFailure += this.OnOpenWindowFailure;
        this.m_UIModule.OpenUIWindowUpdate += this.OnOpenWindowUpdate;
        this.m_UIModule.OpenUIWindowDependencyAsset += this.OnOpenWindowDependencyAsset;
        this.m_UIModule.CloseUIWindowComplete += this.OnCloseWindowComplete;

        InitGroups();
    }

    private void OnDestroy()
    {
        this.m_UIModule.OpenUIWindowSuccess -= this.OnOpenWindowSuccess;
        this.m_UIModule.OpenUIWindowFailure -= this.OnOpenWindowFailure;
        this.m_UIModule.OpenUIWindowUpdate -= this.OnOpenWindowUpdate;
        this.m_UIModule.OpenUIWindowDependencyAsset -= this.OnOpenWindowDependencyAsset;
        this.m_UIModule.CloseUIWindowComplete -= this.OnCloseWindowComplete;
    }

    private void InitCanvas()
    {
        GameObject ui = GameObject.Find(UIManager.kCanvasName);
        if (ui == null) {
            ui = Utility.UI.CreateCanvas();
            ui.name = UIManager.kCanvasName;
        }
        m_Canvas = ui.GetComponent<Canvas>();
    }

    private void InitGroups()
    {
        for (int i = (int)UIGroupType.UIWindow; i < (int)UIGroupType.Count; ++i) 
        {
            AddGroup(Utility.Enum.GetString<UIGroupType>((UIGroupType)i), i);
        }
    }

    private bool AddGroup(string groupName, int depth)
    {
        if (m_UIModule.HasUIGroup(groupName)) {
            return false;
        }
        
        if (m_UIModule.AddUIGroup(groupName, depth, new UIGroupHelper())) {
            GameObject group = new GameObject(groupName);
            var uiGroup = group.AddComponent<UIGroup>();
            uiGroup.Name = groupName;

            RectTransform rt = group.AddComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            rt.pivot = Vector2.one * 0.5f;

            group.transform.SetParent(m_Canvas.transform, false);
            group.transform.localPosition = Vector3.zero;
            group.transform.localScale = Vector3.one;
            group.transform.localRotation = Quaternion.identity;

            var canvas = group.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = depth;
            canvas.gameObject.layer = LayerMask.NameToLayer("UI");

            group.AddComponent<GraphicRaycaster>();
            group.AddComponent<CanvasGroup>();

            m_Groups[groupName] = uiGroup;

            return true;
        }
        return false;
    }

    public int PushWindow(string assetName, UIGroupType groupType = UIGroupType.UIWindow)
    {
        return PushWindow(assetName, true, null, groupType);
    }

    public int PushWindow(string assetName, bool pauseCoveredUIWindow, UIGroupType groupType = UIGroupType.UIWindow)
    {
        return PushWindow(assetName, pauseCoveredUIWindow, null, groupType);
    }

    public int PushWindow(string assetName, object userData, UIGroupType groupType = UIGroupType.UIWindow)
    {
        return PushWindow(assetName, true, userData, groupType);
    }

    private int PushWindow(string assetName, bool pauseCoveredUIWindow, object userData, UIGroupType groupType = UIGroupType.UIWindow)
    {
        return m_UIModule.OpenUIWindow(assetName, Utility.Enum.GetString<UIGroupType>(groupType), pauseCoveredUIWindow, userData);
    }

    public void PopWindow(int serialId)
    {
        m_UIModule.CloseUIWindow(serialId);
    }

    public void PopWindow(int serialId, object userData)
    {
        m_UIModule.CloseUIWindow(serialId, userData);
    }

    public void PopDialog(int serialId)
    {
        this.PopWindow(serialId);
    }

    public void PopDialog(int serialId, object userData)
    {
        this.PopWindow(serialId, userData);
    }

    private void OnOpenWindowSuccess(object sender, OpenUIWindowSuccessEventArgs e)
    {
        Debug.LogFormat("OnOpenWindowSuccess[{0}]", e.UIWindow.UIWindowAssetName);
    }

    private void OnOpenWindowFailure(object sender, OpenUIWindowFailureEventArgs e)
    {
        Debug.LogFormat("OnOpenWindowFailure[{0}]", e.UIWindowAssetName);
    }

    private void OnOpenWindowUpdate(object sender, OpenUIWindowUpdateEventArgs e)
    {
        Debug.LogFormat("OnOpenWindowUpdate[{0}]", e.UIWindowAssetName);
    }

    private void OnOpenWindowDependencyAsset(object sender, OpenUIWindowDependencyAssetEventArgs e)
    {
        Debug.LogFormat("OnOpenWindowDependencyAsset[{0}]", e.DependencyAssetName);
    }

    private void OnCloseWindowComplete(object sender, CloseUIWindowCompleteEventArgs e)
    {
        Debug.LogFormat("OnCloseWindowComplete[{0}]", e.UIWindowAssetName);
    }

    protected override bool IsGlobalScope
    {
        get { return true; }
    }
}
