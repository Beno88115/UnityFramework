using UnityEngine.UI;
using LuaInterface;

public static class LuaButtonExtension 
{
    public static void AddClick(this Button button, LuaFunction function)
    {
        if (function == null) {
            return;
        }

        button.onClick.AddListener(()=>{
            function.Call();
        });
    }
}