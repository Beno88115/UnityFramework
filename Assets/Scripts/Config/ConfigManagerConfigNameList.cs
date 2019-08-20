using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ConfigManager : SingletonMono<ConfigManager>
{
    private string[] m_ConfigNameList = { "Gift", "Prop", "Shop" };
}