using System.Collections.Generic;
using BLL.Data;

namespace BLL.Services
{
    public interface IFileService
    {
        public List<InputRecord> ReadRecords();
        public void SaveToFile(List<string> dataToSave);
    }
}
