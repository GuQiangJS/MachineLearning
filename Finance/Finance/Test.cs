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
            DailyHistoricalDataExtHelper.SaveTxt(extHisiHistoricalData, extHistoricalDataTxtFilePath);
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
