﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Config
{
    /// <summary>
    /// 加载数据表更新事件。
    /// </summary>
    public sealed class LoadConfigUpdateEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载数据表更新事件的新实例。
        /// </summary>
        public LoadConfigUpdateEventArgs()
        {
            ConfigTableAssetName = null;
            LoadType = LoadType.Text;
            Progress = 0f;
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
        /// 获取加载数据表进度。
        /// </summary>
        public float Progress
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
        /// 创建加载数据表更新事件。
        /// </summary>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="loadType">数据表加载方式。</param>
        /// <param name="progress">加载数据表进度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的加载数据表更新事件。</returns>
        public static LoadConfigUpdateEventArgs Create(string dataTableAssetName, LoadType loadType, float progress, object userData)
        {
            LoadConfigUpdateEventArgs loadConfigTableUpdateEventArgs = ReferencePool.Acquire<LoadConfigUpdateEventArgs>();
            loadConfigTableUpdateEventArgs.ConfigTableAssetName = dataTableAssetName;
            loadConfigTableUpdateEventArgs.LoadType = loadType;
            loadConfigTableUpdateEventArgs.Progress = progress;
            loadConfigTableUpdateEventArgs.UserData = userData;
            return loadConfigTableUpdateEventArgs;
        }

        /// <summary>
        /// 清理加载数据表更新事件。
        /// </summary>
        public override void Clear()
        {
            ConfigTableAssetName = null;
            LoadType = LoadType.Text;
            Progress = 0f;
            UserData = null;
        }
    }
}
