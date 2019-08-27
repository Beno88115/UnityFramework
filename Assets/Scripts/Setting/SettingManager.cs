using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class SettingManager : SingletonMono<SettingManager>
{
    private GameFramework.Setting.ISettingModule m_SettingModule;

    public void Initialize()
    {
        this.m_SettingModule = GameFrameworkEntry.GetModule<GameFramework.Setting.ISettingModule>();
        this.m_SettingModule.SetSettingHelper(new SettingHelper());
    }

    protected override bool IsGlobalScope
    {
        get { return true; }
    }
}
