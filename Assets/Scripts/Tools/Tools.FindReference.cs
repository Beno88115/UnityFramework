using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Reflection;
using UnityEngine.SceneManagement;

public partial class Tools : MonoBehaviour
{
    [MenuItem("Assets/MyTools/FindScriptReference %f")]
    private static void FindScriptReference()
    {
        ClearnConsole();
        MonoScript script = Selection.activeObject as MonoScript;
        if (script == null)
        {
            EditorUtility.DisplayDialog("注意", "选择物体非脚本", "ok");
            return;
        }

        Debug.Log(string.Format("开始查找脚本:{0}", script.name));
        EditorSettings.serializationMode = SerializationMode.ForceText;
        string path = AssetDatabase.GetAssetPath(script);
        if (!string.IsNullOrEmpty(path))
        {
            string guid = AssetDatabase.AssetPathToGUID(path);
            List<string> findExtensions = new List<string>() { ".prefab", ".unity" };
            FindReferences(guid, findExtensions);
        }
    }

    private static void FindReferences(string guid, List<string> findExtensions)
    {
        string[] files = GetFileByExtensions(findExtensions);
        int startIndex = 0;
        EditorApplication.update = delegate ()
        {
            string file = files[startIndex];
            bool isCancel = EditorUtility.DisplayCancelableProgressBar("匹配资源中", file, (float)startIndex / (float)files.Length);
            if (Regex.IsMatch(File.ReadAllText(file), guid))
            {
                Debug.Log(file, AssetDatabase.LoadAssetAtPath<Object>(GetRelativeAssetsPath(file)));
            }

            startIndex++;
            if (isCancel || startIndex >= files.Length)
            {
                EditorUtility.ClearProgressBar();
                EditorApplication.update = null;
                startIndex = 0;
                Debug.Log("查找结束");
            }
        };
    }
}