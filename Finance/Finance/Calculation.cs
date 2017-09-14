using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance
{
    /// <summary>
    /// 计算接口
    /// </summary>
    public interface ICalculation
    {
        /// <summary>
        /// 计算分组数组<paramref name="datas"/>中是指定目标百分比<paramref name="percent"/>达到的情况下，同时包含 买盘/(买盘+买盘)的占比<paramref name="poor"/> 的概率（经过拉普拉斯容差转换）
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="poor"></param>
        /// <param name="tpercentype"></param>
        /// <returns></returns>
        double Calculate1(DailyHistoricalDataExtCol datas, Test.PercentScore<double> score);
    }

    public class Calculation:ICalculation
    {
        /// <summary>
        /// 计算分组数组<paramref name="datas"/>中是指定目标百分比<paramref name="percent"/>达到的情况下，同时包含 买盘/(买盘+买盘)的占比<paramref name="poor"/> 的概率（经过拉普拉斯容差转换）
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="poor"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public double Calculate1(DailyHistoricalDataExtCol datas, Test.PercentScore<double> score)
        {
            //符合条件的分组数量
            //DailyHistoricalDataExt[] array = datas
            //    .Where(x => x.IncreaseResults.FirstOrDefault(y => y.Percent == percent).Count > 0).ToArray();

            //包含 买盘/(买盘+买盘)的占比 的信息集合
            List<DailyHistoricalDataExt> containsPoorArray = new List<DailyHistoricalDataExt>();
            for (int i = 31; i < datas.Count; i++)
            {
                //计算日数据
                DailyHistoricalDataExt dailyData = datas[i];
                //计算日前 X 日数据
                DailyHistoricalDataExt[] previous = datas.GetPrevious(dailyData, score.Days).ToArray();

                double d = dailyData.BuyVolumePer / previous.Average(x => x.BuyVolumePer);

                if (Math.Round(d, 2) == Math.Round(score.KeyValue, 2))
                {
                    containsPoorArray.Add(dailyData);
                }
            }

            //包含 买盘/(买盘+买盘)的占比 的信息集合中符合 百分比 的信息集合
            //DailyHistoricalDataExt[] containsPoorArrayIsSuccessProcent =
            //    containsPoorArray.Where(x => x.IncreaseResults.FirstOrDefault(y => y.Percent == percent).Count > 0)
            //        .ToArray();

            //信息类型是 type 时，信息包含 value 的比率
            double per = (double)(containsPoorArray.Count) / (datas.Count);
            //信息类型是 type 时，信息包含 value 的比率
            double laplace = (double)(1 + containsPoorArray.Count) / (1 + datas.Count);

            //#if DEBUG
            //            Debug.WriteLine("原始分组数量:{0}.", lines.Length);
            //            Debug.WriteLine("信息类型是 {1} 分组数量:{0}.", array.Length, Enum.GetName(typeof(DocType), type));
            //            Debug.WriteLine("包含 {1} 的分组数量:{0}.", containsValueArray.Length, value);
            //            Debug.WriteLine("信息类型是 {1} 时，信息包含 {2} 的分组数量:{0}.", containsValueArrayIsType.Length,
            //                Enum.GetName(typeof(DocType), type), value);
            //            Debug.WriteLine(string.Format("原始比率:{0}.", per.ToString("P")));
            //            Debug.WriteLine(string.Format("拉普拉斯变换后的比率:{0}.", laplace.ToString("P")));
            //#endif
            return laplace;
        }
    }
}
