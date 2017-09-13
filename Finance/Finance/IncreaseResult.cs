using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Collection;

namespace Finance
{
    /// <summary>
    /// 涨到指定目标价位所需的交易日的计算结果
    /// </summary>
    public struct IncreaseResult : INamedObject
    {
        public IncreaseResult(float percent, double calcPrice, int count)
        {
            Percent = percent;
            CalcPrice = calcPrice;
            Count = count;
            TargetPrice = Math.Round((calcPrice * percent), 2);
        }
        /// <summary>
        /// 获取或设置目标价格
        /// </summary>
        public double TargetPrice { get; set; }

        /// <summary>
        /// 获取或设置百分比
        /// </summary>
        public float Percent { get; set; }

        /// <summary>
        /// 获取或设置计算价格
        /// </summary>
        public double CalcPrice { get; set; }

        /// <summary>
        /// 获取或设置所需交易日的计算结果
        /// </summary>
        /// <value>返回 -1 表示尚未达到指定涨幅</value>
        public int Count { get; set; }

        public string Name { get { return Percent.ToString(); } }
    }

    public class IncreaseResultCollection : NamedCollection<IncreaseResult>
    {
        //
        // 摘要:
        //     获取具有指定键的元素。
        //
        // 参数:
        //   key:
        //     要获取的元素的键。
        //
        // 返回结果:
        //     带有指定键的元素。 如果未找到具有指定键的元素，则引发异常。
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     key 为 null。
        //
        //   T:System.Collections.Generic.KeyNotFoundException:
        //     集合中不存在具有指定键的元素。
        public IncreaseResult this[float key]
        {
            get
            {
                foreach (IncreaseResult item in this.Items)
                {
                    if (item.Percent == key)
                        return item;
                }
                throw new KeyNotFoundException(key.ToString());
            }
        }
    }
}
