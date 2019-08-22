using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Data
{
    /// <summary>
    /// 数据管理器中数据解析失败事件。
    /// </summary>
    public sealed class ParseDataFailureEventArgs : GameFrameworkEventArgs
    {
        public static ParseDataFailureEventArgs Create()
        {
            ParseDataFailureEventArgs eventArgs = ReferencePool.Acquire<ParseDataFailureEventArgs>();
            return eventArgs;
        }

        public override void Clear()
        {
        }
    }
}