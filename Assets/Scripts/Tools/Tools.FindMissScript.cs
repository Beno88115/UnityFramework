using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Reflection;

public partial class Tools : MonoBehaviour
{
    //(?!.*#region)^[^*/\r\n]{0,100}[\u4e00-\u9fa5]+[^/\r\n]{0,100}(\n|\r)
    [MenuItem(MenuItemPath + "FindMissScriptByPrefab")]
    private static void FindMissScriptByPrefab()
    {
        ClearnConsole();
        Debug.LogError("Find Miss Script Start!");
        List<string> findExtensions = new List<string>() { ".prefab" };//, ".unity" };
        string[] files = GetFileByExtensions(findExtensions);
        int currentIndex = 0;
        EditorApplication.update = delegate ()
        {
            string file = files[currentIndex];
            Transform tr = AssetDatabase.LoadAssetAtPath(GetRelativeAssetsPath(file), typeof(Transform)) as Transform;
            FindMissScriptByObject(file, tr);
            bool isCancel = EditorUtility.DisplayCancelableProgressBar("Finding...", file, (float)currentIndex / (float)files.Length);

            currentIndex++;
            if (isCancel || currentIndex >= files.Length)
            {
                EditorUtility.ClearProgressBar();
                EditorApplication.update = null;
                currentIndex = 0;
                Debug.LogError("Find Miss Script End!");
            }
        };
    }

    [MenuItem(MenuItemPath + "FindMissScritpByScene")]
    private static void FindMissScritpByScene()
    {
        Debug.LogError(111);
    }

    private static void FindMissScriptByObject(string path, Transform go)
    {
        Component[] components = go.GetComponents<Component>();
        for (int j = 0; j < components.Length; j++)
        {
            if (components[j] == null)
            {
                Debug.Log(string.Format("Miss Script File: {0}, node: {1}", path, go.name));
            }
        }

        for (int index = 0; index < go.transform.childCount; index++)
        {
            FindMissScriptByObject(path, go.transform.GetChild(index));
        }
    }
}