using BLL;

namespace swi
{
    class Program
    {
        static void Main(string[] args)
        {
            var outputDataService = new OutputDataService();
            var fileService = new FileService();
            var recordsFromFile = fileService.ReadRecords();
            if (recordsFromFile == null)
            {
                return;
            }
            fileService.SaveToFile(outputDataService.ReshapeData(recordsFromFile));
        }
    }
}
