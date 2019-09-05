using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Extensions.Options;

namespace FortuneTeller.Common
{
    public class OptionsMonitorWrapper<T> : IOptionsMonitor<T>
    {
        private T Options { get; }

        public OptionsMonitorWrapper(T options)
        {
            Options = options;
        }

        public T CurrentValue => Options;

        public T Get(string name)
        {
            throw new NotImplementedException();
        }

        public IDisposable OnChange(Action<T, string> listener)
        {
            throw new NotImplementedException();
        }
    }
}
