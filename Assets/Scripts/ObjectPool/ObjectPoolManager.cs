using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class ObjectPoolManager : SingletonMono<ObjectPoolManager> 
{
    private GameFramework.ObjectPool.IObjectPoolManager m_ObjectPoolModule;

    public void Initialize()
    {
        this.m_ObjectPoolModule = GameFrameworkEntry.GetModule<GameFramework.ObjectPool.IObjectPoolManager>();
    }
}
