using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when an unknown error occurred in the remote end while processing the command.
    /// </summary>
    [WebDriverException(jsonErrorCode: "unknown error", HttpStatusCode = HttpStatusCode.InternalServerError)]
    public class WebDriverException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the WebDriverException class.
        /// </summary>
        public WebDriverException()
        { }

        /// <inheritdoc />
        public WebDriverException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public WebDriverException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
