﻿using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T s_Instance;

    public static T Instance
    {
        get
        {
            if (s_Instance == null) {
                s_Instance = FindObjectOfType(typeof(T)) as T;
                if (s_Instance == null) {
                    GameObject host = new GameObject();
                    s_Instance = host.AddComponent<T>();
                }
            }
            return (T)s_Instance;
        }
    }

    protected virtual void Awake()
    {
        this.OnAwake();
    }

    protected virtual void OnAwake()
    {
        if (s_Instance != null) {
            GameObject.Destroy(this.gameObject);
            return;
        }
        s_Instance = this as T;
        s_Instance.name = "_" + s_Instance.GetType().ToString() + "[autogenerated]";

        if (this.IsGlobalScope) {
            UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
        }
    }
    
    protected virtual bool IsGlobalScope
    {
        get { return false; }
    }
}
