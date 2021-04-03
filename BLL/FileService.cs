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
            List<InputRecord> readRecords;
            using (var reader = new StreamReader($"{_path}\\input.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<InputRecord>().ToList();
                readRecords = records;
            }
            return readRecords;
        }
        public void SaveToFile(List<string> dataToSave)
        {
            using StreamWriter file = new StreamWriter($"{_path}\\output.csv");
            foreach (string data in dataToSave)
            {
                file.WriteLineAsync(data);
            }
        }
    }
}
