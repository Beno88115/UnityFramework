using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        AppFacade.Instance.Initialize();
        // UIManager.Instance.PushForm("UILogin");
    }
}
