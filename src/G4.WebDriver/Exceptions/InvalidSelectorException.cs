using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the element selector value is not supported or invalid.
    /// </summary>
    [WebDriverException(jsonErrorCode: "invalid selector", HttpStatusCode = HttpStatusCode.BadRequest)]
    public class InvalidSelectorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidSelectorException class.
        /// </summary>
        public InvalidSelectorException()
        { }

        /// <inheritdoc />
        public InvalidSelectorException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public InvalidSelectorException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
