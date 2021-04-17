using BLL.Data;
using System.Collections.Generic;

namespace BLL.Services
{
    public interface IFileService
    {
        public List<InputRecord> ReadRecords(string path);
        public void SaveToFile(List<string> dataToSave, string path);
        public string GetPath();
    }
}
