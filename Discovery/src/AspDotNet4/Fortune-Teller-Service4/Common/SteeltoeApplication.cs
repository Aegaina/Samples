using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Logging;
using System;
using System.IO;

namespace FortuneTeller.Common
{
    public class SteeltoeApplication : ExtApplicationContext
    {
        public SteeltoeApplication(string environment) : base(environment)
        {
            Eureka = new EurekaSupport(this);
        }

        #region Properties

        public EurekaSupport Eureka { get; private set; }

        #endregion

        protected override void AppendConfiguration(IConfigurationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder = builder.AddCloudFoundry();
        }
    }
}
