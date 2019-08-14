//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.UI
{
    /// <summary>
    /// 打开界面成功事件。
    /// </summary>
    public sealed class OpenUIWindowSuccessEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化打开界面成功事件的新实例。
        /// </summary>
        public OpenUIWindowSuccessEventArgs()
        {
            UIWindow = null;
            Duration = 0f;
            UserData = null;
        }

        /// <summary>
        /// 获取打开成功的界面。
        /// </summary>
        public IUIWindow UIWindow
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取加载持续时间。
        /// </summary>
        public float Duration
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
        /// 创建打开界面成功事件。
        /// </summary>
        /// <param name="uiWindow">加载成功的界面。</param>
        /// <param name="duration">加载持续时间。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的打开界面成功事件。</returns>
        public static OpenUIWindowSuccessEventArgs Create(IUIWindow uiWindow, float duration, object userData)
        {
            OpenUIWindowSuccessEventArgs openUIWindowSuccessEventArgs = ReferencePool.Acquire<OpenUIWindowSuccessEventArgs>();
            openUIWindowSuccessEventArgs.UIWindow = uiWindow;
            openUIWindowSuccessEventArgs.Duration = duration;
            openUIWindowSuccessEventArgs.UserData = userData;
            return openUIWindowSuccessEventArgs;
        }

        /// <summary>
        /// 清理打开界面成功事件。
        /// </summary>
        public override void Clear()
        {
            UIWindow = null;
            Duration = 0f;
            UserData = null;
        }
    }
}
