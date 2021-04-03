using System;

namespace BLL.Data
{
    public class OutputRecord
    {
        public DateTime Date { get; set; }
        public TimeSpan WorkTime { get; set; }
        public WorkTimeClassifier Classification { get; set; }
        //I will use friday as last day of week by default
        public bool IsLastDayOfWeek { get; set; }
        //only used when IsLastDayOfWeek = true
        public TimeSpan WeeklyWorkTime { get; set; }

        public TimeSpan WeeklyDifference { get; set; }
    }
}
