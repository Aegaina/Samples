using FortuneTellerService4.Models;
using Microsoft.Extensions.Logging;
using System.Web.Http;

namespace FortuneTellerService4
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private ILogger<WebApiApplication> logger;

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //var config = GlobalConfiguration.Configuration;

            // Build application configuration
            ApplicationContext.Init();
            logger = ApplicationContext.Current.LoggerFactory.CreateLogger<WebApiApplication>();

            SampleData.InitializeFortunes();
            logger.LogInformation("Fortunes populated!");

            logger.LogInformation("Finished Application_Start");
        }

        protected void Application_End()
        {
            logger.LogInformation("Shutting down!");

            // Unregister current app with Service Discovery server
            ApplicationContext.Current.Eureka.DiscoveryClient.ShutdownAsync().Wait();
        }
    }
}
