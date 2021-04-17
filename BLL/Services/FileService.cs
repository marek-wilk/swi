using BLL.Data;
using BLL.Data.Enum;
using CsvHelper;
using CsvHelper.TypeConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BLL.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _log;
        private readonly IConfiguration _config;

        public FileService(ILogger<FileService> log, IConfiguration config)
        {
            _log = log;
            _config = config;
        }

        /// <summary>
        /// Reads records from provided input.csv file
        /// </summary>
        /// <returns></returns>
        public List<InputRecord> ReadRecords(string path)
        {
            List<InputRecord> readRecords = null;
            try
            {
                using (var reader = new StreamReader($"{path}\\{_config.GetValue<string>("ProvidedFile")}"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    _log.LogInformation(MessagesDictionary.Informations[LogInformation.Reading]);
                    var records = csv.GetRecords<InputRecord>().ToList();
                    readRecords = records;
                }
                _log.LogInformation(MessagesDictionary.Informations[LogInformation.Read]);
            }
            catch (FileNotFoundException)
            {
                _log.LogError($"{MessagesDictionary.Errors[LogError.FileNotFound]}{path}");
            }
            catch (HeaderValidationException e)
            {
                _log.LogError($"{MessagesDictionary.Errors[LogError.HeaderValidationException]}\n{e.Message}");
            }
            catch (ReaderException e)
            {
                _log.LogError($"{MessagesDictionary.Errors[LogError.ReaderException]}\n{e.Message}");
            }
            catch (TypeConverterException e)
            {
                _log.LogError($"{MessagesDictionary.Errors[LogError.TypeConverterException]}\n{e.Message}");
            }
            catch (Exception e)
            {
                _log.LogError($"{MessagesDictionary.Errors[LogError.Exception]}\n{e.Message}");
            }
            return readRecords;
        }
        /// <summary>
        /// Saves provided data to result file
        /// </summary>
        /// <param name="dataToSave"></param>
        public void SaveToFile(List<string> dataToSave, string path)
        {
            using StreamWriter file = new StreamWriter($"{path}\\{_config.GetValue<string>("ResultingFile")}");
            _log.LogInformation(MessagesDictionary.Informations[LogInformation.Saving]);
            foreach (string data in dataToSave)
            {
                Console.WriteLine($"{data}");
                file.WriteLine(data);
            }
            _log.LogInformation($"{MessagesDictionary.Informations[LogInformation.Saved]}{path}");
        }

        public string GetPath()
        {
            return $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName}";
        }
    }
}
