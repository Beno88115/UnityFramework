using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using SimpleJSON;
using System.Text;
using System.Xml;

public static class BuildAssetBundles 
{
    static readonly string kAssetBundleDirectory = "AssetBundles";

    [MenuItem("Tools/AssetBundles/Build")]
    static void Build()
    {
		// string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
		// for (int i = 0; i < assetBundleNames.Length; ++i) {
		// 	JSONObject assetBundleObject = new JSONObject();

		// 	string assetBundleName = assetBundleNames[i];
		// 	Debug.Log("============================");
		// 	string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
		// 	for (int j = 0; j < assetPaths.Length; ++j) {
						
		// 		string assetPath = assetPaths[j];
		// 		Debug.Log("assetName: " + assetPath);
		// 		string[] des = AssetDatabase.GetDependencies(assetPath);
		// 		if (des.Length > 0) {
		// 			for (int x = 0; x < des.Length; ++x) {
		// 				if (des[x] != assetPath && !des[x].EndsWith(".cs"))
		// 					Debug.Log("++dependencis: " + des[x]);
		// 			}
		// 		}
		// 	}	
		// }

        string outputPath = Path.Combine(kAssetBundleDirectory, GetPlatformName());
        if (!Directory.Exists(outputPath)) {
            Directory.CreateDirectory(outputPath);
        }
        BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

		XmlDocument document = new XmlDocument();
		document.AppendChild(document.CreateXmlDeclaration("1.0", "UTF-8", null));

		XmlElement root = document.CreateElement("AssetBundles");
		root.SetAttribute("Version", "1.0.0");
		root.SetAttribute("VersionCode", "1");
		root.SetAttribute("GameVersion", "1");
		document.AppendChild(root);

		string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
		for (int i = 0; i < assetBundleNames.Length; ++i) {

			string assetBundleName = assetBundleNames[i];
			string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
			if (assetPaths.Length == 0) {
				continue;
			}

			XmlElement assetBundleElement = document.CreateElement("AssetBundle");
			assetBundleElement.SetAttribute("Name", assetBundleName);

			string variant = AssetDatabase.GetImplicitAssetBundleVariantName(assetPaths[0]);
			if (!string.IsNullOrEmpty(variant)) {
				assetBundleElement.SetAttribute("Variant", variant);
			}

			assetBundleElement.SetAttribute("Length", "0");
			assetBundleElement.SetAttribute("HashCode", "0");

			for (int j = 0; j < assetPaths.Length; ++j) {
				string assetPath = assetPaths[j];
				string assetName = assetPath.Substring(assetPath.LastIndexOf("/") + 1).Split('.')[0];

				XmlElement assetElement = document.CreateElement("Asset");
				assetElement.SetAttribute("Name", assetName);
				assetBundleElement.AppendChild(assetElement);

				string[] dependencyAssetPaths = AssetDatabase.GetDependencies(assetPath);
				for (int x = 0; x < dependencyAssetPaths.Length; ++x) {
					string dependencyAssetPath = dependencyAssetPaths[x];
					if (dependencyAssetPath != assetPath && !dependencyAssetPath.EndsWith(".cs")) {
						string dependencyAssetName = dependencyAssetPath.Substring(dependencyAssetPath.LastIndexOf("/") + 1).Split('.')[0];

						XmlElement dependencyAssetElement = document.CreateElement("Dependency");
						dependencyAssetElement.SetAttribute("Name", dependencyAssetName);

						assetElement.AppendChild(dependencyAssetElement);
					}
				}
			}

			root.AppendChild(assetBundleElement);
		}
		
		string filePath = Path.Combine(outputPath, "version.dat");
		document.Save(filePath);

		CopyAssetBundlesTo(Application.streamingAssetsPath);
		AssetDatabase.Refresh();
    }

	private static void CopyAssetBundlesTo(string outputPath)
	{
		FileUtil.DeleteFileOrDirectory(outputPath);
		Directory.CreateDirectory(outputPath);

		string source = Path.Combine(Path.Combine(System.Environment.CurrentDirectory, kAssetBundleDirectory), GetPlatformName());
		if (!System.IO.Directory.Exists(source)) {
			Debug.LogError("BuildMenu.CopyAssetBundles() - No assetBundle output folder, try to build the assetBundles first.");
		}

        DirectoryInfo di = new DirectoryInfo(source);
        FileInfo[] fis = di.GetFiles("*", SearchOption.TopDirectoryOnly);

        EditorUtility.DisplayProgressBar("Copy AssetBundles", "", 0);
        for (int i = 0; i < fis.Length; ++i) {
            EditorUtility.DisplayProgressBar("Copy AssetBundles", "Copy " + fis[i].FullName, (float)i / (float)fis.Length);
            FileUtil.CopyFileOrDirectory(fis[i].FullName, Path.Combine(outputPath, fis[i].Name));
        }
        EditorUtility.ClearProgressBar();
	}

    public static string GetPlatformName()
	{
		return GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
	}

	private static string GetPlatformForAssetBundles(BuildTarget target)
	{
		switch(target)
		{
		case BuildTarget.Android:
			return "Android";
		case BuildTarget.iOS:
			return "iOS";
		case BuildTarget.WebGL:
			return "WebGL";
		case BuildTarget.StandaloneWindows:
		case BuildTarget.StandaloneWindows64:
			return "Windows";
		case BuildTarget.StandaloneOSX:
			return "OSX";
		default:
			return null;
		}
	}
}
