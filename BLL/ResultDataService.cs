using BLL.Data;
using BLL.Data.Enum;
using Serilog;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class ResultDataService
    {
        /// <summary>
        /// Extracts data from Result class and shapes it into string so it can be saved in demanded format in file.
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public List<string> ReshapeData(List<InputRecord> records)
        {
            var reshapedDataList = new List<string>();
            var analyzedRecords = new WorkTimeCalculator().AnalyzeRecords(records);
            Log.Logger.Information(MessagesDictionary.Informations[LogInformation.Shaping]);
            foreach (var record in analyzedRecords)
            {
                var data = $"Day {record.Date.ToString("yyyy-MM-dd")} " +
                           $"Work {record.WorkTime.ToString().TrimStart('0')} " +
                           $"{AddWorkTimeClassification(record)}" +
                           $"{AddWeeklyHours(record)}";
                reshapedDataList.Add(data);
            }
            Log.Logger.Information(MessagesDictionary.Informations[LogInformation.Shaped]);
            return reshapedDataList;
        }
        private string AddWorkTimeClassification(Result record)
        {
            string classification = String.Empty;
            switch (record.Classification)
            {
                case WorkTimeClassifier.Inconclusive:
                    classification = "i ";
                    break;
                case WorkTimeClassifier.Overtime:
                    classification = "ot ";
                    break;
                case WorkTimeClassifier.Undertime:
                    classification = "ut ";
                    break;
                case WorkTimeClassifier.Weekend:
                    classification = "w ";
                    break;
            }
            return classification;
        }
        private string AddWeeklyHours(Result record)
        {
            string hours = String.Empty;
            if (record.IsLastDayOfWeek)
            {
                hours = $"{Math.Floor(record.WeeklyWorkTime.TotalHours)}:{record.WeeklyWorkTime.Minutes}:{record.WeeklyWorkTime.Seconds} {record.WeeklyDifference}";
            }
            return hours;
        }
    }
}
