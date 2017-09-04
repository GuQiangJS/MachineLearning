using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace SpamHam.UnitTest
{
    public class TestBase
    {
        protected string TraningFilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "english.txt");

        protected Dictionary<string, DocType> GetTraningData()
        {
            var r = new TesterReader(TraningFilePath);
            return r.Read();
        }


        protected void WriteLine(string value)
        {
            TestContext.WriteLine(value);
        }

        protected void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }
    }
}