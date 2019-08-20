using UnityEngine;
using GameFramework;

public class AppFacade : SingletonMono<AppFacade>
{
    protected override void Awake()
    {
        base.Awake();

        Debug.unityLogger.logEnabled = AppConst.kLogEnabled;
        if (AppConst.kLogMessageReceived)
            Application.logMessageReceived += OnLogMessage;

#if UNITY_5_6_OR_NEWER
        Application.lowMemory += OnLowMemory;
#endif
    }

    public void Initialize()
    {
        ObjectPoolManager.Instance.Initialize();
        SettingManager.Instance.Initialize();
        ResourceManager.Instance.Initialize();
        ConfigManager.Instance.Initialize();
        LocalizationManager.Instance.Initialize();
        UIManager.Instance.Initialize();

        ResourceManager.Instance.InitResources();
    }

    private void Update()
    {
        GameFrameworkEntry.Update(Time.deltaTime, Time.unscaledDeltaTime);
    }

    private void OnDestroy()
    {
        if (AppConst.kLogMessageReceived) 
            Application.logMessageReceived -= OnLogMessage;

#if UNITY_5_6_OR_NEWER
        Application.lowMemory -= OnLowMemory;
#endif

        GameFrameworkEntry.Shutdown();
    }
    
    private void OnLowMemory()
    {
        // TODO:
    }

    private void OnLogMessage(string condition, string stackTrace, LogType type)
    {
        // TODO:
    }

    protected override bool IsGlobalScope 
    { 
        get { return true; } 
    }
}