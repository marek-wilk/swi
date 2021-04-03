using BLL.Data;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BLL
{
    public class InputReadingService
    {
        public List<InputRecord> ReadRecords()
        {
            var returnedList = new List<InputRecord>();
            //using (var reader = new StreamReader($"{Directory.GetCurrentDirectory()}\\input.csv"))
            string path = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName}\\input.csv";
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<InputRecord>().ToList();
                returnedList = records;
            }
            return returnedList;
        }
    }
}
