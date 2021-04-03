using System;
using BLL;
using CsvHelper.Expressions;

namespace swi
{
    class Program
    {
        static void Main(string[] args)
        {
            var outputDataService = new OutputDataService();
            var fileService = new FileService();
            var recordsFromFile = fileService.ReadRecords();
            fileService.SaveToFile(outputDataService.ReshapeData(recordsFromFile));
        }
    }
}
