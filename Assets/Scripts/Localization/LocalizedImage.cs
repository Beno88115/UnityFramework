using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedImage : MonoBehaviour
{
    [SerializeField]
    Localized.Image m_ImageID = Localized.Image.UNKNOWN;

    void Start()
    {
        if (m_ImageID != Localized.Image.UNKNOWN) {
            GetComponent<Image>().SetSprite(LocalizationManager.Instance.GetSpriteAssetName(m_ImageID));
        }
    }
}
