using System;

namespace DesignPatterns.ChainResponsibility
{
    public static class WorkflowsDemo
    {
        public static void RunDemo()
        {
            var workflowChain = new WorkflowChain();
            
            var jaguarSvgz = new Document("Illustrations", "svgz", "JLR*Jaguar");
            Console.Write($"New Document: {jaguarSvgz}  ==>  ");
            workflowChain.Execute(jaguarSvgz);

            var fiatPng = new Document("Image", "png", "FCA*Fiat");
            Console.Write($"New Document: {fiatPng}  ==>  ");
            workflowChain.Execute(jaguarSvgz);

            var volvoCsv = new Document("Table", "cvs", "Volvo");
            Console.Write($"New Document: {volvoCsv}  ==>  ");
            workflowChain.Execute(volvoCsv);
        }
    }
    
    // --------------------------------------------------------------------
    // Model
    // --------------------------------------------------------------------

    public class Document
    {
        // public Guid DocumentId
        // more document fields
        public readonly string DataType;
        public readonly string MediaType;
        public readonly string Provider;

        public Document(string dataType, string mediaType, string provider)
        {
            DataType = dataType;
            MediaType = mediaType;
            Provider = provider;
        }

        public override string ToString()
            => $"({DataType}, {MediaType}, {Provider})";
    }
    
    // --------------------------------------------------------------------
    // Workflows
    // --------------------------------------------------------------------

    public class WorkflowChain
    {
        private readonly IWorkflow _workflow;

        public WorkflowChain()
        {
            _workflow = new JaguarSvgzWorkflow(
                new FiatImageWorkflow(
                    new Terminator(null)));
        }
        public void Execute(Document document)
        {
            _workflow.Execute(document);
        }
    }
    
    public interface IWorkflow
    {
        void Execute(Document document);
    }

    public abstract class BaseWorkflow : IWorkflow
    {
        private readonly IWorkflow _nextWorkflow;

        protected BaseWorkflow(IWorkflow nextWorkflow)
        {
            _nextWorkflow = nextWorkflow;
        }
        
        public void Execute(Document document)
        {
            if (IsMatchingWorkflow(document))
            {
                DoWorkflowExecute(document);
            }
            else if (_nextWorkflow != null)
            {
                _nextWorkflow.Execute(document);
            }
        }
        
        protected abstract bool IsMatchingWorkflow(Document document);
        protected abstract void DoWorkflowExecute(Document document);
    }
    
    public class JaguarSvgzWorkflow : BaseWorkflow
    {
        private const string ExpectedDataType = "Illustrations";
        private const string ExpectedMediaType = "svgz";
        private const string ExpectedProvider = "JLR*Jaguar";

        public JaguarSvgzWorkflow(IWorkflow nextWorkflow) : base(nextWorkflow) { }
        
        protected override void DoWorkflowExecute(Document document)
        {
            Console.WriteLine("Executing Jaguar SVGZ workflow");
            // DoWorkflowStep1();
            // DoWorkflowDoStep2();
        }

        protected override bool IsMatchingWorkflow(Document document)
            => document.DataType.EqualsWith(ExpectedDataType)
               && document.MediaType.EqualsWith(ExpectedMediaType)
               && document.Provider.EqualsWith(ExpectedProvider);
    }
    
    public class FiatImageWorkflow : BaseWorkflow
    {
        private const string ExpectedDataType = "Images";
        private const string ExpectedMediaType = "png";
        private const string ExpectedProvider = "FCA*Fiat";

        public FiatImageWorkflow(IWorkflow nextWorkflow) : base(nextWorkflow) { }
        
        protected override void DoWorkflowExecute(Document document)
        {
            Console.WriteLine("Executing FiatImage workflow");
            // DoWorkflowStep1();
            // DoWorkflowDoStep2();
        }

        protected override bool IsMatchingWorkflow(Document document)
            => document.DataType.EqualsWith(ExpectedDataType)
               && document.MediaType.EqualsWith(ExpectedMediaType)
               && document.Provider.EqualsWith(ExpectedProvider);
    }
    
    public class Terminator : BaseWorkflow
    {
        public Terminator(IWorkflow nextWorkflow) : base(nextWorkflow) { }
        
        protected override void DoWorkflowExecute(Document document)
        {
            Console.WriteLine("None of the defined workflows was matching");
        }

        protected override bool IsMatchingWorkflow(Document document) => true;
    }
    
    // --------------------------------------------------------------------

    public static class StringExtensions
    {
        public static bool EqualsWith(this string str, string str2) 
            => string.Equals(str, str2, StringComparison.OrdinalIgnoreCase);

    }
}
