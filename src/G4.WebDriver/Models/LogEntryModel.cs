namespace G4.WebDriver.Models
{
    /// <summary>
    /// Represents a log entry in the WebDriver logging system.
    /// </summary>
    public class LogEntryModel
    {
        /// <summary>
        /// Gets or sets the log level of the entry (e.g., "INFO", "WARNING", "ERROR").
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the message associated with the log entry.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the log entry, represented as the number of milliseconds since the Unix epoch.
        /// </summary>
        public long Timestamp { get; set; }
    }
}
