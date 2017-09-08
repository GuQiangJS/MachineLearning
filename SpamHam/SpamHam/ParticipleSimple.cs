using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpamHam
{
    /// <summary>
    /// 简单分词器。只去空格符分割的英文字符，跳过其他字符
    /// </summary>
    public class ParticipleSimple:IParticiple
    {
        /*
         * Ascii码
         * 数字：48~57
         * 大写英文：65~90
         * 小写英文：97~122
         * 空格：32
         */

        private const char spaceChar = (char)32;

        /// <summary>
        /// 分词
        /// </summary>
        /// <param name="value">待进行分词操作的字符串</param>
        /// <param name="toUp">将分词全部转换为大写</param>
        /// <returns>返回分词后的文本数组。</returns>
        public string[] DoParticiple(string value,bool toUp=true)
        {
            Queue<string> result=new Queue<string>();
            char[] chars = value.ToArray();

            Queue<char> cs=new Queue<char>();
            foreach (char c in chars)
            {
                if ((c <= 57 && c >= 48) || (c >= 65 && c <= 90) || (c >= 97 && c <= 122))
                {
                    char temp = c;
                    if (temp >= 97 && temp <= 122)
                    {
                        temp = (char) (temp - 32);
                    }
                    cs.Enqueue(temp);
                    continue;
                }
                if (c == 32)
                {
                    if (cs.Count > 0)
                    {
                        result.Enqueue(new string(cs.ToArray()));
                    }
                    cs=new Queue<char>();
                    continue;
                }
            }
            if (cs.Count > 0)
            {
                result.Enqueue(new string(cs.ToArray()));
            }

            return result.ToArray();
        }
    }
}
