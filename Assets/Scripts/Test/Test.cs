using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        AppFacade.Instance.Initialize();
        ResourceManager.Instance.InitResources(()=>{
            UIManager.Instance.PushWindow("UILogin");
        });
    }
}