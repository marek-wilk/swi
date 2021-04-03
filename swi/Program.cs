using System;
using BLL;
using CsvHelper.Expressions;

namespace swi
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new OutputService();
            //service.SaveToFile(service.ReshapeData());
            foreach (var record in service.ReshapeData())
            {
                Console.WriteLine($"{record}");
            }

            Console.ReadKey();
        }
    }
}
