using System;
using SpamHam;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace SpamHam.UnitTest
{
    [TestFixture]
    public class ReaderTester:TestBase
    {
        [Test]
        public void TestParseLine()
        {
            string context1 = "Urgent! call 09061749602 from Landline. Your complimentary 4* Tenerife Holiday or �10,000 cash await collection SAE T&Cs BOX 528 HP20 1YF 150ppm 18+,spam";
            Line kv1 = new TesterReader1(string.Empty).ParseLine(context1);

            Assert.IsNotNull(kv1);
            Assert.AreEqual(DocType.Spam, kv1.DocType);
            Assert.AreEqual("Urgent! call 09061749602 from Landline. Your complimentary 4* Tenerife Holiday or �10,000 cash await collection SAE T&Cs BOX 528 HP20 1YF 150ppm 18+", kv1.Value);


            string context2 = "ham	U dun say so early hor... U c already then say...";
            Line kv2 = new TesterReader2(string.Empty).ParseLine(context2);

            Assert.IsNotNull(kv2);
            Assert.AreEqual(DocType.Ham, kv2.DocType);
            Assert.AreEqual("U dun say so early hor... U c already then say...", kv2.Value);
        }

        [Test]
        public void TestRead()
        {
            IList<Line> k = GetTraningData();
            
            List<string> lines=new List<string>();
            string[] context = File.ReadAllLines(TraningFilePath);
            foreach (string line in context)
            {
                if(string.IsNullOrEmpty(line))
                    continue;
                if(lines.Contains(line))
                    continue;
                lines.Add(line);
            }

            Assert.AreEqual(lines.Count, k.Count);
        }
    }

    class TesterReader1 : FileReader1
    {
        public TesterReader1(string filePath) : base(filePath) { }

        public Line ParseLine(string lineValue)
        {
            return base.ParseLine(lineValue);
        }
    }

    class TesterReader2 : FileReader2
    {
        public TesterReader2(string filePath) : base(filePath) { }

        public Line ParseLine(string lineValue)
        {
            return base.ParseLine(lineValue);
        }
    }
}
