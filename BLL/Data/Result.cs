using System;

namespace BLL.Data
{
    public class Result
    {
        public DateTime Date { get; set; }
        public TimeSpan WorkTime { get; set; }
        public WorkTimeClassifier Classification { get; set; }
        //Friday will be the last day of week by default
        public bool IsLastDayOfWeek { get; set; }
        //only used when IsLastDayOfWeek = true
        public TimeSpan WeeklyWorkTime { get; set; }

        public TimeSpan WeeklyDifference { get; set; }
    }
}
