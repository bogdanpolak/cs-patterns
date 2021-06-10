using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DesignPatterns.DataPipeline
{
    public static class DataPipelineDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("*** Under construction ***");
            var pipeline = new Pipeline()
                .Register(new FilterCsvFileLoader<FileName, CsvFile>());
            // ---
            var fileName = new FileName("parts.csv");
            var csvFile = (CsvFile)pipeline.Execute(fileName);
            var header = csvFile.Lines[0];
            Console.WriteLine($"|{header[0]}|{header[1]}|{header[2]}|");
            Console.WriteLine("|---|---|---|");
            for (int rowIdx = 1; rowIdx < csvFile.Lines.Count; rowIdx++)
            {
                var data = csvFile.Lines[rowIdx];
                Console.WriteLine($"|{data[0]}|{data[1]}|{data[2]}|");
            }
        }
    }
    
    public class FilterCsvFileLoader<T, TU> : PipelineFilter<T, TU>
        where T : FileName
        where TU : CsvFile, new()
    {
        protected override TU Process(T input) => (TU)CsvFileLoader.Load(input);
    }
    
    public class FileName
    {
        private readonly string _name;
        public FileName(string fileName)
        {
            _name = fileName;
        }
        public override string ToString() => _name;
    }

    public class CsvFile
    {
        public readonly List<List<string>> Lines = new List<List<string>>();
    }

    public static class CsvFileLoader
    {
        public static CsvFile Load(FileName fileName)
        {
            var text = DoLoadFile(fileName.ToString());
            var splitChars = new string[] { "," };
            var rows = text.Split('\n');
            var csvFile = new CsvFile();
            foreach (var row in rows)
            {
                csvFile.Lines.Add(row.Split(splitChars, StringSplitOptions.None).ToList());
            }
            return csvFile;
        }

        private static string DoLoadFile(string filename)
        {
            switch (filename)
            {
                case "parts.csv": return GetData(
                    "'PartID','OemID','AlternatePartID'\n1204,33,10204");
                default: return GetData(
                    $"'FileName','Content','Sep'\n'{filename}','content of file ...',0");
            }
        }
        
        private static string GetData(string csvData) 
            => csvData.Replace("'", "\"");
    }
    
    public interface IFilter
    {
        object Execute(object input);
    }

    public abstract class PipelineFilter<T, TU> : IFilter
    {
        protected abstract TU Process(T input);

        object IFilter.Execute(object input) => Process((T)input);
    }
    
    public class Pipeline
    {
        private readonly List<IFilter> _filters = new List<IFilter>();

        public object Execute(object pipelineInput) =>
            _filters.Aggregate(pipelineInput, 
                (current, filter) => filter.Execute(current));

        public Pipeline Register<T, TU>(PipelineFilter<T, TU> filter)
        {
            _filters.Add(filter);
            return this;
        }
    }
}