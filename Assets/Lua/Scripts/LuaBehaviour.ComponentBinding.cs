using System;
using UnityEngine;
using System.Collections.Generic;

public partial class LuaBehaviour : MonoBehaviour
{
    private enum ComponentType
    {
        TRANSFORM,
        BUTTON,
        TEXT,
        TEXT2,
        BUTTON2,
    }

    [Serializable]
    private class ComponentBinding
    {
        public string name;
        public Transform transform;
        public ComponentType type;
    }

    private Dictionary<ComponentType, Type> m_Types = new Dictionary<ComponentType, Type>();

    private void InitializeComponentInfos()
    {
        m_Types.Add(ComponentType.BUTTON, typeof(LuaButton));
        m_Types.Add(ComponentType.BUTTON2, typeof(UnityEngine.UI.Button));
        m_Types.Add(ComponentType.TEXT, typeof(LuaText));
        m_Types.Add(ComponentType.TEXT2, typeof(UnityEngine.UI.Text));
        m_Types.Add(ComponentType.TRANSFORM, typeof(UnityEngine.Transform));
    }

    private Type GetComponetType(ComponentType type)
    {
        if (m_Types.ContainsKey(type)) {
            return m_Types[type];
        }
        return m_Types[ComponentType.TRANSFORM];
    }
}