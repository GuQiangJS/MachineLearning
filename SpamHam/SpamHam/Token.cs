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
        public Token(string value, float score)
        {
            Value = value;
            Score = score;
        }
        /// <summary>
        /// 获取或设置得分
        /// </summary>
        public float Score { get; set; }
        /// <summary>
        /// 获取或设置单词
        /// </summary>
        public string Value { get; set; }
    }
}
