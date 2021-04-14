using BLL.Data;
using BLL.Data.Enum;
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
        public List<InputRecord> ReadRecords()
        {
            List<InputRecord> readRecords = null;
            try
            {
                using (var reader = new StreamReader($"{_path}\\input.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    Log.Logger.Information(MessagesDictionary.Informations[LogInformation.Reading]);
                    var records = csv.GetRecords<InputRecord>().ToList();
                    readRecords = records;
                }
                Log.Logger.Information(MessagesDictionary.Informations[LogInformation.Read]);
            }
            catch (FileNotFoundException e)
            {
                Log.Logger.Error($"{MessagesDictionary.Errors[LogError.FileNotFound]}{_path}");
            }
            catch (HeaderValidationException e)
            {
                Log.Logger.Error($"{MessagesDictionary.Errors[LogError.HeaderValidationException]}\n{e.Message}");
            }
            catch (ReaderException e)
            {
                Log.Logger.Error($"{MessagesDictionary.Errors[LogError.ReaderException]}\n{e.Message}");
            }
            catch (TypeConverterException e)
            {
                Log.Logger.Error($"{MessagesDictionary.Errors[LogError.TypeConverterException]}\n{e.Message}");
            }
            catch (Exception e)
            {
                Log.Logger.Error($"{MessagesDictionary.Errors[LogError.Exception]}\n{e.Message}");
            }
            return readRecords;
        }
        public void SaveToFile(List<string> dataToSave)
        {
            using StreamWriter file = new StreamWriter($"{_path}\\result");
            Log.Logger.Information(MessagesDictionary.Informations[LogInformation.Saving]);
            foreach (string data in dataToSave)
            {
                Console.WriteLine($"{data}");
                file.WriteLine(data);
            }
            Log.Logger.Information($"{MessagesDictionary.Informations[LogInformation.Saved]}{_path}");
        }
    }
}
