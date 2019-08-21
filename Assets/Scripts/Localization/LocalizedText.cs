using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizedText : MonoBehaviour 
{
    [SerializeField]
    Localized.Text m_LanguageID = Localized.Text.UNKNOWN;

	void Start()
    {
        if (m_LanguageID != Localized.Text.UNKNOWN) {
            GetComponent<Text>().text = LocalizationManager.Instance.GetString(m_LanguageID);
        }
	}
}
