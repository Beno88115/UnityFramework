using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using UnityEngine.UI;
using System;

public class LuaBehaviour : MonoBehaviour
{
    [Serializable]
    public class Widget
    {
        public string name;
        public Transform widget;
        public WidgetType widgetType;
    }

    public enum WidgetType
    {
        TRANSFORM,
        BUTTON,
        TEXT,
    }

    [SerializeField]
    private string m_LuaFile;
    [SerializeField]
    Widget[] m_Widgets;
    // [SerializeField]
    // Button button;

    LuaState m_lua = null;
    LuaFunction m_function = null;
    Dictionary<WidgetType, Type> m_Types = new Dictionary<WidgetType, Type>();

    void Awake()
    {
        m_Types.Add(WidgetType.TRANSFORM, typeof(UnityEngine.Transform));
        m_Types.Add(WidgetType.BUTTON, typeof(LuaButton));
        m_Types.Add(WidgetType.TEXT, typeof(LuaText));

        new LuaResLoader();
        m_lua = new LuaState();
        m_lua.Start();
        LuaBinder.Bind(m_lua);
        DelegateFactory.Init();

        m_lua.AddSearchPath(Application.dataPath + "/Lua/Scripts/Lua");
        m_lua.DoFile("Core/Functions");
        m_lua.DoFile("Core/Helper");
        // m_lua.DoFile("inspect");
        m_lua.DoFile(m_LuaFile);
        
        LuaTable table = m_lua.GetTable("table");
        for (int i = 0; i < m_Widgets.Length; ++i) {
            var widget = m_Widgets[i];
            table[widget.name] = widget.widget.GetComponent(m_Types[widget.widgetType]);
        }

        CallMethod("Extend", this);
        CallLifeMethod("Awake", this, table);
    }

    void Start()
    {
        CallLifeMethod("Start", this);
    }

    void OnDestroy()
    {
        CallLifeMethod("OnDestroy", this);
    }

    void OnEnable()
    {
        CallLifeMethod("OnEnable", this);
    }

    void OnDisable()
    {
        CallLifeMethod("OnDisable", this);
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

    void CallMethod(string methodName, LuaBehaviour go, LuaTable t)
    {
        var func = m_lua.GetFunction(string.Format("{0}.{1}", m_LuaFile, methodName));
        if (func != null) {
            func.Call(go, t);
        }
    }

    void CallMethod(string methodName, LuaTable go, LuaTable lt)
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

    public void RegisterEventHandler(LuaFunction function)
    {
        if (function != null && m_function != function) {
            m_function = function;
        }
    }

    void CallLifeMethod(string methodName, LuaBehaviour go)
    {
        if (m_function != null) {
            m_function.Call(methodName);
        }
    }

    void CallLifeMethod(string methodName, LuaBehaviour go, LuaTable table)
    {
        if (m_function != null) {
            m_function.Call(methodName, table);
        }
    }

    
}