using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SpamHam.UnitTest
{
    [TestFixture]
    public class ParticipleSimpleTest
    {
        [Test]
        public void TestParticipleSimple()
        {
            ParticipleSimple participle = new ParticipleSimple();
            string s =
                "Urgent! call 09061749602 from Landline. Your complimentary 4* Tenerife Holiday or ?0,000 cash await collection SAE T&Cs BOX 528 HP20 1YF 150ppm 18+";
            string[] cs = participle.DoParticiple(s);
        }
    }
}
