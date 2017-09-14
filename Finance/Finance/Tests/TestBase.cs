using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Cyrix;
using Finance.DataExtraction;
using log4net.Repository.Hierarchy;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Finance
{
    public abstract class TestBase
    {
        public TestBase()
        {
            StockInfo = GetStockInfo();
            HistoricalDataExts = ReadExtendHistoricalData(StockInfo);
        }

        protected void WriteLine(string value)
        {
            TestContext.WriteLine(value);
            Console.WriteLine(value);
            File.AppendAllLines(
                Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "log.txt"),
                new string[] {value}, Encoding.UTF8);
        }

        protected void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }

        protected const string Market = "SZ";
        protected const string Code = "000002";
        protected const string Name = "万科A";
        protected const string DomainLocal = "Finance.Stocks.Local";

        protected T LoadJson<T>(string filePath)
        {
            T result;
            JsonSerializer serializer = new JsonSerializer();
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(stream, new UTF8Encoding(false)))
                {
                    using (JsonTextReader jsonReader = new JsonTextReader(reader))
                    {
                        result = serializer.Deserialize<T>(jsonReader);
                    }
                }
            }
            return result;
        }

        public void SaveJson<T>(T datas, string filePath)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;

            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(stream, new UTF8Encoding(false)))
                {
                    using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                    {
                        serializer.Serialize(jsonWriter, datas);
                    }
                }
            }
        }

        protected StockInfo GetStockInfo()
        {
            StockInfo result = null;
            IStockInfoReadService stockInfoRead =
                ServiceContainer.GetService<IStockInfoReadService>(DomainLocal);
            GetStockInfoEventArgs getStockInfoEventArgs = new GetStockInfoEventArgs(Market, Code);
            if (stockInfoRead.TryGetData(getStockInfoEventArgs))
            {
                result = getStockInfoEventArgs.ResultData;
            }
            return result;
        }

        protected HistoricalData GetHistoricalData(StockInfo stockInfo)
        {
            HistoricalData result = null;
            IHistoricalDataReadService historicalDataRead =
                ServiceContainer.GetService<IHistoricalDataReadService>(DomainLocal);
            GetHistoricalDataEventArgs getHistoricalDataEventArgs=new GetHistoricalDataEventArgs(stockInfo);
            if (historicalDataRead.TryGetData(getHistoricalDataEventArgs))
            {
                result = getHistoricalDataEventArgs.ResultData;
            }
            return result;
        }

        protected DailyTransDetail GetDailyTransDetail(StockInfo stockInfo,DateTime dateTime)
        {
            DailyTransDetail result = null;
            IDailyTransDetailReadService dailyTransDetailRead =
                ServiceContainer.GetService<IDailyTransDetailReadService>(DomainLocal);
            GetDailyTransDetailEventArgs getDailyTransDetailEventArgs=new GetDailyTransDetailEventArgs(stockInfo, dateTime);
            if (dailyTransDetailRead.TryGetData(getDailyTransDetailEventArgs))
            {
                result = getDailyTransDetailEventArgs.ResultData;
            }
            return result;
        }

        protected List<DailyTransDetail> GetAllDailyTransDetails(StockInfo stockInfo)
        {
            HistoricalData historicalData = GetHistoricalData(stockInfo);
            List<DailyTransDetail> result = new List<DailyTransDetail>(historicalData.HistoricalDatas.Count);
            foreach (DailyHistoricalData data in historicalData.HistoricalDatas)
            {
                result.Add(GetDailyTransDetail(stockInfo, data.Date));
            }
            return result;
        }


        protected StockInfo StockInfo;
        protected HistoricalData HistoricalData;
        protected List<DailyTransDetail> DailyTransDetails;
        /// <summary>
        /// 完整数据集
        /// </summary>
        protected DailyHistoricalDataExtCol HistoricalDataExts;

        protected string ExtHistoricalDataTxtFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "ExtHis.txt");
        protected string ExtHistoricalDataJsonFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "ExtHis.json");


        protected DailyHistoricalDataExtCol CreateExtendHistoricalData()
        {
            HistoricalData = GetHistoricalData(StockInfo);
            DailyTransDetails = GetAllDailyTransDetails(StockInfo);

            DailyHistoricalDataExtCol extHisiHistoricalData =
                new DailyHistoricalDataExtCol();
            foreach (DailyHistoricalData data in HistoricalData.HistoricalDatas)
            {
                DailyTransDetail detail =
                    DailyTransDetails.FirstOrDefault(x => x != null && x.Market == data.Market && x.StockCode == data.StockCode &&
                                                           x.TradeDate == data.Date);

                //TODO:detail为空的处理
                if (detail == null)
                {
                    TestContext.WriteLine(string.Format("{0} is Empty", data.DateString));
                    continue;
                }
                DailyHistoricalDataExt dataExt =
                    new DailyHistoricalDataExt(data, detail, HistoricalData, DailyTransDetails);
                extHisiHistoricalData.Add(dataExt);
            }
            //DailyHistoricalDataExtHelper.SaveTxt(extHisiHistoricalData, extHistoricalDataTxtFilePath);
            DailyHistoricalDataExtColHelper.SaveJson(extHisiHistoricalData, ExtHistoricalDataJsonFilePath);
            return extHisiHistoricalData;
        }

        protected void RemoveExtHistoricalDataJsonFile()
        {
            if (File.Exists(ExtHistoricalDataJsonFilePath))
            {
                File.Delete(ExtHistoricalDataJsonFilePath);
            }
        }

        protected DailyHistoricalDataExtCol ReadExtendHistoricalData(StockInfo stockInfo)
        {
            if (!File.Exists(ExtHistoricalDataJsonFilePath))
            {
                return CreateExtendHistoricalData();
            }
            return DailyHistoricalDataExtColHelper.LoadJson(stockInfo, ExtHistoricalDataJsonFilePath);
        }
    }
}
