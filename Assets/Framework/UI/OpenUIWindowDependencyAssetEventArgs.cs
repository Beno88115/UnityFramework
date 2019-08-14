//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.UI
{
    /// <summary>
    /// 打开界面时加载依赖资源事件。
    /// </summary>
    public sealed class OpenUIWindowDependencyAssetEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化打开界面时加载依赖资源事件的新实例。
        /// </summary>
        public OpenUIWindowDependencyAssetEventArgs()
        {
            SerialId = 0;
            UIWindowAssetName = null;
            UIGroupName = null;
            PauseCoveredUIWindow = false;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }

        /// <summary>
        /// 获取界面序列编号。
        /// </summary>
        public int SerialId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取界面资源名称。
        /// </summary>
        public string UIWindowAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取界面组名称。
        /// </summary>
        public string UIGroupName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否暂停被覆盖的界面。
        /// </summary>
        public bool PauseCoveredUIWindow
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
        /// 创建打开界面时加载依赖资源事件。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="pauseCoveredUIWindow">是否暂停被覆盖的界面。</param>
        /// <param name="dependencyAssetName">被加载的依赖资源名称。</param>
        /// <param name="loadedCount">当前已加载依赖资源数量。</param>
        /// <param name="totalCount">总共加载依赖资源数量。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的打开界面时加载依赖资源事件。</returns>
        public static OpenUIWindowDependencyAssetEventArgs Create(int serialId, string uiWindowAssetName, string uiGroupName, bool pauseCoveredUIWindow, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            OpenUIWindowDependencyAssetEventArgs openUIWindowDependencyAssetEventArgs = ReferencePool.Acquire<OpenUIWindowDependencyAssetEventArgs>();
            openUIWindowDependencyAssetEventArgs.SerialId = serialId;
            openUIWindowDependencyAssetEventArgs.UIWindowAssetName = uiWindowAssetName;
            openUIWindowDependencyAssetEventArgs.UIGroupName = uiGroupName;
            openUIWindowDependencyAssetEventArgs.PauseCoveredUIWindow = pauseCoveredUIWindow;
            openUIWindowDependencyAssetEventArgs.DependencyAssetName = dependencyAssetName;
            openUIWindowDependencyAssetEventArgs.LoadedCount = loadedCount;
            openUIWindowDependencyAssetEventArgs.TotalCount = totalCount;
            openUIWindowDependencyAssetEventArgs.UserData = userData;
            return openUIWindowDependencyAssetEventArgs;
        }

        /// <summary>
        /// 清理打开界面时加载依赖资源事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            UIWindowAssetName = null;
            UIGroupName = null;
            PauseCoveredUIWindow = false;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }
}
