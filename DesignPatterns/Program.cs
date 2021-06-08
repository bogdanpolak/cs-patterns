using System;
using System.Drawing;
using Pastel;

namespace DesignPatterns
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Strategy Pattern (behavioral)".Pastel(Color.Gold));
            Strategy.StrategyDemo.RunDemo();
            Console.WriteLine("-----------------------------------------------".Pastel(Color.DarkGreen));
            Console.WriteLine("Medatorn Pattern (behavioral)".Pastel(Color.Gold));
            Mediator.MediatorDemo.RunDemo();
            Console.WriteLine("-----------------------------------------------".Pastel(Color.DarkGreen));
            Console.WriteLine("Chain Of Responsibility Pattern (behavioral)".Pastel(Color.Gold));
            ChainResponsibility.ChainOfResponsibilityDemo.RunDemo();
            Console.WriteLine("-----------------------------------------------".Pastel(Color.DarkGreen));
            Console.WriteLine("Data Pipeline Pattern (behavioral)".Pastel(Color.Gold));
            DataPipeline.DataPipelineDemo.RunDemo();
        }
    }
}
