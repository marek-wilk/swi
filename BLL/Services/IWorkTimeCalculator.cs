using BLL.Data;
using BLL.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public interface IWorkTimeCalculator
    {
        public List<Result> AnalyzeRecords(List<InputRecord> records);
        public TimeSpan CalculateWorkTime(IGrouping<DateTime, InputRecord> workDayRecords);
        public WorkTimeClassifier ClassifyWorkTime(IGrouping<DateTime, InputRecord> workDayRecords, TimeSpan workTime);
        public Result SummarizeWorkDay(IGrouping<DateTime, InputRecord> workDayRecords);
    }
}
