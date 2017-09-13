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
        /// <summary>
        /// 训练数据
        /// </summary>
        private DailyHistoricalDataExtCol _trainDatas;
        /// <summary>
        /// 验证数据
        /// </summary>
        private DailyHistoricalDataExtCol _validateDatas;

        public Test() : base()
        {
            MakeData();
        }

        /// <summary>
        /// 制作 训练/测试 数据集
        /// </summary>
        void MakeData()
        {
            _trainDatas=new DailyHistoricalDataExtCol();
            _validateDatas=new DailyHistoricalDataExtCol();
            //取 1/3 的数据作为训练数据
            _trainDatas.AddRange(HistoricalDataExts.Take(HistoricalDataExts.Count / 3));
            //剩余 2/3 的数据作为验证数据
            _validateDatas.AddRange(HistoricalDataExts.Except(_trainDatas));
        }


        [Test]
        public void TestVolume()
        {
            for (int i = 31; i < _trainDatas.Count; i++)
            {
                DailyHistoricalDataExt dailyData = _trainDatas[i];
                IEnumerable<DailyHistoricalDataExt> previous1 = _trainDatas.GetPrevious(dailyData, 1);
                IEnumerable<DailyHistoricalDataExt> previous5 = _trainDatas.GetPrevious(dailyData, 5);
            }
        }
    }



    //[TestFixture]
    //public class Test:TestBase
    //{
    //    /// <summary>
    //    /// 训练得分
    //    /// </summary>
    //    private IList<Score> _trainScores;

    //    private string _trainScoreJsonFilePath =
    //        Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "TrainScore.json");

    //    //无论是训练数据还是验证数据，接近尾端的交易记录中如果有超过如下值才有效的情况下可能会出现找不到数据的情况
    //    private const int RetreatDays = 100;

    //    /// <summary>
    //    /// 原始训练数据集
    //    /// </summary>
    //    private IEnumerable<DailyHistoricalDataExt> _trainDatas;
    //    /// <summary>
    //    /// 原始校验数据集
    //    /// </summary>
    //    private IEnumerable<DailyHistoricalDataExt> _validateDatas;

    //    public Test()
    //    {
    //        StockInfo = GetStockInfo();
    //        HistoricalDataExts = ReadExtendHistoricalData(StockInfo);
    //        MakeData();
    //        //LoadTrainScore();
    //    }

    //    /// <summary>
    //    /// 制作 训练/测试 数据集
    //    /// </summary>
    //    public void MakeData()
    //    {
    //        //取 1/3 的数据作为训练数据
    //        _trainDatas = HistoricalDataExts.Take(HistoricalDataExts.Count / 3);
    //        //剩余 2/3 的数据作为验证数据
    //        _validateDatas = HistoricalDataExts.Except(_trainDatas);
    //    }
    //    [Test]
    //    public void EmptyTest()
    //    { }

    //    [Test]
    //    public void T()
    //    {
    //        /*
    //        Score(SMS是垃圾短信 | SMS包含 free、car 和 txt) = log(P(SMS 是垃圾短信))
    //        + log(Laplace(SMS包含“free”| SMS是垃圾短信))
    //        + log(Laplace(SMS包含“car”| SMS是垃圾短信))
    //        + log(Laplace(SMS包含“txt”| SMS是垃圾短信))
    //        */

    //        TestContext.WriteLine(string.Format("总训练数据条数:{0}.",_trainDatas.Count()));

    //        TestContext.WriteLine(string.Format("训练数据中达到1.05%比率天数的平均值:{0}.",
    //            Math.Round(GetAvgData(_trainDatas, 1.05f), 2)));
    //        TestContext.WriteLine(string.Format("训练数据中达到1.10%比率天数的平均值:{0}.",
    //            Math.Round(GetAvgData(_trainDatas, 1.1f), 2)));
    //        TestContext.WriteLine(string.Format("训练数据中达到1.15%比率天数的平均值:{0}.",
    //            Math.Round(GetAvgData(_trainDatas, 1.15f), 2)));
    //        TestContext.WriteLine(string.Format("训练数据中达到1.20%比率天数的平均值:{0}.",
    //            Math.Round(GetAvgData(_trainDatas, 1.2f), 2)));

    //        //训练数据中 1.05% 的有效涨幅集合
    //        IEnumerable<DailyHistoricalDataExt> vDatas1 = GetVData1(_trainDatas, 1.05f);
    //        //训练数据中 1.05% 的无效涨幅集合
    //        IEnumerable<DailyHistoricalDataExt> datas1 = GetVData2(_trainDatas, 1.05f);
    //        //训练数据中 1.10% 的有效涨幅集合
    //        IEnumerable<DailyHistoricalDataExt> vDatas2 = GetVData1(_trainDatas, 1.1f);
    //        //训练数据中 1.10% 的无效涨幅集合
    //        IEnumerable<DailyHistoricalDataExt> datas2 = GetVData2(_trainDatas, 1.1f);
    //        //训练数据中 1.15% 的有效涨幅集合
    //        IEnumerable<DailyHistoricalDataExt> vDatas3 = GetVData1(_trainDatas, 1.15f);
    //        //训练数据中 1.15% 的无效涨幅集合
    //        IEnumerable<DailyHistoricalDataExt> datas3 = GetVData2(_trainDatas, 1.15f);
    //        //训练数据中 1.2% 的有效涨幅集合
    //        IEnumerable<DailyHistoricalDataExt> vDatas4 = GetVData1(_trainDatas, 1.2f);
    //        //训练数据中 1.2% 的无效涨幅集合
    //        IEnumerable<DailyHistoricalDataExt> datas4 = GetVData2(_trainDatas, 1.2f);

    //        TestContext.WriteLine(string.Format("训练数据中 1.05% 的有效涨幅集合数量:{0}.", vDatas1.Count()));
    //        TestContext.WriteLine(string.Format("训练数据中 1.05% 的无效涨幅集合数量:{0}.", datas1.Count()));
    //        TestContext.WriteLine(string.Format("训练数据中 1.10% 的有效涨幅集合数量:{0}.", vDatas2.Count()));
    //        TestContext.WriteLine(string.Format("训练数据中 1.10% 的无效涨幅集合数量:{0}.", datas2.Count()));
    //        TestContext.WriteLine(string.Format("训练数据中 1.15% 的有效涨幅集合数量:{0}.", vDatas3.Count()));
    //        TestContext.WriteLine(string.Format("训练数据中 1.15% 的无效涨幅集合数量:{0}.", datas3.Count()));
    //        TestContext.WriteLine(string.Format("训练数据中 1.20% 的有效涨幅集合数量:{0}.", vDatas4.Count()));
    //        TestContext.WriteLine(string.Format("训练数据中 1.20% 的无效涨幅集合数量:{0}.", datas4.Count()));

    //        TestContext.WriteLine(string.Format("训练数据中 1.05% 的有效涨幅的占比:{0}.",
    //            Math.Round((double)vDatas1.Count()/_trainDatas.Count(), 2).ToString("P")));
    //        TestContext.WriteLine(string.Format("训练数据中 1.10% 的有效涨幅的占比:{0}.",
    //            Math.Round((double)vDatas2.Count() / _trainDatas.Count(), 2).ToString("P")));
    //        TestContext.WriteLine(string.Format("训练数据中 1.15% 的有效涨幅的占比:{0}.",
    //            Math.Round((double)vDatas3.Count() / _trainDatas.Count(), 2).ToString("P")));
    //        TestContext.WriteLine(string.Format("训练数据中 1.20% 的有效涨幅的占比:{0}.",
    //            Math.Round((double)vDatas4.Count() / _trainDatas.Count(), 2).ToString("P")));

    //    }

    //    [Test]
    //    public void T1()
    //    {
    //        //训练数据
    //        DailyHistoricalDataExt[] trainDataExts = _trainDatas.ToArray();
    //        for (int i = MaxDays; i < trainDataExts.Length - RetreatDays; i++)
    //        {
    //            DailyHistoricalDataExt dailyData = trainDataExts[i];

    //            //分别计算当日与前1~前30之间的买盘/(买盘+卖盘)的占比均值
    //            Dictionary<int, double> buySalePer = GetPerData(trainDataExts.Get(i - MaxDays, MaxDays));

    //            double effScore = 0d;
    //            double valScore = 0d;
    //            foreach (KeyValuePair<int, double> keyValuePair in buySalePer)
    //            {
    //                Score score = _trainScores.FirstOrDefault(x => x.Days == keyValuePair.Key &&
    //                                                               Math.Round(x.Poor, 2) == keyValuePair.Value);
    //                effScore = effScore + score.EffectiveScore;
    //                valScore = valScore + score.InvalidScore;
    //            }
    //        }
    //    }

    //    [Test]
    //    public void T2()
    //    {
    //        //测试数据
    //        DailyHistoricalDataExt[] valDataExts = _validateDatas.ToArray();
    //        TestContext.WriteLine(string.Format("总测试数据量:{0}.", valDataExts.Length));

    //        //数据中的有效数据量
    //        IEnumerable<DailyHistoricalDataExt> tt = GetVData1(_validateDatas, 1.05f);
    //        TestContext.WriteLine(string.Format("总测试数据中的有效量:{0}.", tt.Count()));

    //        int effCount = 0;
    //        StringBuilder t = new StringBuilder();
    //        t.AppendLine(@"DateString\tEffScore\tValScore\tCount");
    //        for (int i = MaxDays; i < valDataExts.Length - RetreatDays; i++)
    //        {
    //            DailyHistoricalDataExt dailyData = valDataExts[i];

    //            //分别计算当日与前1~前30之间的买盘/(买盘+卖盘)的占比均值
    //            Dictionary<int, double> buySalePer = GetPerData(valDataExts.Get(i - MaxDays, MaxDays));

    //            double effScore = 0d;
    //            double valScore = 0d;
    //            foreach (KeyValuePair<int, double> keyValuePair in buySalePer)
    //            {
    //                Score score = _trainScores.FirstOrDefault(x => x.Days == keyValuePair.Key &&
    //                                                               Math.Round(x.Poor, 2) == keyValuePair.Value);
    //                effScore = effScore + score.EffectiveScore;
    //                valScore = valScore + score.InvalidScore;
    //            }

    //            if (effScore > valScore)
    //            {
    //                effCount++;
    //                t.AppendLine(
    //                    string.Format(@"{0}\t{1}\t{2}\t{3}"
    //                        , dailyData.DateString,
    //                        effScore,
    //                        valScore,
    //                        dailyData.IncreaseResults.Where(x => x.Percent == 1.05f).Select(x => x.Count)
    //                            .FirstOrDefault()));
    //            }
    //        }
    //        TestContext.WriteLine(string.Format("有效数据量:{0}.", effCount));
    //        TestContext.WriteLine(t.ToString());
    //    }

    //    private void LoadTrainScore()
    //    {
    //        if (File.Exists(_trainScoreJsonFilePath))
    //        {
    //            _trainScores = LoadJson<IList<Score>>(_trainScoreJsonFilePath);
    //            return;
    //        }
    //        CreateTrainScore();
    //    }

    //    private void CreateTrainScore()
    //    {
    //        //训练数据
    //        DailyHistoricalDataExt[] trainDataArray = _trainDatas.ToArray();

    //        TestContext.WriteLine("总训练数据条数：{0}.", trainDataArray.Length);
    //        //计算器
    //        ICalculation calculation = new Calculation();

    //        //训练数据中目标比率1.05的有效数据
    //        DailyHistoricalDataExt[] trainEffDataArray = GetVData1(_trainDatas, 1.05f).ToArray();

    //        //训练数据中目标比率1.05的无效数据
    //        DailyHistoricalDataExt[] trainValDataArray = GetVData2(_trainDatas, 1.05f).ToArray();

    //        //所有训练数据的买盘/(买盘+卖盘)的占比均值集合，Key值是当前交易日的前X个交易日，Value值是X个交易日每个交易日的占比值(唯一值)
    //        Dictionary<int, List<double>> pers = new Dictionary<int, List<double>>();
    //        for (int i = MaxDays; i < trainDataArray.Length - RetreatDays; i++)
    //        {
    //            //分别计算当日与前1~前30之间的买盘/(买盘+卖盘)的占比均值
    //            Dictionary<int, double> buySalePer = GetPerData(trainDataArray.Get(i - MaxDays, MaxDays));

    //            foreach (KeyValuePair<int, double> keyValuePair in buySalePer)
    //            {
    //                if (!pers.ContainsKey(keyValuePair.Key))
    //                {
    //                    pers.Add(keyValuePair.Key, new List<double>());
    //                }
    //                if (!pers[keyValuePair.Key].Contains(keyValuePair.Value))
    //                {
    //                    pers[keyValuePair.Key].Add(keyValuePair.Value);
    //                }
    //            }
    //        }

    //        //占比在有效数据中的得分
    //        _trainScores = new List<Score>();

    //        //遍历所有交易日的占比，分别记录在有效数据和无效数据间的占比
    //        foreach (KeyValuePair<int, List<double>> pair in pers)
    //        {
    //            foreach (double d in pair.Value)
    //            {
    //                if (_trainScores.Count(x => x.Days == pair.Key && Math.Round(x.Poor,2) == d) > 0)
    //                    continue;

    //                _trainScores.Add(new Score()
    //                {
    //                    Days = pair.Key,
    //                    Poor = d,
    //                    EffectiveScore = calculation.Calculate1(trainEffDataArray, d, 1.05f),
    //                    InvalidScore = calculation.Calculate1(trainValDataArray, d, 1.05f)
    //                });
    //            }
    //        }

    //        IEnumerable<Score> s = _trainScores.Where(x => x.EffectiveScore > x.InvalidScore);

    //        SaveJson(_trainScores, _trainScoreJsonFilePath);
    //    }


    //    private const int MaxDays = 30;

    //    /// <summary>
    //    /// 获取多日买盘/(买盘+卖盘)的占比均值
    //    /// </summary>
    //    /// <param name="datas"></param>
    //    /// <returns>
    //    /// 最多返回之前30天的数据。字典Key值为天数，Value值为均值数据
    //    /// </returns>
    //    private Dictionary<int, double> GetPerData(IEnumerable<DailyHistoricalDataExt> datas)
    //    {
    //        DailyHistoricalDataExt[] array = datas.ToArray();
    //        Dictionary<int, double> result = new Dictionary<int, double>();
    //        List<double> poors = new List<double>();
    //        for (int i = array.Length - 1, j = 1; i >= 0; i--, j++)
    //        {
    //            poors.Add(array[i].BuyVolumePer);
    //            result.Add(j, Math.Round(poors.Average(), 2));
    //            if (j > MaxDays)
    //                break;
    //        }

    //        return result;
    //    }

    //    /// <summary>
    //    /// 获取多日买盘成交量差额数据均值
    //    /// </summary>
    //    /// <param name="datas"></param>
    //    /// <returns>
    //    /// 最多返回之前30天的数据。字典Key值为天数，Value值为均值数据
    //    /// </returns>
    //    private Dictionary<int,double> GetAvgData(IEnumerable<DailyHistoricalDataExt> datas)
    //    {
    //        DailyHistoricalDataExt[] array = datas.ToArray();
    //        Dictionary<int, double> result = new Dictionary<int, double>();
    //        List<double> poors = new List<double>();
    //        for (int i = array.Length - 1, j = 1; i > 0; i--, j++)
    //        {
    //            poors.Add(array[i].BuySaleVolumePoor);
    //            result.Add(j, poors.Average());
    //            if (j > MaxDays)
    //                break;
    //        }

    //        return result;
    //    }

    //    /// <summary>
    //    /// 从指定集合中获取指定百分比的有效数据
    //    /// </summary>
    //    /// <param name="datas">数据集合</param>
    //    /// <param name="percent">比率</param>
    //    /// <param name="maxDays">日期期限，如果是0则取<see cref="RetreatDays"/></param>
    //    /// <returns></returns>
    //    IEnumerable<DailyHistoricalDataExt> GetVData1(IEnumerable<DailyHistoricalDataExt> datas, float percent,int maxDays=0)
    //    {
    //        //查找数据时剔除最后一百个交易日的数据
    //        int trainDays = datas.Count() - ((maxDays == 0) ? RetreatDays : maxDays);
    //        int maxD= ((maxDays == 0) ? RetreatDays : maxDays);
    //        /*
    //         * 首先剔除传入数据（datas）中的最后100条数据
    //         * 取数据中
    //         * 1.DailyHistoricalDataExt.IncreaseResults.Count>0
    //         * 2.同时DailyHistoricalDataExt.IncreaseResults.Count<100
    //         * 3.同时DailyHistoricalDataExt.IncreaseResults.Percent == percent
    //         * 的数据
    //         */
    //        return datas.Take(trainDays).Where(x => x.IncreaseResults.Where(y => y.Count > 0 && y.Count < maxD && y.Percent == percent).Count() > 0);
    //    }

    //    /// <summary>
    //    /// 从指定集合中获取指定百分比的有效数据的平均天数
    //    /// </summary>
    //    /// <param name="datas"></param>
    //    /// <param name="percent"></param>
    //    /// <returns></returns>
    //    double GetAvgData(IEnumerable<DailyHistoricalDataExt> datas, float percent)
    //    {
    //        //查找数据时剔除最后一百个交易日的数据
    //        int trainDays = datas.Count() - RetreatDays;
    //        List<int> cs = new List<int>();
    //        foreach (DailyHistoricalDataExt data in datas.Take(trainDays))
    //        {
    //            if(data.IncreaseResults[percent].Count>0)
    //            {
    //                cs.Add(data.IncreaseResults[percent].Count);
    //            }
    //        }
    //        return cs.Average();
    //    }
    //        /// <summary>
    //    /// 从指定集合中获取指定百分比的无效数据
    //    /// </summary>
    //    /// <param name="datas">数据集合</param>
    //    /// <param name="percent">比率</param>
    //    /// <param name="maxDays">日期期限，如果是0则取<see cref="RetreatDays"/></param>
    //    /// <returns></returns>
    //    IEnumerable<DailyHistoricalDataExt> GetVData2(IEnumerable<DailyHistoricalDataExt> datas, float percent, int maxDays = 0)
    //    {
    //        //查找数据时剔除最后一百个交易日的数据
    //        int trainDays = datas.Count() - ((maxDays == 0) ? RetreatDays : maxDays);
    //        int maxD = ((maxDays == 0) ? RetreatDays : maxDays);
    //        /*
    //         * 首先剔除传入数据（datas）中的最后100条数据
    //         * 取数据中
    //         * 1.DailyHistoricalDataExt.IncreaseResults.Count<0
    //         * 2.或者DailyHistoricalDataExt.IncreaseResults.Count>100
    //         * 3.同时DailyHistoricalDataExt.IncreaseResults.Percent == percent
    //         * 的数据
    //         */
    //        return datas.Take(trainDays).Where(x => x.IncreaseResults.Where(y => (y.Count < 0 || y.Count > maxD) && y.Percent == percent).Count() > 0);
    //    }


    //    /// <summary>
    //    /// 得分
    //    /// </summary>
    //    public struct Score
    //    {
    //        /// <summary>
    //        /// 天数
    //        /// </summary>
    //        public int Days { get; set; }

    //        /// <summary>
    //        /// 差值
    //        /// </summary>
    //        public double Poor { get; set; }

    //        /// <summary>
    //        /// 有效得分
    //        /// </summary>
    //        public double EffectiveScore { get; set; }

    //        /// <summary>
    //        /// 无效得分
    //        /// </summary>
    //        public double InvalidScore { get; set; }

    //    }
    //}
}
