using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Strategy.Order.Strategies
{
    public class ReverseOrderStrategy : IOrderStrategy
    {
        public string GetName() => "Reverse Strategy";
        public IEnumerable<string> DoExecute(IEnumerable<string> data)
        {
            var list = data?.ToList();
            list?.Sort();
            list?.Reverse();
            return list;
        }
    }
}