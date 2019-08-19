using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Resource
{
    internal sealed partial class ResourceModule : GameFrameworkModule, IResourceModule
    {
        /// <summary>
        /// 资源模拟器。
        /// </summary>
        private sealed partial class ResourceSimulator
        {
            private readonly ResourceModule m_ResourceModule;
            private string m_CurrentVariant;

            /// <summary>
            /// 初始化资源检查器的新实例。
            /// </summary>
            /// <param name="resourceModule">资源管理器。</param>
            public ResourceSimulator(ResourceModule resourceModule)
            {
                m_ResourceModule = resourceModule;
                m_CurrentVariant = null;
            }

            /// <summary>
            /// 初始化资源。
            /// </summary>
            public void InitResources(string currentVariant)
            {
                m_CurrentVariant = currentVariant;

                string[] assetBundleNames = m_ResourceModule.m_ResourceSimulationHelper.GetAllResourceNames();
                if (assetBundleNames.Length != 0)
                {
                    m_ResourceModule.m_AssetInfos = new Dictionary<string, AssetInfo>();
                    m_ResourceModule.m_ResourceInfos = new Dictionary<ResourceName, ResourceInfo>(); 
                    for (int i = 0; i < assetBundleNames.Length; ++i)
                    {
                        string assetBundleName = assetBundleNames[i];
                        string[] assetPaths = m_ResourceModule.m_ResourceSimulationHelper.GetAssetPaths(assetBundleName);
                        if (assetPaths.Length != 0)
                        {
                            string variant = m_ResourceModule.m_ResourceSimulationHelper.GetVariantFromAssetName(assetPaths[0]);
                            ResourceName resourceName = new ResourceName(assetBundleName, variant);

                            for (int j = 0; j < assetPaths.Length; ++j)
                            {
                                if (string.IsNullOrEmpty(variant) || variant == m_CurrentVariant)
                                {
                                    string assetPath = assetPaths[j];
                                    string assetName = assetPath.Substring(assetPath.LastIndexOf("/") + 1).Split('.')[0];
                                    m_ResourceModule.m_AssetInfos.Add(assetName, new AssetInfo(assetName, resourceName, null));
                                }
                            }
                            ProcessResourceInfo(resourceName, 0, 0, 0);
                        }
                    }
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
