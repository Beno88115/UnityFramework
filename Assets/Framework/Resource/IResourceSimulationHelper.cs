using System;

namespace GameFramework.Resource
{
    /// <summary>
    /// 模拟资源辅助器接口。
    /// </summary>
    public interface IResourceSimulationHelper
    {
        /// <summary>
        /// 直接从指定文件路径读取资源对象。
        /// </summary>
        /// <param name="assetName">资源名称。</param>
        /// <param name="resourceName">资源包名称。</param>
        object LoadObject(string assetName, string resourceName, Type type);

        /// <summary>
        /// 获取所有的资源包名称。
        /// </summary>
        string[] GetAllResourceNames();

        /// <summary>
        /// 通过资源包名称获取所有的资源路径。
        /// </summary>
        /// <param name="resourceName">资源包名称。</param>
        string[] GetAssetPaths(string resourceName);

        /// <summary>
        /// 获取资源的变体。
        /// </summary>
        /// <param name="resourceName">资源包名称。</param>
        string GetVariantFromAssetName(string resourceName);
    }
}