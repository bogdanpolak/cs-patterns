using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.DataPipeline
{
    public interface IPipelineProcessor
    {
        object Execute(object input);
    }

    public abstract class PipelineProcessor<T, TU> : IPipelineProcessor
    {
        protected abstract TU Process(T input);

        object IPipelineProcessor.Execute(object input) => Process((T)input);
    }
    
    public class Pipeline
    {
        private readonly List<IPipelineProcessor> _filters = new List<IPipelineProcessor>();

        public object Execute(object pipelineInput) =>
            _filters.Aggregate(pipelineInput, 
                (current, filter) => filter.Execute(current));

        public Pipeline Register<T, TU>(PipelineProcessor<T, TU> pipelineProcessor)
        {
            _filters.Add(pipelineProcessor);
            return this;
        }
    }
}