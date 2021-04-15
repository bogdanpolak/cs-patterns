using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Strategy.Order.Strategies
{
    class NormalOrderStrategy : IOrderStrategy
    {
        public string GetName() => "Normal Strategy";
        public IEnumerable<string> DoExecute(IEnumerable<string> data)
        {
            var list = data?.ToList();
            list?.Sort();
            return list;
        }
    }
}