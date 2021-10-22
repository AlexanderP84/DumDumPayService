using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using Serilog;
using Microsoft.Extensions.Hosting;

namespace DumDumPayService.TestWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("/logs/log.ndjson")
                .CreateLogger();

            try
            {
                Log.Information("Application has been started.");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application was terminated due to an exception!");
            }
            finally
            {
                Log.CloseAndFlush();
            }            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)                
                .UseStartup<Startup>()
            .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration));      
    }
}