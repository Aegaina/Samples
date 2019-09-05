using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Steeltoe.Extensions.Logging;

namespace FortuneTeller.Common
{
    public abstract class ExtApplicationContext
    {
        public ExtApplicationContext(string environment)
        {
            // Set up configuration sources.
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(GetContentRoot())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true);
            AppendConfiguration(builder);
            builder = builder.AddEnvironmentVariables();

            Configuration = builder.Build();

            LoggerFactory = new LoggerFactory();
            ILoggerProvider loggerProvider = new DynamicLoggerProvider(new ConsoleLoggerSettings().FromConfiguration(Configuration));
            LoggerFactory.AddProvider(loggerProvider);
        }

        #region Properties

        public IConfigurationRoot Configuration { get; private set; }

        public ILoggerFactory LoggerFactory { get; private set; }

        #endregion

        private string GetContentRoot()
        {
            string basePath = AppDomain.CurrentDomain.GetData("APP_CONTEXT_BASE_DIRECTORY") as string ??
               AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(basePath);
        }

        protected abstract void AppendConfiguration(IConfigurationBuilder builder);
    }
}
