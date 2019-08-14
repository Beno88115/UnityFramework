//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;

namespace GameFramework.UI
{
    internal sealed partial class UIManager : GameFrameworkModule, IUIManager
    {
        /// <summary>
        /// 界面组。
        /// </summary>
        private sealed partial class UIGroup : IUIGroup
        {
            private readonly string m_Name;
            private int m_Depth;
            private bool m_Pause;
            private readonly IUIGroupHelper m_UIGroupHelper;
            private readonly GameFrameworkLinkedList<UIWindowInfo> m_UIWindowInfos;
            private LinkedListNode<UIWindowInfo> m_CachedNode;

            /// <summary>
            /// 初始化界面组的新实例。
            /// </summary>
            /// <param name="name">界面组名称。</param>
            /// <param name="depth">界面组深度。</param>
            /// <param name="uiGroupHelper">界面组辅助器。</param>
            public UIGroup(string name, int depth, IUIGroupHelper uiGroupHelper)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new GameFrameworkException("UI group name is invalid.");
                }

                if (uiGroupHelper == null)
                {
                    throw new GameFrameworkException("UI group helper is invalid.");
                }

                m_Name = name;
                m_Pause = false;
                m_UIGroupHelper = uiGroupHelper;
                m_UIWindowInfos = new GameFrameworkLinkedList<UIWindowInfo>();
                m_CachedNode = null;
                Depth = depth;
            }

            /// <summary>
            /// 获取界面组名称。
            /// </summary>
            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            /// <summary>
            /// 获取或设置界面组深度。
            /// </summary>
            public int Depth
            {
                get
                {
                    return m_Depth;
                }
                set
                {
                    if (m_Depth == value)
                    {
                        return;
                    }

                    m_Depth = value;
                    m_UIGroupHelper.SetDepth(m_Depth);
                    Refresh();
                }
            }

            /// <summary>
            /// 获取或设置界面组是否暂停。
            /// </summary>
            public bool Pause
            {
                get
                {
                    return m_Pause;
                }
                set
                {
                    if (m_Pause == value)
                    {
                        return;
                    }

                    m_Pause = value;
                    Refresh();
                }
            }

            /// <summary>
            /// 获取界面组中界面数量。
            /// </summary>
            public int UIWindowCount
            {
                get
                {
                    return m_UIWindowInfos.Count;
                }
            }

            /// <summary>
            /// 获取当前界面。
            /// </summary>
            public IUIWindow CurrentUIWindow
            {
                get
                {
                    return m_UIWindowInfos.First != null ? m_UIWindowInfos.First.Value.UIWindow : null;
                }
            }

            /// <summary>
            /// 获取界面组辅助器。
            /// </summary>
            public IUIGroupHelper Helper
            {
                get
                {
                    return m_UIGroupHelper;
                }
            }

            /// <summary>
            /// 界面组轮询。
            /// </summary>
            /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
            /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                LinkedListNode<UIWindowInfo> current = m_UIWindowInfos.First;
                while (current != null)
                {
                    if (current.Value.Paused)
                    {
                        break;
                    }

                    m_CachedNode = current.Next;
                    current.Value.UIWindow.OnUpdate(elapseSeconds, realElapseSeconds);
                    current = m_CachedNode;
                    m_CachedNode = null;
                }
            }

            /// <summary>
            /// 界面组中是否存在界面。
            /// </summary>
            /// <param name="serialId">界面序列编号。</param>
            /// <returns>界面组中是否存在界面。</returns>
            public bool HasUIWindow(int serialId)
            {
                foreach (UIWindowInfo uiWindowInfo in m_UIWindowInfos)
                {
                    if (uiWindowInfo.UIWindow.SerialId == serialId)
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// 界面组中是否存在界面。
            /// </summary>
            /// <param name="uiWindowAssetName">界面资源名称。</param>
            /// <returns>界面组中是否存在界面。</returns>
            public bool HasUIWindow(string uiWindowAssetName)
            {
                if (string.IsNullOrEmpty(uiWindowAssetName))
                {
                    throw new GameFrameworkException("UI window asset name is invalid.");
                }

                foreach (UIWindowInfo uiWindowInfo in m_UIWindowInfos)
                {
                    if (uiWindowInfo.UIWindow.UIWindowAssetName == uiWindowAssetName)
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// 从界面组中获取界面。
            /// </summary>
            /// <param name="serialId">界面序列编号。</param>
            /// <returns>要获取的界面。</returns>
            public IUIWindow GetUIWindow(int serialId)
            {
                foreach (UIWindowInfo uiWindowInfo in m_UIWindowInfos)
                {
                    if (uiWindowInfo.UIWindow.SerialId == serialId)
                    {
                        return uiWindowInfo.UIWindow;
                    }
                }

                return null;
            }

            /// <summary>
            /// 从界面组中获取界面。
            /// </summary>
            /// <param name="uiWindowAssetName">界面资源名称。</param>
            /// <returns>要获取的界面。</returns>
            public IUIWindow GetUIWindow(string uiWindowAssetName)
            {
                if (string.IsNullOrEmpty(uiWindowAssetName))
                {
                    throw new GameFrameworkException("UI window asset name is invalid.");
                }

                foreach (UIWindowInfo uiWindowInfo in m_UIWindowInfos)
                {
                    if (uiWindowInfo.UIWindow.UIWindowAssetName == uiWindowAssetName)
                    {
                        return uiWindowInfo.UIWindow;
                    }
                }

                return null;
            }

            /// <summary>
            /// 从界面组中获取界面。
            /// </summary>
            /// <param name="uiWindowAssetName">界面资源名称。</param>
            /// <returns>要获取的界面。</returns>
            public IUIWindow[] GetUIWindows(string uiWindowAssetName)
            {
                if (string.IsNullOrEmpty(uiWindowAssetName))
                {
                    throw new GameFrameworkException("UI window asset name is invalid.");
                }

                List<IUIWindow> results = new List<IUIWindow>();
                foreach (UIWindowInfo uiWindowInfo in m_UIWindowInfos)
                {
                    if (uiWindowInfo.UIWindow.UIWindowAssetName == uiWindowAssetName)
                    {
                        results.Add(uiWindowInfo.UIWindow);
                    }
                }

                return results.ToArray();
            }

            /// <summary>
            /// 从界面组中获取界面。
            /// </summary>
            /// <param name="uiWindowAssetName">界面资源名称。</param>
            /// <param name="results">要获取的界面。</param>
            public void GetUIWindows(string uiWindowAssetName, List<IUIWindow> results)
            {
                if (string.IsNullOrEmpty(uiWindowAssetName))
                {
                    throw new GameFrameworkException("UI window asset name is invalid.");
                }

                if (results == null)
                {
                    throw new GameFrameworkException("Results is invalid.");
                }

                results.Clear();
                foreach (UIWindowInfo uiWindowInfo in m_UIWindowInfos)
                {
                    if (uiWindowInfo.UIWindow.UIWindowAssetName == uiWindowAssetName)
                    {
                        results.Add(uiWindowInfo.UIWindow);
                    }
                }
            }

            /// <summary>
            /// 从界面组中获取所有界面。
            /// </summary>
            /// <returns>界面组中的所有界面。</returns>
            public IUIWindow[] GetAllUIWindows()
            {
                List<IUIWindow> results = new List<IUIWindow>();
                foreach (UIWindowInfo uiWindowInfo in m_UIWindowInfos)
                {
                    results.Add(uiWindowInfo.UIWindow);
                }

                return results.ToArray();
            }

            /// <summary>
            /// 从界面组中获取所有界面。
            /// </summary>
            /// <param name="results">界面组中的所有界面。</param>
            public void GetAllUIWindows(List<IUIWindow> results)
            {
                if (results == null)
                {
                    throw new GameFrameworkException("Results is invalid.");
                }

                results.Clear();
                foreach (UIWindowInfo uiWindowInfo in m_UIWindowInfos)
                {
                    results.Add(uiWindowInfo.UIWindow);
                }
            }

            /// <summary>
            /// 往界面组增加界面。
            /// </summary>
            /// <param name="uiWindow">要增加的界面。</param>
            public void AddUIWindow(IUIWindow uiWindow)
            {
                UIWindowInfo uiWindowInfo = new UIWindowInfo(uiWindow);
                m_UIWindowInfos.AddFirst(uiWindowInfo);
            }

            /// <summary>
            /// 从界面组移除界面。
            /// </summary>
            /// <param name="uiWindow">要移除的界面。</param>
            public void RemoveUIWindow(IUIWindow uiWindow)
            {
                UIWindowInfo uiWindowInfo = GetUIWindowInfo(uiWindow);
                if (uiWindowInfo == null)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Can not find UI window info for serial id '{0}', UI window asset name is '{1}'.", uiWindow.SerialId.ToString(), uiWindow.UIWindowAssetName));
                }

                if (!uiWindowInfo.Covered)
                {
                    uiWindowInfo.Covered = true;
                    uiWindow.OnCover();
                }

                if (!uiWindowInfo.Paused)
                {
                    uiWindowInfo.Paused = true;
                    uiWindow.OnPause();
                }

                if (m_CachedNode != null && m_CachedNode.Value.UIWindow == uiWindow)
                {
                    m_CachedNode = m_CachedNode.Next;
                }

                if (!m_UIWindowInfos.Remove(uiWindowInfo))
                {
                    throw new GameFrameworkException(Utility.Text.Format("UI group '{0}' not exists specified UI window '[{1}]{2}'.", m_Name, uiWindow.SerialId.ToString(), uiWindow.UIWindowAssetName));
                }
            }

            /// <summary>
            /// 激活界面。
            /// </summary>
            /// <param name="uiWindow">要激活的界面。</param>
            /// <param name="userData">用户自定义数据。</param>
            public void RefocusUIWindow(IUIWindow uiWindow, object userData)
            {
                UIWindowInfo uiWindowInfo = GetUIWindowInfo(uiWindow);
                if (uiWindowInfo == null)
                {
                    throw new GameFrameworkException("Can not find UI window info.");
                }

                m_UIWindowInfos.Remove(uiWindowInfo);
                m_UIWindowInfos.AddFirst(uiWindowInfo);
            }

            /// <summary>
            /// 刷新界面组。
            /// </summary>
            public void Refresh()
            {
                LinkedListNode<UIWindowInfo> current = m_UIWindowInfos.First;
                bool pause = m_Pause;
                bool cover = false;
                int depth = UIWindowCount;
                while (current != null)
                {
                    LinkedListNode<UIWindowInfo> next = current.Next;
                    current.Value.UIWindow.OnDepthChanged(Depth, depth--);
                    if (current.Value == null)
                    {
                        return;
                    }

                    if (pause)
                    {
                        if (!current.Value.Covered)
                        {
                            current.Value.Covered = true;
                            current.Value.UIWindow.OnCover();
                            if (current.Value == null)
                            {
                                return;
                            }
                        }

                        if (!current.Value.Paused)
                        {
                            current.Value.Paused = true;
                            current.Value.UIWindow.OnPause();
                            if (current.Value == null)
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (current.Value.Paused)
                        {
                            current.Value.Paused = false;
                            current.Value.UIWindow.OnResume();
                            if (current.Value == null)
                            {
                                return;
                            }
                        }

                        if (current.Value.UIWindow.PauseCoveredUIWindow)
                        {
                            pause = true;
                        }

                        if (cover)
                        {
                            if (!current.Value.Covered)
                            {
                                current.Value.Covered = true;
                                current.Value.UIWindow.OnCover();
                                if (current.Value == null)
                                {
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (current.Value.Covered)
                            {
                                current.Value.Covered = false;
                                current.Value.UIWindow.OnReveal();
                                if (current.Value == null)
                                {
                                    return;
                                }
                            }

                            cover = true;
                        }
                    }

                    current = next;
                }
            }

            internal void InternalGetUIWindows(string uiWindowAssetName, List<IUIWindow> results)
            {
                foreach (UIWindowInfo uiWindowInfo in m_UIWindowInfos)
                {
                    if (uiWindowInfo.UIWindow.UIWindowAssetName == uiWindowAssetName)
                    {
                        results.Add(uiWindowInfo.UIWindow);
                    }
                }
            }

            internal void InternalGetAllUIWindows(List<IUIWindow> results)
            {
                foreach (UIWindowInfo uiWindowInfo in m_UIWindowInfos)
                {
                    results.Add(uiWindowInfo.UIWindow);
                }
            }

            private UIWindowInfo GetUIWindowInfo(IUIWindow uiWindow)
            {
                if (uiWindow == null)
                {
                    throw new GameFrameworkException("UI window is invalid.");
                }

                foreach (UIWindowInfo uiWindowInfo in m_UIWindowInfos)
                {
                    if (uiWindowInfo.UIWindow == uiWindow)
                    {
                        return uiWindowInfo;
                    }
                }

                return null;
            }
        }
    }
}
