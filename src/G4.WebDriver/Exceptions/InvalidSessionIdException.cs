using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the given session id is not in the list of active sessions,
    /// meaning the session either does not exist or that it's not active.
    /// </summary>
    [WebDriverException(jsonErrorCode: "invalid session id", HttpStatusCode = HttpStatusCode.BadRequest)]
    public class InvalidSessionIdException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidSessionId class.
        /// </summary>
        public InvalidSessionIdException()
        { }

        /// <inheritdoc />
        public InvalidSessionIdException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public InvalidSessionIdException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
