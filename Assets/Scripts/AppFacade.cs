using UnityEngine;
using GameFramework;

public class AppFacade : SingletonMono<AppFacade>
{
    protected override void Awake()
    {
        base.Awake();

#if UNITY_5_6_OR_NEWER
        Application.lowMemory += OnLowMemory;
#endif
    }

    private void Start()
    {
    }

    public void Initialize()
    {
        ResourceManager.Instance.Initialized();
        UIManager.Instance.Initialize();
    }

    private void Update()
    {
        GameFrameworkEntry.Update(Time.deltaTime, Time.unscaledDeltaTime);
    }

    private void OnDestroy()
    {
#if UNITY_5_6_OR_NEWER
        Application.lowMemory -= OnLowMemory;
#endif
        GameFrameworkEntry.Shutdown();
    }
    
    private void OnLowMemory()
    {
        // TODO:
    }

    protected override bool IsGlobalScope 
    { 
        get { return true; } 
    }
}