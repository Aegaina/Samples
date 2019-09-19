using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FortuneTeller.Common;
using FortuneTellerService4.Properties;

namespace FortuneTellerService4
{
    public static class ApplicationContext
    {
        public static SteeltoeApplication Current { get; private set; }

        public static void Init()
        {
            Current = new SteeltoeApplication(Settings.Default.Environment, Settings.Default.ConfigFileExt);
        }
    }
}
