using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResourceWindow : UIWindow
{
    [SerializeField]
    Button btnLoadSprite;
    [SerializeField]
    Button btnBack;
    [SerializeField]
    Image image;

    void Start()
    {
        this.btnLoadSprite.onClick.AddListener(OnLoadSpriteButtonClicked);
        this.btnBack.onClick.AddListener(OnBackButtonClicked);
    }

    void OnLoadSpriteButtonClicked()
    {
        image.SetSprite("Chess");
    }

    void OnBackButtonClicked()
    {
        UIManager.Instance.PopWindow(this.SerialId);
    }
}
