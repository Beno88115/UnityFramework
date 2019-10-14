using UnityEngine;
using System;
using LuaInterface;
using System.Collections.Generic;

public class LuaExtendHelper 
{
    public static readonly string AWAKE = "Awake";
    public static readonly string START = "Start";
    public static readonly string ONENABLE = "OnEnable";
    public static readonly string ONDISABLE = "OnDisable";
    public static readonly string ONDESTROY = "OnDestroy";

    public static readonly string ONINIT = "OnInit";
    public static readonly string ONOPEN = "OnOpen";
    public static readonly string ONRECYCLE = "OnRecycle";
    public static readonly string ONPAUSE = "OnPause";
    public static readonly string ONRESUME = "OnResume";
    public static readonly string ONCOVER = "OnCover";
    public static readonly string ONREVEAL = "OnReveal";
    public static readonly string ONREFOCUS = "OnRefocus";
    public static readonly string ONCLOSE = "OnClose";

    public enum ComponentType
    {
        GameObject = 0,

        Transform = 1,
        Button = 2,
        Text = 3,
        Image = 4,

        TableView = 1000,
        TableViewCell = 1001,
    }

    [Serializable]
    public class ComponentBinding
    {
        public string name;
        public GameObject gameObject;
        public ComponentType component;
    }

    private static Dictionary<ComponentType, Type> m_Types = new Dictionary<ComponentType, Type>();

    private LuaTable m_LuaTable = null;
    private MonoBehaviour m_Target = null;
    private string m_LuaFile = null;
    private ComponentBinding[] m_Components = null;

    public LuaExtendHelper(string luafile, ComponentBinding[] components, MonoBehaviour target)
    {
        m_LuaFile = luafile;
        m_Components = components;
        m_Target = target;

        InitializeComponentInfos();

        RequireLuaComponent();
        ExtendLuaComponent();
        BindingComponentForLua();
    }

    private void InitializeComponentInfos()
    {
        if (m_Types.Count == 0) {
            m_Types.Add(ComponentType.Text, typeof(UnityEngine.UI.Text));
            m_Types.Add(ComponentType.Button, typeof(UnityEngine.UI.Button));
            m_Types.Add(ComponentType.Image, typeof(UnityEngine.UI.Image));
            m_Types.Add(ComponentType.Transform, typeof(UnityEngine.Transform));
            m_Types.Add(ComponentType.TableView, typeof(LuaTableView));
            m_Types.Add(ComponentType.TableViewCell, typeof(LuaTableViewCell));
        }
    }

    private Type GetComponetType(ComponentType type)
    {
        if (m_Types.ContainsKey(type)) {
            return m_Types[type];
        }
        return m_Types[ComponentType.Transform];
    }

    public void CallFunction(string functionName)
    {
        if (m_LuaTable != null) {
            m_LuaTable.Call(functionName, m_Target);
        }
    }

    public void CallFunction<T1>(string functionName, T1 arg1)
    {
        if (m_LuaTable != null) {
            m_LuaTable.Call(functionName, m_Target, arg1);
        }
    }

    public void CallFunction<T1, T2>(string functionName, T1 arg1, T2 arg2)
    {
        if (m_LuaTable != null) {
            m_LuaTable.Call(functionName, m_Target, arg1, arg2);
        }
    }

    public void CallFunction<T1, T2, T3>(string functionName, T1 arg1, T2 arg2, T3 arg3)
    {
        if (m_LuaTable != null) {
            m_LuaTable.Call(functionName, m_Target, arg1, arg2, arg3);
        }
    }

    public void Dispose()
    {
        if (m_LuaTable != null) {
            m_LuaTable.Dispose();
            m_LuaTable = null;
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
            helper.Call("Extend", m_Target, m_LuaTable);
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
                if (cmpt.gameObject != null && !string.IsNullOrEmpty(cmpt.name)) {
                    if (cmpt.component == ComponentType.GameObject) {
                        m_LuaTable[cmpt.name] = cmpt.gameObject;
                    }
                    else {
                        m_LuaTable[cmpt.name] = cmpt.gameObject.GetComponent(GetComponetType(cmpt.component));
                    }
                }
            }
        }
    }
}
