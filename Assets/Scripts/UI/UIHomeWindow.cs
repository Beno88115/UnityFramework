using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHomeWindow : UIWindow 
{
    [SerializeField]
    Button backBtn;

    void Start()
    {
        this.backBtn.onClick.AddListener(this.OnBackButtonClicked);        
    }

    void OnBackButtonClicked()
    {
        UIManager.Instance.PopWindow(this.SerialId);
    }
}
