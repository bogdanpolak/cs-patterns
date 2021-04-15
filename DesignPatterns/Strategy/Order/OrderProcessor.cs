using System;
using System.Collections.Generic;

namespace DesignPatterns.Strategy.Order
{
    public class OrderProcessor
    {
        private readonly IOrderStrategy _orderStrategy;

        public OrderProcessor(IOrderStrategy orderStrategy)
        {
            this._orderStrategy = orderStrategy;
        }

        public void Execute(IEnumerable<string> data)
        {
            if (data == null) return;
            
            Console.WriteLine($"Sorting using the strategy: {_orderStrategy.GetName()}");
            var result = _orderStrategy.DoExecute(data);

            Console.WriteLine("  [Initial]  "+string.Join(", ", data));
            Console.WriteLine("  [Result]   "+string.Join(", ", result));
        }
    }
}