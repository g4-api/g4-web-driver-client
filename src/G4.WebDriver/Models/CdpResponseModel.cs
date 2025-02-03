namespace G4.WebDriver.Models
{
    /// <summary>
    /// Represents the response model for a Microsoft Edge DevTools Protocol (CDP) command invocation.
    /// </summary>
    public class CdpResponseModel
    {
        /// <summary>
        /// Gets or sets the ID of the frame associated with the response.
        /// </summary>
        public string FrameId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the loader associated with the response.
        /// </summary>
        public string LoaderId { get; set; }
    }
}
