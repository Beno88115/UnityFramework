using UnityEngine;
using LuaInterface;

public partial class LuaBehaviour : MonoBehaviour
{
    static readonly string AWAKE = "Awake";
    static readonly string START = "Start";
    static readonly string ONENABLE = "OnEnable";
    static readonly string ONDISABLE = "OnDisable";
    static readonly string ONDESTROY = "OnDestroy";

    [SerializeField]
    string m_LuaFile;
    [SerializeField]
    ComponentBinding[] m_Components;

    private LuaTable m_LuaTable = null;

    public string LuaComponentName { get { return m_LuaFile; } }
    public string LuaFile { get { return m_LuaFile; } set { m_LuaFile = value; } }

    protected virtual void Awake()
    {
        InitializeComponentInfos();

        RequireLuaComponent();
        ExtendLuaComponent();
        BindingComponentForLua();

        CallFunction(AWAKE, this);
    }

    protected virtual void Start()
    {
        CallFunction(START, this);
    }

    protected virtual void OnEnable()
    {
        CallFunction(ONENABLE, this);
    }

    protected virtual void OnDisable()
    {
        CallFunction(ONDISABLE, this);
    }

    protected virtual void OnDestroy()
    {
        if (m_LuaTable != null) {
            m_LuaTable.Dispose();
            m_LuaTable = null;
        }
        CallFunction(ONDESTROY, this);
    }

    protected void CallFunction<T1>(string functionName, T1 arg1)
    {
        if (m_LuaTable != null) {
            m_LuaTable.Call(functionName, arg1);
        }
    }

    protected void CallFunction<T1, T2>(string functionName, T1 arg1, T2 arg2)
    {
        if (m_LuaTable != null) {
            m_LuaTable.Call(functionName, arg1, arg2);
        }
    }

    protected void CallFunction<T1, T2, T3>(string functionName, T1 arg1, T2 arg2, T3 arg3)
    {
        if (m_LuaTable != null) {
            m_LuaTable.Call(functionName, arg1, arg2, arg3);
        }
    }

    /// <summary>
    /// 加载LUA脚本
    /// </summary>
    private void RequireLuaComponent()
    {
        m_LuaTable = LuaManager.Instance.Require(m_LuaFile);
        if (m_LuaTable == null) {
            Debug.LogErrorFormat("failed to load [{0}] lua file", m_LuaFile);
        }
    }

    /// <summary>
    /// 绑定LUA脚本
    /// </summary>
    private void ExtendLuaComponent()
    {
        LuaTable helper = LuaManager.Instance.GetTable("Helper");
        if (helper != null) {
            helper.Call("Extend", this, m_LuaTable);
        }
    }

    /// <summary>
    /// 将Inspector面板上绑定的控件映射到LUA脚本中
    /// </summary>
    private void BindingComponentForLua()
    {
        if (m_Components.Length > 0) {
            for (int i = 0; i < m_Components.Length; ++i) {
                var cmpt = m_Components[i];
                if (cmpt.transform != null && !string.IsNullOrEmpty(cmpt.name)) {
                    m_LuaTable[cmpt.name] = cmpt.transform.GetComponent(GetComponetType(cmpt.type));
                }
            }
        }
    }
}