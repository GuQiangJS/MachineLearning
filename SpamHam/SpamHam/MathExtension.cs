using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpamHam
{
    /// <summary>
    /// 数学函数扩展
    /// </summary>
    public static class MathExtension
    {
        /// <summary>
        /// 求贝叶斯值
        /// </summary>
        /// <param name="PBA">A 条件下 B 条件的概率</param>
        /// <param name="PA">A 条件的概率</param>
        /// <param name="PB">B 条件的概率</param>
        /// <returns>返回P(A|B)[B条件下A的概率]</returns>
        public static double P(double PBA,double PA,double PB)
        {
            return PBA * PA / PB;
        }
    }
}
