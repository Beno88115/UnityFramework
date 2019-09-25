using UnityEngine;
using System.IO;
using UnityEditor;
using System.Xml;

public static class BuildAssetBundles 
{
    private const string kAssetBundleDirectory = "AssetBundles";

    [MenuItem("Tools/AssetBundles/Build AssetBundle/Windows")]
	static void BuildAssetBundleForWindow()
	{
		BuildAssetBundle(BuildTarget.StandaloneWindows);
	}

    [MenuItem("Tools/AssetBundles/Build AssetBundle/Windows64")]
	static void BuildAssetBundleForWindow64()
	{
		BuildAssetBundle(BuildTarget.StandaloneWindows64);
	}
	
    [MenuItem("Tools/AssetBundles/Build AssetBundle/OSX")]
	static void BuildAssetBundleForOSX()
	{
		BuildAssetBundle(BuildTarget.StandaloneOSX);
	}
	
    [MenuItem("Tools/AssetBundles/Build AssetBundle/iOS")]
	static void BuildAssetBundleForIOS()
	{
		BuildAssetBundle(BuildTarget.iOS);
	}

    [MenuItem("Tools/AssetBundles/Build AssetBundle/Android")]
	static void BuildAssetBundleForAndroid()
	{
		BuildAssetBundle(BuildTarget.Android);
	}

    static void BuildAssetBundle(BuildTarget target)
    {
        string outputPath = Path.Combine(kAssetBundleDirectory, GetPlatformName(target));
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

		int totalAssetCount = 0;
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

				++totalAssetCount;
			}

			root.AppendChild(assetBundleElement);
		}
		
		root.SetAttribute("AssetCount", totalAssetCount.ToString());
		
		string filePath = Path.Combine(outputPath, "version");
		document.Save(filePath);

		CopyAssetBundlesTo(Application.streamingAssetsPath, target);
		AssetDatabase.Refresh();
    }

	private static void CopyAssetBundlesTo(string outputPath, BuildTarget target)
	{
		FileUtil.DeleteFileOrDirectory(outputPath);
		Directory.CreateDirectory(outputPath);

		string source = Path.Combine(Path.Combine(System.Environment.CurrentDirectory, kAssetBundleDirectory), GetPlatformName(target));
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

    public static string GetPlatformName(BuildTarget target)
	{
		return GetPlatformForAssetBundles(target);
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
