using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private List<DailyHistoricalDataExt> _historicalDataExts;

        private string extHistoricalDataFilePath = Path.Combine(System.Environment.CurrentDirectory, "ExtHis.txt");

        public Test()
        {
            _stockInfo = GetStockInfo();
            _historicalData = GetHistoricalData(_stockInfo);
            //_dailyTransDetails = GetAllDailyTransDetails(_stockInfo);
            _historicalDataExts = ReadExtendHistoricalData(_stockInfo);
        }

        [Test]
        public void Test1()
        {
            //取 1/3 的数据作为训练数据
            IEnumerable<DailyHistoricalDataExt> trainDatas = _historicalDataExts.Take(_historicalDataExts.Count / 3);
            //剩余 2/3 的数据作为验证数据
            IEnumerable<DailyHistoricalDataExt> vaildateDatas = _historicalDataExts.Except(trainDatas);


        }
        
        public List<DailyHistoricalDataExt> CreateExtendHistoricalData()
        {
            StringBuilder stringBuilder = new StringBuilder();
            List<DailyHistoricalDataExt> extHisiHistoricalData =
                new List<DailyHistoricalDataExt>(_historicalData.HistoricalDatas.Count);
            foreach (DailyHistoricalData data in _historicalData.HistoricalDatas)
            {
                extHisiHistoricalData.Add(new DailyHistoricalDataExt(data));
                stringBuilder.AppendLine(string.Format(@"{0}\{1}\{2}\{3}\{4}\{5}\{6}", data.Date.Ticks, data.Open,
                    data.High, data.Low, data.Close, data.Volume, Math.Log(data.Volume)));
            }
            File.WriteAllText(extHistoricalDataFilePath, stringBuilder.ToString());
            return extHisiHistoricalData;
        }

        List<DailyHistoricalDataExt> ReadExtendHistoricalData(StockInfo stockInfo)
        {
            if (!File.Exists(extHistoricalDataFilePath))
            {
                return CreateExtendHistoricalData();
            }

            IEnumerable<string> lines = File.ReadLines(extHistoricalDataFilePath);
            List<DailyHistoricalDataExt> result = new List<DailyHistoricalDataExt>(lines.Count());
            foreach (string line in lines)
            {
                string[] s = line.Split('\t');
                result.Add(new DailyHistoricalDataExt()
                {
                    Date = new DateTime(long.Parse(s[0])),
                    Open = float.Parse(s[1]),
                    High = float.Parse(s[2]),
                    Low = float.Parse(s[3]),
                    Close = float.Parse(s[4]),
                    Volume = int.Parse(s[5]),
                    LogVolumne = double.Parse(s[6]),
                    CompanyName = stockInfo.CompanyName,
                    Market = stockInfo.Market,
                    StockCode = stockInfo.StockCode,
                    Description = stockInfo.Description
                });
            }
            return result;
        }
    }
}
