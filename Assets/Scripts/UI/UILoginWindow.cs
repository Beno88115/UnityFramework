using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoginWindow : UIWindow 
{
    [SerializeField]
    Button btnLogIn;
    [SerializeField]
    Button btnLogUp;

    void Awake()
    {
        this.btnLogIn.onClick.AddListener(this.OnLoginButtonClicked);
    }

    private void OnLoginButtonClicked()
    {

    }
}
