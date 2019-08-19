//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Config
{
    /// <summary>
    /// 加载数据表失败事件。
    /// </summary>
    public sealed class LoadConfigFailureEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载数据表失败事件的新实例。
        /// </summary>
        public LoadConfigFailureEventArgs()
        {
            ConfigTableAssetName = null;
            LoadType = LoadType.Text;
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 获取数据表资源名称。
        /// </summary>
        public string ConfigTableAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取数据表加载方式。
        /// </summary>
        public LoadType LoadType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
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
        /// 创建加载数据表失败事件。
        /// </summary>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="loadType">数据表加载方式。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的加载数据表失败事件。</returns>
        public static LoadConfigFailureEventArgs Create(string dataTableAssetName, LoadType loadType, string errorMessage, object userData)
        {
            LoadConfigFailureEventArgs loadConfigTableFailureEventArgs = ReferencePool.Acquire<LoadConfigFailureEventArgs>();
            loadConfigTableFailureEventArgs.ConfigTableAssetName = dataTableAssetName;
            loadConfigTableFailureEventArgs.LoadType = loadType;
            loadConfigTableFailureEventArgs.ErrorMessage = errorMessage;
            loadConfigTableFailureEventArgs.UserData = userData;
            return loadConfigTableFailureEventArgs;
        }

        /// <summary>
        /// 清理加载数据表失败事件。
        /// </summary>
        public override void Clear()
        {
            ConfigTableAssetName = null;
            LoadType = LoadType.Text;
            ErrorMessage = null;
            UserData = null;
        }
    }
}
