﻿using System;
using UnityEngine;
using GameFramework.Setting;

public class SettingHelper : ISettingHelper
{
    private bool m_MarkChanged = false;

    /// <summary>
    /// 加载配置。
    /// </summary>
    /// <returns>是否加载配置成功。</returns>
    public bool Load()
    {
        return true;
    }

    /// <summary>
    /// 保存配置。
    /// </summary>
    /// <returns>是否保存配置成功。</returns>
    public bool Save()
    {
        if (m_MarkChanged) {
            PlayerPrefs.Save();
            m_MarkChanged = false;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检查是否存在指定配置项。
    /// </summary>
    /// <param name="settingName">要检查配置项的名称。</param>
    /// <returns>指定的配置项是否存在。</returns>
    public bool HasSetting(string settingName)
    {
        return PlayerPrefs.HasKey(Encrypt(settingName));
    }

    /// <summary>
    /// 移除指定配置项。
    /// </summary>
    /// <param name="settingName">要移除配置项的名称。</param>
    public void RemoveSetting(string settingName)
    {
        PlayerPrefs.DeleteKey(Encrypt(settingName));
    }

    /// <summary>
    /// 清空所有配置项。
    /// </summary>
    public void RemoveAllSettings()
    {
        PlayerPrefs.DeleteAll();
    }

    /// <summary>
    /// 从指定配置项中读取布尔值。
    /// </summary>
    /// <param name="settingName">要获取配置项的名称。</param>
    /// <returns>读取的布尔值。</returns>
    public bool GetBool(string settingName)
    {
        return Decrypt<bool>(PlayerPrefs.GetString(Encrypt(settingName)));
    }

    /// <summary>
    /// 从指定配置项中读取布尔值。
    /// </summary>
    /// <param name="settingName">要获取配置项的名称。</param>
    /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
    /// <returns>读取的布尔值。</returns>
    public bool GetBool(string settingName, bool defaultValue)
    {
        return Decrypt<bool>(PlayerPrefs.GetString(Encrypt(settingName), Encrypt(defaultValue)));
    }

    /// <summary>
    /// 向指定配置项写入布尔值。
    /// </summary>
    /// <param name="settingName">要写入配置项的名称。</param>
    /// <param name="value">要写入的布尔值。</param>
    public void SetBool(string settingName, bool value)
    {
        PlayerPrefs.SetString(Encrypt(settingName), Encrypt(value));
        m_MarkChanged = true;
    }

    /// <summary>
    /// 从指定配置项中读取整数值。
    /// </summary>
    /// <param name="settingName">要获取配置项的名称。</param>
    /// <returns>读取的整数值。</returns>
    public int GetInt(string settingName)
    {
        return Decrypt<int>(PlayerPrefs.GetString(Encrypt(settingName)));
    }

    /// <summary>
    /// 从指定配置项中读取整数值。
    /// </summary>
    /// <param name="settingName">要获取配置项的名称。</param>
    /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
    /// <returns>读取的整数值。</returns>
    public int GetInt(string settingName, int defaultValue)
    {
        return Decrypt<int>(PlayerPrefs.GetString(Encrypt(settingName), Encrypt(defaultValue)));
    }

    /// <summary>
    /// 向指定配置项写入整数值。
    /// </summary>
    /// <param name="settingName">要写入配置项的名称。</param>
    /// <param name="value">要写入的整数值。</param>
    public void SetInt(string settingName, int value)
    {
        PlayerPrefs.SetString(Encrypt(settingName), Encrypt(value));
        m_MarkChanged = true;
    }

    /// <summary>
    /// 从指定配置项中读取浮点数值。
    /// </summary>
    /// <param name="settingName">要获取配置项的名称。</param>
    /// <returns>读取的浮点数值。</returns>
    public float GetFloat(string settingName)
    {
        return Decrypt<float>(PlayerPrefs.GetString(Encrypt(settingName)));
    }

    /// <summary>
    /// 从指定配置项中读取浮点数值。
    /// </summary>
    /// <param name="settingName">要获取配置项的名称。</param>
    /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
    /// <returns>读取的浮点数值。</returns>
    public float GetFloat(string settingName, float defaultValue)
    {
        return Decrypt<float>(PlayerPrefs.GetString(Encrypt(settingName), Encrypt(defaultValue)));
    }

    /// <summary>
    /// 向指定配置项写入浮点数值。
    /// </summary>
    /// <param name="settingName">要写入配置项的名称。</param>
    /// <param name="value">要写入的浮点数值。</param>
    public void SetFloat(string settingName, float value)
    {
        PlayerPrefs.SetString(Encrypt(settingName), Encrypt(value));
        m_MarkChanged = true;
    }

    /// <summary>
    /// 从指定配置项中读取字符串值。
    /// </summary>
    /// <param name="settingName">要获取配置项的名称。</param>
    /// <returns>读取的字符串值。</returns>
    public string GetString(string settingName)
    {
        return Decrypt<string>(PlayerPrefs.GetString(Encrypt(settingName)));
    }

    /// <summary>
    /// 从指定配置项中读取字符串值。
    /// </summary>
    /// <param name="settingName">要获取配置项的名称。</param>
    /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
    /// <returns>读取的字符串值。</returns>
    public string GetString(string settingName, string defaultValue)
    {
        return Decrypt<string>(PlayerPrefs.GetString(Encrypt(settingName), Encrypt(defaultValue)));
    }

    /// <summary>
    /// 向指定配置项写入字符串值。
    /// </summary>
    /// <param name="settingName">要写入配置项的名称。</param>
    /// <param name="value">要写入的字符串值。</param>
    public void SetString(string settingName, string value)
    {
        PlayerPrefs.SetString(Encrypt(settingName), Encrypt(value));
        m_MarkChanged = true;
    }

    /// <summary>
    /// 从指定配置项中读取对象。
    /// </summary>
    /// <typeparam name="T">要读取对象的类型。</typeparam>
    /// <param name="settingName">要获取配置项的名称。</param>
    /// <returns>读取的对象。</returns>
    public T GetObject<T>(string settingName)
    {
        return default(T);
    }

    /// <summary>
    /// 从指定配置项中读取对象。
    /// </summary>
    /// <param name="objectType">要读取对象的类型。</param>
    /// <param name="settingName">要获取配置项的名称。</param>
    /// <returns></returns>
    public object GetObject(Type objectType, string settingName)
    {
        return null;
    }

    /// <summary>
    /// 从指定配置项中读取对象。
    /// </summary>
    /// <typeparam name="T">要读取对象的类型。</typeparam>
    /// <param name="settingName">要获取配置项的名称。</param>
    /// <param name="defaultObj">当指定的配置项不存在时，返回此默认对象。</param>
    /// <returns>读取的对象。</returns>
    public T GetObject<T>(string settingName, T defaultObj)
    {
        return default(T);
    }

    /// <summary>
    /// 从指定配置项中读取对象。
    /// </summary>
    /// <param name="objectType">要读取对象的类型。</param>
    /// <param name="settingName">要获取配置项的名称。</param>
    /// <param name="defaultObj">当指定的配置项不存在时，返回此默认对象。</param>
    /// <returns></returns>
    public object GetObject(Type objectType, string settingName, object defaultObj)
    {
        return null;
    }

    /// <summary>
    /// 向指定配置项写入对象。
    /// </summary>
    /// <typeparam name="T">要写入对象的类型。</typeparam>
    /// <param name="settingName">要写入配置项的名称。</param>
    /// <param name="obj">要写入的对象。</param>
    public void SetObject<T>(string settingName, T obj)
    {
        m_MarkChanged = true;
    }

    /// <summary>
    /// 向指定配置项写入对象。
    /// </summary>
    /// <param name="settingName">要写入配置项的名称。</param>
    /// <param name="obj">要写入的对象。</param>
    public void SetObject(string settingName, object obj)
    {
        m_MarkChanged = true;
    }

    private string Encrypt<T>(T value)
    {
        // TODO: 加密
        return value.ToString();
    }

    private T Decrypt<T>(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return default(T);
        }

        // TODO: 解密
        return (T)Convert.ChangeType(value, typeof(T));
    }
}
