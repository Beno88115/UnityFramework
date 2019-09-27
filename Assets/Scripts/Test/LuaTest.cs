using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;

public class LuaTest : MonoBehaviour
{
    LuaState m_lua = null;

    void Start()
    {
        m_lua = new LuaState();                
        m_lua.Start();        
        string fullPath = Application.dataPath + "/LuaFramework/Lua/";
        m_lua.AddSearchPath(fullPath);
    }
}
