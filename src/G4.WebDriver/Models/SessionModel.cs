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

            // Initialize the FirstMatch dictionary with case-insensitive string keys.
            FirstMatch = [];

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
        /// Gets or sets a collection of capabilities that at least one must be matched.
        /// </summary>
        public IEnumerable<IDictionary<string, object>> FirstMatch { get; set; }

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
    }
}
