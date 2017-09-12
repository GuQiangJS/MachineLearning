using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Cyrix;
using Finance.DataExtraction;
using NUnit.Framework;

namespace Finance
{
    [TestFixture]
    public class Test:TestBase
    {
        private StockInfo _stockInfo;
        private HistoricalData _historicalData;
        private List<DailyTransDetail> _dailyTransDetails;
        /// <summary>
        /// 完整数据集
        /// </summary>
        private List<DailyHistoricalDataExt> _historicalDataExts;

        private string extHistoricalDataTxtFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "ExtHis.txt");
        private string extHistoricalDataJsonFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "ExtHis.json");

        //无论是训练数据还是验证数据，接近尾端的交易记录中如果有超过如下值才有效的情况下可能会出现找不到数据的情况
        private const int _retreatDays = 100;

        /// <summary>
        /// 原始训练数据集
        /// </summary>
        private IEnumerable<DailyHistoricalDataExt> _trainDatas;
        /// <summary>
        /// 原始校验数据集
        /// </summary>
        private IEnumerable<DailyHistoricalDataExt> _validateDatas;

        public Test()
        {
            _stockInfo = GetStockInfo();
            _historicalDataExts = ReadExtendHistoricalData(_stockInfo);
            MakeData();
        }

        [Test]
        public void T()
        {
            /*
            Score(SMS是垃圾短信 | SMS包含 free、car 和 txt) = log(P(SMS 是垃圾短信))
            + log(Laplace(SMS包含“free”| SMS是垃圾短信))
            + log(Laplace(SMS包含“car”| SMS是垃圾短信))
            + log(Laplace(SMS包含“txt”| SMS是垃圾短信))
            */

            TestContext.WriteLine(string.Format("总训练数据条数:{0}.",_trainDatas.Count()));

            TestContext.WriteLine(string.Format("训练数据中达到1.05%比率天数的平均值:{0}.",
                Math.Round(GetAvgData(_trainDatas, 1.05f), 2)));
            TestContext.WriteLine(string.Format("训练数据中达到1.10%比率天数的平均值:{0}.",
                Math.Round(GetAvgData(_trainDatas, 1.1f), 2)));
            TestContext.WriteLine(string.Format("训练数据中达到1.15%比率天数的平均值:{0}.",
                Math.Round(GetAvgData(_trainDatas, 1.15f), 2)));
            TestContext.WriteLine(string.Format("训练数据中达到1.20%比率天数的平均值:{0}.",
                Math.Round(GetAvgData(_trainDatas, 1.2f), 2)));

            //训练数据中 1.05% 的有效涨幅集合
            IEnumerable<DailyHistoricalDataExt> vDatas1 = GetVData1(_trainDatas, 1.05f);
            //训练数据中 1.05% 的无效涨幅集合
            IEnumerable<DailyHistoricalDataExt> datas1 = GetVData2(_trainDatas, 1.05f);
            //训练数据中 1.10% 的有效涨幅集合
            IEnumerable<DailyHistoricalDataExt> vDatas2 = GetVData1(_trainDatas, 1.1f);
            //训练数据中 1.10% 的无效涨幅集合
            IEnumerable<DailyHistoricalDataExt> datas2 = GetVData2(_trainDatas, 1.1f);
            //训练数据中 1.15% 的有效涨幅集合
            IEnumerable<DailyHistoricalDataExt> vDatas3 = GetVData1(_trainDatas, 1.15f);
            //训练数据中 1.15% 的无效涨幅集合
            IEnumerable<DailyHistoricalDataExt> datas3 = GetVData2(_trainDatas, 1.15f);
            //训练数据中 1.2% 的有效涨幅集合
            IEnumerable<DailyHistoricalDataExt> vDatas4 = GetVData1(_trainDatas, 1.2f);
            //训练数据中 1.2% 的无效涨幅集合
            IEnumerable<DailyHistoricalDataExt> datas4 = GetVData2(_trainDatas, 1.2f);
            
            TestContext.WriteLine(string.Format("训练数据中 1.05% 的有效涨幅集合数量:{0}.", vDatas1.Count()));
            TestContext.WriteLine(string.Format("训练数据中 1.05% 的无效涨幅集合数量:{0}.", datas1.Count()));
            TestContext.WriteLine(string.Format("训练数据中 1.10% 的有效涨幅集合数量:{0}.", vDatas2.Count()));
            TestContext.WriteLine(string.Format("训练数据中 1.10% 的无效涨幅集合数量:{0}.", datas2.Count()));
            TestContext.WriteLine(string.Format("训练数据中 1.15% 的有效涨幅集合数量:{0}.", vDatas3.Count()));
            TestContext.WriteLine(string.Format("训练数据中 1.15% 的无效涨幅集合数量:{0}.", datas3.Count()));
            TestContext.WriteLine(string.Format("训练数据中 1.20% 的有效涨幅集合数量:{0}.", vDatas4.Count()));
            TestContext.WriteLine(string.Format("训练数据中 1.20% 的无效涨幅集合数量:{0}.", datas4.Count()));

            TestContext.WriteLine(string.Format("训练数据中 1.05% 的有效涨幅的占比:{0}.",
                Math.Round((double)vDatas1.Count()/_trainDatas.Count(), 2).ToString("P")));
            TestContext.WriteLine(string.Format("训练数据中 1.10% 的有效涨幅的占比:{0}.",
                Math.Round((double)vDatas2.Count() / _trainDatas.Count(), 2).ToString("P")));
            TestContext.WriteLine(string.Format("训练数据中 1.15% 的有效涨幅的占比:{0}.",
                Math.Round((double)vDatas3.Count() / _trainDatas.Count(), 2).ToString("P")));
            TestContext.WriteLine(string.Format("训练数据中 1.20% 的有效涨幅的占比:{0}.",
                Math.Round((double)vDatas4.Count() / _trainDatas.Count(), 2).ToString("P")));

            //分别计算当日与前1，前5，前10，前15，前20，前25，前30之间的买盘成交量差额
            
        }

        private int GetData(IEnumerable<DailyHistoricalDataExt> datas, int index, out int d1, out int d2, out int d3,
            out int d4, out int d5, out int d6, out int d7)
        {
            datas.Take()
        }

        /// <summary>
        /// 从指定集合中获取指定百分比的有效数据
        /// </summary>
        /// <param name="datas">数据集合</param>
        /// <param name="percent">比率</param>
        /// <param name="maxDays">日期期限，如果是0则取<see cref="_retreatDays"/></param>
        /// <returns></returns>
        IEnumerable<DailyHistoricalDataExt> GetVData1(IEnumerable<DailyHistoricalDataExt> datas, float percent,int maxDays=0)
        {
            //查找数据时剔除最后一百个交易日的数据
            int trainDays = datas.Count() - ((maxDays == 0) ? _retreatDays : maxDays);
            int maxD= ((maxDays == 0) ? _retreatDays : maxDays);
            /*
             * 首先剔除传入数据（datas）中的最后100条数据
             * 取数据中
             * 1.DailyHistoricalDataExt.IncreaseResults.Count>0
             * 2.同时DailyHistoricalDataExt.IncreaseResults.Count<100
             * 3.同时DailyHistoricalDataExt.IncreaseResults.Percent == percent
             * 的数据
             */
            return datas.Take(trainDays).Where(x => x.IncreaseResults.Where(y => y.Count > 0 && y.Count < maxD && y.Percent == percent).Count() > 0);
        }

        /// <summary>
        /// 从指定集合中获取指定百分比的有效数据的平均天数
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        double GetAvgData(IEnumerable<DailyHistoricalDataExt> datas, float percent)
        {
            //查找数据时剔除最后一百个交易日的数据
            int trainDays = datas.Count() - _retreatDays;
            List<int> cs = new List<int>();
            foreach (DailyHistoricalDataExt data in datas.Take(trainDays))
            {
                DailyHistoricalDataExt.IncreaseResult r =
                    data.IncreaseResults.FirstOrDefault(y => y.Count > 0 && y.Percent == percent);
                if (r.Count>0)
                {
                    cs.Add(r.Count);
                }
            }
            return cs.Average();
        }
            /// <summary>
        /// 从指定集合中获取指定百分比的无效数据
        /// </summary>
        /// <param name="datas">数据集合</param>
        /// <param name="percent">比率</param>
        /// <param name="maxDays">日期期限，如果是0则取<see cref="_retreatDays"/></param>
        /// <returns></returns>
        IEnumerable<DailyHistoricalDataExt> GetVData2(IEnumerable<DailyHistoricalDataExt> datas, float percent, int maxDays = 0)
        {
            //查找数据时剔除最后一百个交易日的数据
            int trainDays = datas.Count() - ((maxDays == 0) ? _retreatDays : maxDays);
            int maxD = ((maxDays == 0) ? _retreatDays : maxDays);
            /*
             * 首先剔除传入数据（datas）中的最后100条数据
             * 取数据中
             * 1.DailyHistoricalDataExt.IncreaseResults.Count<0
             * 2.或者DailyHistoricalDataExt.IncreaseResults.Count>100
             * 3.同时DailyHistoricalDataExt.IncreaseResults.Percent == percent
             * 的数据
             */
            return datas.Take(trainDays).Where(x => x.IncreaseResults.Where(y => (y.Count < 0 || y.Count > maxD) && y.Percent == percent).Count() > 0);
        }

        /// <summary>
        /// 制作 训练/测试 数据集
        /// </summary>
        public void MakeData()
        {
            //取 1/3 的数据作为训练数据
            _trainDatas = _historicalDataExts.Take(_historicalDataExts.Count / 3);
            //剩余 2/3 的数据作为验证数据
            _validateDatas = _historicalDataExts.Except(_trainDatas);
        }
        
        public List<DailyHistoricalDataExt> CreateExtendHistoricalData()
        {
            _historicalData = GetHistoricalData(_stockInfo);
            _dailyTransDetails = GetAllDailyTransDetails(_stockInfo);

            List<DailyHistoricalDataExt> extHisiHistoricalData =
                new List<DailyHistoricalDataExt>(_historicalData.HistoricalDatas.Count);
            foreach (DailyHistoricalData data in _historicalData.HistoricalDatas)
            {
                DailyTransDetail detail =
                    _dailyTransDetails.FirstOrDefault(x => x!=null && x.Market == data.Market && x.StockCode == data.StockCode &&
                                                           x.TradeDate == data.Date);

                //TODO:detail为空的处理
                if (detail == null)
                {
                    TestContext.WriteLine(string.Format("{0} is Empty", data.DateString));
                    continue;
                }
                DailyHistoricalDataExt dataExt =
                    new DailyHistoricalDataExt(data, detail, _historicalData, _dailyTransDetails);
                extHisiHistoricalData.Add(dataExt);
            }
            //DailyHistoricalDataExtHelper.SaveTxt(extHisiHistoricalData, extHistoricalDataTxtFilePath);
            DailyHistoricalDataExtHelper.SaveJson(extHisiHistoricalData, extHistoricalDataJsonFilePath);
            return extHisiHistoricalData;
        }

        List<DailyHistoricalDataExt> ReadExtendHistoricalData(StockInfo stockInfo)
        {
            if (!File.Exists(extHistoricalDataJsonFilePath))
            {
                return CreateExtendHistoricalData();
            }
            return DailyHistoricalDataExtHelper.LoadJson(stockInfo, extHistoricalDataJsonFilePath);
        }
    }
}
