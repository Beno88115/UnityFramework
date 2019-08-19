//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace GameFramework.Config
{
    internal sealed partial class ConfigModule : GameFrameworkModule, IConfigModule
    {
        /// <summary>
        /// 数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        private sealed class ConfigTable<T> : ConfigTableBase, IConfigTable<T> where T : class, IConfigRow, new()
        {
            private readonly Dictionary<int, T> m_ConfigSet;
            private T m_MinIdConfigRow;
            private T m_MaxIdConfigRow;

            /// <summary>
            /// 初始化数据表的新实例。
            /// </summary>
            /// <param name="name">数据表名称。</param>
            public ConfigTable(string name)
                : base(name)
            {
                m_ConfigSet = new Dictionary<int, T>();
                m_MinIdConfigRow = null;
                m_MaxIdConfigRow = null;
            }

            /// <summary>
            /// 获取数据表行的类型。
            /// </summary>
            public override Type Type
            {
                get
                {
                    return typeof(T);
                }
            }

            /// <summary>
            /// 获取数据表行数。
            /// </summary>
            public override int Count
            {
                get
                {
                    return m_ConfigSet.Count;
                }
            }

            /// <summary>
            /// 获取数据表行。
            /// </summary>
            /// <param name="id">数据表行的编号。</param>
            /// <returns>数据表行。</returns>
            public T this[int id]
            {
                get
                {
                    return GetConfigRow(id);
                }
            }

            /// <summary>
            /// 获取编号最小的数据表行。
            /// </summary>
            public T MinIdConfigRow
            {
                get
                {
                    return m_MinIdConfigRow;
                }
            }

            /// <summary>
            /// 获取编号最大的数据表行。
            /// </summary>
            public T MaxIdConfigRow
            {
                get
                {
                    return m_MaxIdConfigRow;
                }
            }

            /// <summary>
            /// 检查是否存在数据表行。
            /// </summary>
            /// <param name="id">数据表行的编号。</param>
            /// <returns>是否存在数据表行。</returns>
            public bool HasConfigRow(int id)
            {
                return m_ConfigSet.ContainsKey(id);
            }

            /// <summary>
            /// 检查是否存在数据表行。
            /// </summary>
            /// <param name="condition">要检查的条件。</param>
            /// <returns>是否存在数据表行。</returns>
            public bool HasConfigRow(Predicate<T> condition)
            {
                if (condition == null)
                {
                    throw new GameFrameworkException("Condition is invalid.");
                }

                foreach (KeyValuePair<int, T> configRow in m_ConfigSet)
                {
                    if (condition(configRow.Value))
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// 获取数据表行。
            /// </summary>
            /// <param name="id">数据表行的编号。</param>
            /// <returns>数据表行。</returns>
            public T GetConfigRow(int id)
            {
                T configRow = null;
                if (m_ConfigSet.TryGetValue(id, out configRow))
                {
                    return configRow;
                }

                return null;
            }

            /// <summary>
            /// 获取符合条件的数据表行。
            /// </summary>
            /// <param name="condition">要检查的条件。</param>
            /// <returns>符合条件的数据表行。</returns>
            /// <remarks>当存在多个符合条件的数据表行时，仅返回第一个符合条件的数据表行。</remarks>
            public T GetConfigRow(Predicate<T> condition)
            {
                if (condition == null)
                {
                    throw new GameFrameworkException("Condition is invalid.");
                }

                foreach (KeyValuePair<int, T> configRow in m_ConfigSet)
                {
                    if (condition(configRow.Value))
                    {
                        return configRow.Value;
                    }
                }

                return null;
            }

            /// <summary>
            /// 获取符合条件的数据表行。
            /// </summary>
            /// <param name="condition">要检查的条件。</param>
            /// <returns>符合条件的数据表行。</returns>
            public T[] GetConfigRows(Predicate<T> condition)
            {
                if (condition == null)
                {
                    throw new GameFrameworkException("Condition is invalid.");
                }

                List<T> results = new List<T>();
                foreach (KeyValuePair<int, T> configRow in m_ConfigSet)
                {
                    if (condition(configRow.Value))
                    {
                        results.Add(configRow.Value);
                    }
                }

                return results.ToArray();
            }

            /// <summary>
            /// 获取符合条件的数据表行。
            /// </summary>
            /// <param name="condition">要检查的条件。</param>
            /// <param name="results">符合条件的数据表行。</param>
            public void GetConfigRows(Predicate<T> condition, List<T> results)
            {
                if (condition == null)
                {
                    throw new GameFrameworkException("Condition is invalid.");
                }

                if (results == null)
                {
                    throw new GameFrameworkException("Results is invalid.");
                }

                results.Clear();
                foreach (KeyValuePair<int, T> configRow in m_ConfigSet)
                {
                    if (condition(configRow.Value))
                    {
                        results.Add(configRow.Value);
                    }
                }
            }

            /// <summary>
            /// 获取排序后的数据表行。
            /// </summary>
            /// <param name="comparison">要排序的条件。</param>
            /// <returns>排序后的数据表行。</returns>
            public T[] GetConfigRows(Comparison<T> comparison)
            {
                if (comparison == null)
                {
                    throw new GameFrameworkException("Comparison is invalid.");
                }

                List<T> results = new List<T>();
                foreach (KeyValuePair<int, T> configRow in m_ConfigSet)
                {
                    results.Add(configRow.Value);
                }

                results.Sort(comparison);
                return results.ToArray();
            }

            /// <summary>
            /// 获取排序后的数据表行。
            /// </summary>
            /// <param name="comparison">要排序的条件。</param>
            /// <param name="results">排序后的数据表行。</param>
            public void GetConfigRows(Comparison<T> comparison, List<T> results)
            {
                if (comparison == null)
                {
                    throw new GameFrameworkException("Comparison is invalid.");
                }

                if (results == null)
                {
                    throw new GameFrameworkException("Results is invalid.");
                }

                results.Clear();
                foreach (KeyValuePair<int, T> configRow in m_ConfigSet)
                {
                    results.Add(configRow.Value);
                }

                results.Sort(comparison);
            }

            /// <summary>
            /// 获取排序后的符合条件的数据表行。
            /// </summary>
            /// <param name="condition">要检查的条件。</param>
            /// <param name="comparison">要排序的条件。</param>
            /// <returns>排序后的符合条件的数据表行。</returns>
            public T[] GetConfigRows(Predicate<T> condition, Comparison<T> comparison)
            {
                if (condition == null)
                {
                    throw new GameFrameworkException("Condition is invalid.");
                }

                if (comparison == null)
                {
                    throw new GameFrameworkException("Comparison is invalid.");
                }

                List<T> results = new List<T>();
                foreach (KeyValuePair<int, T> configRow in m_ConfigSet)
                {
                    if (condition(configRow.Value))
                    {
                        results.Add(configRow.Value);
                    }
                }

                results.Sort(comparison);
                return results.ToArray();
            }

            /// <summary>
            /// 获取排序后的符合条件的数据表行。
            /// </summary>
            /// <param name="condition">要检查的条件。</param>
            /// <param name="comparison">要排序的条件。</param>
            /// <param name="results">排序后的符合条件的数据表行。</param>
            public void GetConfigRows(Predicate<T> condition, Comparison<T> comparison, List<T> results)
            {
                if (condition == null)
                {
                    throw new GameFrameworkException("Condition is invalid.");
                }

                if (comparison == null)
                {
                    throw new GameFrameworkException("Comparison is invalid.");
                }

                if (results == null)
                {
                    throw new GameFrameworkException("Results is invalid.");
                }

                results.Clear();
                foreach (KeyValuePair<int, T> configRow in m_ConfigSet)
                {
                    if (condition(configRow.Value))
                    {
                        results.Add(configRow.Value);
                    }
                }

                results.Sort(comparison);
            }

            /// <summary>
            /// 获取所有数据表行。
            /// </summary>
            /// <returns>所有数据表行。</returns>
            public T[] GetAllConfigRows()
            {
                int index = 0;
                T[] results = new T[m_ConfigSet.Count];
                foreach (KeyValuePair<int, T> configRow in m_ConfigSet)
                {
                    results[index++] = configRow.Value;
                }

                return results;
            }

            /// <summary>
            /// 获取所有数据表行。
            /// </summary>
            /// <param name="results">所有数据表行。</param>
            public void GetAllConfigRows(List<T> results)
            {
                if (results == null)
                {
                    throw new GameFrameworkException("Results is invalid.");
                }

                results.Clear();
                foreach (KeyValuePair<int, T> configRow in m_ConfigSet)
                {
                    results.Add(configRow.Value);
                }
            }

            /// <summary>
            /// 返回一个循环访问数据表的枚举器。
            /// </summary>
            /// <returns>可用于循环访问数据表的对象。</returns>
            public IEnumerator<T> GetEnumerator()
            {
                return m_ConfigSet.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return m_ConfigSet.Values.GetEnumerator();
            }

            /// <summary>
            /// 关闭并清理数据表。
            /// </summary>
            internal override void Shutdown()
            {
                m_ConfigSet.Clear();
            }

            /// <summary>
            /// 增加数据表行。
            /// </summary>
            /// <param name="configRowSegment">要解析的数据表行片段。</param>
            /// <returns>是否增加数据表行成功。</returns>
            internal override bool AddConfigRow(object configRowSegment)
            {
                try
                {
                    T configRow = new T();
                    if (!configRow.ParseConfigRow(configRowSegment))
                    {
                        return false;
                    }

                    InternalAddConfigRow(configRow);
                    return true;
                }
                catch (Exception exception)
                {
                    if (exception is GameFrameworkException)
                    {
                        throw;
                    }

                    throw new GameFrameworkException(Utility.Text.Format("Can not parse config table '{0}' with exception '{1}'.", Utility.Text.GetFullName<T>(Name), exception.ToString()), exception);
                }
            }

            private void InternalAddConfigRow(T configRow)
            {
                if (HasConfigRow(configRow.Id))
                {
                    throw new GameFrameworkException(Utility.Text.Format("Already exist '{0}' in config table '{1}'.", configRow.Id.ToString(), Utility.Text.GetFullName<T>(Name)));
                }

                m_ConfigSet.Add(configRow.Id, configRow);

                if (m_MinIdConfigRow == null || m_MinIdConfigRow.Id > configRow.Id)
                {
                    m_MinIdConfigRow = configRow;
                }

                if (m_MaxIdConfigRow == null || m_MaxIdConfigRow.Id < configRow.Id)
                {
                    m_MaxIdConfigRow = configRow;
                }
            }
        }
    }
}
