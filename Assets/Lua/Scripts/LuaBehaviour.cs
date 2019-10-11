using UnityEngine;
using LuaInterface;

public partial class LuaBehaviour : MonoBehaviour
{
    [SerializeField]
    string m_LuaFile;
    [SerializeField]
    ComponentBinding[] m_Components;

    protected LuaFunction m_LifeCycleFunction = null;

    protected virtual void Awake()
    {
        InitializeComponentInfos();

        // LuaManager.Instance.Initialize();
        LuaManager.Instance.DoFile(m_LuaFile);

        Call("Extend", this);
        CallAwake();
    }

    protected virtual void Start()
    {
        CallLifecycle("Start");
    }

    protected virtual void OnEnable()
    {
        CallLifecycle("OnEnable");
    }

    protected virtual void OnDisable()
    {
        CallLifecycle("OnDisable");
    }

    protected virtual void OnDestroy()
    {
        CallLifecycle("OnDestroy");
    }

    protected void CallLifecycle(string functionName)
    {
        if (m_LifeCycleFunction != null) {
            m_LifeCycleFunction.Call(functionName);
        }
    }

    protected void CallLifecycle<T1>(string functionName, T1 arg1)
    {
        if (m_LifeCycleFunction != null) {
            m_LifeCycleFunction.Call(functionName, arg1);
        }
    }

    protected void CallLifecycle<T1, T2>(string functionName, T1 arg1, T2 arg2)
    {
        if (m_LifeCycleFunction != null) {
            m_LifeCycleFunction.Call(functionName, arg1, arg2);
        }
    }

    protected void Call<T1>(string functionName, T1 arg1)
    {
        LuaManager.Instance.CallFunction(string.Format("{0}.{1}", m_LuaFile, functionName), arg1);
    }

    private void CallAwake()
    {
        if (m_Components.Length > 0) {
            LuaTable tt = LuaManager.Instance.GetTemporaryTable();
            for (int i = 0; i < m_Components.Length; ++i) {
                var cmpt = m_Components[i];
                if (cmpt.transform != null && !string.IsNullOrEmpty(cmpt.name)) {
                    tt[cmpt.name] = cmpt.transform.GetComponent(GetComponetType(cmpt.type));
                }
            }
            CallLifecycle("Awake", tt);
            LuaManager.Instance.ReleaseTemporaryTable(tt);
        }
        else {
            CallLifecycle("Awake");
        }
    }
    
    public void AddEventHandler(LuaFunction function)
    {
        if (function == null || m_LifeCycleFunction == function) {
            return;
        }

        if (m_LifeCycleFunction != null) {
            m_LifeCycleFunction.Dispose();
            m_LifeCycleFunction = null;
        }
        m_LifeCycleFunction = function;
    }
}