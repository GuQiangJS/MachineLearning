using System;
using System.Linq;

namespace SpamHam
{
    /// <summary>
    /// 信息与信息类型的模型
    /// </summary>
    public class Line
    {
        /// <summary>
        ///     简单分词器
        /// </summary>
        private readonly IParticiple _participle = new ParticipleSimple();

        /// <summary>
        ///     <see cref="Line.Value" />经过分词后的单词集合
        /// </summary>
        private readonly string[] words;

        public Line(string value, DocType docType)
        {
            Value = value;
            DocType = docType;

            words = _participle.DoParticiple(value);
        }
        /// <summary>
        /// 获取信息内容
        /// </summary>
        public string Value { get; }
        /// <summary>
        /// 获取信息类型
        /// </summary>
        public DocType DocType { get; }

        /// <summary>
        ///     检测<see cref="Line.Value" />中是否包含<paramref name="word" />
        /// </summary>
        /// <param name="word">待检测的单词</param>
        /// <param name="comparer">比较方法</param>
        /// <returns>如果包含返回<c>true</c>，否则返回<c>false</c>。</returns>
        public bool Contains(string word, StringComparer comparer)
        {
            return words.Contains(word, comparer);
        }

        /// <summary>确定指定的对象是否等于当前对象。</summary>
        /// <returns>如果指定的对象等于当前对象，则为 true，否则为 false。</returns>
        /// <param name="obj">要与当前对象进行比较的对象。</param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            var line = obj as Line;
            if (line == null) return false;
            return line.Value.Equals(Value, StringComparison.Ordinal) &
                   (line.DocType == DocType);
        }

        /// <summary>作为默认哈希函数。</summary>
        /// <returns>当前对象的哈希代码。</returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return DocType.GetHashCode() ^ Value.GetHashCode();
        }
    }
}