//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.ObjectPool;
using GameFramework.Resource;
using System;
using System.Collections.Generic;

namespace GameFramework.UI
{
    /// <summary>
    /// 界面管理器。
    /// </summary>
    internal sealed partial class UIManager : GameFrameworkModule, IUIManager
    {
        private readonly Dictionary<string, UIGroup> m_UIGroups;
        private readonly Dictionary<int, string> m_UIWindowsBeingLoaded;
        private readonly HashSet<int> m_UIWindowsToReleaseOnLoad;
        private readonly Queue<IUIWindow> m_RecycleQueue;
        private readonly LoadAssetCallbacks m_LoadAssetCallbacks;
        private IObjectPoolManager m_ObjectPoolManager;
        private IResourceManager m_ResourceManager;
        private IObjectPool<UIWindowInstanceObject> m_InstancePool;
        private IUIWindowHelper m_UIWindowHelper;
        private int m_Serial;
        private EventHandler<OpenUIWindowSuccessEventArgs> m_OpenUIWindowSuccessEventHandler;
        private EventHandler<OpenUIWindowFailureEventArgs> m_OpenUIWindowFailureEventHandler;
        private EventHandler<OpenUIWindowUpdateEventArgs> m_OpenUIWindowUpdateEventHandler;
        private EventHandler<OpenUIWindowDependencyAssetEventArgs> m_OpenUIWindowDependencyAssetEventHandler;
        private EventHandler<CloseUIWindowCompleteEventArgs> m_CloseUIWindowCompleteEventHandler;

        /// <summary>
        /// 初始化界面管理器的新实例。
        /// </summary>
        public UIManager()
        {
            m_UIGroups = new Dictionary<string, UIGroup>();
            m_UIWindowsBeingLoaded = new Dictionary<int, string>();
            m_UIWindowsToReleaseOnLoad = new HashSet<int>();
            m_RecycleQueue = new Queue<IUIWindow>();
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadUIWindowSuccessCallback, LoadUIWindowFailureCallback, LoadUIWindowUpdateCallback, LoadUIWindowDependencyAssetCallback);
            m_ObjectPoolManager = null;
            m_ResourceManager = null;
            m_InstancePool = null;
            m_UIWindowHelper = null;
            m_Serial = 0;
            m_OpenUIWindowSuccessEventHandler = null;
            m_OpenUIWindowFailureEventHandler = null;
            m_OpenUIWindowUpdateEventHandler = null;
            m_OpenUIWindowDependencyAssetEventHandler = null;
            m_CloseUIWindowCompleteEventHandler = null;
        }

        /// <summary>
        /// 获取界面组数量。
        /// </summary>
        public int UIGroupCount
        {
            get
            {
                return m_UIGroups.Count;
            }
        }

        /// <summary>
        /// 获取或设置界面实例对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        public float InstanceAutoReleaseInterval
        {
            get
            {
                return m_InstancePool.AutoReleaseInterval;
            }
            set
            {
                m_InstancePool.AutoReleaseInterval = value;
            }
        }

        /// <summary>
        /// 获取或设置界面实例对象池的容量。
        /// </summary>
        public int InstanceCapacity
        {
            get
            {
                return m_InstancePool.Capacity;
            }
            set
            {
                m_InstancePool.Capacity = value;
            }
        }

        /// <summary>
        /// 获取或设置界面实例对象池对象过期秒数。
        /// </summary>
        public float InstanceExpireTime
        {
            get
            {
                return m_InstancePool.ExpireTime;
            }
            set
            {
                m_InstancePool.ExpireTime = value;
            }
        }

        /// <summary>
        /// 获取或设置界面实例对象池的优先级。
        /// </summary>
        public int InstancePriority
        {
            get
            {
                return m_InstancePool.Priority;
            }
            set
            {
                m_InstancePool.Priority = value;
            }
        }

        /// <summary>
        /// 打开界面成功事件。
        /// </summary>
        public event EventHandler<OpenUIWindowSuccessEventArgs> OpenUIWindowSuccess
        {
            add
            {
                m_OpenUIWindowSuccessEventHandler += value;
            }
            remove
            {
                m_OpenUIWindowSuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// 打开界面失败事件。
        /// </summary>
        public event EventHandler<OpenUIWindowFailureEventArgs> OpenUIWindowFailure
        {
            add
            {
                m_OpenUIWindowFailureEventHandler += value;
            }
            remove
            {
                m_OpenUIWindowFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// 打开界面更新事件。
        /// </summary>
        public event EventHandler<OpenUIWindowUpdateEventArgs> OpenUIWindowUpdate
        {
            add
            {
                m_OpenUIWindowUpdateEventHandler += value;
            }
            remove
            {
                m_OpenUIWindowUpdateEventHandler -= value;
            }
        }

        /// <summary>
        /// 打开界面时加载依赖资源事件。
        /// </summary>
        public event EventHandler<OpenUIWindowDependencyAssetEventArgs> OpenUIWindowDependencyAsset
        {
            add
            {
                m_OpenUIWindowDependencyAssetEventHandler += value;
            }
            remove
            {
                m_OpenUIWindowDependencyAssetEventHandler -= value;
            }
        }

        /// <summary>
        /// 关闭界面完成事件。
        /// </summary>
        public event EventHandler<CloseUIWindowCompleteEventArgs> CloseUIWindowComplete
        {
            add
            {
                m_CloseUIWindowCompleteEventHandler += value;
            }
            remove
            {
                m_CloseUIWindowCompleteEventHandler -= value;
            }
        }

        /// <summary>
        /// 界面管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            while (m_RecycleQueue.Count > 0)
            {
                IUIWindow uiWindow = m_RecycleQueue.Dequeue();
                uiWindow.OnRecycle();
                m_InstancePool.Unspawn(uiWindow.Handle);
            }

            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                uiGroup.Value.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 关闭并清理界面管理器。
        /// </summary>
        internal override void Shutdown()
        {
            CloseAllLoadedUIWindows();
            m_UIGroups.Clear();
            m_UIWindowsBeingLoaded.Clear();
            m_UIWindowsToReleaseOnLoad.Clear();
            m_RecycleQueue.Clear();
        }

        /// <summary>
        /// 设置对象池管理器。
        /// </summary>
        /// <param name="objectPoolManager">对象池管理器。</param>
        public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
        {
            if (objectPoolManager == null)
            {
                throw new GameFrameworkException("Object pool manager is invalid.");
            }

            m_ObjectPoolManager = objectPoolManager;
            m_InstancePool = m_ObjectPoolManager.CreateSingleSpawnObjectPool<UIWindowInstanceObject>("UI Instance Pool");
        }

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        public void SetResourceManager(IResourceManager resourceManager)
        {
            if (resourceManager == null)
            {
                throw new GameFrameworkException("Resource manager is invalid.");
            }

            m_ResourceManager = resourceManager;
        }

        /// <summary>
        /// 设置界面辅助器。
        /// </summary>
        /// <param name="uiWindowHelper">界面辅助器。</param>
        public void SetUIWindowHelper(IUIWindowHelper uiWindowHelper)
        {
            if (uiWindowHelper == null)
            {
                throw new GameFrameworkException("UI window helper is invalid.");
            }

            m_UIWindowHelper = uiWindowHelper;
        }

        /// <summary>
        /// 是否存在界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>是否存在界面组。</returns>
        public bool HasUIGroup(string uiGroupName)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw new GameFrameworkException("UI group name is invalid.");
            }

            return m_UIGroups.ContainsKey(uiGroupName);
        }

        /// <summary>
        /// 获取界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>要获取的界面组。</returns>
        public IUIGroup GetUIGroup(string uiGroupName)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw new GameFrameworkException("UI group name is invalid.");
            }

            UIGroup uiGroup = null;
            if (m_UIGroups.TryGetValue(uiGroupName, out uiGroup))
            {
                return uiGroup;
            }

            return null;
        }

        /// <summary>
        /// 获取所有界面组。
        /// </summary>
        /// <returns>所有界面组。</returns>
        public IUIGroup[] GetAllUIGroups()
        {
            int index = 0;
            IUIGroup[] results = new IUIGroup[m_UIGroups.Count];
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                results[index++] = uiGroup.Value;
            }

            return results;
        }

        /// <summary>
        /// 获取所有界面组。
        /// </summary>
        /// <param name="results">所有界面组。</param>
        public void GetAllUIGroups(List<IUIGroup> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                results.Add(uiGroup.Value);
            }
        }

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="uiGroupHelper">界面组辅助器。</param>
        /// <returns>是否增加界面组成功。</returns>
        public bool AddUIGroup(string uiGroupName, IUIGroupHelper uiGroupHelper)
        {
            return AddUIGroup(uiGroupName, 0, uiGroupHelper);
        }

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="uiGroupDepth">界面组深度。</param>
        /// <param name="uiGroupHelper">界面组辅助器。</param>
        /// <returns>是否增加界面组成功。</returns>
        public bool AddUIGroup(string uiGroupName, int uiGroupDepth, IUIGroupHelper uiGroupHelper)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw new GameFrameworkException("UI group name is invalid.");
            }

            if (uiGroupHelper == null)
            {
                throw new GameFrameworkException("UI group helper is invalid.");
            }

            if (HasUIGroup(uiGroupName))
            {
                return false;
            }

            m_UIGroups.Add(uiGroupName, new UIGroup(uiGroupName, uiGroupDepth, uiGroupHelper));

            return true;
        }

        /// <summary>
        /// 是否存在界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>是否存在界面。</returns>
        public bool HasUIWindow(int serialId)
        {
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                if (uiGroup.Value.HasUIWindow(serialId))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 是否存在界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <returns>是否存在界面。</returns>
        public bool HasUIWindow(string uiWindowAssetName)
        {
            if (string.IsNullOrEmpty(uiWindowAssetName))
            {
                throw new GameFrameworkException("UI window asset name is invalid.");
            }

            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                if (uiGroup.Value.HasUIWindow(uiWindowAssetName))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>要获取的界面。</returns>
        public IUIWindow GetUIWindow(int serialId)
        {
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                IUIWindow uiWindow = uiGroup.Value.GetUIWindow(serialId);
                if (uiWindow != null)
                {
                    return uiWindow;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <returns>要获取的界面。</returns>
        public IUIWindow GetUIWindow(string uiWindowAssetName)
        {
            if (string.IsNullOrEmpty(uiWindowAssetName))
            {
                throw new GameFrameworkException("UI window asset name is invalid.");
            }

            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                IUIWindow uiWindow = uiGroup.Value.GetUIWindow(uiWindowAssetName);
                if (uiWindow != null)
                {
                    return uiWindow;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取界面。
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
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                results.AddRange(uiGroup.Value.GetUIWindows(uiWindowAssetName));
            }

            return results.ToArray();
        }

        /// <summary>
        /// 获取界面。
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
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                uiGroup.Value.InternalGetUIWindows(uiWindowAssetName, results);
            }
        }

        /// <summary>
        /// 获取所有已加载的界面。
        /// </summary>
        /// <returns>所有已加载的界面。</returns>
        public IUIWindow[] GetAllLoadedUIWindows()
        {
            List<IUIWindow> results = new List<IUIWindow>();
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                results.AddRange(uiGroup.Value.GetAllUIWindows());
            }

            return results.ToArray();
        }

        /// <summary>
        /// 获取所有已加载的界面。
        /// </summary>
        /// <param name="results">所有已加载的界面。</param>
        public void GetAllLoadedUIWindows(List<IUIWindow> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                uiGroup.Value.InternalGetAllUIWindows(results);
            }
        }

        /// <summary>
        /// 获取所有正在加载界面的序列编号。
        /// </summary>
        /// <returns>所有正在加载界面的序列编号。</returns>
        public int[] GetAllLoadingUIWindowSerialIds()
        {
            int index = 0;
            int[] results = new int[m_UIWindowsBeingLoaded.Count];
            foreach (KeyValuePair<int, string> uiWindowBeingLoaded in m_UIWindowsBeingLoaded)
            {
                results[index++] = uiWindowBeingLoaded.Key;
            }

            return results;
        }

        /// <summary>
        /// 获取所有正在加载界面的序列编号。
        /// </summary>
        /// <param name="results">所有正在加载界面的序列编号。</param>
        public void GetAllLoadingUIWindowSerialIds(List<int> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<int, string> uiWindowBeingLoaded in m_UIWindowsBeingLoaded)
            {
                results.Add(uiWindowBeingLoaded.Key);
            }
        }

        /// <summary>
        /// 是否正在加载界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>是否正在加载界面。</returns>
        public bool IsLoadingUIWindow(int serialId)
        {
            return m_UIWindowsBeingLoaded.ContainsKey(serialId);
        }

        /// <summary>
        /// 是否正在加载界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <returns>是否正在加载界面。</returns>
        public bool IsLoadingUIWindow(string uiWindowAssetName)
        {
            if (string.IsNullOrEmpty(uiWindowAssetName))
            {
                throw new GameFrameworkException("UI window asset name is invalid.");
            }

            return m_UIWindowsBeingLoaded.ContainsValue(uiWindowAssetName);
        }

        /// <summary>
        /// 是否是合法的界面。
        /// </summary>
        /// <param name="uiWindow">界面。</param>
        /// <returns>界面是否合法。</returns>
        public bool IsValidUIWindow(IUIWindow uiWindow)
        {
            if (uiWindow == null)
            {
                return false;
            }

            return HasUIWindow(uiWindow.SerialId);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>界面的序列编号。</returns>
        public int OpenUIWindow(string uiWindowAssetName, string uiGroupName)
        {
            return OpenUIWindow(uiWindowAssetName, uiGroupName, Constant.DefaultPriority, false, null);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="priority">加载界面资源的优先级。</param>
        /// <returns>界面的序列编号。</returns>
        public int OpenUIWindow(string uiWindowAssetName, string uiGroupName, int priority)
        {
            return OpenUIWindow(uiWindowAssetName, uiGroupName, priority, false, null);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="pauseCoveredUIWindow">是否暂停被覆盖的界面。</param>
        /// <returns>界面的序列编号。</returns>
        public int OpenUIWindow(string uiWindowAssetName, string uiGroupName, bool pauseCoveredUIWindow)
        {
            return OpenUIWindow(uiWindowAssetName, uiGroupName, Constant.DefaultPriority, pauseCoveredUIWindow, null);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        public int OpenUIWindow(string uiWindowAssetName, string uiGroupName, object userData)
        {
            return OpenUIWindow(uiWindowAssetName, uiGroupName, Constant.DefaultPriority, false, userData);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="priority">加载界面资源的优先级。</param>
        /// <param name="pauseCoveredUIWindow">是否暂停被覆盖的界面。</param>
        /// <returns>界面的序列编号。</returns>
        public int OpenUIWindow(string uiWindowAssetName, string uiGroupName, int priority, bool pauseCoveredUIWindow)
        {
            return OpenUIWindow(uiWindowAssetName, uiGroupName, priority, pauseCoveredUIWindow, null);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="priority">加载界面资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        public int OpenUIWindow(string uiWindowAssetName, string uiGroupName, int priority, object userData)
        {
            return OpenUIWindow(uiWindowAssetName, uiGroupName, priority, false, userData);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="pauseCoveredUIWindow">是否暂停被覆盖的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        public int OpenUIWindow(string uiWindowAssetName, string uiGroupName, bool pauseCoveredUIWindow, object userData)
        {
            return OpenUIWindow(uiWindowAssetName, uiGroupName, Constant.DefaultPriority, pauseCoveredUIWindow, userData);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="priority">加载界面资源的优先级。</param>
        /// <param name="pauseCoveredUIWindow">是否暂停被覆盖的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        public int OpenUIWindow(string uiWindowAssetName, string uiGroupName, int priority, bool pauseCoveredUIWindow, object userData)
        {
            if (m_ResourceManager == null)
            {
                throw new GameFrameworkException("You must set resource manager first.");
            }

            if (m_UIWindowHelper == null)
            {
                throw new GameFrameworkException("You must set UI window helper first.");
            }

            if (string.IsNullOrEmpty(uiWindowAssetName))
            {
                throw new GameFrameworkException("UI window asset name is invalid.");
            }

            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw new GameFrameworkException("UI group name is invalid.");
            }

            UIGroup uiGroup = (UIGroup)GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("UI group '{0}' is not exist.", uiGroupName));
            }

            int serialId = m_Serial++;
            UIWindowInstanceObject uiWindowInstanceObject = m_InstancePool.Spawn(uiWindowAssetName);
            if (uiWindowInstanceObject == null)
            {
                m_UIWindowsBeingLoaded.Add(serialId, uiWindowAssetName);
                m_ResourceManager.LoadAsset(uiWindowAssetName, priority, m_LoadAssetCallbacks, OpenUIWindowInfo.Create(serialId, uiGroup, pauseCoveredUIWindow, userData));
            }
            else
            {
                InternalOpenUIWindow(serialId, uiWindowAssetName, uiGroup, uiWindowInstanceObject.Target, pauseCoveredUIWindow, false, 0f, userData);
            }

            return serialId;
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="serialId">要关闭界面的序列编号。</param>
        public void CloseUIWindow(int serialId)
        {
            CloseUIWindow(serialId, null);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="serialId">要关闭界面的序列编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void CloseUIWindow(int serialId, object userData)
        {
            if (IsLoadingUIWindow(serialId))
            {
                m_UIWindowsToReleaseOnLoad.Add(serialId);
                m_UIWindowsBeingLoaded.Remove(serialId);
                return;
            }

            IUIWindow uiWindow = GetUIWindow(serialId);
            if (uiWindow == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Can not find UI window '{0}'.", serialId.ToString()));
            }

            CloseUIWindow(uiWindow, userData);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiWindow">要关闭的界面。</param>
        public void CloseUIWindow(IUIWindow uiWindow)
        {
            CloseUIWindow(uiWindow, null);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiWindow">要关闭的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void CloseUIWindow(IUIWindow uiWindow, object userData)
        {
            if (uiWindow == null)
            {
                throw new GameFrameworkException("UI window is invalid.");
            }

            UIGroup uiGroup = (UIGroup)uiWindow.UIGroup;
            if (uiGroup == null)
            {
                throw new GameFrameworkException("UI group is invalid.");
            }

            uiGroup.RemoveUIWindow(uiWindow);
            uiWindow.OnClose(userData);
            uiGroup.Refresh();

            if (m_CloseUIWindowCompleteEventHandler != null)
            {
                CloseUIWindowCompleteEventArgs closeUIWindowCompleteEventArgs = CloseUIWindowCompleteEventArgs.Create(uiWindow.SerialId, uiWindow.UIWindowAssetName, uiGroup, userData);
                m_CloseUIWindowCompleteEventHandler(this, closeUIWindowCompleteEventArgs);
                ReferencePool.Release(closeUIWindowCompleteEventArgs);
            }

            m_RecycleQueue.Enqueue(uiWindow);
        }

        /// <summary>
        /// 关闭所有已加载的界面。
        /// </summary>
        public void CloseAllLoadedUIWindows()
        {
            CloseAllLoadedUIWindows(null);
        }

        /// <summary>
        /// 关闭所有已加载的界面。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        public void CloseAllLoadedUIWindows(object userData)
        {
            IUIWindow[] uiWindows = GetAllLoadedUIWindows();
            foreach (IUIWindow uiWindow in uiWindows)
            {
                if (!HasUIWindow(uiWindow.SerialId))
                {
                    continue;
                }

                CloseUIWindow(uiWindow, userData);
            }
        }

        /// <summary>
        /// 关闭所有正在加载的界面。
        /// </summary>
        public void CloseAllLoadingUIWindows()
        {
            foreach (KeyValuePair<int, string> uiWindowBeingLoaded in m_UIWindowsBeingLoaded)
            {
                m_UIWindowsToReleaseOnLoad.Add(uiWindowBeingLoaded.Key);
            }

            m_UIWindowsBeingLoaded.Clear();
        }

        /// <summary>
        /// 激活界面。
        /// </summary>
        /// <param name="uiWindow">要激活的界面。</param>
        public void RefocusUIWindow(IUIWindow uiWindow)
        {
            RefocusUIWindow(uiWindow, null);
        }

        /// <summary>
        /// 激活界面。
        /// </summary>
        /// <param name="uiWindow">要激活的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void RefocusUIWindow(IUIWindow uiWindow, object userData)
        {
            if (uiWindow == null)
            {
                throw new GameFrameworkException("UI window is invalid.");
            }

            UIGroup uiGroup = (UIGroup)uiWindow.UIGroup;
            if (uiGroup == null)
            {
                throw new GameFrameworkException("UI group is invalid.");
            }

            uiGroup.RefocusUIWindow(uiWindow, userData);
            uiGroup.Refresh();
            uiWindow.OnRefocus(userData);
        }

        /// <summary>
        /// 设置界面实例是否被加锁。
        /// </summary>
        /// <param name="uiWindowInstance">要设置是否被加锁的界面实例。</param>
        /// <param name="locked">界面实例是否被加锁。</param>
        public void SetUIWindowInstanceLocked(object uiWindowInstance, bool locked)
        {
            if (uiWindowInstance == null)
            {
                throw new GameFrameworkException("UI window instance is invalid.");
            }

            m_InstancePool.SetLocked(uiWindowInstance, locked);
        }

        /// <summary>
        /// 设置界面实例的优先级。
        /// </summary>
        /// <param name="uiWindowInstance">要设置优先级的界面实例。</param>
        /// <param name="priority">界面实例优先级。</param>
        public void SetUIWindowInstancePriority(object uiWindowInstance, int priority)
        {
            if (uiWindowInstance == null)
            {
                throw new GameFrameworkException("UI window instance is invalid.");
            }

            m_InstancePool.SetPriority(uiWindowInstance, priority);
        }

        private void InternalOpenUIWindow(int serialId, string uiWindowAssetName, UIGroup uiGroup, object uiWindowInstance, bool pauseCoveredUIWindow, bool isNewInstance, float duration, object userData)
        {
            try
            {
                IUIWindow uiWindow = m_UIWindowHelper.CreateUIWindow(uiWindowInstance, uiGroup, userData);
                if (uiWindow == null)
                {
                    throw new GameFrameworkException("Can not create UI window in helper.");
                }

                uiWindow.OnInit(serialId, uiWindowAssetName, uiGroup, pauseCoveredUIWindow, isNewInstance, userData);
                uiGroup.AddUIWindow(uiWindow);
                uiWindow.OnOpen(userData);
                uiGroup.Refresh();

                if (m_OpenUIWindowSuccessEventHandler != null)
                {
                    OpenUIWindowSuccessEventArgs openUIWindowSuccessEventArgs = OpenUIWindowSuccessEventArgs.Create(uiWindow, duration, userData);
                    m_OpenUIWindowSuccessEventHandler(this, openUIWindowSuccessEventArgs);
                    ReferencePool.Release(openUIWindowSuccessEventArgs);
                }
            }
            catch (Exception exception)
            {
                if (m_OpenUIWindowFailureEventHandler != null)
                {
                    OpenUIWindowFailureEventArgs openUIWindowFailureEventArgs = OpenUIWindowFailureEventArgs.Create(serialId, uiWindowAssetName, uiGroup.Name, pauseCoveredUIWindow, exception.ToString(), userData);
                    m_OpenUIWindowFailureEventHandler(this, openUIWindowFailureEventArgs);
                    ReferencePool.Release(openUIWindowFailureEventArgs);
                    return;
                }

                throw;
            }
        }

        private void LoadUIWindowSuccessCallback(string uiWindowAssetName, object uiWindowAsset, float duration, object userData)
        {
            OpenUIWindowInfo openUIWindowInfo = (OpenUIWindowInfo)userData;
            if (openUIWindowInfo == null)
            {
                throw new GameFrameworkException("Open UI window info is invalid.");
            }

            if (m_UIWindowsToReleaseOnLoad.Contains(openUIWindowInfo.SerialId))
            {
                m_UIWindowsToReleaseOnLoad.Remove(openUIWindowInfo.SerialId);
                ReferencePool.Release(openUIWindowInfo);
                m_UIWindowHelper.ReleaseUIWindow(uiWindowAsset, null);
                return;
            }

            m_UIWindowsBeingLoaded.Remove(openUIWindowInfo.SerialId);
            UIWindowInstanceObject uiWindowInstanceObject = new UIWindowInstanceObject(uiWindowAssetName, uiWindowAsset, m_UIWindowHelper.InstantiateUIWindow(uiWindowAsset), m_UIWindowHelper);
            m_InstancePool.Register(uiWindowInstanceObject, true);

            InternalOpenUIWindow(openUIWindowInfo.SerialId, uiWindowAssetName, openUIWindowInfo.UIGroup, uiWindowInstanceObject.Target, openUIWindowInfo.PauseCoveredUIWindow, true, duration, openUIWindowInfo.UserData);
            ReferencePool.Release(openUIWindowInfo);
        }

        private void LoadUIWindowFailureCallback(string uiWindowAssetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            OpenUIWindowInfo openUIWindowInfo = (OpenUIWindowInfo)userData;
            if (openUIWindowInfo == null)
            {
                throw new GameFrameworkException("Open UI window info is invalid.");
            }

            if (m_UIWindowsToReleaseOnLoad.Contains(openUIWindowInfo.SerialId))
            {
                m_UIWindowsToReleaseOnLoad.Remove(openUIWindowInfo.SerialId);
                ReferencePool.Release(openUIWindowInfo);
                return;
            }

            m_UIWindowsBeingLoaded.Remove(openUIWindowInfo.SerialId);
            string appendErrorMessage = Utility.Text.Format("Load UI window failure, asset name '{0}', status '{1}', error message '{2}'.", uiWindowAssetName, status.ToString(), errorMessage);
            if (m_OpenUIWindowFailureEventHandler != null)
            {
                OpenUIWindowFailureEventArgs openUIWindowFailureEventArgs = OpenUIWindowFailureEventArgs.Create(openUIWindowInfo.SerialId, uiWindowAssetName, openUIWindowInfo.UIGroup.Name, openUIWindowInfo.PauseCoveredUIWindow, appendErrorMessage, openUIWindowInfo.UserData);
                m_OpenUIWindowFailureEventHandler(this, openUIWindowFailureEventArgs);
                ReferencePool.Release(openUIWindowFailureEventArgs);
                ReferencePool.Release(openUIWindowInfo);
                return;
            }

            ReferencePool.Release(openUIWindowInfo);
            throw new GameFrameworkException(appendErrorMessage);
        }

        private void LoadUIWindowUpdateCallback(string uiWindowAssetName, float progress, object userData)
        {
            OpenUIWindowInfo openUIWindowInfo = (OpenUIWindowInfo)userData;
            if (openUIWindowInfo == null)
            {
                throw new GameFrameworkException("Open UI window info is invalid.");
            }

            if (m_OpenUIWindowUpdateEventHandler != null)
            {
                OpenUIWindowUpdateEventArgs openUIWindowUpdateEventArgs = OpenUIWindowUpdateEventArgs.Create(openUIWindowInfo.SerialId, uiWindowAssetName, openUIWindowInfo.UIGroup.Name, openUIWindowInfo.PauseCoveredUIWindow, progress, openUIWindowInfo.UserData);
                m_OpenUIWindowUpdateEventHandler(this, openUIWindowUpdateEventArgs);
                ReferencePool.Release(openUIWindowUpdateEventArgs);
            }
        }

        private void LoadUIWindowDependencyAssetCallback(string uiWindowAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            OpenUIWindowInfo openUIWindowInfo = (OpenUIWindowInfo)userData;
            if (openUIWindowInfo == null)
            {
                throw new GameFrameworkException("Open UI window info is invalid.");
            }

            if (m_OpenUIWindowDependencyAssetEventHandler != null)
            {
                OpenUIWindowDependencyAssetEventArgs openUIWindowDependencyAssetEventArgs = OpenUIWindowDependencyAssetEventArgs.Create(openUIWindowInfo.SerialId, uiWindowAssetName, openUIWindowInfo.UIGroup.Name, openUIWindowInfo.PauseCoveredUIWindow, dependencyAssetName, loadedCount, totalCount, openUIWindowInfo.UserData);
                m_OpenUIWindowDependencyAssetEventHandler(this, openUIWindowDependencyAssetEventArgs);
                ReferencePool.Release(openUIWindowDependencyAssetEventArgs);
            }
        }
    }
}
