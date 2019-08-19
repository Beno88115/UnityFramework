//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.ObjectPool;

namespace GameFramework.UI
{
    internal sealed partial class UIModule : GameFrameworkModule, IUIModule
    {
        /// <summary>
        /// 界面实例对象。
        /// </summary>
        private sealed class UIWindowInstanceObject : ObjectBase
        {
            private readonly object m_UIWindowAsset;
            private readonly IUIWindowHelper m_UIWindowHelper;

            public UIWindowInstanceObject(string name, object uiWindowAsset, object uiWindowInstance, IUIWindowHelper uiWindowHelper)
                : base(name, uiWindowInstance)
            {
                if (uiWindowAsset == null)
                {
                    throw new GameFrameworkException("UI window asset is invalid.");
                }

                if (uiWindowHelper == null)
                {
                    throw new GameFrameworkException("UI window helper is invalid.");
                }

                m_UIWindowAsset = uiWindowAsset;
                m_UIWindowHelper = uiWindowHelper;
            }

            protected internal override void Release(bool isShutdown)
            {
                m_UIWindowHelper.ReleaseUIWindow(m_UIWindowAsset, Target);
            }
        }
    }
}
