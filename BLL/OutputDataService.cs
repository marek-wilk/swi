using BLL.Data;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class OutputDataService
    {
        public List<string> ReshapeData(List<InputRecord> records)
        {
            var reshapedDataList = new List<string>();
            var analyzedRecords = new WorkTimeCalculator().AnalyzeRecords(records);
            foreach (var record in analyzedRecords)
            {
                var data = $"Day {record.Date.ToString("yyyy-MM-dd")} " +
                           $"Work {record.WorkTime.ToString().TrimStart('0')} " +
                           $"{AddWorkTimeClassification(record)}" +
                           $"{AddWeeklyHours(record)}";
                reshapedDataList.Add(data);
            }
            return reshapedDataList;
        }
        public string AddWorkTimeClassification(OutputRecord record)
        {
            string classification = String.Empty;
            switch (record.Classification)
            {
                case Data.WorkTimeClassifier.Inconclusive:
                    classification = "i ";
                    break;
                case Data.WorkTimeClassifier.Overtime:
                    classification = "ot ";
                    break;
                case Data.WorkTimeClassifier.Undertime:
                    classification = "ut ";
                    break;
                case Data.WorkTimeClassifier.Weekend:
                    classification = "w ";
                    break;
            }
            return classification;
        }
        public string AddWeeklyHours(OutputRecord record)
        {
            string hours = String.Empty;
            if (record.IsLastDayOfWeek)
            {
                hours = $"{Math.Floor(record.WeeklyWorkTime.TotalHours)}:{record.WeeklyWorkTime.Minutes} {record.WeeklyDifference}";
            }
            return hours;
        }
    }
}
