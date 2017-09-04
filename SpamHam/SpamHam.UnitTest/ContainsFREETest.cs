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
            var trainingData = GetTraningData();

            //包含FREE的数量
            var containsFree = trainingData.Count(x => x.Key.Contains("free"));
            //不包含FREE的数量
            var noFree = trainingData.Count(x => !x.Key.Contains("free"));
            //包含FREE，同时被标记为Spam的数量
            var containsFreeAndSpam = trainingData.Count(x => x.Key.Contains("free") && x.Value == DocType.Spam);
            //包含FREE，同时被标记为Ham的数量
            var containsFreeAndHam = trainingData.Count(x => x.Key.Contains("free") && x.Value == DocType.Ham);
            //不包含FREE，同时被标记为Spam的数量
            var noFreeAndSpam = trainingData.Count(x => !x.Key.Contains("free") && x.Value == DocType.Spam);
            //不包含FREE，同时被标记为Ham的数量
            var noFreeAndHam = trainingData.Count(x => !x.Key.Contains("free") && x.Value == DocType.Ham);


            WriteLine("包含FREE的数量：{0}，占比：{1}.", containsFree,
                MathExtension.Avg(containsFree, trainingData.Count).ToString("P"));
            WriteLine("不包含FREE的数量：{0}，占比：{1}.", noFree, MathExtension.Avg(noFree, trainingData.Count).ToString("P"));


            WriteLine("包含FREE，同时被标记为Spam的数量：{0}，占比：{1}.", containsFreeAndSpam,
                MathExtension.Avg(containsFreeAndSpam, containsFree).ToString("P"));
            WriteLine("包含FREE，同时被标记为Ham的数量：{0}，占比：{1}.", containsFreeAndHam,
                MathExtension.Avg(containsFreeAndHam, containsFree).ToString("P"));

            WriteLine("不包含FREE，同时被标记为Spam的数量：{0}，占比：{1}.", noFreeAndSpam,
                MathExtension.Avg(noFreeAndSpam, noFree).ToString("P"));
            WriteLine("不包含FREE，同时被标记为Ham的数量：{0}，占比：{1}.", noFreeAndHam,
                MathExtension.Avg(noFreeAndHam, noFree).ToString("P"));
        }
    }
}