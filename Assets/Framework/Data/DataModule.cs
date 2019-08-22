using System;
using System.IO;
using System.Collections.Generic;

namespace GameFramework.Data
{
    /// <summary>
    /// 数据中心管理器接口。
    /// </summary>
    internal sealed partial class DataModule : GameFrameworkModule, IDataModule 
    {
        private int m_Serial;
        private IDataHelper m_DataHelper;
        private readonly Dictionary<string, DataBase> m_Datas;
        private EventHandler<CreateDataSuccessEventArgs> m_CreateDataSuccessEventHandler;
        private EventHandler<UpdateDataCompleteEventArgs> m_UpdateDataCompleteEventHandler;
        private EventHandler<ParseDataFailureEventArgs> m_ParseDataFailureEventHandler;

        public DataModule()
        {
            m_Serial = 0;
            m_Datas = new Dictionary<string, DataBase>();
            m_DataHelper = null;
            m_CreateDataSuccessEventHandler = null;
            m_UpdateDataCompleteEventHandler = null;
            m_ParseDataFailureEventHandler = null;
        }

        /// <summary>
        /// 获取已经缓存在内存中数据对象个数。
        /// </summary>
        public int DataCount
        {
            get { return m_Datas.Count; }
        }

        /// <summary>
        /// 数据解析失败事件。
        /// </summary>
        public event EventHandler<ParseDataFailureEventArgs> ParseDataFailure
        {
            add
            {
                m_ParseDataFailureEventHandler += value;
            }
            remove
            {
                m_ParseDataFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// 创建新数据对象事件。
        /// </summary>
        public event EventHandler<CreateDataSuccessEventArgs> CreateDataSuccess
        {
            add
            {
                m_CreateDataSuccessEventHandler += value;
            }
            remove
            {
                m_CreateDataSuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// 数据发生变动事件。
        /// </summary>
        public event EventHandler<UpdateDataCompleteEventArgs> UpdateDataComplete
        {
            add
            {
                m_UpdateDataCompleteEventHandler += value;
            }
            remove
            {
                m_UpdateDataCompleteEventHandler -= value;
            }
        }

        /// <summary>
        /// 是否存在数据。
        /// </summary>
        /// <param name="dataType">数据类型。</param>
        public bool HasData(Type dataType)
        {
            return m_Datas.ContainsKey(dataType.Name);
        }

        /// <summary>
        /// 是否存在数据。
        /// </summary>
        public bool HasData<T>() where T : DataBase
        {
            return m_Datas.ContainsKey(typeof(T).Name);
        }

        /// <summary>
        /// 通过Type类型名称，获取数据对象序列ID。
        /// </summary>
        public int GetDataSerialID(string dataTypeName)
        {
            DataBase target = null;
            if (m_Datas.TryGetValue(dataTypeName, out target))
            {
                return target.SerialId;
            }
            return 0;
        }

        /// <summary>
        /// 通过Type类型，获取数据对象。
        /// </summary>
        public DataBase GetData(Type dataType)
        {
            DataBase target = null;
            if (m_Datas.TryGetValue(dataType.Name, out target))
            {
                return target;
            }
            
            if (dataType.IsInterface)
            {
                throw new GameFrameworkException("data type is not allow as an interface.");
            }

            if (!dataType.IsSubclassOf(typeof(DataBase)))
            {
                throw new GameFrameworkException("data type can only be a sub class of IData.");
            }

            target = (DataBase)Activator.CreateInstance(dataType, true);
            target.SerialId = ++m_Serial;
            m_Datas[dataType.Name] = target;

            if (m_CreateDataSuccessEventHandler != null)
            {
                CreateDataSuccessEventArgs dataCreatedEventArgs = CreateDataSuccessEventArgs.Create(dataType.Name);
                m_CreateDataSuccessEventHandler(this, dataCreatedEventArgs);
                ReferencePool.Release(dataCreatedEventArgs);
            }

            return target;
        }

        /// <summary>
        /// 通过泛型获取数据对象。
        /// </summary>
        public T GetData<T>() where T : DataBase
        {
            string typeName = typeof(T).Name;
            if (m_Datas.ContainsKey(typeName)) 
            {
                return (T)m_Datas[typeName];
            }

            T data = (T)Activator.CreateInstance(typeof(T), true);
            data.SerialId = ++m_Serial;
            m_Datas[typeName] = data;

            if (m_CreateDataSuccessEventHandler != null)
            {
                CreateDataSuccessEventArgs dataCreatedEventArgs = CreateDataSuccessEventArgs.Create(typeName);
                m_CreateDataSuccessEventHandler(this, dataCreatedEventArgs);
                ReferencePool.Release(dataCreatedEventArgs);
            }

            return data;
        }

        /// <summary>
        /// 解析数据。
        /// </summary>
        /// <param name="text">用于解析的原始数据。</param>
        public void ParseData(string text)
        {
            if (m_DataHelper == null)
            {
                throw new GameFrameworkException("you need set data helper first.");
            }
            
            string[] modifiedDataNames = m_DataHelper.ParseData(text);
            if (modifiedDataNames == null)
            {
                if (m_ParseDataFailureEventHandler != null)
                {
                    ParseDataFailureEventArgs dataParseFailureEventArgs = ParseDataFailureEventArgs.Create();
                    m_ParseDataFailureEventHandler(this, dataParseFailureEventArgs);
                    ReferencePool.Release(dataParseFailureEventArgs);
                }
                return;
            }

            if (m_UpdateDataCompleteEventHandler != null)
            {
                UpdateDataCompleteEventArgs dataModifiedEventArgs = UpdateDataCompleteEventArgs.Create(modifiedDataNames);
                m_UpdateDataCompleteEventHandler(this, dataModifiedEventArgs);
                ReferencePool.Release(dataModifiedEventArgs);
            }
        }

        /// <summary>
        /// 解析数据。
        /// </summary>
        /// <param name="bytes">用于解析的原始数据。</param>
        public void ParseData(byte[] bytes)
        {
            if (m_DataHelper == null)
            {
                throw new GameFrameworkException("you need set data helper first.");
            }

            string[] modifiedDataNames = m_DataHelper.ParseData(bytes);
            if (modifiedDataNames == null)
            {
                if (m_ParseDataFailureEventHandler != null)
                {
                    ParseDataFailureEventArgs dataParseFailureEventArgs = ParseDataFailureEventArgs.Create();
                    m_ParseDataFailureEventHandler(this, dataParseFailureEventArgs);
                    ReferencePool.Release(dataParseFailureEventArgs);
                }
                return;
            }

            if (m_UpdateDataCompleteEventHandler != null)
            {
                UpdateDataCompleteEventArgs dataModifiedEventArgs = UpdateDataCompleteEventArgs.Create(modifiedDataNames);
                m_UpdateDataCompleteEventHandler(this, dataModifiedEventArgs);
                ReferencePool.Release(dataModifiedEventArgs);
            }
        }

        /// <summary>
        /// 解析数据。
        /// </summary>
        /// <param name="stream">用于解析的原始数据流。</param>
        public void ParseData(Stream stream)
        {
            if (m_DataHelper == null)
            {
                throw new GameFrameworkException("you need set data helper first.");
            }

            string[] modifiedDataNames = m_DataHelper.ParseData(stream);
            if (modifiedDataNames == null)
            {
                if (m_ParseDataFailureEventHandler != null)
                {
                    ParseDataFailureEventArgs dataParseFailureEventArgs = ParseDataFailureEventArgs.Create();
                    m_ParseDataFailureEventHandler(this, dataParseFailureEventArgs);
                    ReferencePool.Release(dataParseFailureEventArgs);
                }
                return;
            }

            if (m_UpdateDataCompleteEventHandler != null)
            {
                UpdateDataCompleteEventArgs dataModifiedEventArgs = UpdateDataCompleteEventArgs.Create(modifiedDataNames);
                m_UpdateDataCompleteEventHandler(this, dataModifiedEventArgs);
                ReferencePool.Release(dataModifiedEventArgs);
            }
        }

        /// <summary>
        /// 设置数据辅助器。
        /// </summary>
        /// <param name="dataHelper">数据辅助器。</param>
        public void SetDataHelper(IDataHelper dataHelper)
        {
            if (dataHelper == null)
            {
                throw new GameFrameworkException("data helper is invalid.");
            }
            m_DataHelper = dataHelper;
        }

        /// <summary>
        /// 游戏框架模块轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 关闭并清理游戏框架模块。
        /// </summary>
        internal override void Shutdown()
        {
            ClearAllDatas();
        }

        /// <summary>
        /// 清空所有的数据对象。
        /// </summary>
        private void ClearAllDatas()
        {
            foreach (var keyValue in m_Datas)
            {
                keyValue.Value.Clear();
            }
            m_Datas.Clear();
        }
    }
}
