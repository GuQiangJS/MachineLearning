using System;
using System.Collections.Generic;

namespace SpamHam
{
    /// <summary>
    /// 文档读取接口，用来将文档中的每一行解析为 文本/文本类型 字典
    /// </summary>
    public interface IReader
    {
        Dictionary<string,DocType> Read();
    }
}