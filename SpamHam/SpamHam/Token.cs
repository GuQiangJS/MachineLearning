using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpamHam
{
    /// <summary>
    /// 标签，用以记录每个单词的得分
    /// </summary>
    [DebuggerDisplay("Value = {Value} , Score= {Score}")]
    public struct Token
    {
        public Token(string value,DocType type, double score)
        {
            Value = value;
            Score = score;
            Type = type;
        }
        public DocType Type { get; set; }
        /// <summary>
        /// 获取或设置得分
        /// </summary>
        public double Score { get; set; }
        /// <summary>
        /// 获取或设置单词
        /// </summary>
        public string Value { get; set; }
    }
}
