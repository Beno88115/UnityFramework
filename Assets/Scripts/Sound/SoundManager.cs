using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class SoundManager : SingletonMono<SoundManager> 
{
    private GameFramework.Sound.ISoundManager m_SoundModule;

    protected override void Awake()
    {
        base.Awake();

        this.m_SoundModule = GameFrameworkEntry.GetModule<GameFramework.Sound.ISoundManager>();
        this.m_SoundModule.SetSoundHelper(new SoundHelper());
        this.m_SoundModule.SetResourceManager(GameFrameworkEntry.GetModule<GameFramework.Resource.IResourceManager>());
    }

    private void Start()
    {
    }
}