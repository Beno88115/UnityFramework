using GameFramework;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Xml;

public class AssetBundleBuilder : EditorWindow 
{
    enum Platform
    {
        Undefined = 0,

        /// <summary>
        /// 微软 Windows 32 位。
        /// </summary>
        Windows = 1 << 0,

        /// <summary>
        /// 微软 Windows 64 位。
        /// </summary>
        Windows64 = 1 << 1,

        /// <summary>
        /// 苹果 macOS。
        /// </summary>
        MacOS = 1 << 2,

        /// <summary>
        /// Linux 32 位。
        /// </summary>
        Linux = 1 << 3,

        /// <summary>
        /// Linux 64 位。
        /// </summary>
        Linux64 = 1 << 4,

        /// <summary>
        /// Linux 通用。
        /// </summary>
        LinuxUniversal = 1 << 5,

        /// <summary>
        /// 苹果 iOS。
        /// </summary>
        IOS = 1 << 6,

        /// <summary>
        /// 谷歌 Android。
        /// </summary>
        Android = 1 << 7,

        /// <summary>
        /// 微软 Windows Store。
        /// </summary>
        WindowsStore = 1 << 8,

        /// <summary>
        /// WebGL。
        /// </summary>
        WebGL = 1 << 9,
    }

    private const string kDefaultAssetBundleDirectoryName = "AssetBundles";

    private int m_InternalResourceVersion = 0;
    private string m_OutputDirectory = null;
    private string m_OutputPackagePath = string.Empty;
    private bool m_OrderBuildAssetBundles = false;
    private Platform m_Platforms = Platform.Undefined;
    private bool m_UncompressedAssetBundleSelected = false;
    private bool m_ChunkBasedCompressionSelected = false;
    private bool m_DisableWriteTypeTreeSelected = false;
    private bool m_IgnoreTypeTreeChangesSelected = false;
    private bool m_AppendHashToAssetBundleNameSelected = false;
    private bool m_DeterministicAssetBundleSelected = false;
    private bool m_ForceRebuildAssetBundleSelected = false;

    [MenuItem("Tools/AssetBundles/AssetBundleBuilder", false, 1)]
    private static void Open()
    {
        var window = GetWindow<AssetBundleBuilder>(true, "AssetBundle Builder", true);
        window.minSize = window.maxSize = new Vector2(700f, 402f);
    }

    private void OnEnable() 
    {
        LoadConfiguration();
        m_OutputDirectory = m_OutputDirectory ?? Path.Combine(System.Environment.CurrentDirectory, kDefaultAssetBundleDirectoryName);
    }
    
    private void Update() 
    {
        if (m_OrderBuildAssetBundles) {
            m_OrderBuildAssetBundles = false;
            BuildAssetBundles();
        }
    }

    private void OnGUI() 
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(position.width), GUILayout.Height(position.height));
        {
            GUILayout.Space(5f);
            EditorGUILayout.LabelField("Environment Information", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Product Name", GUILayout.Width(200f));
                    EditorGUILayout.LabelField(Application.productName);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Company Name", GUILayout.Width(200f));
                    EditorGUILayout.LabelField(Application.companyName);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Game Identifier", GUILayout.Width(200f));
                    EditorGUILayout.LabelField(Application.identifier);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Applicable Game Version", GUILayout.Width(200f));
                    EditorGUILayout.LabelField(Application.version);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(5f);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.LabelField("Platforms", EditorStyles.boldLabel);
                    EditorGUILayout.BeginHorizontal("box");
                    {
                        EditorGUILayout.BeginVertical();
                        {
                            DrawPlatform(Platform.Windows, "Microsoft Windows (x86)");
                            DrawPlatform(Platform.Windows64, "Microsoft Windows (x64)");
                            DrawPlatform(Platform.MacOS, "Apple OSX");
                            DrawPlatform(Platform.IOS, "Apple iOS");
                            DrawPlatform(Platform.Android, "Google Android");
                            DrawPlatform(Platform.WindowsStore, "Microsoft Windows Store");
                            DrawPlatform(Platform.WebGL, "WebGL");
                        }
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.BeginVertical();
                        {
                            DrawPlatform(Platform.Linux, "Linux (x86)");
                            DrawPlatform(Platform.Linux64, "Linux (x64)");
                            DrawPlatform(Platform.LinuxUniversal, "Linux (Universal)");
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.LabelField("AssetBundle Options", EditorStyles.boldLabel);
                    EditorGUILayout.BeginVertical("box");
                    {
                        bool uncompressedAssetBundleSelected = EditorGUILayout.ToggleLeft("Uncompressed AssetBundle", m_UncompressedAssetBundleSelected);
                        if (m_UncompressedAssetBundleSelected != uncompressedAssetBundleSelected)
                        {
                            m_UncompressedAssetBundleSelected = uncompressedAssetBundleSelected;
                            if (m_UncompressedAssetBundleSelected)
                            {
                                m_ChunkBasedCompressionSelected = false;
                            }
                        }

                        bool disableWriteTypeTreeSelected = EditorGUILayout.ToggleLeft("Disable Write TypeTree", m_DisableWriteTypeTreeSelected);
                        if (m_DisableWriteTypeTreeSelected != disableWriteTypeTreeSelected)
                        {
                            m_DisableWriteTypeTreeSelected = disableWriteTypeTreeSelected;
                            if (m_DisableWriteTypeTreeSelected)
                            {
                                m_IgnoreTypeTreeChangesSelected = false;
                            }
                        }

                        m_DeterministicAssetBundleSelected = EditorGUILayout.ToggleLeft("Deterministic AssetBundle", m_DeterministicAssetBundleSelected);
                        m_ForceRebuildAssetBundleSelected = EditorGUILayout.ToggleLeft("Force Rebuild AssetBundle", m_ForceRebuildAssetBundleSelected);

                        bool ignoreTypeTreeChangesSelected = EditorGUILayout.ToggleLeft("Ignore TypeTree Changes", m_IgnoreTypeTreeChangesSelected);
                        if (m_IgnoreTypeTreeChangesSelected != ignoreTypeTreeChangesSelected)
                        {
                            m_IgnoreTypeTreeChangesSelected = ignoreTypeTreeChangesSelected;
                            if (m_IgnoreTypeTreeChangesSelected)
                            {
                                m_DisableWriteTypeTreeSelected = false;
                            }
                        }

                        EditorGUI.BeginDisabledGroup(true);
                        {
                            m_AppendHashToAssetBundleNameSelected = EditorGUILayout.ToggleLeft("Append Hash To AssetBundle Name", m_AppendHashToAssetBundleNameSelected);
                        }
                        EditorGUI.EndDisabledGroup();

                        bool chunkBasedCompressionSelected = EditorGUILayout.ToggleLeft("Chunk Based Compression", m_ChunkBasedCompressionSelected);
                        if (m_ChunkBasedCompressionSelected != chunkBasedCompressionSelected)
                        {
                            m_ChunkBasedCompressionSelected = chunkBasedCompressionSelected;
                            if (m_ChunkBasedCompressionSelected)
                            {
                                m_UncompressedAssetBundleSelected = false;
                            }
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5f);
            EditorGUILayout.LabelField("Build", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Internal Resource Version", GUILayout.Width(160f));
                    m_InternalResourceVersion = EditorGUILayout.IntField(m_InternalResourceVersion);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Resource Version", GUILayout.Width(160f));
                    GUILayout.Label(Utility.Text.Format("{0} ({1})", Application.version, m_InternalResourceVersion.ToString()));
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Output Directory", GUILayout.Width(160f));
                    m_OutputDirectory = EditorGUILayout.TextField(m_OutputDirectory);
                    if (GUILayout.Button("Browse...", GUILayout.Width(80f)))
                    {
                        BrowseOutputDirectory();
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                {
                    m_OutputPackagePath = Path.Combine(m_OutputDirectory, Utility.Text.Format("{0}_{1}", Application.version.Replace(".", "_"), m_InternalResourceVersion));
                    EditorGUILayout.LabelField("Output Package Path", GUILayout.Width(160f));
                    GUILayout.Label(m_OutputPackagePath);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                GUILayout.Space(2f);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUI.BeginDisabledGroup(m_Platforms == Platform.Undefined);
                    {
                        if (GUILayout.Button("Start Build AssetBundles"))
                        {
                            m_OrderBuildAssetBundles = true;
                        }
                    }
                    EditorGUI.EndDisabledGroup();
                    if (GUILayout.Button("Save", GUILayout.Width(80f)))
                    {
                        SaveConfiguration();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }
    }

    private void BrowseOutputDirectory()
    {
        string directory = EditorUtility.OpenFolderPanel("Select Output Directory", m_OutputDirectory, string.Empty);
        if (!string.IsNullOrEmpty(directory)) {
            m_OutputDirectory = directory;
        }
    }

    private void DrawPlatform(Platform platform, string platformName)
    {
        if (EditorGUILayout.ToggleLeft(platformName, (m_Platforms & platform) != 0)) {
            m_Platforms |= platform;
        }
        else {
            m_Platforms &= ~platform;
        }
    }

    private void LoadConfiguration()
    {
        try {
            XmlDocument document = new XmlDocument();
            document.Load(Path.Combine(System.Environment.CurrentDirectory, "AssetBundleBuilder.xml"));

            XmlElement root = (XmlElement)document.SelectSingleNode("AssetBundleBuilder");
            XmlElement element = (XmlElement)root.SelectSingleNode("Platforms");
            if (element != null) {
                m_Platforms = (Platform)int.Parse(element.InnerText);
            }

            element = (XmlElement)root.SelectSingleNode("InternalResourceVersion");
            if (element != null) {
                m_InternalResourceVersion = int.Parse(element.InnerText);
            }

            element = (XmlElement)root.SelectSingleNode("OutputDirectory");
            if (element != null) {
                m_OutputDirectory = element.InnerText;
            }

            element = (XmlElement)root.SelectSingleNode("UncompressedAssetBundleSelected");
            if (element != null) {
                m_UncompressedAssetBundleSelected = bool.Parse(element.InnerText);
            }

            element = (XmlElement)root.SelectSingleNode("ChunkBasedCompressionSelected");
            if (element != null) {
                m_ChunkBasedCompressionSelected = bool.Parse(element.InnerText);
            }

            element = (XmlElement)root.SelectSingleNode("DisableWriteTypeTreeSelected");
            if (element != null) {
                m_DisableWriteTypeTreeSelected = bool.Parse(element.InnerText);
            }

            element = (XmlElement)root.SelectSingleNode("IgnoreTypeTreeChangesSelected");
            if (element != null) {
                m_IgnoreTypeTreeChangesSelected = bool.Parse(element.InnerText);
            }
            
            element = (XmlElement)root.SelectSingleNode("AppendHashToAssetBundleNameSelected");
            if (element != null) {
                m_AppendHashToAssetBundleNameSelected = bool.Parse(element.InnerText);
            }

            element = (XmlElement)root.SelectSingleNode("DeterministicAssetBundleSelected");
            if (element != null) {
                m_DeterministicAssetBundleSelected = bool.Parse(element.InnerText);
            }

            element = (XmlElement)root.SelectSingleNode("ForceRebuildAssetBundleSelected");
            if (element != null) {
                m_ForceRebuildAssetBundleSelected = bool.Parse(element.InnerText);
            }
        }
        catch (System.Exception ex) {
            Debug.LogError(ex);
        }
    }

    private void SaveConfiguration()
    {
        XmlDocument document = new XmlDocument();
		document.AppendChild(document.CreateXmlDeclaration("1.0", "UTF-8", null));

        XmlElement root = document.CreateElement("AssetBundleBuilder");
        document.AppendChild(root);

        XmlElement element = document.CreateElement("Platforms");
        element.InnerText = ((int)m_Platforms).ToString();
        root.AppendChild(element);

        element = document.CreateElement("InternalResourceVersion");
        element.InnerText = m_InternalResourceVersion.ToString();
        root.AppendChild(element);

        element = document.CreateElement("OutputDirectory");
        element.InnerText = m_OutputDirectory;
        root.AppendChild(element);

        element = document.CreateElement("UncompressedAssetBundleSelected");
        element.InnerText = m_UncompressedAssetBundleSelected.ToString();
        root.AppendChild(element);

        element = document.CreateElement("ChunkBasedCompressionSelected");
        element.InnerText = m_ChunkBasedCompressionSelected.ToString();
        root.AppendChild(element);

        element = document.CreateElement("DisableWriteTypeTreeSelected");
        element.InnerText = m_DisableWriteTypeTreeSelected.ToString();
        root.AppendChild(element);

        element = document.CreateElement("IgnoreTypeTreeChangesSelected");
        element.InnerText = m_IgnoreTypeTreeChangesSelected.ToString();
        root.AppendChild(element);

        element = document.CreateElement("AppendHashToAssetBundleNameSelected");
        element.InnerText = m_AppendHashToAssetBundleNameSelected.ToString();
        root.AppendChild(element);

        element = document.CreateElement("DeterministicAssetBundleSelected");
        element.InnerText = m_DeterministicAssetBundleSelected.ToString();
        root.AppendChild(element);

        element = document.CreateElement("ForceRebuildAssetBundleSelected");
        element.InnerText = m_ForceRebuildAssetBundleSelected.ToString();
        root.AppendChild(element);

        document.Save(Path.Combine(System.Environment.CurrentDirectory, "AssetBundleBuilder.xml"));
    }

    private void BuildAssetBundles()
    {
        // TODO:
    }
}
