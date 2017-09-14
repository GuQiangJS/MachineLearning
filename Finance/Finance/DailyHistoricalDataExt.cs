using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Finance.Collection;
using Finance.DataExtraction;
using Newtonsoft.Json;

namespace Finance
{
    /// <summary>
    ///     每日成交汇总信息的扩展（增加了每日成交量的对数值）
    /// </summary>
    public class DailyHistoricalDataExt : DailyHistoricalData,INamedObject
    {
        /// <summary>
        /// 买盘总交易额
        /// </summary>
        private float _buyAmount;
        /// <summary>
        /// 卖盘总交易额
        /// </summary>
        private float _saleAmount;
        /// <summary>
        /// 中性盘交易总额
        /// </summary>
        private float _neutralAmount;

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

            _buyAmount = GetAmount(buyDatas);
            BuyVolumeSum = buyDatas.Select(x => x.Volume).Sum();
            BuyVolumeSumLog = Math.Log(BuyVolumeSum);
            BuyPriceAvg = (BuyVolumeSum != 0) ? Math.Round((_buyAmount / BuyVolumeSum), 2) : 0;

            _saleAmount = GetAmount(saleDatas);
            SaleVolumeSum = saleDatas.Select(x => x.Volume).Sum();
            SaleVolumeSumLog = Math.Log(SaleVolumeSum);
            SalePriceAvg = (SaleVolumeSum != 0) ? Math.Round((_saleAmount / SaleVolumeSum), 2) : 0;

            _neutralAmount = GetAmount(neutralDatas);
            NeutralVolumeSum = neutralDatas.Select(x => x.Volume).Sum();
            NeutralVolumeSumLog = Math.Log(NeutralVolumeSum);
            NeutralPriceAvg = (NeutralVolumeSum != 0) ? Math.Round((_neutralAmount / NeutralVolumeSum), 2) : 0;

            BuySaleVolume = (SaleVolumeSum > BuyVolumeSum) ? -1 : 1;
            BuySaleVolumePoor = BuySaleVolume * (BuyVolumeSum - SaleVolumeSum);
            BuySaleVolumePoorLog = Math.Log(BuySaleVolumePoor);

            BuyVolumePer = (double) BuyVolumeSum / (double) (BuyVolumeSum + SaleVolumeSum);

            //当前计算日期之后的所有交易汇总
            IEnumerable<DailyHistoricalData> transDatas = hisData.HistoricalDatas.SkipWhile(x => x.Date <= data.Date);

            IncreaseResults = new IncreaseResultCollection();

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
        /// 获取或设置 买盘/(买盘+卖盘) 占比值
        /// </summary>
        public double BuyVolumePer { get; set; }

        /// <summary>
        /// 获取或设置买盘/卖盘交易总量间的差额绝对值
        /// </summary>
        public double BuySaleVolumePoor { get; set; }

        /// <summary>
        /// 获取或设置买盘/买盘交易总量间的差额（卖盘>买盘=-1，否则=1）
        /// </summary>
        public int BuySaleVolume { get; set; }

        /// <summary>
        /// 获取或设置买盘/卖盘交易总量间的差额绝对值的对数
        /// </summary>
        public double BuySaleVolumePoorLog { get; set; }

        /// <summary>
        /// 获取或设置买盘交易总量的对数
        /// </summary>
        public double SaleVolumeSumLog { get; set; }

        /// <summary>
        /// 获取或设置买盘交易总量的对数
        /// </summary>
        public double NeutralVolumeSumLog { get; set; }

        /// <summary>
        /// 获取或设置买盘交易总量的对数
        /// </summary>
        public double BuyVolumeSumLog { get; set; }

        /// <summary>
        /// 获取或设置涨到指定目标价位所需的交易日的计算结果集合
        /// </summary>
        public IncreaseResultCollection IncreaseResults { get; set; }

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
                if (target <= transData.Low)
                {
                    return count;
                }
                else
                {
                    if (target <= transData.High)
                    {
                        return count;
                    }
                }
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
        /// 对象的名称
        /// </summary>
        public override string Name {
            get { return DateString; }
            set { }
        }
    }

    public class DailyHistoricalDataExtCol : NamedCollection<DailyHistoricalDataExt>
    {
        /// <summary>
        /// 获取指定列表中指定实例<paramref name="data"/>之前的 <paramref name="count"/> 个数据集合
        /// </summary>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<DailyHistoricalDataExt> GetPrevious(DailyHistoricalDataExt data, int count)
        {
            if(data==null)
                throw new ArgumentNullException(nameof(data));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            //集合中指定实例之前的对象集合
            Stack<DailyHistoricalDataExt> stack = new Stack<DailyHistoricalDataExt>();
            foreach (DailyHistoricalDataExt item in this.Items)
            {
                if (!ReferenceEquals(data, item))
                {
                    stack.Push(item);
                }
                else
                {
                    break;
                }
            }
            if (stack.Count < count)
            {
                throw new ArgumentOutOfRangeException("count过大！");
            }
            Stack<DailyHistoricalDataExt> result = new Stack<DailyHistoricalDataExt>(count);
            while (result.Count < count)
            {
                result.Push(stack.Pop());
            }
            return result.ToArray();
        }

        /// <summary>
        /// 获取指定列表中指定实例<paramref name="data"/>之后的 <paramref name="count"/> 个数据集合
        /// </summary>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<DailyHistoricalDataExt> GetNexts(DailyHistoricalDataExt data, int count)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            int index = -1;
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (ReferenceEquals(this.Items[i], data))
                {
                    index = i;
                    break;
                }
            }
            if(index==-1)
                throw new ArgumentOutOfRangeException("data不存在！");
            return this.Items.Skip(index).Take(count);
        }

        public void AddRange(IEnumerable<DailyHistoricalDataExt> col)
        {
            foreach (DailyHistoricalDataExt ext in col)
            {
                this.Items.Add(ext);
            }
        }
    }

    public static class DailyHistoricalDataExtHelper
    {
        /// <summary>
        /// 指定的<see cref="DailyHistoricalDataExt"/>的指定比率是否是有效数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="percent"></param>
        /// <returns>当指定比率的 100 > 达标天数 > 0 时返回<c>true</c>，否则返回<c>false</c>。</returns>
        public static bool IsEffective(this DailyHistoricalDataExt data, float percent)
        {
            IncreaseResult r = data.IncreaseResults[percent];
            return r.Count > 0 && r.Count < 100;
        }
    }
}