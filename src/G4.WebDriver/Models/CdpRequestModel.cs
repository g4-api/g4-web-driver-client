using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace G4.WebDriver.Models
{
    /// <summary>
    /// Represents a request model for a Microsoft Edge DevTools Protocol (CDP) command.
    /// </summary>
    public class CdpRequestModel
    {
        /// <summary>
        /// Gets or sets the command to be executed.
        /// </summary>
        [JsonPropertyName("cmd")]
        public string Command { get; set; }

        /// <summary>
        /// Gets or sets the parameters for the command.
        /// </summary>
        [JsonPropertyName("params")]
        public IDictionary<string, object> Parameters { get; set; }
    }
}
