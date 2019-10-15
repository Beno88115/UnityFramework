using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;

public class LuaManager : SingletonMono<LuaManager> 
{
    private LuaState m_LuaState;
    private LuaLooper m_LuaLooper;

    protected override void Awake()
    {
        base.Awake();

        new LuaResLoader();
        m_LuaState = new LuaState();
        m_LuaState.LuaSetTop(0);

        LuaBinder.Bind(m_LuaState);
        DelegateFactory.Init();

        m_LuaLooper = gameObject.AddComponent<LuaLooper>();
        m_LuaLooper.luaState = m_LuaState;

        m_LuaState.Start();
    }

    public void Initialize()
    {
        this.InitLuaPath();
        this.InitLuaLib();
    }

    private void InitLuaPath() 
    {
#if UNITY_EDITOR
        m_LuaState.AddSearchPath(Application.dataPath + "/Lua/Scripts/Lua");
        m_LuaState.AddSearchPath(Application.dataPath + "/Lua/Examples/Scripts");
#else
        m_LuaState.AddSearchPath(Application.streamingAssetsPath);
#endif
    }

    private void InitLuaLib()
    {
        m_LuaState.DoFile("Core/Functions");
        m_LuaState.DoFile("Core/Helper");
    }

    public void DoFile(string fileName) 
    {
        m_LuaState.DoFile(fileName);
    }

    public T DoFile<T>(string fileName)
    {
        return m_LuaState.DoFile<T>(fileName);
    }

    public void CallFunction<T1>(string funcName, T1 arg1)
    {
        LuaFunction func = m_LuaState.GetFunction(funcName);
        if (func != null) {
            func.Call(arg1);
        }
    }

    public void CallFunction<T1, T2>(string funcName, T1 arg1, T2 arg2)
    {
        LuaFunction func = m_LuaState.GetFunction(funcName);
        if (func != null) {
            func.Call(arg1, arg2);
        }
    }

    public LuaTable GetTable(string tableName)
    {
        return m_LuaState.GetTable(tableName);
    }

    public void LuaGC() 
    {
        m_LuaState.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
    }

    public void Close() 
    {
        m_LuaLooper.Destroy();
        m_LuaLooper = null;

        m_LuaState.Dispose();
        m_LuaState = null;
    }
}