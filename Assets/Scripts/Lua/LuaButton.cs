using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LuaInterface;

public class LuaButton : Button 
{
    private LuaFunction m_ClickFunc;

    public void AddClick(LuaFunction function)
    {
        if (function == null) {
            return;
        }

        m_ClickFunc = function;
        this.onClick.AddListener(()=>{
            m_ClickFunc.Call();
        });
    }
}
