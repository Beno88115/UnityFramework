using System;

namespace GameFramework.Data
{
    /// <summary>
    /// 数据管理器创建新数据对象事件。
    /// </summary>
    public sealed class CreateDataSuccessEventArgs : GameFrameworkEventArgs
    {
        public CreateDataSuccessEventArgs()
        {
            NewDataName = string.Empty;
        }

        public string NewDataName
        {
            get;
            private set;
        }

        public static CreateDataSuccessEventArgs Create(string dataName)
        {
            CreateDataSuccessEventArgs eventArgs = ReferencePool.Acquire<CreateDataSuccessEventArgs>();
            eventArgs.NewDataName = dataName;
            return eventArgs;
        }

        public override void Clear()
        {
            NewDataName = string.Empty;
        }
    }
}
