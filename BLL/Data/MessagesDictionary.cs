using BLL.Data.Enum;
using System.Collections.Generic;

namespace BLL.Data
{
    /// <summary>
    /// Class that keeps dictionaries as its properties containing logs messages. &lt;enum, message&gt;
    /// Kept dictionaries:
    /// <list>
    ///     Informations - keeps messages for logs of information level
    ///     <para>Errors - keeps messages for logs of error</para>
    /// </list>
    /// </summary>
    public class MessagesDictionary
    {
        public static Dictionary<LogInformation, string> Informations = new Dictionary<LogInformation, string>()
        {
            {LogInformation.Started, "Application has started."},
            {LogInformation.Reading, "Reading from file."},
            {LogInformation.Read, "Data has been successfully read."},
            {LogInformation.Analyzing, "Getting results. Result no." },
            {LogInformation.Analyzed, "Analyzing finished, got all results."},
            {LogInformation.Shaping, "Changing data to saveable, demanded format."},
            {LogInformation.Shaped, "Data successfully changed."},
            {LogInformation.Saving, "Saving results to file."},
            {LogInformation.Saved, "Saving complete. File 'result' can be found here "},
            {LogInformation.Finished, "Application has finished working."}
        };

        public static Dictionary<LogError, string> Errors = new Dictionary<LogError, string>()
        {
            {LogError.FileNotFound, "File couldn't be found in designated path."},
            {LogError.HeaderValidationException, "Headers in file don't match properties' names. Try fixing it."},
            {LogError.ReaderException, "Data type doesn't match property type or is empty."},
            {LogError.TypeConverterException, "Cannot convert data type from file to the one designated in class."},
            {LogError.Exception, "Something went wrong."}
        };
    }
}