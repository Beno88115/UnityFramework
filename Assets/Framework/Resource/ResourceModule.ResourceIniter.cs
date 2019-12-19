//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace GameFramework.Resource
{
    internal sealed partial class ResourceModule : GameFrameworkModule, IResourceModule
    {
        /// <summary>
        /// 资源初始化器。
        /// </summary>
        private sealed class ResourceIniter
        {
            private readonly ResourceModule m_ResourceModule;
            private string m_CurrentVariant;

            public GameFrameworkAction ResourceInitComplete;

            /// <summary>
            /// 初始化资源初始化器的新实例。
            /// </summary>
            /// <param name="resourceModule">资源管理器。</param>
            public ResourceIniter(ResourceModule resourceModule)
            {
                m_ResourceModule = resourceModule;
                m_CurrentVariant = null;

                ResourceInitComplete = null;
            }

            /// <summary>
            /// 关闭并清理资源初始化器。
            /// </summary>
            public void Shutdown()
            {
            }

            /// <summary>
            /// 初始化资源。
            /// </summary>
            public void InitResources(string currentVariant)
            {
                m_CurrentVariant = currentVariant;

                if (m_ResourceModule.m_ResourceHelper == null)
                {
                    throw new GameFrameworkException("Resource helper is invalid.");
                }

                if (string.IsNullOrEmpty(m_ResourceModule.m_ReadOnlyPath))
                {
                    throw new GameFrameworkException("Readonly path is invalid.");
                }

                // m_ResourceModule.m_ResourceHelper.LoadBytes(Utility.Path.GetRemotePath(m_ResourceModule.m_ReadOnlyPath, Utility.Path.GetResourceNameWithSuffix(VersionListFileName)), ParsePackageList);
                m_ResourceModule.m_ResourceHelper.LoadBytes(Path.Combine(m_ResourceModule.m_ReadOnlyPath, Utility.Path.GetResourceNameWithSuffix(VersionListFileName)), ParsePackageList);
            }

            /// <summary>
            /// 解析资源包资源列表。
            /// </summary>
            /// <param name="fileUri">版本资源列表文件路径。</param>
            /// <param name="bytes">要解析的数据。</param>
            /// <param name="errorMessage">错误信息。</param>
            private void ParsePackageList(string fileUri, byte[] bytes, string errorMessage)
            {
                if (bytes == null || bytes.Length <= 0)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Package list '{0}' is invalid, error message is '{1}'.", fileUri, string.IsNullOrEmpty(errorMessage) ? "<Empty>" : errorMessage));
                }

                try 
                {
                    string text = System.Text.Encoding.UTF8.GetString(bytes);
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(text);

                    XmlElement root = (XmlElement)xmlDocument.SelectSingleNode("AssetBundles");
                    m_ResourceModule.m_ApplicableGameVersion = root.GetAttribute("GameVersion"); 
                    m_ResourceModule.m_InternalResourceVersion = int.Parse(root.GetAttribute("VersionCode"));

                    int assetCount = int.Parse(root.GetAttribute("AssetCount"));
                    m_ResourceModule.m_AssetInfos = new Dictionary<string, AssetInfo>(assetCount);
                    int resourceCount = root.ChildNodes.Count;
                    m_ResourceModule.m_ResourceInfos = new Dictionary<ResourceName, ResourceInfo>(resourceCount, new ResourceNameComparer());
                    ResourceLength[] resourceLengths = new ResourceLength[resourceCount];

                    LoadType loadType = (LoadType)int.Parse(root.GetAttribute("LoadType"));

                    for (int i = 0; i < resourceCount; i++)
                    {
                        XmlElement resourceElement = (XmlElement)root.ChildNodes[i];

                        string name = resourceElement.GetAttribute("Name");
                        string variant = resourceElement.GetAttribute("Variant");
                        ResourceName resourceName = new ResourceName(name, variant);

                        int length = int.Parse(resourceElement.GetAttribute("Length"));
                        int hashCode = int.Parse(resourceElement.GetAttribute("HashCode"));
                        byte[] hashCodeBytes = new byte[4];
                        Utility.Converter.GetBytes(hashCode, hashCodeBytes);
                        resourceLengths[i] = new ResourceLength(resourceName, length, length);

                        int assetNamesCount = resourceElement.ChildNodes.Count;
                        for (int j = 0; j < assetNamesCount; j++)
                        {
                            XmlElement assetElement = (XmlElement)resourceElement.ChildNodes[j];
                            string assetName = assetElement.GetAttribute("Name");

                            int dependencyAssetNamesCount = assetElement.ChildNodes.Count;
                            string[] dependencyAssetNames = new string[dependencyAssetNamesCount];
                            for (int k = 0; k < dependencyAssetNamesCount; k++)
                            {
                                XmlElement dependencyElement = (XmlElement)assetElement.ChildNodes[k];
                                dependencyAssetNames[k] = dependencyElement.GetAttribute("Name");
                            }

                            if (string.IsNullOrEmpty(variant) || variant == m_CurrentVariant)
                            {
                                m_ResourceModule.m_AssetInfos.Add(assetName, new AssetInfo(assetName, resourceName, dependencyAssetNames));
                            }
                        }

                        if (string.IsNullOrEmpty(variant) || variant == m_CurrentVariant)
                        {
                            ProcessResourceInfo(resourceName, loadType, length, hashCode);
                        }
                    }

                    ResourceGroup defaultResourceGroup = m_ResourceModule.GetOrAddResourceGroup(string.Empty);
                    for (int i = 0; i < resourceCount; i++)
                    {
                        if (resourceLengths[i].ResourceName.Variant == null || resourceLengths[i].ResourceName.Variant == m_CurrentVariant)
                        {
                            defaultResourceGroup.AddResource(resourceLengths[i].ResourceName, resourceLengths[i].Length, resourceLengths[i].ZipLength);
                        }
                    }

                    // int resourceGroupCount = binaryReader.ReadInt32();
                    // for (int i = 0; i < resourceGroupCount; i++)
                    // {
                    //     string resourceGroupName = m_ResourceModule.GetEncryptedString(binaryReader, encryptBytes);
                    //     ResourceGroup resourceGroup = m_ResourceModule.GetOrAddResourceGroup(resourceGroupName);
                    //     int resourceGroupResourceCount = binaryReader.ReadInt32();
                    //     for (int j = 0; j < resourceGroupResourceCount; j++)
                    //     {
                    //         ushort index = binaryReader.ReadUInt16();
                    //         if (index >= resourceCount)
                    //         {
                    //             throw new GameFrameworkException(Utility.Text.Format("Package index '{0}' is invalid, resource count is '{1}'.", index.ToString(), resourceCount.ToString()));
                    //         }

                    //         if (resourceLengths[index].ResourceName.Variant == null || resourceLengths[index].ResourceName.Variant == m_CurrentVariant)
                    //         {
                    //             resourceGroup.AddResource(resourceLengths[index].ResourceName, resourceLengths[index].Length, resourceLengths[index].ZipLength);
                    //         }
                    //     }
                    // }
                    
                    ResourceInitComplete();
                }
                catch (Exception exception)
                {
                    if (exception is GameFrameworkException)
                    {
                        throw;
                    }
                    throw new GameFrameworkException(Utility.Text.Format("Parse package list exception '{0}'.", exception.Message), exception);
                }
            }

            private void ProcessResourceInfo(ResourceName resourceName, LoadType loadType, int length, int hashCode)
            {
                if (m_ResourceModule.m_ResourceInfos.ContainsKey(resourceName))
                {
                    throw new GameFrameworkException(Utility.Text.Format("Resource info '{0}' is already exist.", resourceName.FullName));
                }

                m_ResourceModule.m_ResourceInfos.Add(resourceName, new ResourceInfo(resourceName, loadType, length, hashCode, true));
            }
        }
    }
}
