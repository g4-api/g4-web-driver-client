using System.Collections.Generic;

namespace G4.WebDriver.Models
{
    /// <summary>
    /// Represents the proxy configuration capabilities in WebDriver.
    /// The proxy configuration is a JSON object nested within the primary capabilities.
    /// Additional proxy configuration options may be defined by implementations,
    /// but they must not alter the semantics of the predefined ones.
    /// </summary>
    public class ProxyModel
    {
        #region *** Properties   ***
        /// <summary>
        /// Gets or sets the proxy host for FTP traffic.
        /// This is only used when <see cref="ProxyType"/> is set to "manual".
        /// </summary>
        public string FtpProxy { get; set; }

        /// <summary>
        /// Gets or sets the proxy host for HTTP traffic.
        /// This is only used when <see cref="ProxyType"/> is set to "manual".
        /// </summary>
        public string HttpProxy { get; set; }

        /// <summary>
        /// Gets or sets a list of addresses for which the proxy should be bypassed.
        /// This is only used when <see cref="ProxyType"/> is set to "manual".
        /// </summary>
        public IEnumerable<string> NoProxy { get; set; }

        /// <summary>
        /// Gets or sets the URL for the proxy auto-config (PAC) file.
        /// This is only used when <see cref="ProxyType"/> is set to "pac".
        /// </summary>
        public string ProxyAutoconfigUrl { get; set; }

        /// <summary>
        /// Gets or sets the type of proxy configuration to be used.
        /// </summary>
        public string ProxyType { get; set; }

        /// <summary>
        /// Gets or sets the proxy host for encrypted TLS traffic (SSL).
        /// This is only used when <see cref="ProxyType"/> is set to "manual".
        /// </summary>
        public string SslProxy { get; set; }

        /// <summary>
        /// Gets or sets the proxy host for a SOCKS proxy.
        /// This is only used when <see cref="ProxyType"/> is set to "manual".
        /// </summary>
        public string SocksProxy { get; set; }

        /// <summary>
        /// Gets or sets the version of the SOCKS protocol to be used.
        /// This is only used when <see cref="ProxyType"/> is set to "manual".
        /// </summary>
        public byte SocksVersion { get; set; }
        #endregion

        #region *** Nested Types ***
        /// <summary>
        /// Provides a set of constants representing different types of proxy configurations.
        /// </summary>
        public static class ProxyConfigurationTypes
        {
            /// <summary>
            /// Indicates that the proxy should be auto-detected in an implementation-specific way.
            /// </summary>
            public const string AutoDetect = "autodetect";

            /// <summary>
            /// Indicates that no proxy should be used.
            /// </summary>
            public const string Direct = "direct";

            /// <summary>
            /// Indicates that the proxy settings should be configured manually.
            /// </summary>
            public const string Manual = "manual";

            /// <summary>
            /// Indicates that a proxy auto-config (PAC) file should be used.
            /// </summary>
            public const string Pac = "pac";

            /// <summary>
            /// Indicates that the browser should use the proxy settings configured in the underlying operating system.
            /// </summary>
            public const string System = "system";
        }
        #endregion
    }
}
