namespace G4.WebDriver.Models
{
    /// <summary>
    /// Represents a target model for developer tools.
    /// </summary>
    public class DeveloperToolsTargetModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether the target is attached.
        /// </summary>
        public bool Attached { get; set; }

        /// <summary>
        /// Gets or sets the ID of the browser context the target belongs to.
        /// </summary>
        public string BrowserContextId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the target can access the opener target.
        /// </summary>
        public bool CanAccessOpener { get; set; }

        /// <summary>
        /// Gets or sets the frame ID of the opener target, if any.
        /// </summary>
        public string OpenerFrameId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the opener target, if any.
        /// </summary>
        public string OpenerId { get; set; }

        /// <summary>
        /// Gets or sets additional details for specific target types.
        /// For example, for the type of "page", this may be set to "portal" or "prerender".
        /// </summary>
        public string Subtype { get; set; }

        /// <summary>
        /// Gets or sets the ID of the target.
        /// </summary>
        public string TargetId { get; set; }

        /// <summary>
        /// Gets or sets the title of the target.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the type of the target.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the URL of the target.
        /// </summary>
        public string Url { get; set; }
    }
}
