using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace G4.WebDriver.Models
{
    /// <summary>
    /// Represents a session configuration model, including capabilities and credentials for intermediary node authentication.
    /// This model is used to configure WebDriver sessions with specific features and authentication details.
    /// </summary>
    public class SessionModel
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionModel"/> class.
        /// Sets up default values for capabilities and desired capabilities.
        /// </summary>
        public SessionModel()
        {
            // Initialize the Capabilities model.
            Capabilities = new CapabilitiesModel();

            // Initialize the DesiredCapabilities dictionary with case-insensitive string keys.
            DesiredCapabilities = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            // Set the StartNewSession flag to true by default.
            StartNewSession = true;
        }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets or sets the capabilities that communicate the features supported by the WebDriver implementation.
        /// </summary>
        public CapabilitiesModel Capabilities { get; set; }

        /// <summary>
        /// Gets or sets the desired capabilities that specify the desired features for the WebDriver session.
        /// These capabilities define what the WebDriver session should support.
        /// </summary>
        public IDictionary<string, object> DesiredCapabilities { get; set; }

        /// <summary>
        /// Gets or sets the password needed for intermediary node authentication.
        /// This is used when the WebDriver session requires authentication against a node.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to start a new session.
        /// This property is ignored during JSON serialization.
        /// </summary>
        [JsonIgnore]
        public bool StartNewSession { get; set; }

        /// <summary>
        /// Gets or sets the username needed for intermediary node authentication.
        /// This is used in conjunction with the Password property for node authentication.
        /// </summary>
        public string User { get; set; }
        #endregion

        #region *** Methods      ***
        /// <summary>
        /// Builds the session model by merging the capabilities into the desired capabilities.
        /// Merges both FirstMatch and AlwaysMatch capabilities into the DesiredCapabilities dictionary.
        /// </summary>
        /// <returns>Returns the current <see cref="SessionModel"/> instance with updated desired capabilities.</returns>
        public SessionModel Build()
        {
            // Merge capabilities from the FirstMatch list into DesiredCapabilities.
            for (int i = 0; i < Capabilities.FirstMatch.Count(); i++)
            {
                foreach (var item in Capabilities.FirstMatch.ElementAt(i))
                {
                    DesiredCapabilities[item.Key] = item.Value;
                }
            }

            // Merge capabilities from the AlwaysMatch dictionary into DesiredCapabilities.
            foreach (var item in Capabilities.AlwaysMatch)
            {
                DesiredCapabilities[item.Key] = item.Value;
            }

            // Return the updated session model instance.
            return this;
        }
        #endregion
    }
}
