//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.UI
{
    internal sealed partial class UIModule : GameFrameworkModule, IUIModule
    {
        private sealed partial class UIGroup : IUIGroup
        {
            /// <summary>
            /// 界面组界面信息。
            /// </summary>
            private sealed class UIWindowInfo
            {
                private readonly IUIWindow m_UIWindow;
                private bool m_Paused;
                private bool m_Covered;

                /// <summary>
                /// 初始化界面组界面信息的新实例。
                /// </summary>
                /// <param name="uiWindow">界面。</param>
                public UIWindowInfo(IUIWindow uiWindow)
                {
                    if (uiWindow == null)
                    {
                        throw new GameFrameworkException("UI window is invalid.");
                    }

                    m_UIWindow = uiWindow;
                    m_Paused = true;
                    m_Covered = true;
                }

                /// <summary>
                /// 获取界面。
                /// </summary>
                public IUIWindow UIWindow
                {
                    get
                    {
                        return m_UIWindow;
                    }
                }

                /// <summary>
                /// 获取或设置界面是否暂停。
                /// </summary>
                public bool Paused
                {
                    get
                    {
                        return m_Paused;
                    }
                    set
                    {
                        m_Paused = value;
                    }
                }

                /// <summary>
                /// 获取或设置界面是否遮挡。
                /// </summary>
                public bool Covered
                {
                    get
                    {
                        return m_Covered;
                    }
                    set
                    {
                        m_Covered = value;
                    }
                }
            }
        }
    }
}
