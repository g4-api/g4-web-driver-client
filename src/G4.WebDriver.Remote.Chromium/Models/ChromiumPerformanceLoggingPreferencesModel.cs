/*
 * RESOURCES
 * https://learn.microsoft.com/en-us/microsoft-edge/webdriver-chromium/capabilities-edge-options
 */
using G4.WebDriver.Attributes;

namespace G4.WebDriver.Remote.Chromium.Models
{
    /// <summary>
    /// Specifies performance logging preferences.
    /// </summary>
    public class ChromiumPerformanceLoggingPreferencesModel : CapabilitiesDictionaryBase
    {
        #region *** Properties ***
        /// <summary>
        /// Gets or sets the requested number of milliseconds between DevTools trace buffer usage events.
        /// For example, if 1000, then once per second, DevTools reports how full the trace buffer is.
        /// If a report indicates the buffer usage is 100%, a warning is issued.
        /// </summary>
        [KnownCapability(capabilityName: "bufferUsageReportingInterval")]
        public int BufferUsageReportingInterval { get; set; }

        /// <summary>
        /// Gets or sets a value indicating to collect (or not collect) events from Network domain.
        /// </summary>
        [KnownCapability(capabilityName: "enableNetwork")]
        public bool EnableNetwork { get; set; }

        /// <summary>
        /// Gets or sets a value indicating to collect (or not collect) events from Page domain.
        /// </summary>
        [KnownCapability(capabilityName: "enablePage")]
        public bool EnablePage { get; set; }

        /// <summary>
        /// A comma-separated string of Microsoft Edge tracing categories for which trace events should be collected.
        /// An unspecified or empty string disables tracing.
        /// </summary>
        [KnownCapability(capabilityName: "traceCategories")]
        public string TraceCategories { get; set; }
        #endregion
    }
}
