using Serilog;
using System.Web;
using System.Web.Http;

namespace CustomerOrder.Api
{
    /// <summary>
    /// The .NET Framework equivalent of Program.cs - but only the entry point.
    /// All real configuration lives in App_Start.
    /// </summary>
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);  // Register Web API configuration and services
        }

        protected void Application_End()
        {
            Log.CloseAndFlush();
        }
    }
}
