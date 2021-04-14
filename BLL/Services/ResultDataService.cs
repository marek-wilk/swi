using BLL.Data;
using BLL.Data.Enum;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;

namespace BLL.Services
{
    public class ResultDataService : IResultDataService
    {
        private readonly ILogger<ResultDataService> _log;
        private readonly IConfiguration _config;
        private readonly IWorkTimeCalculator _calculator;

        public ResultDataService(ILogger<ResultDataService> log, IConfiguration config, IWorkTimeCalculator calculator)
        {
            _log = log;
            _config = config;
            _calculator = calculator;
        }

        /// <summary>
        /// Extracts data from Result class and shapes it into string so it can be saved in demanded format in file.
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public List<string> ReshapeData(List<InputRecord> records)
        {
            var reshapedDataList = new List<string>();
            var analyzedRecords = _calculator.AnalyzeRecords(records);
            _log.LogInformation(MessagesDictionary.Informations[LogInformation.Shaping]);
            foreach (var record in analyzedRecords)
            {
                var data = $"Day {record.Date.ToString(_config.GetValue<string>("DateFormat"))} " +
                           $"Work {record.WorkTime.ToString().TrimStart('0')} " +
                           $"{AddWorkTimeClassification(record)}" +
                           $"{AddWeeklyHours(record)}";
                reshapedDataList.Add(data);
            }
            _log.LogInformation(MessagesDictionary.Informations[LogInformation.Shaped]);
            return reshapedDataList;
        }

        /// <summary>
        /// Adds work time classification by adding 'i', 'w', 'ot' or 'ut' at the end to resulting record
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public string AddWorkTimeClassification(Result record)
        {
            string classification = string.Empty;
            switch (record.Classification)
            {
                case WorkTimeClassifier.Inconclusive:
                    classification = _config.GetValue<string>("Inconclusive");
                    break;
                case WorkTimeClassifier.Overtime:
                    classification = _config.GetSection("Overtime").GetValue<string>("classification");
                    break;
                case WorkTimeClassifier.Undertime:
                    classification = _config.GetSection("Undertime").GetValue<string>("classification");
                    break;
                case WorkTimeClassifier.Weekend:
                    classification = _config.GetValue<string>("Weekend");
                    break;
            }
            return classification;
        }

        /// <summary>
        /// Adds information about employee's work time during the whole week and the difference between standard work time (40h/week by default).
        /// The information is added to the last day of week (friday by default or the last from the list of records)
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public string AddWeeklyHours(Result record)
        {
            string hours = string.Empty;
            if (record.IsLastDayOfWeek)
            {
                hours = $"{Math.Floor(record.WeeklyWorkTime.TotalHours)}:{record.WeeklyWorkTime.Minutes}:{record.WeeklyWorkTime.Seconds} {record.WeeklyDifference}";
            }
            return hours;
        }
    }
}
