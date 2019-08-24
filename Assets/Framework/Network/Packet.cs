//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Network
{
    /// <summary>
    /// 网络消息包基类。
    /// </summary>
    public abstract class Packet : BaseEventArgs
    {
        /// <summary>
        /// 消息结构序列化。
        /// </summary>
        /// <returns>序列化数据。</returns>
        public abstract object Serialize();

        /// <summary>
        /// 消息结构反序列化。
        /// </summary>
        /// <param name="data">网络消息包。</param>
        public abstract void Deserialize(object data);
    }
}
