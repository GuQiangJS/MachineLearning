using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpamHam
{
    /// <summary>
    /// 训练
    /// </summary>
    public class Calculation : ICalculation
    {
        public Calculation()
        {
            _participle = new ParticipleSimple();
        }
        /// <summary>
        /// 分词器
        /// </summary>
        private IParticiple _participle;

        /// <summary>
        /// 对一个字符串进行计算，分解出每个单词，并计算每个单词在字符串中出现次数比率
        /// </summary>
        /// <param name="value">待解析的字符串</param>
        /// <returns>返回计算后的标签集合，对应了每个单词及其出现次数比率</returns>
        public Token[] Calculate(string value)
        {
            string[] words = _participle.DoParticiple(value);
            Token[] result = new Token[words.Length];
            for (int i = 0; i < words.Length; i++)
            {
                result[i] = new Token(words[i], (float) words.Count(x => x.Equals(words[i])) / (float) words.Length);
            }
            return result;
        }

        /// <summary>
        /// 计算单词<paramref name="value"/>在字符串数组<paramref name="lines"/>中的出现次数
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="value"></param>
        /// <returns>
        /// 返回单词<paramref name="value"/>在字符串数组<paramref name="lines"/>中的出现次数
        /// </returns>
        public int Calculate(string[] lines, string value)
        {
            int result = 0;
            foreach (string line in lines)
            {
                string[] words = _participle.DoParticiple(line);
                result = result + words.Count(x => x.Equals(value, StringComparison.OrdinalIgnoreCase));
            }
            return result;
        }

        /// <summary>
        /// 计算单词<paramref name="value"/>在字符串数组<paramref name="lines"/>中的出现次数
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="value"></param>
        /// <returns>
        /// 返回单词<paramref name="value"/>在字符串数组<paramref name="lines"/>中的出现次数
        /// </returns>
        public int Calculate2(string[] lines, string value)
        {
            int result = 0;
            foreach (string line in lines)
            {
                string[] words = _participle.DoParticiple(line);
                result = result + ((words.Count(x => x.Equals(value, StringComparison.OrdinalIgnoreCase)) > 0) ? 1 : 0);
            }
            return result;
        }
    }
}
