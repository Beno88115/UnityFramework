using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LuaInterface;

public class UILuaWindow : UIWindow 
{
    [SerializeField]
    Button btnCallFunc;
    [SerializeField]
    Button btnClose;
    [SerializeField]
    Button btnAccessVariable;

    LuaState m_lua = null;

    private Dictionary<string, LuaFunction> buttons = new Dictionary<string, LuaFunction>();

    void Awake()
    {
        // LuaBehaviour
        // btnCallFunc.onClick.AddListener(OnCallFuncButtonClicked);
        // btnClose.onClick.AddListener(OnCloseButtonClicked);
        // btnAccessVariable.onClick.AddListener(OnAccessVariableButtonClicked);

        m_lua = new LuaState();
        m_lua.Start();
        LuaBinder.Bind(m_lua);
        DelegateFactory.Init(); 

        string fullPath = Application.dataPath + "/LuaFramework/Lua/";
        m_lua.AddSearchPath(fullPath);

        // // 设置xxx变量
        // m_lua["xxx"] = "fdfddd";
        // m_lua.DoFile("Main");

        m_lua.DoFile("UILuaCtrl");

        var func = m_lua.GetFunction("UILuaCtrl.Awake");
        func.Call(gameObject, btnClose);
    }

    public void AddClick(Button go, LuaFunction luafunc) {
        if (go == null || luafunc == null) return;
        buttons.Add(go.name, luafunc);
        go.GetComponent<Button>().onClick.AddListener(
            delegate() {
                luafunc.Call(go);
            }
        );
    }

    public void RemoveClick(GameObject go) {
        if (go == null) return;
        LuaFunction luafunc = null;
        if (buttons.TryGetValue(go.name, out luafunc)) {
            luafunc.Dispose();
            luafunc = null;
            buttons.Remove(go.name);
        }
    }

    public override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        // 设置xxx变量
        // m_lua["xxx"] = "fdfddd";
        // m_lua.DoFile("Main");

        var func = m_lua.GetFunction("UILuaCtrl.Start");
        func.Call();
    }

    void OnCallFuncButtonClicked()
    {
        LuaFunction lf = m_lua.GetFunction("Main");
        lf.Call();

        m_lua.Collect();
        m_lua.CheckTop();
    }

    void OnAccessVariableButtonClicked()
    {
        Debug.LogFormat("a: {0}, b: {1}, c: {2}", m_lua["a"], m_lua["b"], m_lua["c"]);
        Debug.Log("t: " + m_lua["t"]);
        
        LuaTable lt = m_lua.GetTable("t");
        Debug.LogFormat("t.name: {0}, t.age: {1}", lt["name"], lt["age"]);
    }

    void OnCloseButtonClicked()
    {
        UIManager.Instance.PopWindow(this.SerialId);
    }

    public override void OnClose(object userData)
    {
        m_lua.Dispose();

        base.OnClose(userData);
    }
}
