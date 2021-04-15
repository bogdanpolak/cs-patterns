using System.Collections.Generic;

namespace DesignPatterns.Strategy.Order
{
    public interface IOrderStrategy
    {
        string GetName();
        IEnumerable<string> DoExecute(IEnumerable<string> data);
    }
}