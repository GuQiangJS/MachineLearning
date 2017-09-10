using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpamHam
{
    /// <summary>
    /// 分词器接口
    /// </summary>
    public interface IParticiple
    {
        /// <summary>
        /// 分词
        /// </summary>
        /// <param name="value">待进行分词操作的字符串</param>
        /// <returns>返回分词后的文本数组。</returns>
        string[] DoParticiple(string value);
    }
}
