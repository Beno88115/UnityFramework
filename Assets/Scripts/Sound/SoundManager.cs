using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class SoundManager : SingletonMono<SoundManager> 
{
    private GameFramework.Sound.ISoundModule m_SoundModule;

    protected override void Awake()
    {
        base.Awake();

        this.m_SoundModule = GameFrameworkEntry.GetModule<GameFramework.Sound.ISoundModule>();
        this.m_SoundModule.SetSoundHelper(new SoundHelper());
        this.m_SoundModule.SetResourceModule(GameFrameworkEntry.GetModule<GameFramework.Resource.IResourceModule>());
    }

    private void Start()
    {
    }
}