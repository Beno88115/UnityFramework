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
        private sealed class OpenUIWindowInfo : IReference
        {
            private int m_SerialId;
            private UIGroup m_UIGroup;
            private bool m_PauseCoveredUIWindow;
            private object m_UserData;

            public OpenUIWindowInfo()
            {
                m_SerialId = 0;
                m_UIGroup = null;
                m_PauseCoveredUIWindow = false;
                m_UserData = null;
            }

            public int SerialId
            {
                get
                {
                    return m_SerialId;
                }
            }

            public UIGroup UIGroup
            {
                get
                {
                    return m_UIGroup;
                }
            }

            public bool PauseCoveredUIWindow
            {
                get
                {
                    return m_PauseCoveredUIWindow;
                }
            }

            public object UserData
            {
                get
                {
                    return m_UserData;
                }
            }

            public static OpenUIWindowInfo Create(int serialId, UIGroup uiGroup, bool pauseCoveredUIWindow, object userData)
            {
                OpenUIWindowInfo openUIWindowInfo = ReferencePool.Acquire<OpenUIWindowInfo>();
                openUIWindowInfo.m_SerialId = serialId;
                openUIWindowInfo.m_UIGroup = uiGroup;
                openUIWindowInfo.m_PauseCoveredUIWindow = pauseCoveredUIWindow;
                openUIWindowInfo.m_UserData = userData;
                return openUIWindowInfo;
            }

            public void Clear()
            {
                m_SerialId = 0;
                m_UIGroup = null;
                m_PauseCoveredUIWindow = false;
                m_UserData = null;
            }
        }
    }
}
