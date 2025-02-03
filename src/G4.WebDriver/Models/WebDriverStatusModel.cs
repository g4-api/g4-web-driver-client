using System.Text.Json.Serialization;

namespace G4.WebDriver.Models
{
    /// <summary>
    /// Information about whether a remote end is in a state in which it can create new sessions,
    /// but may additionally include arbitrary meta information that is specific to the implementation.
    /// </summary>
    public class WebDriverStatusModel
    {
        #region *** Properties   ***
        /// <summary>
        /// Gets or sets the remote end's build information.
        /// </summary>
        public BuildModel Build { get; set; }

        /// <summary>
        /// Gets or sets an implementation-defined string explaining the remote end's readiness state.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the remote end's operating system information.
        /// </summary>
        [JsonPropertyName("os")]
        public OsModel OperatingSystem { get; set; }

        /// <summary>
        /// Gets or sets the remote end's readiness state.
        /// </summary>
        public bool Ready { get; set; }
        #endregion

        #region *** Nested Types ***
        /// <summary>
        /// Remote end's build information.
        /// </summary>
        public class BuildModel
        {
            /// <summary>
            /// Gets or sets the remote end's WebDriver service version.
            /// </summary>
            public string Version { get; set; }
        }

        /// <summary>
        /// Remote end's operating system information.
        /// </summary>
        public class OsModel
        {
            /// <summary>
            /// Gets or sets the remote end's operating system architecture.
            /// </summary>
            [JsonPropertyName("arch")]
            public string Architecture { get; set; }

            /// <summary>
            /// Gets or sets the remote end's operating system name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the remote end's operating system version.
            /// </summary>
            public string Version { get; set; }
        }
        #endregion
    }
}
