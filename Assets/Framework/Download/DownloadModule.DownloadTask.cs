﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Download
{
    internal sealed partial class DownloadModule : GameFrameworkModule, IDownloadModule
    {
        /// <summary>
        /// 下载任务。
        /// </summary>
        private sealed class DownloadTask : ITask
        {
            private static int s_Serial = 0;

            private int m_SerialId;
            private int m_Priority;
            private bool m_Done;
            private DownloadTaskStatus m_Status;
            private string m_DownloadPath;
            private string m_DownloadUri;
            private int m_FlushSize;
            private float m_Timeout;
            private object m_UserData;

            /// <summary>
            /// 初始化下载任务的新实例。
            /// </summary>
            public DownloadTask()
            {
                m_SerialId = 0;
                m_Priority = 0;
                m_Done = false;
                m_Status = DownloadTaskStatus.Todo;
                m_DownloadPath = null;
                m_DownloadUri = null;
                m_FlushSize = 0;
                m_Timeout = 0f;
                m_UserData = null;
            }

            /// <summary>
            /// 获取下载任务的序列编号。
            /// </summary>
            public int SerialId
            {
                get
                {
                    return m_SerialId;
                }
            }

            /// <summary>
            /// 获取下载任务的优先级。
            /// </summary>
            public int Priority
            {
                get
                {
                    return m_Priority;
                }
            }

            /// <summary>
            /// 获取或设置下载任务是否完成。
            /// </summary>
            public bool Done
            {
                get
                {
                    return m_Done;
                }
                set
                {
                    m_Done = value;
                }
            }

            /// <summary>
            /// 获取或设置下载任务的状态。
            /// </summary>
            public DownloadTaskStatus Status
            {
                get
                {
                    return m_Status;
                }
                set
                {
                    m_Status = value;
                }
            }

            /// <summary>
            /// 获取下载后存放路径。
            /// </summary>
            public string DownloadPath
            {
                get
                {
                    return m_DownloadPath;
                }
            }

            /// <summary>
            /// 获取原始下载地址。
            /// </summary>
            public string DownloadUri
            {
                get
                {
                    return m_DownloadUri;
                }
            }

            /// <summary>
            /// 获取将缓冲区写入磁盘的临界大小。
            /// </summary>
            public int FlushSize
            {
                get
                {
                    return m_FlushSize;
                }
            }

            /// <summary>
            /// 获取下载超时时长，以秒为单位。
            /// </summary>
            public float Timeout
            {
                get
                {
                    return m_Timeout;
                }
            }

            /// <summary>
            /// 获取用户自定义数据。
            /// </summary>
            public object UserData
            {
                get
                {
                    return m_UserData;
                }
            }

            /// <summary>
            /// 创建下载任务。
            /// </summary>
            /// <param name="downloadPath">下载后存放路径。</param>
            /// <param name="downloadUri">原始下载地址。</param>
            /// <param name="priority">下载任务的优先级。</param>
            /// <param name="flushSize">将缓冲区写入磁盘的临界大小。</param>
            /// <param name="timeout">下载超时时长，以秒为单位。</param>
            /// <param name="userData">用户自定义数据。</param>
            /// <returns>创建的下载任务。</returns>
            public static DownloadTask Create(string downloadPath, string downloadUri, int priority, int flushSize, float timeout, object userData)
            {
                DownloadTask downloadTask = ReferencePool.Acquire<DownloadTask>();
                downloadTask.m_SerialId = s_Serial++;
                downloadTask.m_Priority = priority;
                downloadTask.m_DownloadPath = downloadPath;
                downloadTask.m_DownloadUri = downloadUri;
                downloadTask.m_FlushSize = flushSize;
                downloadTask.m_Timeout = timeout;
                downloadTask.m_UserData = userData;
                return downloadTask;
            }

            /// <summary>
            /// 清理下载任务。
            /// </summary>
            public void Clear()
            {
                m_SerialId = 0;
                m_Priority = 0;
                m_Done = false;
                m_Status = DownloadTaskStatus.Todo;
                m_DownloadPath = null;
                m_DownloadUri = null;
                m_FlushSize = 0;
                m_Timeout = 0f;
                m_UserData = null;
            }
        }
    }
}
