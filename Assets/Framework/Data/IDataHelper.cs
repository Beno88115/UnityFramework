using System.IO;

namespace GameFramework.Data
{
    /// <summary>
    /// 数据辅助器接口。
    /// </summary>
    public interface IDataHelper
    {
        /// <summary>
        /// 解析数据。
        /// </summary>
        /// <param name="text">要解析的数据文本。</param>
        /// <returns>成功解析的数据对象名称，错误解析返回null。</returns>
        string[] ParseData(string text);

        /// <summary>
        /// 解析数据。
        /// </summary>
        /// <param name="text">要解析的二进制数据流。</param>
        /// <returns>成功解析的数据对象名称，错误解析返回null。</returns>
        string[] ParseData(byte[] bytes);

        /// <summary>
        /// 解析数据。
        /// </summary>
        /// <param name="text">要解析的文件流。</param>
        /// <returns>成功解析的数据对象名称，错误解析返回null。</returns>
        string[] ParseData(Stream stream);
    }
}