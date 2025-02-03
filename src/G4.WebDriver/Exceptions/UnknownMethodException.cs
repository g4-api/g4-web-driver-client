using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the requested command matched a known URL but did not match an method for that URL.
    /// </summary>
    [WebDriverException(jsonErrorCode: "unknown method", HttpStatusCode = HttpStatusCode.MethodNotAllowed)]
    public class UnknownMethodException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the UnknownMethodException class.
        /// </summary>
        public UnknownMethodException()
        { }

        /// <inheritdoc />
        public UnknownMethodException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public UnknownMethodException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
