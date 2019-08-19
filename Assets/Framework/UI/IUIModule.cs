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
    /// 界面管理器接口。
    /// </summary>
    public interface IUIModule
    {
        /// <summary>
        /// 获取界面组数量。
        /// </summary>
        int UIGroupCount
        {
            get;
        }

        /// <summary>
        /// 获取或设置界面实例对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        float InstanceAutoReleaseInterval
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置界面实例对象池的容量。
        /// </summary>
        int InstanceCapacity
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置界面实例对象池对象过期秒数。
        /// </summary>
        float InstanceExpireTime
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置界面实例对象池的优先级。
        /// </summary>
        int InstancePriority
        {
            get;
            set;
        }

        /// <summary>
        /// 打开界面成功事件。
        /// </summary>
        event EventHandler<OpenUIWindowSuccessEventArgs> OpenUIWindowSuccess;

        /// <summary>
        /// 打开界面失败事件。
        /// </summary>
        event EventHandler<OpenUIWindowFailureEventArgs> OpenUIWindowFailure;

        /// <summary>
        /// 打开界面更新事件。
        /// </summary>
        event EventHandler<OpenUIWindowUpdateEventArgs> OpenUIWindowUpdate;

        /// <summary>
        /// 打开界面时加载依赖资源事件。
        /// </summary>
        event EventHandler<OpenUIWindowDependencyAssetEventArgs> OpenUIWindowDependencyAsset;

        /// <summary>
        /// 关闭界面完成事件。
        /// </summary>
        event EventHandler<CloseUIWindowCompleteEventArgs> CloseUIWindowComplete;

        /// <summary>
        /// 设置对象池管理器。
        /// </summary>
        /// <param name="objectPoolModule">对象池管理器。</param>
        void SetObjectPoolModule(IObjectPoolModule objectPoolModule);

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceModule">资源管理器。</param>
        void SetResourceModule(IResourceModule resourceModule);

        /// <summary>
        /// 设置界面辅助器。
        /// </summary>
        /// <param name="uiWindowHelper">界面辅助器。</param>
        void SetUIWindowHelper(IUIWindowHelper uiWindowHelper);

        /// <summary>
        /// 是否存在界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>是否存在界面组。</returns>
        bool HasUIGroup(string uiGroupName);

        /// <summary>
        /// 获取界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>要获取的界面组。</returns>
        IUIGroup GetUIGroup(string uiGroupName);

        /// <summary>
        /// 获取所有界面组。
        /// </summary>
        /// <returns>所有界面组。</returns>
        IUIGroup[] GetAllUIGroups();

        /// <summary>
        /// 获取所有界面组。
        /// </summary>
        /// <param name="results">所有界面组。</param>
        void GetAllUIGroups(List<IUIGroup> results);

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="uiGroupHelper">界面组辅助器。</param>
        /// <returns>是否增加界面组成功。</returns>
        bool AddUIGroup(string uiGroupName, IUIGroupHelper uiGroupHelper);

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="uiGroupDepth">界面组深度。</param>
        /// <param name="uiGroupHelper">界面组辅助器。</param>
        /// <returns>是否增加界面组成功。</returns>
        bool AddUIGroup(string uiGroupName, int uiGroupDepth, IUIGroupHelper uiGroupHelper);

        /// <summary>
        /// 是否存在界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>是否存在界面。</returns>
        bool HasUIWindow(int serialId);

        /// <summary>
        /// 是否存在界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <returns>是否存在界面。</returns>
        bool HasUIWindow(string uiWindowAssetName);

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>要获取的界面。</returns>
        IUIWindow GetUIWindow(int serialId);

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <returns>要获取的界面。</returns>
        IUIWindow GetUIWindow(string uiWindowAssetName);

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <returns>要获取的界面。</returns>
        IUIWindow[] GetUIWindows(string uiWindowAssetName);

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <param name="results">要获取的界面。</param>
        void GetUIWindows(string uiWindowAssetName, List<IUIWindow> results);

        /// <summary>
        /// 获取所有已加载的界面。
        /// </summary>
        /// <returns>所有已加载的界面。</returns>
        IUIWindow[] GetAllLoadedUIWindows();

        /// <summary>
        /// 获取所有已加载的界面。
        /// </summary>
        /// <param name="results">所有已加载的界面。</param>
        void GetAllLoadedUIWindows(List<IUIWindow> results);

        /// <summary>
        /// 获取所有正在加载界面的序列编号。
        /// </summary>
        /// <returns>所有正在加载界面的序列编号。</returns>
        int[] GetAllLoadingUIWindowSerialIds();

        /// <summary>
        /// 获取所有正在加载界面的序列编号。
        /// </summary>
        /// <param name="results">所有正在加载界面的序列编号。</param>
        void GetAllLoadingUIWindowSerialIds(List<int> results);

        /// <summary>
        /// 是否正在加载界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>是否正在加载界面。</returns>
        bool IsLoadingUIWindow(int serialId);

        /// <summary>
        /// 是否正在加载界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <returns>是否正在加载界面。</returns>
        bool IsLoadingUIWindow(string uiWindowAssetName);

        /// <summary>
        /// 是否是合法的界面。
        /// </summary>
        /// <param name="uiWindow">界面。</param>
        /// <returns>界面是否合法。</returns>
        bool IsValidUIWindow(IUIWindow uiWindow);

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>界面的序列编号。</returns>
        int OpenUIWindow(string uiWindowAssetName, string uiGroupName);

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="priority">加载界面资源的优先级。</param>
        /// <returns>界面的序列编号。</returns>
        int OpenUIWindow(string uiWindowAssetName, string uiGroupName, int priority);

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="pauseCoveredUIWindow">是否暂停被覆盖的界面。</param>
        /// <returns>界面的序列编号。</returns>
        int OpenUIWindow(string uiWindowAssetName, string uiGroupName, bool pauseCoveredUIWindow);

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        int OpenUIWindow(string uiWindowAssetName, string uiGroupName, object userData);

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="priority">加载界面资源的优先级。</param>
        /// <param name="pauseCoveredUIWindow">是否暂停被覆盖的界面。</param>
        /// <returns>界面的序列编号。</returns>
        int OpenUIWindow(string uiWindowAssetName, string uiGroupName, int priority, bool pauseCoveredUIWindow);

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="priority">加载界面资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        int OpenUIWindow(string uiWindowAssetName, string uiGroupName, int priority, object userData);

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="pauseCoveredUIWindow">是否暂停被覆盖的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        int OpenUIWindow(string uiWindowAssetName, string uiGroupName, bool pauseCoveredUIWindow, object userData);

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="priority">加载界面资源的优先级。</param>
        /// <param name="pauseCoveredUIWindow">是否暂停被覆盖的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        int OpenUIWindow(string uiWindowAssetName, string uiGroupName, int priority, bool pauseCoveredUIWindow, object userData);

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="serialId">要关闭界面的序列编号。</param>
        void CloseUIWindow(int serialId);

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="serialId">要关闭界面的序列编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        void CloseUIWindow(int serialId, object userData);

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiWindow">要关闭的界面。</param>
        void CloseUIWindow(IUIWindow uiWindow);

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiWindow">要关闭的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        void CloseUIWindow(IUIWindow uiWindow, object userData);

        /// <summary>
        /// 关闭所有已加载的界面。
        /// </summary>
        void CloseAllLoadedUIWindows();

        /// <summary>
        /// 关闭所有已加载的界面。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        void CloseAllLoadedUIWindows(object userData);

        /// <summary>
        /// 关闭所有正在加载的界面。
        /// </summary>
        void CloseAllLoadingUIWindows();

        /// <summary>
        /// 激活界面。
        /// </summary>
        /// <param name="uiWindow">要激活的界面。</param>
        void RefocusUIWindow(IUIWindow uiWindow);

        /// <summary>
        /// 激活界面。
        /// </summary>
        /// <param name="uiWindow">要激活的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        void RefocusUIWindow(IUIWindow uiWindow, object userData);

        /// <summary>
        /// 设置界面实例是否被加锁。
        /// </summary>
        /// <param name="uiWindowInstance">要设置是否被加锁的界面实例。</param>
        /// <param name="locked">界面实例是否被加锁。</param>
        void SetUIWindowInstanceLocked(object uiWindowInstance, bool locked);

        /// <summary>
        /// 设置界面实例的优先级。
        /// </summary>
        /// <param name="uiWindowInstance">要设置优先级的界面实例。</param>
        /// <param name="priority">界面实例优先级。</param>
        void SetUIWindowInstancePriority(object uiWindowInstance, int priority);
    }
}
