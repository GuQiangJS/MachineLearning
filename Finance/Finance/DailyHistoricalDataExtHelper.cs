using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Finance.DataExtraction;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Finance
{
    public class DailyHistoricalDataExtContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// Creates properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract" />.
        /// </summary>
        /// <param name="type">The type to create properties for.</param>
        ///             /// <param name="memberSerialization">The member serialization mode for the type.</param>
        /// <returns>Properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract" />.</returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            string[] skipProps = new string[] {"Name", "StockCode", "Market", "CompanyName", "Version", "Description"};
            IList<JsonProperty> result = base.CreateProperties(type, memberSerialization);
            result = result.Where(x => !skipProps.Contains(x.PropertyName)).ToList();
            return result;
        }
    }

    /// <summary>
    /// 每日成交汇总信息帮助类
    /// </summary>
    public class DailyHistoricalDataExtColHelper
    {
        public static void SaveJson(DailyHistoricalDataExtCol datas, string filePath)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.Formatting = Formatting.Indented;

            serializer.ContractResolver = new DailyHistoricalDataExtContractResolver();

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

        public static DailyHistoricalDataExtCol LoadJson(StockInfo stockInfo, string filePath)
        {
            DailyHistoricalDataExtCol result = new DailyHistoricalDataExtCol();
            JsonSerializer serializer = new JsonSerializer();
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(stream, new UTF8Encoding(false)))
                {
                    using (JsonTextReader jsonReader = new JsonTextReader(reader))
                    {
                        result = serializer.Deserialize<DailyHistoricalDataExtCol>(jsonReader);
                    }
                }
            }
            foreach (DailyHistoricalDataExt dataExt in result)
            {
                dataExt.Market = stockInfo.Market;
                dataExt.StockCode = stockInfo.StockCode;
                dataExt.Description = stockInfo.Description;
                dataExt.CompanyName = stockInfo.CompanyName;
                dataExt.Version = stockInfo.Version;
            }
            return result;
        }

        public static List<DailyHistoricalDataExt> LoadTxt(StockInfo stockInfo, string filePath)
        {
            IEnumerable<string> lines = File.ReadLines(filePath);
            List<DailyHistoricalDataExt> result = new List<DailyHistoricalDataExt>(lines.Count());
            foreach (string line in lines)
            {
                string[] s = line.Split('\\');
                result.Add(new DailyHistoricalDataExt()
                {
                    Date = new DateTime(long.Parse(s[0])),
                    Open = float.Parse(s[1]),
                    High = float.Parse(s[2]),
                    Low = float.Parse(s[3]),
                    Close = float.Parse(s[4]),
                    Volume = int.Parse(s[5]),
                    LogVolume = double.Parse(s[6]),
                    BuyPriceAvg = double.Parse(s[7]),
                    BuyVolumeSum = Int64.Parse(s[8]),
                    NeutralPriceAvg = double.Parse(s[9]),
                    NeutralVolumeSum = Int64.Parse(s[10]),
                    SalePriceAvg = double.Parse(s[11]),
                    SaleVolumeSum = Int64.Parse(s[12]),
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
