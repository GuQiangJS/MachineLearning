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

        /// <summary>
        /// 计算分组数组<paramref name="lines"/>中是<paramref name="type"/>类型，同时包含<paramref name="value"/>的概率（经过拉普拉斯容差转换）
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        double Calculate3(Line[] lines, string value, DocType type);
    }
}
