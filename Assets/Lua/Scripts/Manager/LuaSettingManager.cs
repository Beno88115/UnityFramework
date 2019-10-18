using System;

public class LuaSettingManager : Singleton<LuaSettingManager> 
{
    public void Initialize()
    {
        SettingManager.Instance.Initialize();
    }

    public bool HasSetting(string settingName)
    {
        return SettingManager.Instance.HasSetting(settingName);
    }

    public void RemoveSetting(string settingName)
    {
        SettingManager.Instance.RemoveSetting(settingName);
    }

    public void RemoveAllSettings()
    {
        SettingManager.Instance.RemoveAllSettings();
    }

    public bool GetBool(string settingName)
    {
        return SettingManager.Instance.GetBool(settingName);
    }

    public bool GetBool(string settingName, bool defaultValue)
    {
        return SettingManager.Instance.GetBool(settingName, defaultValue);
    }

    public void SetBool(string settingName, bool value)
    {
        SettingManager.Instance.SetBool(settingName, value);
    }

    public int GetInt(string settingName)
    {
        return SettingManager.Instance.GetInt(settingName);
    }

    public int GetInt(string settingName, int defaultValue)
    {
        return SettingManager.Instance.GetInt(settingName, defaultValue);
    }

    public void SetInt(string settingName, int value)
    {
        SettingManager.Instance.SetInt(settingName, value);
    }

    public float GetFloat(string settingName)
    {
        return SettingManager.Instance.GetFloat(settingName);
    }

    public float GetFloat(string settingName, float defaultValue)
    {
        return SettingManager.Instance.GetFloat(settingName, defaultValue);
    }

    public void SetFloat(string settingName, float value)
    {
        SettingManager.Instance.SetFloat(settingName, value); 
    }

    public string GetString(string settingName)
    {
        return SettingManager.Instance.GetString(settingName);
    }

    public string GetString(string settingName, string defaultValue)
    {
        return SettingManager.Instance.GetString(settingName, defaultValue);
    }

    public void SetString(string settingName, string value)
    {
        SettingManager.Instance.SetString(settingName, value);
    }
}