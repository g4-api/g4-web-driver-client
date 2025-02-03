using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a command to switch to a frame could not be satisfied because the frame could not be found.
    /// </summary>
    [WebDriverException(jsonErrorCode: "no such frame", HttpStatusCode = HttpStatusCode.NotFound)]
    public class NoSuchFrameException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the NoSuchFrameException class.
        /// </summary>
        public NoSuchFrameException()
        { }

        /// <inheritdoc />
        public NoSuchFrameException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public NoSuchFrameException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
