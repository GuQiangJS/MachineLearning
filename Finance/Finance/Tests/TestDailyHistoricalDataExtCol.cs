using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Finance.Tests
{
    [TestFixture]
    public class TestDailyHistoricalDataExtCol:TestBase
    {
        public TestDailyHistoricalDataExtCol() : base()
        {
        }

        [Test]
        public void TestGetPrevious()
        {
            DailyHistoricalDataExt[] p1 = HistoricalDataExts.GetPrevious(HistoricalDataExts[100], 1).ToArray();
            Assert.AreEqual(HistoricalDataExts[99], p1[0]);

            DailyHistoricalDataExt[] p2 = HistoricalDataExts.GetPrevious(HistoricalDataExts[100], 10).ToArray();
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(HistoricalDataExts[90 + i], p2[i]);
            }

            bool countMax = false;
            try
            {
                HistoricalDataExts.GetPrevious(HistoricalDataExts[10], 20);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                countMax = true;
            }
            Assert.IsTrue(countMax);
        }

        [Test]
        public void TestGetNexts()
        {
            DailyHistoricalDataExt[] p1 = HistoricalDataExts.GetNexts(HistoricalDataExts[100], 10).ToArray();
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(HistoricalDataExts[100 + i], p1[i]);
            }

            DailyHistoricalDataExt[] p2 = HistoricalDataExts
                .GetNexts(HistoricalDataExts[HistoricalDataExts.Count - 10], 20).ToArray();
            Assert.AreEqual(10, p2.Length);
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(HistoricalDataExts[HistoricalDataExts.Count - 10 + i], p2[i]);
            }
        }
    }
}
