using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Data
{
    /// <summary>
    /// 数据管理器中数据发生变动事件。
    /// </summary>
    public sealed class UpdateDataCompleteEventArgs : GameFrameworkEventArgs
    {
        private readonly List<string> m_UpdateDataNames = null;

        public UpdateDataCompleteEventArgs()
        {
            m_UpdateDataNames = new List<string>();
        }

        public string[] UpdateDataNames
        {
            get { return m_UpdateDataNames.ToArray(); }
        }

        public static UpdateDataCompleteEventArgs Create(string[] modifiedDataNames)
        {
            UpdateDataCompleteEventArgs eventArgs = ReferencePool.Acquire<UpdateDataCompleteEventArgs>();
            eventArgs.m_UpdateDataNames.AddRange(modifiedDataNames);
            return eventArgs;
        }

        public override void Clear()
        {
            m_UpdateDataNames.Clear();
        }
    }
}