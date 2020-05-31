using Dell.Solution.Extensions.SpringConfiguration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace CentralTrade.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);

            var configRoot = configBuilder.Build();

            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(Logger)
                .UseConfiguration(configRoot)
                .UseStartup<Startup>() 
                .Build().Run();
        }

        private static void Logger(WebHostBuilderContext ctx, ILoggingBuilder logging)
        {
            if (ctx == null || logging == null) return;
            logging.ClearProviders();
        }
    }
}
