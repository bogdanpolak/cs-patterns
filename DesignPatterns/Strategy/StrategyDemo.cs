using System;
using System.Collections.Generic;
using DesignPatterns.Strategy.Order;
using DesignPatterns.Strategy.Order.Strategies;

namespace DesignPatterns.Strategy
{
    public class StrategyDemo
    {
        public static void RunDemo()
        {
            var data = new List<string> {"f", "h", "e", "c", "d", "b", "o", "g", "d", "a", "n"};
            
            var normalOrderProcessor = new OrderProcessor(new NormalOrderStrategy());
            var orderProcessor = new OrderProcessor(new ReverseOrderStrategy());

            normalOrderProcessor.Execute(data);
            orderProcessor.Execute(data);
        }
    }
}