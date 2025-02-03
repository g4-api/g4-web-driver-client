using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a new session could not be created.
    /// </summary>
    [WebDriverException(jsonErrorCode: "session not created", HttpStatusCode = HttpStatusCode.InternalServerError)]
    public class SessionNotCreatedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the SessionNotCreatedException class.
        /// </summary>
        public SessionNotCreatedException()
        { }

        /// <inheritdoc />
        public SessionNotCreatedException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public SessionNotCreatedException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
