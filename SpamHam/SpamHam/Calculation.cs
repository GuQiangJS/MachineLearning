using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        /// <summary>
        /// 计算分组数组<paramref name="lines"/>中是<paramref name="type"/>类型，同时包含<paramref name="value"/>的概率（经过拉普拉斯容差转换）
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public double Calculate3(Line[] lines, string value, DocType type)
        {
            //符合条件的分组数量
            Line[] array = lines.Where(x => x.DocType == type).ToArray();
            //包含 value 的信息集合
            Line[] containsValueArray = lines.Where(x => x.Contains(value, StringComparer.Ordinal)).ToArray();
            //包含 value 的信息集合中符合 type 的信息集合
            Line[] containsValueArrayIsType = containsValueArray.Where(x => x.DocType == type).ToArray();

            //信息类型是 type 时，信息包含 value 的比率
            double per = (double)(containsValueArrayIsType.Length) / (array.Length);
            //信息类型是 type 时，信息包含 value 的比率
            double laplace = (double)(1 + containsValueArrayIsType.Length) / (1 + array.Length);

#if DEBUG
            Debug.WriteLine("原始分组数量:{0}.", lines.Length);
            Debug.WriteLine("信息类型是 {1} 分组数量:{0}.", array.Length, Enum.GetName(typeof(DocType), type));
            Debug.WriteLine("包含 {1} 的分组数量:{0}.", containsValueArray.Length, value);
            Debug.WriteLine("信息类型是 {1} 时，信息包含 {2} 的分组数量:{0}.", containsValueArrayIsType.Length,
                Enum.GetName(typeof(DocType), type), value);
            Debug.WriteLine("原始比率:{0}.", per.ToString("P"));
            Debug.WriteLine("拉普拉斯变换后的比率:{0}.", laplace.ToString("P"));
#endif
            return laplace;
        }
    }
}
