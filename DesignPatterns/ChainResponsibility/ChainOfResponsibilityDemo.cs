using System;

namespace DesignPatterns.ChainResponsibility
{
    public class MyRequest
    {
        public int Value { get; set; }
    }
    public abstract class BaseHandler
    {
        private BaseHandler _nextHandler;

        public abstract void Handle(MyRequest myRequest);

        protected void Next(MyRequest myRequest)
        {
            if (_nextHandler != null) _nextHandler.Handle(myRequest);
        }
        
        public BaseHandler SetNext(BaseHandler nextHandler)
        {
            _nextHandler = nextHandler;
            return _nextHandler;
        }
    }

    public class StepInitializeValue : BaseHandler
    {
        private readonly int _value;
        public StepInitializeValue(int value) { _value = value; }

        public override void Handle(MyRequest myRequest)
        {
            myRequest.Value = _value;
            Next(myRequest);
        }
    }
    
    public class StepAddValue : BaseHandler
    {
        private readonly int _value;
        public StepAddValue(int value) { _value = value; }

        public override void Handle(MyRequest myRequest)
        {
            myRequest.Value += _value;
            Next(myRequest);
        }
    }
    
    public class StepCalcModulo : BaseHandler
    {
        private readonly int _divisor;
        public StepCalcModulo(int divisor) { _divisor = divisor; }

        public override void Handle(MyRequest myRequest)
        {
            myRequest.Value %= _divisor;
            Next(myRequest);
        }
    }
    
    public class StepMultiply : BaseHandler
    {
        private readonly int _multiplier;
        public StepMultiply(int multiplier) { _multiplier = multiplier; }

        public override void Handle(MyRequest myRequest)
        {
            myRequest.Value *= _multiplier;
            Next(myRequest);
        }
    }
    
    public static class ChainOfResponsibilityDemo
    {
        public static void RunDemo()
        {
            var calculatorChain = new StepInitializeValue(16);
            calculatorChain
                .SetNext(new StepAddValue(-5))
                .SetNext(new StepCalcModulo(7))
                .SetNext(new StepMultiply(2));

            var request = new MyRequest(){Value = -1};
            
            calculatorChain.Handle(request);
            // ((16 + -5) % 7) * 2 = (11 % 7) * 2 = 4 * 2 = 8
            
            Console.WriteLine($"Calculation: ((16 + -5) % 7) * 2,  Result: {request.Value}");
        }
    }
}