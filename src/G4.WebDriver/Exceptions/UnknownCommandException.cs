using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a command could not be executed because the remote end is not aware of it.
    /// </summary>
    [WebDriverException(jsonErrorCode: "unknown command", HttpStatusCode = HttpStatusCode.NotFound)]
    public class UnknownCommandException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the UnknownCommandException class.
        /// </summary>
        public UnknownCommandException()
        { }

        /// <inheritdoc />
        public UnknownCommandException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public UnknownCommandException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
