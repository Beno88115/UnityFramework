using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Reflection;
using UnityEngine.SceneManagement;
using System.Text;

public partial class Tools : MonoBehaviour
{
    [MenuItem("MyTools/FindChinese")]
    private static void FindChinese()
    {
        ClearnConsole();
        EditorSettings.serializationMode = SerializationMode.ForceText;
        List<string> findExtensions = new List<string>() { ".prefab" };
        string[] files = GetFileByExtensions(findExtensions);
        int startIndex = 0;
        EditorApplication.update = delegate ()
        {
            string file = files[startIndex];
            bool isCancel = EditorUtility.DisplayCancelableProgressBar("匹配资源中", file, (float)startIndex / (float)files.Length);
            if (Regex.IsMatch(File.ReadAllText(file), @"((?!(\*|//)).)+[\u4e00-\u9fa5]"))
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

    [MenuItem("Assets/MyTools/代码中文检测/CS")]
    private static void CSChineseCheck()
    {
        ClearnConsole();
        //var CodePath = Path.Combine(Application.dataPath, "../../Code/Game");
        GameObject obj = Selection.activeGameObject;
        string op = AssetDatabase.GetAssetPath(obj);
        string id = AssetDatabase.AssetPathToGUID(op);
        Debug.LogError(id);
        Debug.LogError(Selection.activeInstanceID);
        if (Selection.activeObject.name.Contains("."))
        {
            return;
        }
        
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (Directory.Exists(path))
        {
            var dir = new DirectoryInfo(path);
            var files = dir.GetFiles("*.cs", SearchOption.AllDirectories);
            int len = files.Length;
            float pro = 0;
            StringBuilder sb = new StringBuilder();

            //需要过滤不进行检测的脚本
            string[] igonerFile = new string[] { "CityAiCreator.cs", "MeshPainter.cs" };
            bool igoner = false;
            for (int i = 0; i < len; i++)
            {
                igoner = false;
                //Debug.Log(files[i].FullName);
                for (int j = 0, jmax = igonerFile.Length; j < jmax; j++)
                {
                    if (files[i].Name == igonerFile[j])
                    {
                        igoner = true;
                        break;
                    }
                }
                if (igoner) continue;
                var tsb = ChineseChecker.CheckCSFile(files[i].FullName);
                if (tsb != null)
                {
                    sb.AppendLine(files[i].Name);
                    sb.Append(tsb.ToString());
                }
                pro = (float)i / (float)len;
                EditorUtility.DisplayProgressBar("检测CS代码", string.Format(":进度{0}/{1}", i, len), pro);
            }
            //string cs_log_text = Path.Combine(Application.dataPath, "cs_中文检测_log.txt");
            //ExportLanguageScripts.WriteFile(sb, cs_log_text);
            EditorUtility.ClearProgressBar();
        }
    }
}