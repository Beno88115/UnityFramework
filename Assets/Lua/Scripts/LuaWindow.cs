using UnityEngine;
using LuaInterface;
using GameFramework.UI;

[DisallowMultipleComponent]
public class LuaWindow : LuaBehaviour, IUIWindow
{
    private LuaFunction m_UILifeCycleFunction = null;

    /// <summary>
    /// 获取界面序列编号。
    /// </summary>
    public int SerialId
    {
        get;
        private set;
    }

    /// <summary>
    /// 获取界面资源名称。
    /// </summary>
    public string UIWindowAssetName
    {
        get;
        private set;
    }

    /// <summary>
    /// 获取界面实例。
    /// </summary>
    public object Handle
    {
        get;
        private set;
    }

    /// <summary>
    /// 获取界面所属的界面组。
    /// </summary>
    public IUIGroup UIGroup
    {
        get;
        private set;
    }

    /// <summary>
    /// 获取界面在界面组中的深度。
    /// </summary>
    public int DepthInUIGroup
    {
        get;
        private set;
    }

    /// <summary>
    /// 获取是否暂停被覆盖的界面。
    /// </summary>
    public bool PauseCoveredUIWindow
    {
        get;
        private set;
    }

    /// <summary>
    /// 初始化界面。
    /// </summary>
    /// <param name="serialId">界面序列编号。</param>
    /// <param name="uiWindowAssetName">界面资源名称。</param>
    /// <param name="uiGroup">界面所属的界面组。</param>
    /// <param name="pauseCoveredUIWindow">是否暂停被覆盖的界面。</param>
    /// <param name="isNewInstance">是否是新实例。</param>
    /// <param name="userData">用户自定义数据。</param>
    public virtual void OnInit(int serialId, string uiWindowAssetName, IUIGroup uiGroup, bool pauseCoveredUIWindow, bool isNewInstance, object userData)
    {
        SerialId = serialId;
        UIGroup = uiGroup;
        PauseCoveredUIWindow = pauseCoveredUIWindow;
        UIWindowAssetName = uiWindowAssetName;
        Handle = gameObject;

        CallLifecycle("OnInit", isNewInstance, (LuaTable)userData);
    }

    /// <summary>
    /// 界面回收。
    /// </summary>
    public virtual void OnRecycle()
    {
        CallLifecycle("OnRecycle");
    }

    /// <summary>
    /// 界面打开。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    public virtual void OnOpen(object userData)
    {
        CallLifecycle("OnOpen", (LuaTable)userData);
    }

    /// <summary>
    /// 界面关闭。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    public virtual void OnClose(object userData)
    {
        CallLifecycle("OnClose", (LuaTable)userData);
    }

    /// <summary>
    /// 界面暂停。
    /// </summary>
    public virtual void OnPause()
    {
        if (this != null) {
            ((RectTransform)transform).anchoredPosition = new Vector2(0, 5000);
            CallLifecycle("OnPause");
        }
    }

    /// <summary>
    /// 界面暂停恢复。
    /// </summary>
    public virtual void OnResume()
    {
        if (this != null) {
            ((RectTransform)transform).anchoredPosition = Vector2.zero;
            CallLifecycle("OnResume");
        }
    }

    /// <summary>
    /// 界面遮挡。
    /// </summary>
    public virtual void OnCover()
    {
        CallLifecycle("OnCover");
    }

    /// <summary>
    /// 界面遮挡恢复。
    /// </summary>
    public virtual void OnReveal()
    {
        CallLifecycle("OnReveal");
    }

    /// <summary>
    /// 界面激活。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    public virtual void OnRefocus(object userData)
    {
        CallLifecycle("OnRefocus", (LuaTable)userData);
    }

    /// <summary>
    /// 界面轮询。
    /// </summary>
    /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
    /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
    public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
    }

    /// <summary>
    /// 界面深度改变。
    /// </summary>
    /// <param name="uiGroupDepth">界面组深度。</param>
    /// <param name="depthInUIGroup">界面在界面组中的深度。</param>
    public virtual void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
    {
        DepthInUIGroup = depthInUIGroup;
    }
}