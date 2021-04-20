using System;
using System.Drawing;
using Pastel;

namespace DesignPatterns
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Strategy.StrategyDemo.RunDemo();
            Console.WriteLine("-----------------------------------------------".Pastel(Color.DarkGreen));
            Mediator.MediatorDemo.RunDemo();
        }
    }
}
