using GameFramework;
using GameFramework.Setting;
using System;

public class SettingManager : SingletonMono<SettingManager>
{
    private ISettingModule m_SettingModule;
    private bool m_MarkModified = false;

    public void Initialize()
    {
        this.m_SettingModule = GameFrameworkEntry.GetModule<ISettingModule>();
        this.m_SettingModule.SetSettingHelper(new SettingHelper());

        if (!this.m_SettingModule.Load()) {
            // TODO:
        }
    }

    public bool HasSetting(string settingName)
    {
        return this.m_SettingModule.HasSetting(settingName);
    }

    public void RemoveSetting(string settingName)
    {
        this.m_MarkModified = true;
        this.m_SettingModule.RemoveSetting(settingName);
    }

    public void RemoveAllSettings()
    {
        this.m_MarkModified = true;
        this.m_SettingModule.RemoveAllSettings();
    }

    public bool GetBool(string settingName)
    {
        return this.m_SettingModule.GetBool(settingName);
    }

    public bool GetBool(string settingName, bool defaultValue)
    {
        return this.m_SettingModule.GetBool(settingName, defaultValue);
    }

    public void SetBool(string settingName, bool value)
    {
        this.m_MarkModified = true;
        this.m_SettingModule.SetBool(settingName, value);
    }

    public int GetInt(string settingName)
    {
        return this.m_SettingModule.GetInt(settingName);
    }

    public int GetInt(string settingName, int defaultValue)
    {
        return this.m_SettingModule.GetInt(settingName, defaultValue);
    }

    public void SetInt(string settingName, int value)
    {
        this.m_MarkModified = true;
        this.m_SettingModule.SetInt(settingName, value);
    }

    public float GetFloat(string settingName)
    {
        return this.m_SettingModule.GetFloat(settingName);
    }

    public float GetFloat(string settingName, float defaultValue)
    {
        return this.m_SettingModule.GetFloat(settingName, defaultValue);
    }

    public void SetFloat(string settingName, float value)
    {
        this.m_MarkModified = true;
        this.m_SettingModule.SetFloat(settingName, value); 
    }

    public string GetString(string settingName)
    {
        return this.m_SettingModule.GetString(settingName);
    }

    public string GetString(string settingName, string defaultValue)
    {
        return this.m_SettingModule.GetString(settingName, defaultValue);
    }

    public void SetString(string settingName, string value)
    {
        this.m_MarkModified = true;
        this.m_SettingModule.SetString(settingName, value);
    }

    public T GetObject<T>(string settingName)
    {
        return this.m_SettingModule.GetObject<T>(settingName);
    }

    public object GetObject(Type objectType, string settingName)
    {
        return this.m_SettingModule.GetObject(objectType, settingName);
    }

    public T GetObject<T>(string settingName, T defaultObj)
    {
        return this.m_SettingModule.GetObject<T>(settingName, defaultObj);
    }

    public object GetObject(Type objectType, string settingName, object defaultObj)
    {
        return this.m_SettingModule.GetObject(objectType, settingName, defaultObj);
    }

    public void SetObject<T>(string settingName, T obj)
    {
        this.m_MarkModified = true;
        this.m_SettingModule.SetObject<T>(settingName, obj);
    }

    public void SetObject(string settingName, object obj)
    {
        this.m_MarkModified = true;
        this.m_SettingModule.SetObject(settingName, obj);
    }

    private bool Save()
    {
        return this.m_SettingModule.Save();
    }

    private void Update()
    {
        if (this.m_MarkModified) {
            this.Save();
            m_MarkModified = false;
        }
    }

    protected override bool IsGlobalScope
    {
        get { return true; }
    }
}
