//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.UI
{
    /// <summary>
    /// 关闭界面完成事件。
    /// </summary>
    public sealed class CloseUIWindowCompleteEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化关闭界面完成事件的新实例。
        /// </summary>
        public CloseUIWindowCompleteEventArgs()
        {
            SerialId = 0;
            UIWindowAssetName = null;
            UIGroup = null;
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
        /// 获取界面所属的界面组。
        /// </summary>
        public IUIGroup UIGroup
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
        /// 创建关闭界面完成事件。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <param name="uiGroup">界面所属的界面组。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的关闭界面完成事件。</returns>
        public static CloseUIWindowCompleteEventArgs Create(int serialId, string uiWindowAssetName, IUIGroup uiGroup, object userData)
        {
            CloseUIWindowCompleteEventArgs closeUIWindowCompleteEventArgs = ReferencePool.Acquire<CloseUIWindowCompleteEventArgs>();
            closeUIWindowCompleteEventArgs.SerialId = serialId;
            closeUIWindowCompleteEventArgs.UIWindowAssetName = uiWindowAssetName;
            closeUIWindowCompleteEventArgs.UIGroup = uiGroup;
            closeUIWindowCompleteEventArgs.UserData = userData;
            return closeUIWindowCompleteEventArgs;
        }

        /// <summary>
        /// 清理关闭界面完成事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            UIWindowAssetName = null;
            UIGroup = null;
            UserData = null;
        }
    }
}
