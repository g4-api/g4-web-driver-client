using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the arguments passed to a command are either invalid or malformed.
    /// </summary>
    [WebDriverException(jsonErrorCode: "invalid argument", HttpStatusCode = HttpStatusCode.BadRequest)]
    public class InvalidArgumentException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidArgumentException class.
        /// </summary>
        public InvalidArgumentException()
        { }

        /// <inheritdoc />
        public InvalidArgumentException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public InvalidArgumentException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
