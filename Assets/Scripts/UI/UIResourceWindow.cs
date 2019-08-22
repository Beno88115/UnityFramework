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
    [SerializeField]
    Image image1;

    void Start()
    {
        this.btnLoadSprite.onClick.AddListener(OnLoadSpriteButtonClicked);
        this.btnBack.onClick.AddListener(OnBackButtonClicked);
    }

    public override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        this.image.sprite = null;
        this.image1.sprite = null;
    }

    void OnLoadSpriteButtonClicked()
    {
        image.SetSprite("Chess");
        image1.SetSprite("Chess");
    }

    void OnBackButtonClicked()
    {
        UIManager.Instance.PopWindow(this.SerialId);
    }
}
