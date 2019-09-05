using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pivotal.Discovery.Eureka;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.Services;
using Steeltoe.Common.Discovery;
using Steeltoe.Discovery.Eureka;

namespace FortuneTeller.Common
{
    public class EurekaSupport
    {
        private readonly SteeltoeApplication appContext;
        private readonly ILogger<EurekaSupport> logger;

        public EurekaSupport(SteeltoeApplication appContext)
        {
            if (appContext == null)
            {
                throw new ArgumentNullException(nameof(appContext));
            }

            this.appContext = appContext;
            logger = this.appContext.LoggerFactory.CreateLogger<EurekaSupport>();

            DiscoveryClient = CreateDiscoveryClient();
            logger.LogInformation("Discovery service started!");
        }

        #region Properties

        public IDiscoveryClient DiscoveryClient { get; private set; }

        #endregion

        private IServiceInfo GetSingletonDiscoveryServiceInfo()
        {
            List<EurekaServiceInfo> svcInfos = appContext.Configuration.GetServiceInfos<EurekaServiceInfo>();
            if (svcInfos != null && svcInfos.Any())
            {
                if (svcInfos.Count == 1)
                {
                    return svcInfos[0];
                }
                else
                {
                    throw new ConnectorException(string.Format("Multiple discovery service types bound to application."));
                }
            }
            else
            {
                return null;
            }
        }

        private IDiscoveryClient CreateDiscoveryClient()
        {
            IConfigurationSection clientConfigSection = appContext.Configuration.GetSection(EurekaClientOptions.EUREKA_CLIENT_CONFIGURATION_PREFIX);
            if (clientConfigSection == null)
            {
                throw new ConfigurationErrorsException("Unable to load the Eureka client configuration section");
            }

            IConfigurationSection instanceConfigSection = appContext.Configuration.GetSection(EurekaInstanceOptions.EUREKA_INSTANCE_CONFIGURATION_PREFIX);
            if (instanceConfigSection == null)
            {
                throw new ConfigurationErrorsException("Unable to load the Eureka instance configuration section");
            }

            EurekaClientOptions clientOptions = new EurekaClientOptions();
            clientConfigSection.Bind(clientOptions);

            EurekaInstanceOptions instanceOptions = new EurekaInstanceOptions();
            instanceConfigSection.Bind(instanceOptions);

            EurekaServiceInfo svcInfo = GetSingletonDiscoveryServiceInfo() as EurekaServiceInfo;
            if (svcInfo != null)
            {
                PivotalEurekaConfigurer.UpdateConfiguration(appContext.Configuration, svcInfo, clientOptions);
                PivotalEurekaConfigurer.UpdateConfiguration(appContext.Configuration, svcInfo, instanceOptions);
            }

            EurekaApplicationInfoManager appInfoManager = new EurekaApplicationInfoManager
                (new OptionsMonitorWrapper<EurekaInstanceOptions>(instanceOptions), appContext.LoggerFactory);

            return new EurekaDiscoveryClient(new OptionsMonitorWrapper<EurekaClientOptions>(clientOptions),
                new OptionsMonitorWrapper<EurekaInstanceOptions>(instanceOptions),
                appInfoManager, null, appContext.LoggerFactory);
        }
    }
}
