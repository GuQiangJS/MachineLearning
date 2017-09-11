using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace SpamHam.UnitTest
{
    [TestFixture]
    public class Test : TestBase
    {
        [Test]
        public void DoTest()
        {
            /*
            Score(SMS是垃圾短信 | SMS包含 free、car 和 txt) = log(P(SMS 是垃圾短信))
            + log(Laplace(SMS包含“free”| SMS是垃圾短信))
            + log(Laplace(SMS包含“car”| SMS是垃圾短信))
            + log(Laplace(SMS包含“txt”| SMS是垃圾短信))
            */

            IList<Line> trainingData = GetTraningData();

            ICalculation calculation = new Calculation();

            TestContext.WriteLine("总训练数据条数：{0}.", trainingData.Count);

            //SMS是垃圾短信
            Line[] spamArray = trainingData.Where(x => x.DocType == DocType.Spam).ToArray();
            //SMS不是垃圾短信
            Line[] hamArray = trainingData.Where(x => x.DocType == DocType.Ham).ToArray();
            double spamPer = (double) spamArray.Length / trainingData.Count;
            TestContext.WriteLine("log(P(SMS 是垃圾短信))：{0}.", Math.Log(spamPer).ToString("F"));
            double hamPer = (double)hamArray.Length / trainingData.Count;
            TestContext.WriteLine("log(P(SMS 不是垃圾短信))：{0}.", Math.Log(hamPer).ToString("F"));

            //Laplace(SMS包含“free”| SMS是垃圾短信)
            double laplaceContainsFreeSpam = calculation.Calculate3(trainingData.ToArray(), "free", DocType.Spam);
            TestContext.WriteLine("log(Laplace(SMS包含“free”| SMS是垃圾短信))：{0}.", Math.Log(laplaceContainsFreeSpam).ToString("F"));
            //Laplace(SMS包含“free”| SMS不是垃圾短信)
            double laplaceContainsfreeHam = calculation.Calculate3(trainingData.ToArray(), "free", DocType.Ham);
            TestContext.WriteLine("log(Laplace(SMS包含“free”| SMS不是垃圾短信))：{0}.", Math.Log(laplaceContainsfreeHam).ToString("F"));

            //Laplace(SMS包含“car”| SMS是垃圾短信)
            double laplaceContainsCarSpam = calculation.Calculate3(trainingData.ToArray(), "car", DocType.Spam);
            TestContext.WriteLine("log(Laplace(SMS包含“car”| SMS是垃圾短信))：{0}.", Math.Log(laplaceContainsCarSpam).ToString("F"));
            //Laplace(SMS包含“car”| SMS不是垃圾短信)
            double laplaceContainsCarHam = calculation.Calculate3(trainingData.ToArray(), "car", DocType.Ham);
            TestContext.WriteLine("log(Laplace(SMS包含“car”| SMS不是垃圾短信))：{0}.", Math.Log(laplaceContainsCarHam).ToString("F"));

            //Laplace(SMS包含“txt”| SMS是垃圾短信)
            double laplaceContainsTxtSpam = calculation.Calculate3(trainingData.ToArray(), "txt", DocType.Spam);
            TestContext.WriteLine("log(Laplace(SMS包含“txt”| SMS是垃圾短信))：{0}.", Math.Log(laplaceContainsTxtSpam).ToString("F"));
            //Laplace(SMS包含“txt”| SMS不是垃圾短信)
            double laplaceContainsTxtHam = calculation.Calculate3(trainingData.ToArray(), "txt", DocType.Ham);
            TestContext.WriteLine("log(Laplace(SMS包含“txt”| SMS不是垃圾短信))：{0}.", Math.Log(laplaceContainsTxtHam).ToString("F"));


            double scoreSpam = Math.Log(spamPer) + Math.Log(laplaceContainsFreeSpam) + Math.Log(laplaceContainsCarSpam) +
                           Math.Log(laplaceContainsTxtSpam);
            double scoreHam = Math.Log(hamPer) + Math.Log(laplaceContainsfreeHam) + Math.Log(laplaceContainsCarHam) +
                               Math.Log(laplaceContainsTxtHam);

            TestContext.WriteLine("-------------------------");

            TestContext.WriteLine("Spam Score:{0}", scoreSpam.ToString("F"));
            TestContext.WriteLine("Ham Score:{0}", scoreHam.ToString("F"));

            //            TestContext.WriteLine(@"Score(SMS是垃圾短信 | SMS包含 free、car 和 txt) = log(P(SMS 是垃圾短信))
            //log(Laplace(SMS包含“free”| SMS是垃圾短信))
            //log(Laplace(SMS包含“car”| SMS是垃圾短信))
            //log(Laplace(SMS包含“txt”| SMS是垃圾短信))={0}", score.ToString("F"));
        }


        [Test]
        public void DoTest2()
        {
            //取得所有数据
            IList<Line> data = GetTraningData();
            //取前1000条为训练数据
            IEnumerable<Line> trainingData = data.Take(1000);
            //剩余为验证数据
            IEnumerable<Line> validateData = data.Except(trainingData);

            ICalculation calculation = new Calculation();
            IParticiple participle = new ParticipleSimple();

            TestContext.WriteLine("总训练数据条数：{0}.", trainingData.Count());

            //训练数据中的垃圾短信集合
            Line[] spamArray = trainingData.Where(x => x.DocType == DocType.Spam).ToArray();
            //训练数据中的非垃圾短信集合
            Line[] hamArray = trainingData.Where(x => x.DocType == DocType.Ham).ToArray();

            //多所有训练数据进行简单分词，并取唯一值集合
            List<string> words = new List<string>();
            foreach (Line line in trainingData)
            {
                words.AddRange(participle.DoParticiple(line.Value));
            }
            IEnumerable<string> distinctWords = words.Distinct();
            
            //单词在垃圾短信中的得分字典
            Dictionary<string, double> spamSocres = new Dictionary<string, double>();
            //单词在非垃圾短信中的得分字典
            Dictionary<string, double> hamSocres = new Dictionary<string, double>();

            //遍历所有单词，分别记录在垃圾短信和非垃圾短信中的得分
            foreach (string word in distinctWords)
            {
                Token spamToken = new Token(word, DocType.Spam,
                    calculation.Calculate3(spamArray.ToArray(), word, DocType.Spam));
                Token hamToken = new Token(word, DocType.Ham,
                    calculation.Calculate3(hamArray.ToArray(), word, DocType.Ham));

                spamSocres.Add(spamToken.Value, Math.Log(spamToken.Score));
                hamSocres.Add(hamToken.Value, Math.Log(hamToken.Score));
            }

            //训练数据中的短信及其垃圾得分和非垃圾得分的字典
            //Key值为短信对象
            //Value值的Key值表示垃圾得分，Value值表示非垃圾得分
            Dictionary<Line, KeyValuePair<double, double>> trainScore =
                new Dictionary<Line, KeyValuePair<double, double>>();

            foreach (Line line in trainingData)
            {
                double spamScore = 0d;
                double hamScore = 0d;
                string[] v = participle.DoParticiple(line.Value);
                foreach (string s in v)
                {
                    if (spamSocres.ContainsKey(s) && hamSocres.ContainsKey(s))
                    {
                        spamScore = spamScore + spamSocres[s];
                        hamScore = hamScore + hamSocres[s];
                    }
                }
                trainScore.Add(line, new KeyValuePair<double, double>(spamScore, hamScore));
            }

            //总得分中所有被标记为垃圾信息的 垃圾分和非垃圾分 的分差
            //平均值
            double spamAvgValue = trainScore.Where(x => x.Key.DocType == DocType.Spam)
                .Average(x => x.Value.Key - x.Value.Value);
            //最小值
            double spamMinValue = trainScore.Where(x => x.Key.DocType == DocType.Spam)
                .Min(x => x.Value.Key - x.Value.Value);
            //最大值
            double spamMaxValue = trainScore.Where(x => x.Key.DocType == DocType.Spam)
                .Max(x => x.Value.Key - x.Value.Value);

            //验证数据中的短信及其垃圾得分和非垃圾得分的字典
            //Key值为短信对象
            //Value值的Key值表示垃圾得分，Value值表示非垃圾得分
            Dictionary<Line, KeyValuePair<double, double>> finalScore =
                new Dictionary<Line, KeyValuePair<double, double>>();

            foreach (Line line in validateData)
            {
                double spamScore = 0d;
                double hamScore = 0d;
                string[] v = participle.DoParticiple(line.Value);
                foreach (string s in v)
                {
                    if (spamSocres.ContainsKey(s) && hamSocres.ContainsKey(s))
                    {
                        spamScore = spamScore + spamSocres[s];
                        hamScore = hamScore + hamSocres[s];
                    }
                }
                finalScore.Add(line, new KeyValuePair<double, double>(spamScore, hamScore));
            }

            //分别根据 最大差异、最小差异、平均差异 来进行验证
            Dictionary<string, double> test = new Dictionary<string, double>();
            test.Add("最大差异值验证", spamMaxValue);
            test.Add("最小差异值验证", spamMinValue);
            test.Add("平均差异值验证", spamAvgValue);

            TestContext.WriteLine("------------------------------------");
            TestContext.WriteLine("总验证数据条数：{0}.", validateData.Count());
            TestContext.WriteLine("垃圾信息条数:{0}.", validateData.Count(x => x.DocType == DocType.Spam));
            TestContext.WriteLine("非垃圾信息条数:{0}.", validateData.Count(x => x.DocType == DocType.Ham));
            TestContext.WriteLine("------------------------------------");
            foreach (KeyValuePair<string,double> kv in test)
            {
                TestContext.WriteLine(kv.Key);

                IEnumerable<Line> finalSpam = finalScore.Where(x => (x.Value.Key - x.Value.Value) >= kv.Value)
                    .Select(x => x.Key);
                IEnumerable<Line> finalHam = finalScore.Select(x => x.Key).Except(finalSpam);

                TestContext.WriteLine("垃圾信息条数:{0}.", finalSpam.Count());
                TestContext.WriteLine("非垃圾信息条数:{0}.", finalHam.Count());

                TestContext.WriteLine(string.Format("垃圾信息验证准确率:{0}",
                    ((double)finalSpam.Count(x => x.DocType == DocType.Spam) / finalSpam.Count()).ToString("P")));
                TestContext.WriteLine(string.Format("非垃圾信息验证准确率:{0}",
                    ((double)finalHam.Count(x => x.DocType == DocType.Ham) / finalHam.Count()).ToString("P")));
                TestContext.WriteLine("------------------------------------");
            }
        }
    }
}
