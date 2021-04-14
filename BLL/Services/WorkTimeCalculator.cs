using BLL.Data;
using BLL.Data.Enum;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class WorkTimeCalculator : IWorkTimeCalculator
    {
        private readonly ILogger<WorkTimeCalculator> _log;
        private readonly IConfiguration _config;

        public WorkTimeCalculator(ILogger<WorkTimeCalculator> log, IConfiguration config)
        {
            _log = log;
            _config = config;
        }

        /// <summary>
        /// Groups records by date then call other functions to calculate work time, classify it and return data in List.
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
                if (groupedRecords.Count() == results.Count() + _config.GetValue<int>("One"))
                {
                    result.IsLastDayOfWeek = true;
                }
                if(result.IsLastDayOfWeek)
                {
                    result.WeeklyWorkTime = weeklyWorkTime;
                    result.WeeklyDifference = weeklyWorkTime - _config.GetValue<TimeSpan>("StandardWeeklyWorkTime");
                }
                results.Add(result);
                _log.LogInformation($"{MessagesDictionary.Informations[LogInformation.Analyzing]}{results.Count}");
            }
            _log.LogInformation(MessagesDictionary.Informations[LogInformation.Analyzed]);
            return results;
        }
        /// <summary>
        /// Calculates daily work time.
        /// </summary>
        /// <param name="workDayRecords"></param>
        /// <returns></returns>
        public TimeSpan CalculateWorkTime(IGrouping<DateTime, InputRecord> workDayRecords)
        {
            return workDayRecords.Last().Date - workDayRecords.First().Date;
        }
        /// <summary>
        /// Decides whether our daily worktime is inconclusive, was done on weekend, was overtime or undertime.
        /// </summary>
        /// <param name="workDayRecords"></param>
        /// <param name="workTime"></param>
        /// <returns></returns>
        public WorkTimeClassifier ClassifyWorkTime(IGrouping<DateTime,InputRecord> workDayRecords, TimeSpan workTime)
        {
            
            if(workDayRecords.Last().Event)
            {
                return WorkTimeClassifier.Inconclusive;
            }
            if(workDayRecords.Key.DayOfWeek == DayOfWeek.Saturday || workDayRecords.Key.DayOfWeek == DayOfWeek.Sunday)
            {
                return WorkTimeClassifier.Weekend;
            }
            if(workTime > _config.GetSection("Overtime").GetValue<TimeSpan>("timespan"))
            {
                return WorkTimeClassifier.Overtime;
            }
            if(workTime < _config.GetSection("Undertime").GetValue<TimeSpan>("timespan"))
            {
                return WorkTimeClassifier.Undertime;
            }
            return WorkTimeClassifier.Normal;
        }
        /// <summary>
        /// Gathers data from records from one day in Result class.
        /// </summary>
        /// <param name="workDayRecords"></param>
        /// <returns></returns>
        public Result SummarizeWorkDay(IGrouping<DateTime, InputRecord> workDayRecords)
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
