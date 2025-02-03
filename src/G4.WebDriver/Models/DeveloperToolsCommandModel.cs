using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace G4.WebDriver.Models
{
    /// <summary>
    /// Represents a command model for developer tools.
    /// </summary>
    public class DeveloperToolsCommandModel
    {
        /// <summary>
        /// Gets or sets the ID of the command.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the method name of the command.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets the parameters associated with the command.
        /// </summary>
        [JsonPropertyName("params")]
        public IDictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the session ID associated with the command.
        /// </summary>
        public string SessionId { get; set; }
    }
}
