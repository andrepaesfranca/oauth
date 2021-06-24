using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;

namespace oauth_api
{
    public class Program
    {
        private static NLog.Logger _logger;
        public static string HostingEnvironmentValue { get; set; }

        public static void Main(string[] args)
        {
            const string environmentVariablesPrefix = "ASPNETCORE_";
            var hostingEnvironmentKey = $"{environmentVariablesPrefix}ENVIRONMENT";

            try
            {
                HostingEnvironmentValue = Environment.GetEnvironmentVariable(hostingEnvironmentKey);
            }
            catch (Exception)
            {
            }

            if (string.IsNullOrWhiteSpace(HostingEnvironmentValue))
            {
                HostingEnvironmentValue = "Development";
            }

            var configuration = new ConfigurationBuilder()
                        .AddJsonFile($"appsettings.{HostingEnvironmentValue}.json", optional: false, reloadOnChange: true)
                        .Build();

            _logger = NLog.Web.NLogBuilder.ConfigureNLog($"nlog.config").GetCurrentClassLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders()
                    .SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog();
    }
}
