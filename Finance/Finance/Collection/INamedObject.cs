using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection
{
    /// <summary>
    /// 包含名称的对象
    /// </summary>
    /// <remarks>
    /// 符合此接口的实例允许检索出其名称，通常的此名称在集合中是唯一的，即作为此对象的唯一键。
    /// 此接口一般和<see cref="INamedCollection{T}"/>联合使用
    /// </remarks>
    public interface INamedObject
    {

        /// <summary>
        /// 对象的名称
        /// </summary>
        string Name { get; }
    }
}
