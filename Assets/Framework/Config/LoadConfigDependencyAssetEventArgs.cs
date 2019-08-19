//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Config
{
    /// <summary>
    /// 加载数据表时加载依赖资源事件。
    /// </summary>
    public sealed class LoadConfigDependencyAssetEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载数据表时加载依赖资源事件的新实例。
        /// </summary>
        public LoadConfigDependencyAssetEventArgs()
        {
            ConfigTableAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }

        /// <summary>
        /// 获取数据表资源名称。
        /// </summary>
        public string ConfigTableAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取被加载的依赖资源名称。
        /// </summary>
        public string DependencyAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取当前已加载依赖资源数量。
        /// </summary>
        public int LoadedCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取总共加载依赖资源数量。
        /// </summary>
        public int TotalCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建加载数据表时加载依赖资源事件。
        /// </summary>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="dependencyAssetName">被加载的依赖资源名称。</param>
        /// <param name="loadedCount">当前已加载依赖资源数量。</param>
        /// <param name="totalCount">总共加载依赖资源数量。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的加载数据表时加载依赖资源事件。</returns>
        public static LoadConfigDependencyAssetEventArgs Create(string dataTableAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            LoadConfigDependencyAssetEventArgs loadConfigTableDependencyAssetEventArgs = ReferencePool.Acquire<LoadConfigDependencyAssetEventArgs>();
            loadConfigTableDependencyAssetEventArgs.ConfigTableAssetName = dataTableAssetName;
            loadConfigTableDependencyAssetEventArgs.DependencyAssetName = dependencyAssetName;
            loadConfigTableDependencyAssetEventArgs.LoadedCount = loadedCount;
            loadConfigTableDependencyAssetEventArgs.TotalCount = totalCount;
            loadConfigTableDependencyAssetEventArgs.UserData = userData;
            return loadConfigTableDependencyAssetEventArgs;
        }

        /// <summary>
        /// 清理加载数据表时加载依赖资源事件。
        /// </summary>
        public override void Clear()
        {
            ConfigTableAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }
}
