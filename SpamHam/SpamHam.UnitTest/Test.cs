using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SpamHam.UnitTest
{
    [TestFixture]
    public class Test : TestBase
    {
        [Test]
        public void DoTest()
        {
            //P（垃圾短信包含”xyz”)=(1+包含”xyz“的垃圾短信计数)/(1+垃圾短信计数)

            //读取训练文档
            IReader reader = new FileReader(this.TraningFilePath);
            //解析训练文档
            Dictionary<string, DocType> lines = reader.Read();
            //垃圾信息集合
            List<string> spams = lines.Where(x => x.Value == DocType.Spam).Select(y => y.Key).ToList();
            //非垃圾信息集合
            List<string> hams = lines.Where(x => x.Value == DocType.Ham).Select(y => y.Key).ToList();

            //对垃圾信息进行分词
            List<string> spamWords = new List<string>();
            //垃圾信息分词后的出现频率
            Dictionary<string,int> spamWordsDic=new Dictionary<string, int>();

            //遍历所有的垃圾信息，对齐进行分词，并计算每一个单词总共出现的次数
            IParticiple participle = new ParticipleSimple();

            //计算器
            ICalculation calculation=new Calculation();
            
            /*
             * 包含Free的垃圾短信比率
             * P（垃圾短信包含”Free”)=(1+包含”Free“的垃圾短信计数)/(1+垃圾短信计数)
             */

            //包含Free的信息条数
            int containsFreeCount = calculation.Calculate2(lines.Keys.ToArray(), "FREE");
            //不包含Free的信息条数
            int notContainsFreeCount = lines.Count - containsFreeCount;
            //包含Free的信息数占比
            float containsFreePrecent = (float) containsFreeCount / lines.Count;
            //不包含Free的信息数占比
            float notContainsFreePrecent = (float)notContainsFreeCount / lines.Count;
            
        }
    }
}
