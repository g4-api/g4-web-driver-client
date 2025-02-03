using System.Text.Json.Serialization;

namespace G4.WebDriver.DeveloperTools.Models
{
    /// <summary>
    /// Provides information on the browser of the host machine and which version of the DevTools Protocol it supports.
    /// </summary>
    public class VersionModel
    {
        /// <summary>
        /// Gets or sets the name of the browser running on the host machine.
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// Gets or sets the version of the DevTools Protocol supported by the browser.
        /// </summary>
        [JsonPropertyName(name: "Protocol-Version")]
        public string ProtocolVersion { get; set; }

        /// <summary>
        /// Gets or sets the User-Agent string of the browser.
        /// </summary>
        [JsonPropertyName(name: "User-Agent")]
        public string UserAgent { get; set; }

        /// <summary>
        /// Gets or sets the version of the V8 JavaScript engine used by the browser.
        /// </summary>
        [JsonPropertyName(name: "V8-Version")]
        public string V8Version { get; set; }

        /// <summary>
        /// Gets or sets the version of the WebKit rendering engine used by the browser (if applicable).
        /// </summary>
        [JsonPropertyName(name: "WebKit-Version")]
        public string WebKitVersion { get; set; }

        /// <summary>
        /// Gets or sets the WebSocket URL for debugging the browser remotely using DevTools.
        /// </summary>
        public string WebSocketDebuggerUrl { get; set; }
    }
}
