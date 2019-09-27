using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;

public class LuaBehaviour : MonoBehaviour
{
    [SerializeField]
    private string m_LuaFile;

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
        
        CallMethod("Awake", gameObject);
    }

    void Start()
    {
        CallMethod("Start");
    }

    void OnDestroy()
    {
        CallMethod("OnDestroy");
    }

    void CallMethod(string methodName)
    {
        var func = m_lua.GetFunction(string.Format("{0}.{1}", m_LuaFile, methodName));
        if (func != null) {
            func.Call();
        }
    }

    void CallMethod(string methodName, GameObject go)
    {
        var func = m_lua.GetFunction(string.Format("{0}.{1}", m_LuaFile, methodName));
        if (func != null) {
            func.Call(go);
        }
    }
}