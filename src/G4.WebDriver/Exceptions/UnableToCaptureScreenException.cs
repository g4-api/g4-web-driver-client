using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a screen capture was made impossible.
    /// </summary>
    [WebDriverException(jsonErrorCode: "unable to capture screen", HttpStatusCode = HttpStatusCode.InternalServerError)]
    public class UnableToCaptureScreenException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the UnableToCaptureScreenException class.
        /// </summary>
        public UnableToCaptureScreenException()
        { }

        /// <inheritdoc />
        public UnableToCaptureScreenException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public UnableToCaptureScreenException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
