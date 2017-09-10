using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SpamHam.UnitTest
{
    [TestFixture]
    public class ContainsFREETest : TestBase
    {
        [Test]
        public void ContainsFreeTest()
        {
            IList<Line> trainingData = GetTraningData();

            TestContext.WriteLine("总训练数据条数：{0}.", trainingData.Count);

            const string keyWord = "FREE";

            //垃圾信息数量
            Line[] spamArray = trainingData.Where(x => x.DocType == DocType.Spam).ToArray();
            TestContext.WriteLine("垃圾信息数量:{0}.", spamArray.Length);
            //非垃圾信息数量
            Line[] hamArray = trainingData.Where(x => x.DocType == DocType.Ham).ToArray();
            TestContext.WriteLine("非垃圾信息数量:{0}.", hamArray.Length);

            //包含Free的信息集合
            Line[] containsFreeArray = trainingData.Where(x => x.Contains(keyWord, StringComparer.Ordinal)).ToArray();
            //不包含Free的信息集合
            Line[] notContainsFreeArray = trainingData.Where(x => !x.Contains(keyWord, StringComparer.Ordinal)).ToArray();
            //包含Free的信息集合中的垃圾信息
            Line[] containsFreeArrayIsSpam = containsFreeArray.Where(x => x.DocType == DocType.Spam).ToArray();
            //包含Free的信息集合中的非垃圾信息
            Line[] containsFreeArrayIsHam = containsFreeArray.Where(x => x.DocType == DocType.Ham).ToArray();
            //不包含Free的信息集合中的垃圾信息
            Line[] notContainsFreeArrayIsSpam = notContainsFreeArray.Where(x => x.DocType == DocType.Spam).ToArray();
            //不包含Free的信息集合中的非垃圾信息
            Line[] notContainsFreeArrayIsHam = notContainsFreeArray.Where(x => x.DocType == DocType.Ham).ToArray();

            TestContext.WriteLine("包含 {1} 的信息集合数量:{0}.", containsFreeArray.Length, keyWord);
            TestContext.WriteLine("包含 {1} 的信息集合占比:{0}.", ((float)containsFreeArray.Length/trainingData.Count).ToString("P"), keyWord);
            TestContext.WriteLine("不包含 {1} 的信息集合数量:{0}.", notContainsFreeArray.Length, keyWord);
            TestContext.WriteLine("不包含 {1} 的信息集合占比:{0}.",
                ((float) notContainsFreeArray.Length / trainingData.Count).ToString("P"), keyWord);
            TestContext.WriteLine("包含 {1} 的信息集合中的垃圾信息数量:{0}.", containsFreeArrayIsSpam.Length, keyWord);
            TestContext.WriteLine("包含 {1} 的信息集合中的垃圾信息占比:{0}.", ((float)containsFreeArrayIsSpam.Length / containsFreeArray.Length).ToString("P"), keyWord);
            TestContext.WriteLine("包含 {1} 的信息集合中的非垃圾信息数量:{0}.", containsFreeArrayIsHam.Length, keyWord);
            TestContext.WriteLine("包含 {1} 的信息集合中的非垃圾信息占比:{0}.", ((float)containsFreeArrayIsHam.Length / containsFreeArray.Length).ToString("P"), keyWord);
            TestContext.WriteLine("不包含 {1} 的信息集合中的垃圾信息数量:{0}.", notContainsFreeArrayIsSpam.Length, keyWord);
            TestContext.WriteLine("不包含 {1} 的信息集合中的垃圾信息占比:{0}.", ((float)notContainsFreeArrayIsSpam.Length / notContainsFreeArray.Length).ToString("P"), keyWord);
            TestContext.WriteLine("不包含 {1} 的信息集合中的非垃圾信息数量:{0}.", notContainsFreeArrayIsHam.Length, keyWord);
            TestContext.WriteLine("不包含 {1} 的信息集合中的非垃圾信息占比:{0}.", ((float)notContainsFreeArrayIsHam.Length / notContainsFreeArray.Length).ToString("P"), keyWord);

            //是垃圾邮件的条件下，包含 Free 的数量比率
            double pBA = (double) containsFreeArrayIsSpam.Length / spamArray.Length;
            TestContext.WriteLine("是垃圾邮件的条件下，包含 {1} 的数量比率:{0}.", pBA.ToString("P"), keyWord);
            //垃圾邮件比率
            double pA = (double) spamArray.Length / trainingData.Count;
            //包含Free的比率
            double pB = (double) containsFreeArray.Length / trainingData.Count;
            //包含 Free 时，是垃圾邮件的比率
            double pAB1 = MathExtension.P(pBA, pA, pB);
            TestContext.WriteLine("包含 {1} 时，是垃圾邮件的比率:{0}.", pAB1.ToString("P"), keyWord);

            //拉普拉斯转换后的 是垃圾邮件的条件下，包含 Free 的数量比率
            double laplace = (double)(1 + containsFreeArrayIsSpam.Length) / (1 + spamArray.Length);
            TestContext.WriteLine("拉普拉斯转换后的 是垃圾邮件的条件下，包含 {1} 时，是垃圾邮件的比率:{0}.", laplace.ToString("P"), keyWord);
        }
    }
}