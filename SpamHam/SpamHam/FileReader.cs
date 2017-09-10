using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpamHam
{
    /// <summary>
    /// 文件读取器基类
    /// </summary>
    public abstract class FileReaderBase : IReader
    {
        protected string _filePath;

        public FileReaderBase(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// 获取解析文件的完整路径。
        /// </summary>
        public string FilePath => _filePath;

        /// <summary>
        ///     读取<see cref="IReader.FilePath"/>，并将每一行解析解析为<see cref="Line"/>实例
        /// </summary>
        /// <returns>返回解析<see cref="IReader.FilePath"/>后的<see cref="Line"/>集合。</returns>
        public IList<Line> Read()
        {
            var result = new List<Line>();
            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                var l = ParseLine(line);
                if(l==null)
                    continue;
                if(!result.Contains(l))
                    result.Add(l);
            }
            return result;
        }

        /// <summary>
        ///     解析类型
        /// </summary>
        /// <returns>待解析的文本(exp:spam,ham)</returns>
        /// <param name="value">信息类型</param>
        protected DocType ParseDocType(string value)
        {
            if (value.Equals("ham", StringComparison.OrdinalIgnoreCase))
                return DocType.Ham;
            if (value.Equals("Spam", StringComparison.OrdinalIgnoreCase))
                return DocType.Spam;
            throw new ArgumentOutOfRangeException(value);
        }

        /// <summary>
        ///     解析单行
        /// </summary>
        /// <param name="value">文本行
        /// </param>
        /// <returns>文本/文本类型的键值对</returns>
        protected abstract Line ParseLine(string value);
    }


    /// <summary>
    /// 文件读取器
    /// </summary>
    /// <remarks>
    /// <para>用来读取训练文档</para>
    /// <para>文档内容如下：</para>
    /// <para>Urgent! call 09061749602 from Landline. Your complimentary 4* Tenerife Holiday or ?0,000 cash await collection SAE T&Cs BOX 528 HP20 1YF 150ppm 18+,spam</para>
    /// <para>+449071512431 URGENT! This is the 2nd attempt to contact U!U have WON?250 CALL 09071512433 b4 050703 T&CsBCM4235WC1N3XX.callcost 150ppm mobilesvary.max?. 50, spam</para>
    /// <para>FREE for 1st week! No1 Nokia tone 4 ur mob every week just txt NOKIA to 8007 Get txting and tell ur mates www.getzed.co.uk POBox 36504 W45WQ norm150p/tone 16+, spam</para>
    /// <para>Urgent! call 09066612661 from landline. Your complementary 4* Tenerife Holiday or?0,000 cash await collection SAE T&Cs PO Box 3 WA14 2PX 150ppm 18+ Sender: Hol Offer, spam</para>
    /// <para>WINNER!! As a valued network customer you have been selected to receivea?00 prize reward! To claim call 09061701461. Claim code KL341.Valid 12 hours only., spam</para>
    /// <para>最后一个 逗号 后面标记该条语句是否为垃圾信息</para>
    /// </remarks>
    public class FileReader1 : FileReaderBase
    {
        public FileReader1(string filePath) : base(filePath)
        {
        }

        /// <summary>
        ///     解析单行
        /// </summary>
        /// <param name="value">文本行
        /// <para>exp:Urgent! call 09061749602 from Landline. Your complimentary 4* Tenerife Holiday or ?0,000 cash await collection SAE T&Cs BOX 528 HP20 1YF 150ppm 18+,spam</para>
        /// </param>
        /// <returns>文本/文本类型的键值对</returns>
        protected override Line ParseLine(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            var result = new Line(
                value.Substring(0, value.LastIndexOf(',')),
                ParseDocType(value.Substring(value.LastIndexOf(',') + 1, value.Length - value.LastIndexOf(',') - 1))
            );
            return result;
        }
    }

    /// <summary>
    /// 文件读取器
    /// </summary>
    /// <remarks>
    /// <para>用来读取训练文档</para>
    /// <para>文档内容如下：</para>
    /// <para>ham	Go until jurong point, crazy.. Available only in bugis n great world la e buffet... Cine there got amore wat...</para>
    /// <para>ham Ok lar...Joking wif u oni...</para>
    /// <para>spam Free entry in 2 a wkly comp to win FA Cup final tkts 21st May 2005. Text FA to 87121 to receive entry question(std txt rate)T&C's apply 08452810075over18's</para>
    /// <para>ham U dun say so early hor...U c already then say...</para>
    /// <para>第一个 Tab 前面标记该条语句是否为垃圾信息</para>
    /// </remarks>
    public class FileReader2 : FileReaderBase
    {
        public FileReader2(string filePath) : base(filePath)
        {
        }

        /// <summary>
        ///     解析单行
        /// </summary>
        /// <param name="value">文本行
        /// <para>exp:Urgent! call 09061749602 from Landline. Your complimentary 4* Tenerife Holiday or ?0,000 cash await collection SAE T&Cs BOX 528 HP20 1YF 150ppm 18+,spam</para>
        /// </param>
        /// <returns>文本/文本类型的键值对</returns>
        protected override Line ParseLine(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            var result = new Line(
                value.Substring(value.LastIndexOf('\t') + 1, value.Length - value.LastIndexOf('\t') - 1),
                ParseDocType(value.Substring(0, value.LastIndexOf('\t')))
            );
            return result;
        }
    }
}