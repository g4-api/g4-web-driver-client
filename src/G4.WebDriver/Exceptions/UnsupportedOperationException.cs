using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is indicating that a command that should have executed properly cannot be supported for some reason.
    /// </summary>
    [WebDriverException(jsonErrorCode: "unsupported operation", HttpStatusCode = HttpStatusCode.MethodNotAllowed)]
    public class UnsupportedOperationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the UnsupportedOperationException class.
        /// </summary>
        public UnsupportedOperationException()
        { }

        /// <inheritdoc />
        public UnsupportedOperationException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public UnsupportedOperationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
