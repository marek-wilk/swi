using BLL.Data;
using BLL.Data.Enum;
using BLL.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace swi
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information(MessagesDictionary.Informations[LogInformation.Started]);

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IFileService, FileService>();
                    services.AddTransient<IWorkTimeCalculator, WorkTimeCalculator>();
                    services.AddTransient<IResultDataService, ResultDataService>();
                })
                .UseSerilog()
                .Build();
            var fileService = ActivatorUtilities.CreateInstance<FileService>(host.Services);
            var resultDataService = ActivatorUtilities.CreateInstance<ResultDataService>(host.Services);
            var path = fileService.GetPath();
            var recordsFromFile = fileService.ReadRecords(path);
            if (recordsFromFile == null)
            {
                return;
            }
            fileService.SaveToFile(resultDataService.ReshapeData(recordsFromFile), path);

            Log.Logger.Information(MessagesDictionary.Informations[LogInformation.Finished]);
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }
    }
}
