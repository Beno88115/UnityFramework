using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;

public class UIManager : SingletonMono<UIManager> 
{
    private static readonly string kCanvasName = "UICanvas";

    private GameFramework.UI.IUIManager m_UIModule = null;
    private Dictionary<string, UIGroup> m_Groups = new Dictionary<string, UIGroup>();
    private Canvas m_Canvas = null;

    protected override void Awake()
    {
        base.Awake();
        InitCanvas();

        this.m_UIModule = GameFrameworkEntry.GetModule<GameFramework.UI.IUIManager>();
        this.m_UIModule.SetUIWindowHelper(new UIWindowHelper(m_Groups));
        this.m_UIModule.SetResourceManager(GameFrameworkEntry.GetModule<GameFramework.Resource.IResourceManager>());
        this.m_UIModule.SetObjectPoolManager(GameFrameworkEntry.GetModule<GameFramework.ObjectPool.IObjectPoolManager>());

        this.m_UIModule.OpenUIWindowSuccess += this.OnOpenFormSuccess;
        this.m_UIModule.OpenUIWindowFailure += this.OnOpenFormFailure;
        this.m_UIModule.OpenUIWindowUpdate += this.OnOpenFormUpdate;
        this.m_UIModule.OpenUIWindowDependencyAsset += this.OnOpenFormDependencyAsset;
        this.m_UIModule.CloseUIWindowComplete += this.OnCloseFormComplete;

        InitGroups();
    }

    private void OnDestroy()
    {
        this.m_UIModule.OpenUIWindowSuccess -= this.OnOpenFormSuccess;
        this.m_UIModule.OpenUIWindowFailure -= this.OnOpenFormFailure;
        this.m_UIModule.OpenUIWindowUpdate -= this.OnOpenFormUpdate;
        this.m_UIModule.OpenUIWindowDependencyAsset -= this.OnOpenFormDependencyAsset;
        this.m_UIModule.CloseUIWindowComplete -= this.OnCloseFormComplete;
    }

    public void Initialize()
    {
        
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
        if (m_UIModule.HasUIGroup(groupName))
        {
            return false;
        }
        
        if (m_UIModule.AddUIGroup(groupName, depth, new UIGroupHelper()))
        {
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

    public int PushForm(string assetName)
    {
        return PushForm(assetName, true, null);
    }

    public int PushForm(string assetName, bool pauseCoveredUIWindow)
    {
        return PushForm(assetName, pauseCoveredUIWindow, null);
    }

    public int PushForm(string assetName, object userData)
    {
        return PushForm(assetName, true, userData);
    }

    public int PushForm(string assetName, bool pauseCoveredUIWindow, object userData)
    {
        return Push(assetName, UIGroupType.UIWindow, pauseCoveredUIWindow, userData);
    }

    public int PushDialog(string assetName)
    {
        return PushDialog(assetName, true, null);
    }

    public int PushDialog(string assetName, bool pauseCoveredUIWindow)
    {
        return PushDialog(assetName, pauseCoveredUIWindow, null);
    }

    public int PushDialog(string assetName, object userData)
    {
        return PushDialog(assetName, true, userData);
    }

    public int PushDialog(string assetName, bool pauseCoveredUIWindow, object userData)
    {
        return Push(assetName, UIGroupType.UIDialog, pauseCoveredUIWindow, userData);
    }

    private int Push(string assetName, UIGroupType groupType, bool pauseCoveredUIWindow, object userData)
    {
        return m_UIModule.OpenUIWindow(assetName, Utility.Enum.GetString<UIGroupType>(groupType), pauseCoveredUIWindow, userData);
    }

    private void OnOpenFormSuccess(object sender, GameFramework.UI.OpenUIWindowSuccessEventArgs e)
    {
        Debug.Log("====================success");
    }

    private void OnOpenFormFailure(object sender, GameFramework.UI.OpenUIWindowFailureEventArgs e)
    {
        Debug.Log("====================failure");
    }

    private void OnOpenFormUpdate(object sender, GameFramework.UI.OpenUIWindowUpdateEventArgs e)
    {
    }

    private void OnOpenFormDependencyAsset(object sender, GameFramework.UI.OpenUIWindowDependencyAssetEventArgs e)
    {
    }

    private void OnCloseFormComplete(object sender, GameFramework.UI.CloseUIWindowCompleteEventArgs e)
    {
        Debug.Log("====================close");
    }

    protected override bool IsGlobalScope
    {
        get { return true; }
    }
}
