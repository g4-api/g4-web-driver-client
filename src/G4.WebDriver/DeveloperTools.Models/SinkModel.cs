namespace G4.WebDriver.DeveloperTools.Models
{
    /// <summary>
    /// Represents a sink model that contains information about a cast sink (cast device).
    /// </summary>
    public class SinkModel
    {
        /// <summary>
        /// Gets or sets the ID of the cast sink.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the cast sink.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the session of the cast sink.
        /// </summary>
        public string Session { get; set; }
    }
}
