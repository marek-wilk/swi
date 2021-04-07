using BLL;
using Serilog;

namespace swi
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();
            var outputDataService = new ResultDataService();
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
