using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Extensions.Options;

namespace FortuneTeller.Common
{
    /// <typeparam name="T">The options type.</typeparam>
    public class OptionsMonitorWrapper<T> : IOptionsMonitor<T>
    {
        public OptionsMonitorWrapper(T options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            CurrentValue = options;
        }

        #region Properties

        /// <summary>
        /// Returns the current TOptions instance with the Microsoft.Extensions.Options.Options.DefaultName.
        /// </summary>
        public T CurrentValue { get; private set; }

        #endregion

        /// <summary>
        /// Returns a configured TOptions instance with the given name
        /// </summary>
        public T Get(string name)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Registers a listener to be called whenever a named TOptions changes.
        /// </summary>
        /// <param name="listener">The action to be invoked when TOptions has changed.</param>
        /// <returns>An IDisposable which should be disposed to stop listening for changes.</returns>
        public IDisposable OnChange(Action<T, string> listener)
        {
            throw new NotSupportedException();
        }
    }
}
