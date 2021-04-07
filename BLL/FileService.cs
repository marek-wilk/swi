using BLL.Data;
using CsvHelper;
using CsvHelper.TypeConversion;
using Serilog;
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
        private readonly string _saving = "Saving results to file.";
        private readonly string _saved = "Saving complete. File result can be found here ";
        private readonly string _reading = "Reading from file.";
        public List<InputRecord> ReadRecords()
        {
            List<InputRecord> readRecords = null;
            try
            {
                using (var reader = new StreamReader($"{_path}\\input.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    Log.Logger.Information(_reading);
                    var records = csv.GetRecords<InputRecord>().ToList();
                    readRecords = records;
                }
            }
            catch (FileNotFoundException e)
            {
                Log.Logger.Error(e.Message);
            }
            catch (HeaderValidationException e)
            {
                Log.Logger.Error(e.Message);
            }
            catch (ReaderException e)
            {
                Log.Logger.Error(e.Message);
            }
            catch (TypeConverterException e)
            {
                Log.Logger.Error(e.Message);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e.Message);
            }
            return readRecords;
        }
        public void SaveToFile(List<string> dataToSave)
        {
            using StreamWriter file = new StreamWriter($"{_path}\\result");
            Log.Logger.Information(_saving);
            foreach (var data in dataToSave)
            {
                Console.WriteLine($"{data}");
            }
            foreach (string data in dataToSave)
            {
                file.WriteLineAsync(data);
            }
            Log.Logger.Information($"{_saved}{_path}");
        }
    }
}
