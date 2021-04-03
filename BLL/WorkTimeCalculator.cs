using BLL.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class WorkTimeCalculator
    {
        private const string EXIT = "exit";
        private readonly TimeSpan _standardWeeklyWorkTime = new TimeSpan(40, 0, 0);
        private readonly TimeSpan _overtime = new TimeSpan(9, 0, 0);
        private readonly TimeSpan _undertime = new TimeSpan(6, 0, 0);
        public List<OutputRecord> AnalyzeRecords(List<InputRecord> records)
        {
            var outputRecords = new List<OutputRecord>();
            var weeklyWorkTime = new TimeSpan();
            var groupedRecords = records.GroupBy(x => x.Date.Date);
            foreach (var groupedRecord in groupedRecords)
            {
                var analyzedRecord = SummarizeWorkDay(groupedRecord);
                weeklyWorkTime += analyzedRecord.WorkTime;
                //To make sure Weekly Work Time will show even if last record is from the middle of the week
                if (groupedRecords.Count() == outputRecords.Count() + 1)
                {
                    analyzedRecord.IsLastDayOfWeek = true;
                }
                if(analyzedRecord.IsLastDayOfWeek)
                {
                    analyzedRecord.WeeklyWorkTime = weeklyWorkTime;
                    analyzedRecord.WeeklyDifference = weeklyWorkTime - _standardWeeklyWorkTime;
                }
                outputRecords.Add(analyzedRecord);
            }
            return outputRecords;
        }

        //
        public TimeSpan CalculateWorkTime(IGrouping<DateTime, InputRecord> workDayRecords)
        {
            return workDayRecords.Last().Date - workDayRecords.First().Date;
        }
        public WorkTimeClassifier ClassifyWorkTime(IGrouping<DateTime,InputRecord> workDayRecords, TimeSpan workTime)
        {
            
            if(!workDayRecords.Last().Event.Contains(EXIT))
            {
                return WorkTimeClassifier.Inconclusive;
            }
            if(workDayRecords.Key.DayOfWeek == DayOfWeek.Saturday || workDayRecords.Key.DayOfWeek == DayOfWeek.Sunday)
            {
                return WorkTimeClassifier.Weekend;
            }
            if(workTime > _overtime)
            {
                return WorkTimeClassifier.Overtime;
            }
            if(workTime < _undertime)
            {
                return WorkTimeClassifier.Undertime;
            }
            return WorkTimeClassifier.Normal;
        }

        public OutputRecord SummarizeWorkDay(IGrouping<DateTime, InputRecord> workDayRecords)
        {
            var summarizedDay = new OutputRecord();
            summarizedDay.Date = workDayRecords.Key;
            summarizedDay.WorkTime = CalculateWorkTime(workDayRecords);
            summarizedDay.Classification = ClassifyWorkTime(workDayRecords, summarizedDay.WorkTime);
            summarizedDay.IsLastDayOfWeek = workDayRecords.Key.DayOfWeek == DayOfWeek.Friday;

            return summarizedDay;
        }
    }
}
