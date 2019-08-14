//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.UI
{
    /// <summary>
    /// 打开界面更新事件。
    /// </summary>
    public sealed class OpenUIWindowUpdateEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化打开界面更新事件的新实例。
        /// </summary>
        public OpenUIWindowUpdateEventArgs()
        {
            SerialId = 0;
            UIWindowAssetName = null;
            UIGroupName = null;
            PauseCoveredUIWindow = false;
            Progress = 0f;
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
        /// 获取打开界面进度。
        /// </summary>
        public float Progress
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
        /// 创建打开界面更新事件。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="pauseCoveredUIWindow">是否暂停被覆盖的界面。</param>
        /// <param name="progress">打开界面进度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的打开界面更新事件。</returns>
        public static OpenUIWindowUpdateEventArgs Create(int serialId, string uiWindowAssetName, string uiGroupName, bool pauseCoveredUIWindow, float progress, object userData)
        {
            OpenUIWindowUpdateEventArgs openUIWindowUpdateEventArgs = ReferencePool.Acquire<OpenUIWindowUpdateEventArgs>();
            openUIWindowUpdateEventArgs.SerialId = serialId;
            openUIWindowUpdateEventArgs.UIWindowAssetName = uiWindowAssetName;
            openUIWindowUpdateEventArgs.UIGroupName = uiGroupName;
            openUIWindowUpdateEventArgs.PauseCoveredUIWindow = pauseCoveredUIWindow;
            openUIWindowUpdateEventArgs.Progress = progress;
            openUIWindowUpdateEventArgs.UserData = userData;
            return openUIWindowUpdateEventArgs;
        }

        /// <summary>
        /// 清理打开界面更新事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            UIWindowAssetName = null;
            UIGroupName = null;
            PauseCoveredUIWindow = false;
            Progress = 0f;
            UserData = null;
        }
    }
}
