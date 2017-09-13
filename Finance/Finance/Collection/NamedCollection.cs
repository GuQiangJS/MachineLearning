using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection
{
    /// <summary>
    /// INamedCollection的默认实现
    /// </summary>
    [Serializable]
    public class NamedCollection<T>
        : KeyedCollection<string, T>, INamedCollection<T>
        where T : INamedObject
    {

        #region INamedCollection<T> 成员

        /// <summary>
        /// 从集合中尝试使用指定的名称检索对象
        /// </summary>
        /// <param name="key">尝试检索的名称</param>
        /// <param name="item">如果寻找到此项目，将赋值此项目，否则为default(T)</param>
        /// <returns>如果寻找到此项目，将返回true，否则返回false</returns>
        public bool TryGet(string key, out T item)
        {
            if (key == null)
            {
                item = default(T);
                return false;
            }

            if (this.Dictionary == null)
            {
                foreach (T item2 in base.Items)
                {
                    if (Comparer.Equals(this.GetKeyForItem(item2), key))
                    {
                        item = item2;
                        return true;
                    }
                }
                item = default(T);
                return false;
            }
            return this.Dictionary.TryGetValue(key, out item);
        }

        #endregion

        /// <summary>
        /// 从指定元素提取键
        /// </summary>
        /// <param name="item">从中提取键的元素</param>
        /// <returns>指定元素的键</returns>
        protected override string GetKeyForItem(T item)
        {
            return item.Name;
        }
    }
}
