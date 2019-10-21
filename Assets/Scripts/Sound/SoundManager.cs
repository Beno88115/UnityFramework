using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Sound;
using GameFramework.Resource;

public class SoundManager : SingletonMono<SoundManager> 
{
    private ISoundModule m_SoundModule;

    protected override void Awake()
    {
        base.Awake();

        this.m_SoundModule = GameFrameworkEntry.GetModule<ISoundModule>();
        this.m_SoundModule.SetSoundHelper(new SoundHelper());
        this.m_SoundModule.SetResourceModule(GameFrameworkEntry.GetModule<IResourceModule>());
    }

    private void Start()
    {
    }
}