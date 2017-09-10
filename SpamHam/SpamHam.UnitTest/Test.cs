using System;
using System.Collections.Generic;
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
    }
}
