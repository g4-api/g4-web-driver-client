using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a command to set a cookie's value could not be satisfied.
    /// </summary>
    [WebDriverException(jsonErrorCode: "unable to set cookie", HttpStatusCode = HttpStatusCode.InternalServerError)]
    public class UnableToSetCookieException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the UnableToSetCookieException class.
        /// </summary>
        public UnableToSetCookieException()
        { }

        /// <inheritdoc />
        public UnableToSetCookieException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public UnableToSetCookieException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
