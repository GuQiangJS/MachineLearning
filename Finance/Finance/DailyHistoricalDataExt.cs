using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.DataExtraction;

namespace Finance
{
    public class DailyHistoricalDataExt:DailyHistoricalData
    {
        public DailyHistoricalDataExt()
        {
        }

        public DailyHistoricalDataExt(DailyHistoricalData data):this()
        {
            this.Date = data.Date;
            this.Adj_Close = data.Adj_Close;
            this.Close = data.Close;
            this.CompanyName = data.CompanyName;
            this.DateString = data.DateString;
            this.Description = data.Description;
            this.High = data.High;
            this.Low = data.Low;
            this.Market = data.Market;
            this.Name = data.Name;
            this.Open = data.Open;
            this.StockCode = data.StockCode;
            this.Symbol = data.Symbol;
            this.Version = data.Version;
            this.Volume = data.Volume;
            this.LogVolumne = Math.Log(Volume);
        }

        public double LogVolumne { get; set; }
    }
}
