using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class SettingManager : SingletonMono<SettingManager>
{
    private GameFramework.Setting.ISettingModule m_SettingModule;

    protected override void Awake()
    {
        base.Awake();

        this.m_SettingModule = GameFrameworkEntry.GetModule<GameFramework.Setting.SettingModule>();
        this.m_SettingModule.SetSettingHelper(new SettingHelper());
    }
}
