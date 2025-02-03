using System;
using System.Collections.Generic;

namespace G4.WebDriver.Models
{
    /// <summary>
    /// Used to communicate the features supported by a given implementation.
    /// The local end may use capabilities to define which features it requires the remote end to satisfy when creating a new session.
    /// Likewise, the remote end uses capabilities to describe the full feature set for a session.
    /// </summary>
    public class SessionCapabilitiesModel
    {
        /// <summary>
        /// Gets or sets the capabilities which will bare to communicate
        /// the features supported by a given implementation.
        /// </summary>
        public IDictionary<string, object> AlwaysMatch { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the capabilities which will bare to communicate
        /// the features supported by a given implementation.
        /// </summary>
        public IEnumerable<IDictionary<string, object>> FirstMatch { get; set; } = Array.Empty<Dictionary<string, object>>();
    }
}
