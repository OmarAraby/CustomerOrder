using Serilog;
using System.Web.Hosting;

namespace CustomerOrder.Api
{
    public static class LoggingConfig
    {
        public static void Register()
        {
            var path = HostingEnvironment.MapPath("~/logs/api-.txt");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(
                    path: path,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 14, // for delete file that has more 14 days
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Log.Information("Logger initialised.");
        }
    }

}