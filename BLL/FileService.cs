using BLL.Data;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BLL
{
    public class FileService
    {
        private readonly string _path = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName}";
        public List<InputRecord> ReadRecords()
        {
            List<InputRecord> readRecords = null;
            try
            {
                using (var reader = new StreamReader($"{_path}\\input.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<InputRecord>().ToList();
                    readRecords = records;
                }
            }
            catch (FileNotFoundException e)
            {
                //TODO: Logging instead of Console.WriteLine
                Console.WriteLine($"{e.Message} Try putting it here {_path} then run program again");
                Console.WriteLine("Closing program, waiting for any key to be pressed...");
                Console.ReadKey();
            }
            catch (HeaderValidationException e)
            {
                Console.WriteLine($"{e.InvalidHeaders} {e.Message} Fix your file and run program again.");
                Console.WriteLine("Closing program, waiting for any key to be pressed...");
                Console.ReadKey();
            }
            catch (ReaderException)
            {
                Console.WriteLine("Couldn't read data from file. Check if data format from your file is correct");
                Console.WriteLine("Closing program, waiting for any key to be pressed...");
                Console.ReadKey();
            }
            return readRecords;
        }
        public void SaveToFile(List<string> dataToSave)
        {
            using StreamWriter file = new StreamWriter($"{_path}\\result.csv");
            foreach (string data in dataToSave)
            {
                file.WriteLineAsync(data);
            }
        }
    }
}
