using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Finance
{
    [TestFixture]
    public class TestEnumerableHelper
    {
        [Test]
        public void TestArrayToString()
        {
            int[] i = new[] {1, 2, 3, 4, 5, 6};
            string s = i.TransToString();
        }
    }


    public static class EnumerableHelper
    {
        public static string TransToString(this int[] array)
        {
            string result = string.Concat(array.Select(x => x.ToString() + ","));
            if (result.Length > 0)
                result = result.Remove(result.Length - 1);
            return result;
        }

        public static int IndexOf<T>(this IEnumerable<T> array, T v)
        {
            int index = 0;
            foreach (T variable in array)
            {
                if (ReferenceEquals(v, variable))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public static IEnumerable<T> Get<T>(this IEnumerable<T> array, int startIndex, int length)
        {
            T[] result = new T[length];
            int index = 0;
            int j = 0;
            int max = startIndex + length;
            foreach (T variable in array)
            {
                if (index >= startIndex && index < startIndex + length)
                {
                    result[j] = variable;
                    j++;
                }
                if (index > max)
                    break;
                index++;
            }
            return result;
        }
    }
}
