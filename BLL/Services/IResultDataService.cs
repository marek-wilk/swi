using System.Collections.Generic;
using BLL.Data;

namespace BLL.Services
{
    public interface IResultDataService
    {
        public List<string> ReshapeData(List<InputRecord> records);
        public string AddWorkTimeClassification(Result record);
        public string AddWeeklyHours(Result record);
    }
}
