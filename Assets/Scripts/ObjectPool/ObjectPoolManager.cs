using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class ObjectPoolManager : SingletonMono<ObjectPoolManager> 
{
    private GameFramework.ObjectPool.IObjectPoolManager m_ObjectPoolModule;

    protected override void Awake()
    {
        base.Awake();

        this.m_ObjectPoolModule = GameFrameworkEntry.GetModule<GameFramework.ObjectPool.IObjectPoolManager>();
    }
}
