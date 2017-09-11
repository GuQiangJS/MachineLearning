using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using Cyrix;
using Finance.DataExtraction;

namespace Finance
{
    public abstract class TestBase
    {
        protected const string MARKET = "SZ";
        protected const string CODE = "000002";
        protected const string NAME = "万科A";
        protected const string DOMAIN_LOCAL = "Finance.Stocks.Local";

        protected StockInfo GetStockInfo()
        {
            StockInfo result = null;
            IStockInfoReadService stockInfoRead =
                ServiceContainer.GetService<IStockInfoReadService>(DOMAIN_LOCAL);
            GetStockInfoEventArgs getStockInfoEventArgs = new GetStockInfoEventArgs(MARKET, CODE);
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
                ServiceContainer.GetService<IHistoricalDataReadService>(DOMAIN_LOCAL);
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
                ServiceContainer.GetService<IDailyTransDetailReadService>(DOMAIN_LOCAL);
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
    }
}
