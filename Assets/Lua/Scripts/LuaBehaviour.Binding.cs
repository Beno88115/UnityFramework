using System;
using UnityEngine;
using System.Collections.Generic;

public partial class LuaBehaviour : MonoBehaviour
{
    private enum ComponentType
    {
        Transform,
        Button,
        Text,
        Image,
    }

    [Serializable]
    private class ComponentBinding
    {
        public string name;
        public Transform transform;
        public ComponentType type;
    }

    private static Dictionary<ComponentType, Type> m_Types = new Dictionary<ComponentType, Type>();

    private void InitializeComponentInfos()
    {
        if (m_Types.Count == 0) {
            m_Types.Add(ComponentType.Text, typeof(UnityEngine.UI.Text));
            m_Types.Add(ComponentType.Button, typeof(UnityEngine.UI.Button));
            m_Types.Add(ComponentType.Image, typeof(UnityEngine.UI.Image));
            m_Types.Add(ComponentType.Transform, typeof(UnityEngine.Transform));
        }
    }

    private Type GetComponetType(ComponentType type)
    {
        if (m_Types.ContainsKey(type)) {
            return m_Types[type];
        }
        return m_Types[ComponentType.Transform];
    }
}