namespace G4.WebDriver.Models
{
    /// <summary>
    /// Represents the preferences for logging in WebDriver, including the log type and log level.
    /// </summary>
    public class LogPreferencesModel
    {
        /// <summary>
        /// Gets or sets the type of log (e.g., "browser", "driver") for which the logging preferences apply.
        /// </summary>
        public string LogType { get; set; }

        /// <summary>
        /// Gets or sets the log level (e.g., "INFO", "WARNING", "SEVERE") for the specified log type.
        /// </summary>
        public string LogLevel { get; set; }
    }
}
