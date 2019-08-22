using System;
using System.IO;

namespace GameFramework.Data
{
    /// <summary>
    /// 数据中心管理器接口。
    /// </summary>
    public interface IDataModule
    {
        /// <summary>
        /// 获取已经缓存在内存中数据对象个数。
        /// </summary>
        int DataCount
        {
            get;
        }

        /// <summary>
        /// 数据解析失败事件。
        /// </summary>
        event EventHandler<ParseDataFailureEventArgs> ParseDataFailure;

        /// <summary>
        /// 创建新数据对象事件。
        /// </summary>
        event EventHandler<CreateDataSuccessEventArgs> CreateDataSuccess;

        /// <summary>
        /// 数据发生变动事件。
        /// </summary>
        event EventHandler<UpdateDataCompleteEventArgs> UpdateDataComplete;

        /// <summary>
        /// 是否存在数据。
        /// </summary>
        /// <param name="dataType">数据类型。</param>
        bool HasData(Type dataType);

        /// <summary>
        /// 是否存在数据。
        /// </summary>
        bool HasData<T>() where T : DataBase;

        /// <summary>
        /// 通过Type类型名称，获取数据对象序列ID。
        /// </summary>
        int GetDataSerialID(string dataTypeName);

        /// <summary>
        /// 通过Type类型，获取数据对象。
        /// </summary>
        DataBase GetData(Type dataType);

        /// <summary>
        /// 通过泛型获取数据对象。
        /// </summary>
        T GetData<T>() where T : DataBase;

        /// <summary>
        /// 解析数据。
        /// </summary>
        /// <param name="text">用于解析的原始数据。</param>
        void ParseData(string text);

        /// <summary>
        /// 解析数据。
        /// </summary>
        /// <param name="bytes">用于解析的原始数据。</param>
        void ParseData(byte[] bytes);

        /// <summary>
        /// 解析数据。
        /// </summary>
        /// <param name="stream">用于解析的原始数据流。</param>
        void ParseData(Stream stream);

        /// <summary>
        /// 设置数据辅助器。
        /// </summary>
        /// <param name="dataHelper">数据辅助器。</param>
        void SetDataHelper(IDataHelper dataHelper);
    }
}