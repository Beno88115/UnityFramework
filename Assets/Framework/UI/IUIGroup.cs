//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;

namespace GameFramework.UI
{
    /// <summary>
    /// 界面组接口。
    /// </summary>
    public interface IUIGroup
    {
        /// <summary>
        /// 获取界面组名称。
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// 获取或设置界面组深度。
        /// </summary>
        int Depth
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置界面组是否暂停。
        /// </summary>
        bool Pause
        {
            get;
            set;
        }

        /// <summary>
        /// 获取界面组中界面数量。
        /// </summary>
        int UIWindowCount
        {
            get;
        }

        /// <summary>
        /// 获取当前界面。
        /// </summary>
        IUIWindow CurrentUIWindow
        {
            get;
        }

        /// <summary>
        /// 获取界面组辅助器。
        /// </summary>
        IUIGroupHelper Helper
        {
            get;
        }

        /// <summary>
        /// 界面组中是否存在界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>界面组中是否存在界面。</returns>
        bool HasUIWindow(int serialId);

        /// <summary>
        /// 界面组中是否存在界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <returns>界面组中是否存在界面。</returns>
        bool HasUIWindow(string uiWindowAssetName);

        /// <summary>
        /// 从界面组中获取界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>要获取的界面。</returns>
        IUIWindow GetUIWindow(int serialId);

        /// <summary>
        /// 从界面组中获取界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <returns>要获取的界面。</returns>
        IUIWindow GetUIWindow(string uiWindowAssetName);

        /// <summary>
        /// 从界面组中获取界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <returns>要获取的界面。</returns>
        IUIWindow[] GetUIWindows(string uiWindowAssetName);

        /// <summary>
        /// 从界面组中获取界面。
        /// </summary>
        /// <param name="uiWindowAssetName">界面资源名称。</param>
        /// <param name="results">要获取的界面。</param>
        void GetUIWindows(string uiWindowAssetName, List<IUIWindow> results);

        /// <summary>
        /// 从界面组中获取所有界面。
        /// </summary>
        /// <returns>界面组中的所有界面。</returns>
        IUIWindow[] GetAllUIWindows();

        /// <summary>
        /// 从界面组中获取所有界面。
        /// </summary>
        /// <param name="results">界面组中的所有界面。</param>
        void GetAllUIWindows(List<IUIWindow> results);
    }
}
