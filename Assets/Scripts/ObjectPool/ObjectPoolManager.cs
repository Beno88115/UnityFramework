using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class ObjectPoolManager : SingletonMono<ObjectPoolManager> 
{
    private GameFramework.ObjectPool.IObjectPoolModule m_ObjectPoolModule;

    public void Initialize()
    {
        this.m_ObjectPoolModule = GameFrameworkEntry.GetModule<GameFramework.ObjectPool.IObjectPoolModule>();
    }

    protected override bool IsGlobalScope 
    { 
        get { return true; } 
    }
}
