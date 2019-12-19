using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        AppFacade.Instance.Initialize();
        // UIManager.Instance.PushWindow("UILogin");
    }
}