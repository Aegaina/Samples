using Microsoft.Extensions.Configuration;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System;
using System.IO;

namespace FortuneTeller.Common
{
    public class SteeltoeApplication : MsExtApplicationContext
    {
        public SteeltoeApplication(string environment) : base(environment)
        {
            Eureka = new EurekaSupport(this);
        }

        #region Properties

        public EurekaSupport Eureka { get; private set; }

        #endregion

        protected override IConfigurationBuilder AppendConfiguration(IConfigurationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.AddCloudFoundry();
        }
    }
}
