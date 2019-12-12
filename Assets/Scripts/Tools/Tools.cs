using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Reflection;

public partial class Tools : MonoBehaviour
{
    private const string MenuItemPath = "MyTools/";

    [MenuItem(MenuItemPath + "ScreenShoot %q")]
    private static void ScreenShoot()
    {
        ScreenCapture.CaptureScreenshot("c:/" + System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ff") + ".png", 1);
    }

    private static string[] GetFileByExtensions(List<string> findExtensions)
    {
        if (findExtensions != null && findExtensions.Count != 0)
        {
            return Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories).Where(s => findExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();
        }
        else
        {
            return Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories);
        }
    }

    private static string GetRelativeAssetsPath(string path)
    {
        return "Assets" + Path.GetFullPath(path).Replace(Path.GetFullPath(Application.dataPath), "").Replace('\\', '/');
    }

    private static void ClearnConsole()
    {
        Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
        System.Type logEntries = assembly.GetType("UnityEditor.LogEntries");
        MethodInfo clearConsoleMethod = logEntries.GetMethod("Clear");
        clearConsoleMethod.Invoke(new object(), null);
    }
}