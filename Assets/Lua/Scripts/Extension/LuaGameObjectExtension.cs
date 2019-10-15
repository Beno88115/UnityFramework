using UnityEngine;

public static class LuaGameObjectExtension 
{
    public static LuaBehaviour GetLuaComponent(this GameObject gameObject, string componentName)
    {
        LuaBehaviour[] behaviours = gameObject.GetComponentsInChildren<LuaBehaviour>();
        for (int i = 0; i < behaviours.Length; ++i) {
            var behaviour = behaviours[i];
            if (behaviour.LuaComponentName.Equals(componentName)) {
                return behaviour;
            }
        }
        return null;
    }
}
