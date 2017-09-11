using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance
{
    public class DailyHistoricalDataAnalysis
    {
        public DailyHistoricalDataAnalysis(DailyHistoricalDataExt analysisDateData,
            ICollection<DailyHistoricalDataExt> datas)
        {
            if (datas.Count < DateCount)
            {
                throw new ArgumentOutOfRangeException(string.Format("datas.count<{0}", DateCount));
            }

            DateData = analysisDateData;
            Datas = datas;
        }

        public DailyHistoricalDataExt DateData { get; private set; }

        /// <summary>
        /// 获取当前计算时间前 30 天的数据
        /// </summary>
        public ICollection<DailyHistoricalDataExt> Datas { get; private set; }

        /// <summary>
        /// 需要当前数据之前多少天的数据进行计算
        /// </summary>
        public const int DateCount = 30;
    }

    public class DailyHisDataAnalysis
    {
        /// <summary>
        /// 前多少个交易日的数据
        /// </summary>
        public int DateCount { get; set; }
        /// <summary>
        /// <see cref="DailyHisDataAnalysis.DateCount"/>的收盘价平均值
        /// </summary>
        public float PreAvgClose { get; set; }
        /// <summary>
        /// <see cref="DailyHisDataAnalysis.DateCount"/>的最高价平均值
        /// </summary>
        public float PreAvgHigh { get; set; }
        /// <summary>
        /// <see cref="DailyHisDataAnalysis.DateCount"/>的最低价平均值
        /// </summary>
        public float PreAvgLow { get; set; }
        /// <summary>
        /// <see cref="DailyHisDataAnalysis.DateCount"/>的开盘价平均值
        /// </summary>
        public float PreAvgOpen { get; set; }
        /// <summary>
        /// <see cref="DailyHisDataAnalysis.DateCount"/>的成交量平均值
        /// </summary>
        public double PreAvgVolume { get; set; }
        /// <summary>
        /// <see cref="DailyHisDataAnalysis.DateCount"/>的成交量对数平均值
        /// </summary>
        public double PreAvgLogVolume { get; set; }

        public DailyHisDataAnalysis(ICollection<DailyHistoricalDataExt> datas)
        {
            DateCount = datas.Count;
            PreAvgClose = datas.Average(x => x.Close);
            PreAvgOpen = datas.Average(x => x.Open);
            PreAvgHigh = datas.Average(x => x.High);
            PreAvgLow = datas.Average(x => x.Low);
            PreAvgVolume = datas.Average(x => x.Volume);
            PreAvgLogVolume = datas.Average(x => x.LogVolumne);
        }
    }
}
