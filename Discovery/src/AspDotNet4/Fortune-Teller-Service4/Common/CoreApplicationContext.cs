using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Aegaina.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FortuneTeller.Common
{
    public abstract class CoreApplicationContext
    {
        public CoreApplicationContext(string environment, string configFileExt)
        {
            if (string.IsNullOrWhiteSpace(configFileExt))
            {
                configFileExt = "json";
            }

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(GetRootPath())
                .AddJsonFile($"appsettings.{configFileExt}", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environment}.{configFileExt}", optional: true);
            builder = AppendConfiguration(builder);
            builder = builder.AddEnvironmentVariables();

            Configuration = builder.Build();

            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddProvider(Log4NetProvider.Instance);
        }

        #region Properties

        public IConfigurationRoot Configuration { get; private set; }

        public ILoggerFactory LoggerFactory { get; private set; }

        #endregion

        protected abstract IConfigurationBuilder AppendConfiguration(IConfigurationBuilder builder);

        public static string GetRootPath()
        {
            string basePath = AppDomain.CurrentDomain.GetData("APP_CONTEXT_BASE_DIRECTORY") as string ??
               AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(basePath);
        }
    }
}
