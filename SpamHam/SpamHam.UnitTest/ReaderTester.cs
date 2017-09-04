using System;
using SpamHam;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace SpamHam.UnitTest
{
    [TestFixture]
    public class ReaderTester
    {
        [Test]
        public void TestParseLine()
        {
            string context = "Urgent! call 09061749602 from Landline. Your complimentary 4* Tenerife Holiday or �10,000 cash await collection SAE T&Cs BOX 528 HP20 1YF 150ppm 18+,spam";
            KeyValuePair<string, DocType>? kv = new TesterReader(string.Empty).ParseLine(context);

            Assert.IsTrue(kv.HasValue);
            Assert.AreEqual(DocType.Spam, kv.Value.Value);
            Assert.AreEqual("Urgent! call 09061749602 from Landline. Your complimentary 4* Tenerife Holiday or �10,000 cash await collection SAE T&Cs BOX 528 HP20 1YF 150ppm 18+", kv.Value.Key);
        }

        [Test]
        public void TestRead()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "english.txt");
            TesterReader r = new TesterReader(path);
            Dictionary<string, DocType> k = r.Read();
            
            List<string> lines=new List<string>();
            string[] context = File.ReadAllLines(path);
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

    class TesterReader : FileReader
    {
        public TesterReader(string filePath) : base(filePath) { }

        public KeyValuePair<string, DocType>? ParseLine(string lineValue)
        {
            return base.parseLine(lineValue);
        }
    }
}
