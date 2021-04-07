using System.Collections.Generic;
using BLL.Data.Enum;

namespace BLL.Data
{
    class MessagesDictionary
    {
        public static Dictionary<LogInformation, string> Informations = new Dictionary<LogInformation, string>()
        {
            {LogInformation.Reading, "Reading from file."},
            {LogInformation.Read, "Data has been successfully read."},
            {LogInformation.Analyzing, "Getting results. Result no." },
            {LogInformation.Analyzed, "Analyzing finished, got all results."},
            {LogInformation.Shaping, "Changing data to saveable, demanded format."},
            {LogInformation.Shaped, "Data successfully changed."},
            {LogInformation.Saving, "Saving results to file."},
            {LogInformation.Saved, "Saving complete. File 'result' can be found here "}
        };

        public static Dictionary<LogError, string> Errors = new Dictionary<LogError, string>()
        {
            {LogError.FileNotFound, "File couldn't be found in designated path."},
            {LogError.HeaderValidationException, "Headers in file don't match properties' names. Try fixing it."},
            {LogError.ReaderException, "Data type doesn't match property type."},
            {LogError.TypeConverterException, "Cannot convert data type from file to the one designated in class."},
            {LogError.Exception, "Something went wrong."}
        };
    }
}