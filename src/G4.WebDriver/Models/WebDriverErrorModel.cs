namespace G4.WebDriver.Models
{
    /// <summary>
    /// A model for a WebDriverError.
    /// </summary>
    public class WebDriverErrorModel
    {
        /// <summary>
        /// Gets or sets the JsonErrorCode sent by the WebDriverServer.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets the error message sent by the WebDriverServer.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the error stack trace sent by the WebDriverServer.
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the HTTP status code sent by the WebDriverServer
        /// </summary>
        public int StatusCode { get; set; }
    }
}
