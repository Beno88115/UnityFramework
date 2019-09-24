using GameFramework;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetBundleBuilder : EditorWindow 
{
    public enum Platform
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

    private int m_InternalResourceVersion = 0;
    private string m_OutputDirectory = string.Empty;
    private bool m_OutputPackageSelected = false;
    private string m_OutputPackagePath = string.Empty;
    private bool m_OutputFullSelected = false;
    private string m_OutputFullPath = string.Empty;
    private bool m_OrderBuildAssetBundles = false;
    private Platform m_Platforms = Platform.Undefined;
    private bool m_UncompressedAssetBundleSelected = false;
    private bool m_ChunkBasedCompressionSelected = false;
    private bool m_DisableWriteTypeTreeSelected = false;
    private bool m_IgnoreTypeTreeChangesSelected = false;
    private bool m_AppendHashToAssetBundleNameSelected = false;
    private bool m_DeterministicAssetBundleSelected = false;
    private bool m_ForceRebuildAssetBundleSelected = false;

    [MenuItem("Tools/AssetBundles/AssetBundleBuilder")]
    private static void Open()
    {
        var window = GetWindow<AssetBundleBuilder>(true, "AssetBundle Builder", true);
        window.minSize = window.maxSize = new Vector2(700f, 570f);
    }

    private void OnEnable() 
    {
    }
    
    private void Update() 
    {
        
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
                    EditorGUI.BeginDisabledGroup(!m_OutputPackageSelected);
                    EditorGUILayout.LabelField("Output Package Path", GUILayout.Width(160f));
                    GUILayout.Label(m_OutputPackagePath);
                    EditorGUI.EndDisabledGroup();
                    m_OutputPackageSelected = EditorGUILayout.ToggleLeft("Generate", m_OutputPackageSelected, GUILayout.Width(70f));
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUI.BeginDisabledGroup(!m_OutputFullSelected);
                    EditorGUILayout.LabelField("Output Full Path", GUILayout.Width(160f));
                    GUILayout.Label(m_OutputFullPath);
                    EditorGUI.EndDisabledGroup();
                    m_OutputFullSelected = EditorGUILayout.ToggleLeft("Generate", m_OutputFullSelected, GUILayout.Width(70f));
                }
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
                        //SaveConfiguration();
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
}
