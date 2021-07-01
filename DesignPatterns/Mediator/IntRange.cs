using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Mediator
{
    public static class IntRange
    {
        public static List<int> Gen(int start, int end) 
            => Enumerable.Range(start, end-start+1).ToList();
    }
}