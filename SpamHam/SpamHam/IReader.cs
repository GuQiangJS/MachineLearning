using System;
using System.Collections.Generic;

namespace SpamHam
{
    /// <summary>
    /// 文档读取接口，用来将文档中的每一行解析为 文本/文本类型 字典
    /// </summary>
    public interface IReader
    {
        /// <summary>
        ///     读取<see cref="IReader.FilePath"/>，并将每一行解析解析为<see cref="Line"/>实例
        /// </summary>
        /// <returns>返回解析<see cref="IReader.FilePath"/>后的<see cref="Line"/>集合。</returns>
        IList<Line> Read();

        /// <summary>
        /// 获取解析文件的完整路径。
        /// </summary>
        string FilePath { get; }
    }
}