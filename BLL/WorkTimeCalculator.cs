using BLL.Data;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class WorkTimeCalculator
    {
        private readonly string _exit = "exit";
        private readonly string _analyzedRecord = "Getting results. Result no. ";
        private readonly string _analyzingFinished = "Analyzing finished, got all results.";
        private readonly TimeSpan _standardWeeklyWorkTime = new TimeSpan(40, 0, 0);
        private readonly TimeSpan _overtime = new TimeSpan(9, 0, 0);
        private readonly TimeSpan _undertime = new TimeSpan(6, 0, 0);
        /// <summary>
        /// Groups records by date then call other functions to calculate work time, classify it and return data in Result class.
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public List<Result> AnalyzeRecords(List<InputRecord> records)
        {
            var results = new List<Result>();
            var weeklyWorkTime = new TimeSpan();
            var groupedRecords = records.GroupBy(x => x.Date.Date);
            foreach (var groupedRecord in groupedRecords)
            {
                var result = SummarizeWorkDay(groupedRecord);
                weeklyWorkTime += result.WorkTime;
                //To make sure Weekly Work Time will show even if last record is from the middle of the week
                if (groupedRecords.Count() == results.Count() + 1)
                {
                    result.IsLastDayOfWeek = true;
                }
                if(result.IsLastDayOfWeek)
                {
                    result.WeeklyWorkTime = weeklyWorkTime;
                    result.WeeklyDifference = weeklyWorkTime - _standardWeeklyWorkTime;
                }
                results.Add(result);
                Log.Information($"{_analyzedRecord}{results.Count}");
            }
            Log.Information($"{_analyzingFinished}");
            return results;
        }

        /// <summary>
        /// Calculates daily work time.
        /// </summary>
        /// <param name="workDayRecords"></param>
        /// <returns></returns>
        private TimeSpan CalculateWorkTime(IGrouping<DateTime, InputRecord> workDayRecords)
        {
            return workDayRecords.Last().Date - workDayRecords.First().Date;
        }

        /// <summary>
        /// Decides whether our daily worktime is inconclusive, was done on weekend, was overtime or undertime.
        /// </summary>
        /// <param name="workDayRecords"></param>
        /// <param name="workTime"></param>
        /// <returns></returns>
        private WorkTimeClassifier ClassifyWorkTime(IGrouping<DateTime,InputRecord> workDayRecords, TimeSpan workTime)
        {
            
            if(!workDayRecords.Last().Event.Contains(_exit))
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

        /// <summary>
        /// Gathers data from one day in Result class.
        /// </summary>
        /// <param name="workDayRecords"></param>
        /// <returns></returns>
        private Result SummarizeWorkDay(IGrouping<DateTime, InputRecord> workDayRecords)
        {
            var summarizedDay = new Result();
            summarizedDay.Date = workDayRecords.Key;
            summarizedDay.WorkTime = CalculateWorkTime(workDayRecords);
            summarizedDay.Classification = ClassifyWorkTime(workDayRecords, summarizedDay.WorkTime);
            summarizedDay.IsLastDayOfWeek = workDayRecords.Key.DayOfWeek == DayOfWeek.Friday;

            return summarizedDay;
        }
    }
}
