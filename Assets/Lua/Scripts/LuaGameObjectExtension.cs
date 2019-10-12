using UnityEngine;
using LuaInterface;

public static class LuaGameObjectExtension 
{
    public static LuaBehaviour GetLuaComponent(this GameObject gameObject, string componentName)
    {
        LuaBehaviour[] cmpts = gameObject.GetComponentsInChildren<LuaBehaviour>();
        for (int i = 0; i < cmpts.Length; ++i) {
            var cmpt = cmpts[i];
            if (cmpt.LuaComponentName.Equals(componentName)) {
                return cmpt;
                // return cmpt.LuaComponent;
            }
        }
        return null;
    }
}
