using System;
using System.Collections.Generic;
using System.IO;

namespace SpamHam
{
    public class FileReader : IReader
    {
        protected string _filePath;

        public FileReader(string filePath)
        {
            _filePath = filePath;
        }

        public string FilePath => _filePath;

        /// <summary>
        ///     读取文件，并将每一行解析
        /// </summary>
        /// <returns>The read.</returns>
        public Dictionary<string, DocType> Read()
        {
            var result = new Dictionary<string, DocType>();
            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                var kv = parseLine(line);
                if(!kv.HasValue)
                    continue;
                if(!result.ContainsKey(kv.Value.Key))
                    result.Add(kv.Value.Key, kv.Value.Value);
            }
            return result;
        }

        /// <summary>
        ///     解析类型
        /// </summary>
        /// <returns>The document type.</returns>
        /// <param name="value">Value.</param>
        protected DocType parseDocType(string value)
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
        /// <returns>The line.</returns>
        /// <param name="value">Value.</param>
        protected KeyValuePair<string, DocType>? parseLine(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            var result = new KeyValuePair<string, DocType>(
                value.Substring(0, value.LastIndexOf(',')),
                parseDocType(value.Substring(value.LastIndexOf(',') + 1, value.Length - value.LastIndexOf(',') - 1))
            );
            return result;
        }
    }
}