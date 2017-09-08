using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpamHam
{
    /// <summary>
    /// 计算接口
    /// </summary>
    public interface ICalculation
    {
        /// <summary>
        /// 对一个字符串进行计算，分解出每个单词，并计算每个单词在字符串中出现次数比率
        /// </summary>
        /// <param name="value">待解析的字符串</param>
        /// <returns>返回计算后的标签集合，对应了每个单词及其出现次数比率</returns>
        Token[] Calculate(string value);

        /// <summary>
        /// 计算单词<paramref name="value"/>在字符串数组<paramref name="lines"/>中的出现次数。（单词在字符串中出现次数的总数）
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="value"></param>
        /// <returns>
        /// 返回单词<paramref name="value"/>在字符串数组<paramref name="lines"/>中的出现次数
        /// </returns>
        int Calculate(string[] lines, string value);

        /// <summary>
        /// 计算单词<paramref name="value"/>在字符串数组<paramref name="lines"/>中的出现次数。（一个字符串只计算一次，无论单词是否在字符串中多次出现）
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="value"></param>
        /// <returns>
        /// 返回单词<paramref name="value"/>在字符串数组<paramref name="lines"/>中的出现次数
        /// </returns>
        int Calculate2(string[] lines, string value);
    }
}
