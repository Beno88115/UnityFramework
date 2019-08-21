using System;
using System.Reflection;

public class Singleton<T> where T : class
{
    /*  Instance  */
    private static T s_Instance;

    /* Serve the single instance to callers */
    public static T Instance
    {
        get
        {
            if (s_Instance == null) {
                s_Instance = (T)Activator.CreateInstance(typeof(T), true);
                Type type = typeof(T);
                MethodInfo mi = type.GetMethod("OnInit");
                if (mi != null) {
                    mi.Invoke(s_Instance, null);
                }
            }
            return s_Instance;
        }
    }

    /*  Destroy */
    public static void Destroy()
    {
        Type type = typeof(T);
        MethodInfo mi = type.GetMethod("OnDestroy");
        if (mi != null) {
            mi.Invoke(s_Instance, null);
        }
        s_Instance = null;
        return;
    }
}
