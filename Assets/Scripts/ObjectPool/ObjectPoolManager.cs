using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.ObjectPool;

public class ObjectPoolManager : SingletonMono<ObjectPoolManager> 
{
    private IObjectPoolModule m_ObjectPoolModule;

    public void Initialize()
    {
        this.m_ObjectPoolModule = GameFrameworkEntry.GetModule<IObjectPoolModule>();
    }

    protected override bool IsGlobalScope 
    { 
        get { return true; } 
    }
}
