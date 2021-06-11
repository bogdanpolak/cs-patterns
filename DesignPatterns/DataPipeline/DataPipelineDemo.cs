using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace DesignPatterns.DataPipeline
{
    public static class DataPipelineDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("*** Under construction ***");
            var pipeline = new Pipeline()
                .Register(new FtpDownloader<AssetInfo, LocalTextFile>())
                .Register(new CsvReader<LocalTextFile, CsvFile>())
                .Register(new CsvNormalizer<CsvFile, CsvFile>())
                .Register(new CsvMapper<CsvFile, AlternateParts>());
            // ---
            var assetInfo = new AssetInfo("localhost", "/usr/data/fca/", 
                "fca-alt-parts.csv","FCA");
            var alternateParts = (AlternateParts)pipeline.Execute(assetInfo);
            alternateParts.Parts.ForEach(
                ap => Console.WriteLine($"{ap.Oem} | {ap.PartId} | {ap.AlternatePartId} | {ap.Price}"));
        }
    }
    
    // --------------------------------------------------------------------
    // Domain - Model
    // --------------------------------------------------------------------

    public class AssetInfo
    {
        public readonly string Channel;
        public readonly string ServerName;
        public readonly string RemoteFolder;
        public readonly string FileName;

        public AssetInfo(string serverName, string remoteFolder, string fileName, string channel)
        {
            ServerName = serverName;
            RemoteFolder = remoteFolder;
            FileName = fileName;
            Channel = channel;
        }
    }

    public class LocalTextFile
    {
        public string Oem;
        public string Data;

        public LocalTextFile(string oem, string data)
        {
            Oem = oem;
            Data = data;
        }
    }

    public class CsvFile
    {
        public string Oem;
        public List<List<string>> Data = new List<List<string>>();
    }

    public class AlternateParts
    {
        public List<AlternatePart> Parts = new List<AlternatePart>();
    }
    public class AlternatePart
    {
        public string Oem;
        public int PartId;
        public int AlternatePartId;
        public float Price;

        public AlternatePart(string oem, int partId, int alternatePartId, float price)
        {
            Oem = oem;
            PartId = partId;
            AlternatePartId = alternatePartId;
            Price = price;
        }
    }
    
    // --------------------------------------------------------------------
    // Domain - Processors
    // --------------------------------------------------------------------

    public class FtpDownloader<T, TU> : PipelineProcessor<T, TU>
        where T : AssetInfo
        where TU : LocalTextFile
    {
        protected override TU Process(T input) => 
            (TU)new LocalTextFile(
                FtpChannel.ToOem(input.Channel),
                FtpClient.GetData(input.ServerName, input.RemoteFolder, input.FileName));
    }

    public class CsvReader<T, TU> : PipelineProcessor<T, TU>
        where T : LocalTextFile
        where TU : CsvFile
    {
        protected override TU Process(T input) => 
            (TU)new CsvFile{ Oem = input.Oem, Data = CsvTools.ParseString(input.Data) };
    }
    
    public class CsvNormalizer<T, TU> : PipelineProcessor<T, TU>
        where T : CsvFile
        where TU : CsvFile
    {
        protected override TU Process(T input) =>
            (TU) new CsvFile { Oem = input.Oem, Data = CsvTools.Normalize(input.Data) };

    }
    
    public class CsvMapper<T, TU> : PipelineProcessor<T, TU>
        where T : CsvFile
        where TU : AlternateParts
    {
        protected override TU Process(T input) =>
            (TU) Transformation.GenerateAlternateParts(input);
    }

    // --------------------------------------------------------------------
    // Implementations
    // --------------------------------------------------------------------

    public static class FtpChannel
    {
        public static string ToOem(string channel) => channel.ToLower();
    }

    public static class FtpClient
    {
        public static string GetData(string serverName, string remoteFolder, string fileName)
        {
            if (string.IsNullOrEmpty(serverName) || string.IsNullOrEmpty(remoteFolder) ||
                string.IsNullOrEmpty(fileName)) throw new ArgumentNullException();
            switch (fileName)
            {
                case "fca-alt-parts.csv": return SingleToDoubleQuote(
                    "'PartID','OemID','AlternatePartID','Price'\n"+
                    "10904,33,204,72.33\n"+
                    "11062,33,4040,185.12\n");
                default: return SingleToDoubleQuote(
                    $"PartID','AlternatePartID','Price'"+
                    "6741,842,256.99\n"+
                    "7179,519,99.99\n");
            }
        }
        private static string SingleToDoubleQuote(string text) => text.Replace("'", "\"");
    }

    public static class CsvTools
    {
        private static readonly string[] SplitChars = new string[] { "," };
        
        public static List<List<string>> ParseString(string csvString)
        {
            var rows = csvString.Split('\n');
            return rows
                .Select(row => row
                    .Split(SplitChars, StringSplitOptions.None)
                    .ToList())
                .ToList();
        }
        
        public static List<List<string>> Normalize(List<List<string>> csvMatrix)
        {
            var maxCols = csvMatrix.Max(row => row?.Count ?? 0);
            var csvNormalized = csvMatrix.Aggregate(new List<List<string>>(),
                (acc, row) =>
                {
                    if (row == null || row.Count == 0 || (row.Count == 1 && row[0] == "")) 
                        return acc;
                    var newRow = new List<string>(row);
                    while (newRow.Count < maxCols) newRow.Add("");
                    acc.Add(newRow);
                    return acc;
                });
            return csvNormalized;
        }
    }

    public static class Transformation
    {
        public static AlternateParts GenerateAlternateParts(CsvFile input)
        {
            var headerRow = input.Data[0];
            var idxPartID = headerRow.IndexOf("\"PartID\"");
            var idxAlternatePartID = headerRow.IndexOf("\"AlternatePartID\""); 
            var idxPrice = headerRow.IndexOf("\"Price\"");
            var alternateParts = new AlternateParts();
            if (idxPartID >= 0 && idxAlternatePartID >= 0 && idxPrice >= 0)
            {
                for (var idx = 1; idx < input.Data.Count; idx++)
                {
                    var row = input.Data[idx];
                    var partId = int.Parse(row[idxPartID]);
                    var alternatePartId = int.Parse(row[idxAlternatePartID]);
                    var price = float.Parse(row[idxPrice]);
                    alternateParts.Parts.Add(new AlternatePart(input.Oem, partId, alternatePartId, price));
                }
            }
            return alternateParts;
        }
    }

}