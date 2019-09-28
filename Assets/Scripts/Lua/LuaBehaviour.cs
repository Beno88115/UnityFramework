using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using UnityEngine.UI;

public class LuaBehaviour : MonoBehaviour
{
    [SerializeField]
    private string m_LuaFile;
    [SerializeField]
    Button button;

    LuaState m_lua = null;

    void Awake()
    {
        m_lua = new LuaState();
        m_lua.Start();
        LuaBinder.Bind(m_lua);
        DelegateFactory.Init(); 

        string fullPath = Application.dataPath + "/LuaFramework/Lua/";
        m_lua.AddSearchPath(fullPath);
        m_lua.DoFile(m_LuaFile);
        
        LuaTable table = m_lua.GetTable("table");
        table["button"] = button;
        
        // CallMethod("Awake", gameObject, table);
        CallMethod("Create", this);
    }

    void Start()
    {
        CallMethod("Start", this);
    }

    void OnDestroy()
    {
        CallMethod("OnDestroy", this);
    }

    void CallMethod(string methodName)
    {
        var func = m_lua.GetFunction(string.Format("{0}.{1}", m_LuaFile, methodName));
        if (func != null) {
            func.Call();
        }
    }

    void CallMethod(string methodName, LuaBehaviour go)
    {
        var func = m_lua.GetFunction(string.Format("{0}.{1}", m_LuaFile, methodName));
        if (func != null) {
            func.Call(go);
        }
    }

    void CallMethod(string methodName, LuaBehaviour go, LuaTable lt)
    {
        var func = m_lua.GetFunction(string.Format("{0}.{1}", m_LuaFile, methodName));
        if (func != null) {
            func.Call(go, lt);
        }
    }

    public void Add(int a, int b)
    {
        Debug.LogFormat("a: {0}, b: {1}", a, b);
    }
}