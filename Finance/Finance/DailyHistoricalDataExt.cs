using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Finance.DataExtraction;
using Newtonsoft.Json;

namespace Finance
{
    /// <summary>
    ///     每日成交汇总信息的扩展（增加了每日成交量的对数值）
    /// </summary>
    public class DailyHistoricalDataExt : DailyHistoricalData
    {
        /// <summary>
        /// 买盘总交易额
        /// </summary>
        private float buyAmount;
        /// <summary>
        /// 卖盘总交易额
        /// </summary>
        private float saleAmount;
        /// <summary>
        /// 中性盘交易总额
        /// </summary>
        private float neutralAmount;

        public DailyHistoricalDataExt()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">当日汇总数据</param>
        /// <param name="detail">当日成交明细</param>
        /// <param name="detahisDatail">所有汇总数据</param>
        /// <param name="transDetails">所有交易明细</param>
        public DailyHistoricalDataExt(DailyHistoricalData data,DailyTransDetail detail, HistoricalData hisData, List<DailyTransDetail> transDetails) : this()
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (detail == null)
                throw new ArgumentNullException(nameof(detail));

            Date = data.Date;
            Adj_Close = data.Adj_Close;
            Close = data.Close;
            CompanyName = data.CompanyName;
            DateString = data.DateString;
            Description = data.Description;
            High = data.High;
            Low = data.Low;
            Market = data.Market;
            Name = data.Name;
            Open = data.Open;
            StockCode = data.StockCode;
            Symbol = data.Symbol;
            Version = data.Version;
            Volume = data.Volume;
            LogVolume = Math.Log(Volume);

            IEnumerable<SingleTransData> buyDatas =
                detail.SingleTrans.Where(x => x.Nature == SingleTransData.NatureEnum.Buy);
            IEnumerable<SingleTransData> saleDatas =
                detail.SingleTrans.Where(x => x.Nature == SingleTransData.NatureEnum.Sale);
            IEnumerable<SingleTransData> neutralDatas =
                detail.SingleTrans.Where(x => x.Nature == SingleTransData.NatureEnum.Neutral);

            buyAmount = GetAmount(buyDatas);
            BuyVolumeSum = buyDatas.Select(x => x.Volume).Sum();
            BuyPriceAvg = (BuyVolumeSum != 0) ? Math.Round((buyAmount / BuyVolumeSum), 2) : 0;

            saleAmount = GetAmount(saleDatas);
            SaleVolumeSum = saleDatas.Select(x => x.Volume).Sum();
            SalePriceAvg = (SaleVolumeSum != 0) ? Math.Round((saleAmount / SaleVolumeSum), 2) : 0;

            neutralAmount = GetAmount(neutralDatas);
            NeutralVolumeSum = neutralDatas.Select(x => x.Volume).Sum();
            NeutralPriceAvg = (NeutralVolumeSum != 0) ? Math.Round((neutralAmount / NeutralVolumeSum), 2) : 0;

            //当前计算日期之后的所有交易汇总
            IEnumerable<DailyHistoricalData> transDatas = hisData.HistoricalDatas.SkipWhile(x => x.Date <= data.Date);

            IncreaseResults = new List<IncreaseResult>();

            IncreaseResults.Add(new IncreaseResult(1.05f, BuyPriceAvg,
                GetIncreaseDates(data, BuyPriceAvg, transDatas, 1.05f)));
            IncreaseResults.Add(new IncreaseResult(1.1f, BuyPriceAvg,
                GetIncreaseDates(data, BuyPriceAvg, transDatas, 1.1f)));
            IncreaseResults.Add(new IncreaseResult(1.15f, BuyPriceAvg,
                GetIncreaseDates(data, BuyPriceAvg, transDatas, 1.15f)));
            IncreaseResults.Add(new IncreaseResult(1.20f, BuyPriceAvg,
                GetIncreaseDates(data, BuyPriceAvg, transDatas, 1.20f)));
        }

        /// <summary>
        /// 获取或设置涨到指定目标价位所需的交易日的计算结果集合
        /// </summary>
        public List<IncreaseResult> IncreaseResults { get; set; }

        /// <summary>
        /// 尝试获取当前交易日后多少个交易日可以达到指定涨幅
        /// </summary>
        /// <param name="data">当前交易日数据</param>
        /// <param name="calcPrice">计算价格</param>
        /// <param name="transDatas">当前交易日后的所有交易日汇总数据</param>
        /// <param name="increasePre">涨幅</param>
        /// <returns>当找到数据时返回找到的天数值，否则返回-1。</returns>
        int GetIncreaseDates(DailyHistoricalData data,double calcPrice, IEnumerable<DailyHistoricalData> transDatas,
            float increasePre)
        {
            //目标价
            double target = Math.Round((calcPrice * increasePre), 2);
            //天数
            int count = 1;

            foreach (DailyHistoricalData transData in transDatas)
            {
                if (transData.Low <= target && transData.High >= target)
                    return count;
                count++;
            }
            return -1;
        }
        
        /// <summary>
        /// 获取总交易金额
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        private float GetAmount(IEnumerable<SingleTransData> datas)
        {
            return datas.Select(x => x.TransPrice * x.Volume).Sum();
        }

        /// <summary>
        /// 获取总交易量
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        private float GetVolumnSum(IEnumerable<SingleTransData> datas)
        {
            return datas.Select(x => x.Volume).Sum();
        }

        /// <summary>
        /// 获取或设置买盘均价
        /// </summary>
        public double BuyPriceAvg { get; set; }

        /// <summary>
        /// 获取或设置买盘交易总量
        /// </summary>
        public Int64 BuyVolumeSum { get; set; }

        /// <summary>
        /// 获取或设置卖盘均价
        /// </summary>
        public double SalePriceAvg { get; set; }

        /// <summary>
        /// 获取或设置卖盘交易总量
        /// </summary>
        public Int64 SaleVolumeSum { get; set; }

        /// <summary>
        /// 获取或设置中性盘均价
        /// </summary>
        public double NeutralPriceAvg { get; set; }

        /// <summary>
        /// 获取或设置中性盘交易总量
        /// </summary>
        public Int64 NeutralVolumeSum { get; set; }

        /// <summary>
        ///     获取或设置每日成交量的对数值
        /// </summary>
        public double LogVolume { get; set; }

        /// <summary>
        /// 涨到指定目标价位所需的交易日的计算结果
        /// </summary>
        public struct IncreaseResult
        {
            public IncreaseResult(float percent,double calcPrice,int count)
            {
                Percent = percent;
                CalcPrice = calcPrice;
                Count = count;
                TargetPrice= Math.Round((calcPrice * percent), 2);
            }
            /// <summary>
            /// 获取或设置目标价格
            /// </summary>
            public double TargetPrice { get; set; }

            /// <summary>
            /// 获取或设置百分比
            /// </summary>
            public float Percent { get; set; }

            /// <summary>
            /// 获取或设置计算价格
            /// </summary>
            public double CalcPrice { get; set; }

            /// <summary>
            /// 获取或设置所需交易日的计算结果
            /// </summary>
            /// <value>返回 -1 表示尚未达到指定涨幅</value>
            public int Count { get; set; }
        }
    }
}