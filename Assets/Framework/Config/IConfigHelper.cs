//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

namespace GameFramework.Config
{
    /// <summary>
    /// 数据表辅助器接口。
    /// </summary>
    public interface IConfigHelper
    {
        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="configTableAsset">数据表资源。</param>
        /// <param name="loadType">数据表加载方式。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        bool LoadConfigTable(object configTableAsset, LoadType loadType, object userData);

        /// <summary>
        /// 获取数据表行片段。
        /// </summary>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>数据表行片段。</returns>
        IEnumerable<object> GetConfigRowSegments(string text);

        /// <summary>
        /// 获取数据表行片段。
        /// </summary>
        /// <param name="bytes">要解析的数据表二进制流。</param>
        /// <returns>数据表行片段。</returns>
        IEnumerable<object> GetConfigRowSegments(byte[] bytes);

        /// <summary>
        /// 获取数据表行片段。
        /// </summary>
        /// <param name="stream">要解析的数据表二进制流。</param>
        /// <returns>数据表行片段。</returns>
        IEnumerable<object> GetConfigRowSegments(Stream stream);

        /// <summary>
        /// 释放数据表资源。
        /// </summary>
        /// <param name="configTableAsset">要释放的数据表资源。</param>
        void ReleaseConfigTableAsset(object configTableAsset);
    }
}
