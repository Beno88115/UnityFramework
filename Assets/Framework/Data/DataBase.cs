using System;

namespace GameFramework.Data
{
    /// <summary>
    /// 数据基类。
    /// </summary>
    public abstract class DataBase
    {
        /// <summary>
        /// 获取数据序列编号。
        /// </summary>
        public int SerialId
        {
            get;
            set;
        }

        /// <summary>
        /// 解析数据。
        /// </summary>
        /// <param name="dataObject">原始的数据对象。</param>
        /// <returns>数据是否发生变化。</returns>
        public abstract bool Parse(object dataObject);

        /// <summary>
        /// 清空数据。
        /// </summary>
        public abstract void Clear();
    }
}