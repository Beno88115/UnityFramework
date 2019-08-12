using UnityEngine;
using GameFramework;

public class AppFacade : SingletonMono<AppFacade>
{
    void Start()
    {
    }

    void Update()
    {
        GameFrameworkEntry.Update(Time.time, Time.unscaledTime);
    }

    public T GetModule<T>() where T : class
    {
        return GameFrameworkEntry.GetModule<T>();
    }

    void OnApplicationQuit()
    {
        GameFrameworkEntry.Shutdown();
    }

    protected override bool IsGlobalScope 
    { 
        get { return true; } 
    }
}